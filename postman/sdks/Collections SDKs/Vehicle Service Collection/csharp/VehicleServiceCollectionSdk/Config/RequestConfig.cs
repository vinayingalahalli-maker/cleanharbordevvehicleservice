using Environment = VehicleServiceCollectionSdk.Http.Environment;

namespace VehicleServiceCollectionSdk.Config;

/// <summary>
/// Per-request configuration overrides. When provided, these values take precedence
/// over method-level, service-level, and SDK-level configuration in that order.
/// </summary>
public record RequestConfig(
    /// <summary>Overrides the base URL for this request.</summary>
    string? BaseUrl = null,
    /// <summary>Overrides the environment for this request.</summary>
    Environment? Environment = null,
    /// <summary>Overrides the request timeout duration.</summary>
    TimeSpan? Timeout = null,
    /// <summary>Overrides the API key authentication for this request.</summary>
    ApiKeyAuthConfig? ApiKeyAuth = null,
    /// <summary>Overrides the retry configuration for this request.</summary>
    RetryConfig? RetryConfig = null,
    /// <summary>
    /// Sets an idempotency key for this request, sent under the spec's declared idempotency
    /// header (default <c>Idempotency-Key</c>). Supplying one re-enables retrying a POST or
    /// PATCH, which the retry pipeline otherwise refuses to replay. Only the retry pipeline
    /// consults it, and a spec-declared idempotency header on the operation takes precedence.
    /// </summary>
    string? IdempotencyKey = null
);
