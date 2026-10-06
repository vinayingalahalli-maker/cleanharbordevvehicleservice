using VehicleServiceCollectionSdk.Models;
using Environment = VehicleServiceCollectionSdk.Http.Environment;

namespace VehicleServiceCollectionSdk.Config;

/// <summary>
/// Configuration options for the VehicleServiceCollectionSdkClient.
/// </summary>
public record VehicleServiceCollectionSdkConfig(
    /// <value>The environment to use for the SDK.</value>
    Environment? Environment = null,
    /// <value>The api-key authentication configuration.</value>
    ApiKeyAuthConfig? ApiKeyAuth = null
);
