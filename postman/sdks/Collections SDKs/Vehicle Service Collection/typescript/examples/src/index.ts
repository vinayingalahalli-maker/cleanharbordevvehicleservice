import { VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.getAllVehicles();

  console.log(data);
})();
