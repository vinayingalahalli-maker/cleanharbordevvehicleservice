using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;
using Environment = VehicleServiceCollectionSdk.Http.Environment;

var apiKeyConfig = new ApiKeyAuthConfig("YOUR_API_KEY");

var config = new VehicleServiceCollectionSdkConfig { ApiKeyAuth = apiKeyConfig };

var client = new VehicleServiceCollectionSdkClient(config);

var response = await client.VehicleServiceCollectionSdk.GetAllVehiclesAsync();

Console.WriteLine(response);
