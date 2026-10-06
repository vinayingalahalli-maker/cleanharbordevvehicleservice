import { Environment } from './http/environment';
import { SdkConfig } from './http/types';
import { VehicleServiceCollectionSdkService } from './services/vehicle-service-collection-sdk';

export * from './services/vehicle-service-collection-sdk';

export * from './http';
export { Environment } from './http/environment';

export class VehicleServiceCollectionSdk {
  public readonly vehicleServiceCollectionSdk: VehicleServiceCollectionSdkService;

  constructor(public config: SdkConfig = {}) {
    this.vehicleServiceCollectionSdk = new VehicleServiceCollectionSdkService(this.config);
  }

  set baseUrl(baseUrl: string) {
    this.vehicleServiceCollectionSdk.baseUrl = baseUrl;
  }

  set environment(environment: Environment) {
    this.vehicleServiceCollectionSdk.baseUrl = environment;
  }

  set timeoutMs(timeoutMs: number) {
    this.vehicleServiceCollectionSdk.timeoutMs = timeoutMs;
  }

  set apiKey(apiKey: string) {
    this.vehicleServiceCollectionSdk.apiKey = apiKey;
  }

  set apiKeyHeader(apiKeyHeader: string) {
    this.vehicleServiceCollectionSdk.apiKeyHeader = apiKeyHeader;
  }
}

// c029837e0e474b76bc487506e8799df5e3335891efe4fb02bda7a1441840310c
