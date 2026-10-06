package com.example;

import com.vehicleservicecollectionsdk.VehicleServiceCollectionSdk;
import com.vehicleservicecollectionsdk.config.ApiKeyAuthConfig;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;
import com.vehicleservicecollectionsdk.exceptions.ApiError;

public class Main {

  public static void main(String[] args) {
    VehicleServiceCollectionSdkConfig config = VehicleServiceCollectionSdkConfig.builder()
      .apiKeyAuthConfig(ApiKeyAuthConfig.builder().apiKey("YOUR_API_KEY").build())
      .build();

    VehicleServiceCollectionSdk vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk(
      config
    );

    try {
      Object response = vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.getAllVehicles();

      System.out.println(response);
    } catch (ApiError e) {
      e.printStackTrace();
    }

    System.exit(0);
  }
}
