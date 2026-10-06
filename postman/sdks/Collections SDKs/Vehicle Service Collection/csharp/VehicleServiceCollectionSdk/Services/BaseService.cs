using System.Text.Json;
using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Http;
using VehicleServiceCollectionSdk.Http.Exceptions;

namespace VehicleServiceCollectionSdk.Services;

/// <summary>
/// Base service class providing core HTTP request execution and JSON serialization for all service endpoints.
/// </summary>
public class BaseService
{
    /// <summary>
    /// Constant representing the absence of a specific HTTP status code, used for default error mappings.
    /// </summary>
    protected const int NoStatusCode = 0;

    protected readonly Client _httpClient;
    protected readonly JsonSerializerOptions _jsonSerializerOptions;
    private RequestConfig? _serviceConfig;

    public BaseService(Client httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _jsonSerializerOptions.PropertyNameCaseInsensitive = false;
        _jsonSerializerOptions.Converters.Add(
            new VehicleServiceCollectionSdk.Models.OptionalConverterFactory()
        );
    }

    /// <summary>
    /// Sets service-level configuration that applies to all methods in this service.
    /// Service-level config is overridden by method-level and request-level config.
    /// </summary>
    public BaseService SetConfig(RequestConfig config)
    {
        _serviceConfig = config;
        return this;
    }

    /// <summary>
    /// Resolves configuration by merging service, method, and request configs.
    /// Priority order (highest to lowest): request config > method config > service config > SDK config.
    /// </summary>
    protected RequestConfig? GetResolvedConfig(
        RequestConfig? methodConfig,
        RequestConfig? requestConfig
    )
    {
        if (_serviceConfig == null && methodConfig == null && requestConfig == null)
        {
            return null;
        }

        return new RequestConfig(
            BaseUrl: requestConfig?.BaseUrl ?? methodConfig?.BaseUrl ?? _serviceConfig?.BaseUrl,
            Environment: requestConfig?.Environment
                ?? methodConfig?.Environment
                ?? _serviceConfig?.Environment,
            Timeout: requestConfig?.Timeout ?? methodConfig?.Timeout ?? _serviceConfig?.Timeout,
            ApiKeyAuth: requestConfig?.ApiKeyAuth
                ?? methodConfig?.ApiKeyAuth
                ?? _serviceConfig?.ApiKeyAuth,
            RetryConfig: requestConfig?.RetryConfig
                ?? methodConfig?.RetryConfig
                ?? _serviceConfig?.RetryConfig,
            IdempotencyKey: requestConfig?.IdempotencyKey
                ?? methodConfig?.IdempotencyKey
                ?? _serviceConfig?.IdempotencyKey
        );
    }

    /// <summary>
    /// Executes an HTTP request, applying any per-request configuration overrides.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="resolvedConfig">Optional resolved configuration with per-request overrides.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>The HTTP response message if the request succeeds.</returns>
    protected async Task<HttpResponseMessage> ExecuteAsync(
        Request request,
        RequestConfig? resolvedConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        CancellationTokenSource? timeoutCts = null;
        TimeSpan? appliedTimeout = null;
        // Kept before `cancellationToken` is replaced by the linked timeout token below: the linked
        // source cancels for either reason and records neither, so this is the only thing that keeps
        // a timeout distinguishable from the caller cancelling (FSDK-1585).
        var callerToken = cancellationToken;

        try
        {
            if (resolvedConfig?.Timeout != null)
            {
                appliedTimeout = resolvedConfig.Timeout.Value;
                timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(resolvedConfig.Timeout.Value);
                cancellationToken = timeoutCts.Token;
            }

            ApplyRequestConfigOverrides(request, resolvedConfig);

            var response = await _httpClient
                .SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }
        catch (OperationCanceledException ex) when (IsRequestTimeout(timeoutCts, callerToken))
        {
            throw new ApiTimeoutException($"The request timed out after {appliedTimeout}.", ex);
        }
        finally
        {
            // Safe to dispose here: SendAsync has completed and the response body
            // is buffered by the time we return. Streaming requests use a separate
            // ExecuteStreamAsync path and are not affected.
            timeoutCts?.Dispose();
        }
    }

    /// <summary>
    /// Whether an <see cref="OperationCanceledException"/> came from the per-request timeout rather
    /// than from the caller. The BCL raises <see cref="TaskCanceledException"/> for both and the
    /// linked <see cref="CancellationTokenSource"/> records neither cause, so the test is: the
    /// timeout source fired AND the caller's own token did not. When both fire the caller wins and
    /// the original cancellation propagates untouched (FSDK-1585).
    /// </summary>
    private static bool IsRequestTimeout(
        CancellationTokenSource? timeoutCts,
        CancellationToken callerToken
    )
    {
        return timeoutCts is not null
            && timeoutCts.IsCancellationRequested
            && !callerToken.IsCancellationRequested;
    }

    /// <summary>
    /// Applies BaseUrl and authentication overrides from the resolved config to the outgoing request.
    /// BaseUrl is applied by rewriting the request URI; auth overrides are passed via request options
    /// so that the delegating handlers in the pipeline can apply them instead of their stored credentials.
    /// </summary>
    private static void ApplyRequestConfigOverrides(Request request, RequestConfig? resolvedConfig)
    {
        if (resolvedConfig == null)
            return;

        if (resolvedConfig.BaseUrl != null)
        {
            var baseUri = new Uri(resolvedConfig.BaseUrl.TrimEnd('/') + "/");
            request.HttpRequestMessage.RequestUri = new Uri(baseUri, request.Url);
        }

        if (resolvedConfig.ApiKeyAuth != null)
        {
            request.HttpRequestMessage.Options.Set(
                new HttpRequestOptionsKey<string>("_RequestConfig_OverrideApiKey"),
                resolvedConfig.ApiKeyAuth.ApiKey
            );
            if (resolvedConfig.ApiKeyAuth.ApiKeyHeader != null)
            {
                request.HttpRequestMessage.Options.Set(
                    new HttpRequestOptionsKey<string>("_RequestConfig_OverrideHeader"),
                    resolvedConfig.ApiKeyAuth.ApiKeyHeader
                );
            }
        }
        if (resolvedConfig.RetryConfig != null)
        {
            request.HttpRequestMessage.Options.Set(
                new HttpRequestOptionsKey<RetryConfig>("_RequestConfig_RetryConfig"),
                resolvedConfig.RetryConfig
            );
        }

        // Carried as a request option rather than written straight onto the headers here, because
        // the header NAME comes from the spec's declared idempotency header and RetryHandler is
        // where that list lives. RetryHandler stamps it onto the request before the retry gate
        // reads it, which is still upstream of the transport, so the server sees it (FSDK-1712).
        if (!string.IsNullOrWhiteSpace(resolvedConfig.IdempotencyKey))
        {
            request.HttpRequestMessage.Options.Set(
                new HttpRequestOptionsKey<string>("_RequestConfig_IdempotencyKey"),
                resolvedConfig.IdempotencyKey
            );
        }
    }
}
