using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Polly;
using Polly.Retry;
using VehicleServiceCollectionSdk.Config;

namespace VehicleServiceCollectionSdk.Http.Handlers;

/// <summary>
/// A handler for retrying requests when they fail.
/// </summary>
public class RetryHandler : DelegatingHandler
{
    private static readonly HttpRequestOptionsKey<RetryConfig> RetryConfigKey =
        new HttpRequestOptionsKey<RetryConfig>("_RequestConfig_RetryConfig");

    private static readonly HttpRequestOptionsKey<string> IdempotencyKeyConfigKey =
        new HttpRequestOptionsKey<string>("_RequestConfig_IdempotencyKey");

    private readonly int _defaultMaxRetryAttempts = 3;
    private readonly TimeSpan _defaultDelay = TimeSpan.FromMilliseconds(150);
    private readonly TimeSpan _defaultMaxDelay = TimeSpan.FromMilliseconds(5000);
    private readonly TimeSpan _defaultMaxRetryAfterDelay = TimeSpan.FromMilliseconds(60000);
    private readonly double _defaultBackoffMultiplier = 2;
    private readonly bool _defaultUseJitter = true;

    // HTTP methods allowed to retry at all. A method absent here is never retried; a method present
    // here is retried only if replaying it is safe (see IsRetryableRequest).
    private readonly HashSet<string> _defaultRetryableHttpMethods = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase
    )
    {
        "GET",
        "POST",
        "PUT",
        "DELETE",
        "PATCH",
        "HEAD",
        "OPTIONS",
    };

    // Methods RFC 9110 defines as idempotent: replaying one cannot cause a second effect, so a
    // retry is safe. POST and PATCH are absent deliberately — see IsRetryableRequest.
    private static readonly HashSet<string> _idempotentHttpMethods = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase
    )
    {
        "GET",
        "HEAD",
        "PUT",
        "DELETE",
        "OPTIONS",
        "TRACE",
    };

    // Request headers that carry a caller-supplied idempotency key. A server that honours one
    // deduplicates a replay, which is what makes retrying a non-idempotent method safe.
    private static readonly string[] _idempotencyKeyHeaders = new[] { "Idempotency-Key" };

    private readonly ResiliencePipeline<HttpResponseMessage> _defaultPipeline;
    private readonly ConcurrentDictionary<
        RetryConfig,
        ResiliencePipeline<HttpResponseMessage>
    > _pipelineCache = new();

    public RetryHandler(HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? Client.CreateDefaultTransport())
    {
        _defaultPipeline = BuildPipeline(null);
    }

    private static bool ShouldRetryStatus(HttpStatusCode statusCode, HashSet<int>? specificCodes)
    {
        if (specificCodes != null)
            return specificCodes.Contains((int)statusCode);

        return (int)statusCode >= 500
            || statusCode == HttpStatusCode.RequestTimeout
            || statusCode == HttpStatusCode.TooManyRequests;
    }

    /// <summary>
    /// Whether this request may be replayed.
    /// </summary>
    /// <remarks>
    /// Two gates. The configured method list decides whether the method is eligible at all.
    /// Eligibility alone is not enough for a non-idempotent method, though: replaying a POST or a
    /// PATCH can duplicate its effect, so one is retried only when the caller supplied an
    /// idempotency key for the server to deduplicate on.
    /// </remarks>
    private static bool IsRetryableRequest(
        HttpRequestMessage request,
        HashSet<string> retryableHttpMethods
    )
    {
        var method = request.Method.Method;
        if (!retryableHttpMethods.Contains(method))
            return false;
        if (_idempotentHttpMethods.Contains(method))
            return true;

        return HasIdempotencyKey(request);
    }

    /// <summary>
    /// Whether the request carries a non-empty idempotency key header.
    /// </summary>
    /// <remarks>
    /// TryGetValues rather than Contains: HttpHeaders.Contains throws InvalidOperationException for
    /// a name the collection does not accept, so it cannot be used as a probe.
    /// </remarks>
    private static bool HasIdempotencyKey(HttpRequestMessage request)
    {
        foreach (var name in _idempotencyKeyHeaders)
        {
            if (
                request.Headers.TryGetValues(name, out var values)
                && values.Any(value => !string.IsNullOrWhiteSpace(value))
            )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Applies a <c>RequestConfig.IdempotencyKey</c> to the outgoing request as a header, so that
    /// the gate below sees it and the server receives it.
    /// </summary>
    /// <remarks>
    /// The key arrives as a request option because <c>RequestConfig</c> cannot know the header
    /// name -- that comes from the spec's declared idempotency header, which only this handler
    /// has. Stamping it here still reaches the wire: this runs before <c>base.SendAsync</c>, so
    /// the server can deduplicate the replay, which is the whole reason a key makes retrying a
    /// non-idempotent method safe. A key that never reached the server would make this an
    /// unsafe-retry switch rather than an idempotency lever (FSDK-1712).
    ///
    /// A header the request already carries wins: a spec-declared idempotency parameter is the
    /// more specific source, and adding a second value would send both.
    /// </remarks>
    private static void ApplyConfiguredIdempotencyKey(HttpRequestMessage request)
    {
        if (HasIdempotencyKey(request))
            return;

        if (!request.Options.TryGetValue(IdempotencyKeyConfigKey, out var configuredKey))
            return;

        if (string.IsNullOrWhiteSpace(configuredKey))
            return;

        // CR/LF would split the request: everything after the newline is read by the server as
        // further headers (CWE-113). `TryAddWithoutValidation` skips .NET's newline check, so each
        // caller-supplied header path guards it; see caller-header-crlf-guard.spec.ts for the
        // full set.
        if (
            configuredKey != null
            && (configuredKey.IndexOf('\r') >= 0 || configuredKey.IndexOf('\n') >= 0)
        )
        {
            throw new ArgumentException(
                $"Header '{_idempotencyKeyHeaders[0]}' has a value containing a CR or LF character, which would split the request."
            );
        }
        request.Headers.TryAddWithoutValidation(_idempotencyKeyHeaders[0], configuredKey);
    }

    private static readonly Regex DeltaSecondsRegex = new(@"^\d+(\.\d+)?$", RegexOptions.Compiled);

    /// <summary>
    /// Returns the server-directed retry delay from rate-limit response headers, honoring
    /// Retry-After (delta-seconds or HTTP-date) and, when absent, X-RateLimit-Reset (epoch
    /// seconds), clamped to <paramref name="maxCap"/>. Returns null when no usable header is
    /// present so the caller falls back to the computed exponential backoff.
    /// </summary>
    private static TimeSpan? GetRetryAfterDelay(HttpResponseMessage? response, TimeSpan maxCap)
    {
        if (response is null || maxCap <= TimeSpan.Zero)
            return null;

        // retry-after-ms (milliseconds) is a non-standard but finer-grained hint some APIs send
        // (e.g. OpenAI); it takes precedence over the whole-second Retry-After.
        var retryAfterMs = response.Headers.TryGetValues(
            "Retry-After-Ms",
            out var retryAfterMsValues
        )
            ? retryAfterMsValues.FirstOrDefault()?.Trim()
            : null;
        // The strict delta format rules out negatives/NaN; a huge value parses to
        // +Infinity and Math.Min clamps it to the cap (parity with the other SDKs).
        if (
            !string.IsNullOrEmpty(retryAfterMs)
            && DeltaSecondsRegex.IsMatch(retryAfterMs)
            && double.TryParse(
                retryAfterMs,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var ms
            )
        )
        {
            return TimeSpan.FromMilliseconds(Math.Min(ms, maxCap.TotalMilliseconds));
        }

        double? seconds = null;
        if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
            seconds = ParseRetryAfter(retryAfterValues.FirstOrDefault());

        // X-RateLimit-Reset (epoch seconds) is only consulted when Retry-After is absent.
        // An already-elapsed reset window is treated as stale (fall back to backoff), whereas
        // an elapsed Retry-After above resolves to 0 ("retry now") — this mirrors the Ruby SDK.
        if (
            seconds is null
            && response.Headers.TryGetValues("X-RateLimit-Reset", out var resetValues)
            && long.TryParse(
                resetValues.FirstOrDefault(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var epoch
            )
        )
        {
            var delta = (
                DateTimeOffset.FromUnixTimeSeconds(epoch) - DateTimeOffset.UtcNow
            ).TotalSeconds;
            if (delta > 0)
                seconds = delta;
        }

        if (seconds is null)
            return null;

        var clampedSeconds = Math.Clamp(seconds.Value, 0.0, maxCap.TotalSeconds);
        return TimeSpan.FromSeconds(clampedSeconds);
    }

    /// <summary>
    /// Parses a Retry-After header value: integer/float delta-seconds, or an HTTP-date
    /// (a past date yields 0). Returns the delay in seconds, or null if unparseable.
    /// </summary>
    private static double? ParseRetryAfter(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmed = value.Trim();
        if (DeltaSecondsRegex.IsMatch(trimmed))
            return double.Parse(trimmed, CultureInfo.InvariantCulture);

        if (
            DateTimeOffset.TryParse(
                trimmed,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var date
            )
        )
        {
            var delta = (date - DateTimeOffset.UtcNow).TotalSeconds;
            return delta > 0 ? delta : 0.0;
        }

        return null;
    }

    private ResiliencePipeline<HttpResponseMessage> BuildPipeline(RetryConfig? overrideConfig)
    {
        var maxRetryAttempts = overrideConfig?.MaxRetryAttempts ?? _defaultMaxRetryAttempts;
        var delay = overrideConfig?.Delay ?? _defaultDelay;
        var maxDelay = overrideConfig?.MaxDelay ?? _defaultMaxDelay;
        var maxRetryAfterDelay = overrideConfig?.MaxRetryAfterDelay ?? _defaultMaxRetryAfterDelay;
        var backoffMultiplier = overrideConfig?.BackoffMultiplier ?? _defaultBackoffMultiplier;
        var useJitter = overrideConfig?.UseJitter ?? _defaultUseJitter;
        HashSet<int>? specificCodes =
            overrideConfig?.RetryableStatusCodes != null
                ? new HashSet<int>(overrideConfig.RetryableStatusCodes)
                : null;

        return new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(
                new RetryStrategyOptions<HttpResponseMessage>()
                {
                    MaxRetryAttempts = maxRetryAttempts,
                    ShouldHandle = (args) =>
                    {
                        var response = args.Outcome.Result;
                        if (response is null)
                            return ValueTask.FromResult(false);
                        return ValueTask.FromResult(
                            ShouldRetryStatus(response.StatusCode, specificCodes)
                        );
                    },
                    DelayGenerator = (args) =>
                    {
                        var headerDelay = GetRetryAfterDelay(
                            args.Outcome.Result,
                            maxRetryAfterDelay
                        );
                        if (headerDelay is not null)
                            return new ValueTask<TimeSpan?>(headerDelay);

                        var exponentialMs =
                            delay.TotalMilliseconds
                            * Math.Pow(backoffMultiplier, args.AttemptNumber);
                        if (useJitter)
                        {
                            var jitterFactor = 1.0 + (Random.Shared.NextDouble() - 0.5) * 0.5;
                            exponentialMs *= jitterFactor;
                        }
                        var cappedMs = Math.Min(exponentialMs, maxDelay.TotalMilliseconds);
                        return new ValueTask<TimeSpan?>(TimeSpan.FromMilliseconds(cappedMs));
                    },
                }
            )
            .Build();
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        request.Options.TryGetValue(RetryConfigKey, out var retryConfig);

        var retryableHttpMethods =
            retryConfig?.RetryableHttpMethods != null
                ? new HashSet<string>(
                    retryConfig.RetryableHttpMethods,
                    StringComparer.OrdinalIgnoreCase
                )
                : _defaultRetryableHttpMethods;

        ApplyConfiguredIdempotencyKey(request);

        if (!IsRetryableRequest(request, retryableHttpMethods))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        if (retryConfig?.MaxRetryAttempts == 0)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var pipeline = retryConfig is null
            ? _defaultPipeline
            : _pipelineCache.GetOrAdd(retryConfig, BuildPipeline);
        return await pipeline.ExecuteAsync<HttpResponseMessage>(
            async (token) => await base.SendAsync(request, token),
            cancellationToken
        );
    }
}
