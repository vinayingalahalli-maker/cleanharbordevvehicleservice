using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Http;
using VehicleServiceCollectionSdk.Http.Extensions;
using VehicleServiceCollectionSdk.Http.Handlers;
using VehicleServiceCollectionSdk.Services;
using Environment = VehicleServiceCollectionSdk.Http.Environment;

namespace VehicleServiceCollectionSdk;

/// <summary>
/// The main SDK client that provides access to all service endpoints.
/// Manages HTTP client lifecycle, authentication handlers, and service instances with centralized configuration.
/// Implements IDisposable to properly clean up HTTP resources.
/// </summary>
public class VehicleServiceCollectionSdkClient : IDisposable
{
    private readonly Client _httpClient;

    private readonly TokenHandler _apiKeyHandler;

    public VehicleServiceCollectionSdkService VehicleServiceCollectionSdk { get; private set; }

    /// <summary>Initializes a new instance of the VehicleServiceCollectionSdkClient client.</summary>
    /// <param name="config">SDK configuration options.</param>
    /// <param name="handler">Optional HttpMessageHandler used as the underlying transport (e.g. a custom or mocked handler). The SDK does not dispose a caller-supplied handler; the caller owns its lifetime.</param>
    public VehicleServiceCollectionSdkClient(
        VehicleServiceCollectionSdkConfig? config = null,
        HttpMessageHandler? handler = null
    )
    {
        var retryHandler = new RetryHandler(handler);
        _apiKeyHandler = new TokenHandler(retryHandler)
        {
            Header = config?.ApiKeyAuth?.ApiKeyHeader ?? ApiKeyAuthConfig.DefaultApiKeyHeader,
            Prefix = "",
            Token = config?.ApiKeyAuth?.ApiKey,
            OverrideTokenOptionsKey = "_RequestConfig_OverrideApiKey",
        };

        _httpClient = new Client(config, _apiKeyHandler, disposeHandler: handler is null);

        VehicleServiceCollectionSdk = new VehicleServiceCollectionSdkService(_httpClient);
    }

    /// <summary>
    /// Set the environment for the entire SDK.
    /// </summary>
    public void SetEnvironment(Environment environment)
    {
        SetBaseUrl(environment.Uri);
    }

    /// <summary>
    /// Sets the base URL for the entire SDK.
    /// </summary>
    public void SetBaseUrl(string baseUrl)
    {
        SetBaseUrl(new Uri(baseUrl));
    }

    /// <summary>
    /// Sets the base URL for the entire SDK.
    /// </summary>
    public void SetBaseUrl(Uri uri)
    {
        _httpClient.SetBaseAddress(uri.EnsureTrailingSlash());
    }

    /// <summary>
    /// Sets the timeout for the entire SDK.
    /// </summary>
    /// <param name="timeout">The timeout value. Must be a positive TimeSpan or Timeout.InfiniteTimeSpan.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the timeout is not valid.</exception>
    public void SetTimeout(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero && timeout != Timeout.InfiniteTimeSpan)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                "Timeout must be a positive value or Timeout.InfiniteTimeSpan."
            );
        }

        _httpClient.SetTimeout(timeout);
    }

    /// <summary>
    /// Sets the API key for the entire SDK.
    /// </summary>
    public void SetApiKey(string apiKey)
    {
        _apiKeyHandler.Token = apiKey;
    }

    /// <summary>
    /// Sets the API key header for the entire SDK.
    /// </summary>
    public void SetApiKeyHeader(string apiKeyHeader)
    {
        _apiKeyHandler.Header = apiKeyHeader;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}

// c029837e0e474b76bc487506e8799df5e3335891efe4fb02bda7a1441840310c
