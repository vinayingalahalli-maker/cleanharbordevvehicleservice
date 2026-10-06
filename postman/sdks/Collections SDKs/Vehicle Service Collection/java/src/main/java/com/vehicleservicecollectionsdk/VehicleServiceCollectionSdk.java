package com.vehicleservicecollectionsdk;

import com.vehicleservicecollectionsdk.config.ApiKeyAuthConfig;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;
import com.vehicleservicecollectionsdk.http.Environment;
import com.vehicleservicecollectionsdk.http.interceptors.DefaultHeadersInterceptor;
import com.vehicleservicecollectionsdk.http.interceptors.LoggingInterceptor;
import com.vehicleservicecollectionsdk.http.interceptors.RetryInterceptor;
import com.vehicleservicecollectionsdk.logging.Logger;
import com.vehicleservicecollectionsdk.services.VehicleServiceCollectionSdkService;
import java.util.concurrent.TimeUnit;
import okhttp3.OkHttpClient;

/**
 * This collection covers the core CRUD operations for the **Vehicle Service API**, providing a complete set of endpoints to create, retrieve, update, and delete vehicle resources.
 *
 * ### Endpoints
 *
 * | Method | Endpoint | Description |
 * |--------|----------|-------------|
 * | `POST` | `/vehicles` | Create a new vehicle |
 * | `GET` | `/vehicles/:id` | Retrieve a vehicle by its ID |
 * | `PATCH` | `/vehicles/:id` | Partially update an existing vehicle |
 * | `PUT` | `/vehicles/:id` | Fully replace an existing vehicle |
 * | `DELETE` | `/vehicles/:id` | Delete a vehicle by its ID |
 *
 * ### Response Coverage
 *
 * Each request includes saved examples that cover both success and error scenarios:
 *
 * - **2xx** — `200 Success`, `201 Created`, `204 No Content`
 * - **4xx** — `400 Missing/Bad Request`, `404 Not Found`, `409 Conflict (Vehicle already exists)`
 * - **5xx** — `500 Unexpected Error`
 *
 * This makes the collection well-suited for both active development and mock server usage.
 *
 * ### Configuration
 *
 * All requests use the `{{baseUrl}}` variable. Make sure your active environment has `baseUrl` set to the correct server URL before sending requests.
 */
public class VehicleServiceCollectionSdk {

  public final VehicleServiceCollectionSdkService vehicleServiceCollectionSdk;

  private final VehicleServiceCollectionSdkConfig config;

  /**
   * Constructs a new instance of VehicleServiceCollectionSdk with default configuration.
   */
  public VehicleServiceCollectionSdk() {
    // Default configs
    this(VehicleServiceCollectionSdkConfig.builder().build());
  }

  /**
   * Constructs a new instance of VehicleServiceCollectionSdk with custom configuration.
   * Initializes all services, HTTP client, and optional OAuth token manager.
   *
   * @param config The SDK configuration including base URL, authentication, timeout, and retry settings
   */
  public VehicleServiceCollectionSdk(VehicleServiceCollectionSdkConfig config) {
    this.config = config;

    // A user-supplied client is augmented (not replaced): the SDK derives its client from
    // the injected instance so its transport settings and interceptors are preserved, then
    // layers the SDK's own interceptors on top.
    final OkHttpClient customHttpClient = config.getHttpClient();
    final OkHttpClient.Builder httpClientBuilder =
      (customHttpClient != null
          ? customHttpClient.newBuilder()
          : new OkHttpClient.Builder()).addInterceptor(new DefaultHeadersInterceptor(config))
        .addInterceptor(new RetryInterceptor(config.getRetryConfig()))
        // Logging is added last so it observes the fully-decorated request (auth headers
        // included, then redacted). Silent by default — see LogConfig.
        .addInterceptor(new LoggingInterceptor(Logger.from(config.getLogConfig())));

    // Only apply the SDK's default read timeout when building the client ourselves; a
    // user-supplied client owns its own transport (timeout) settings.
    if (customHttpClient == null) {
      httpClientBuilder.readTimeout(config.getTimeout(), TimeUnit.MILLISECONDS);
    }

    final OkHttpClient httpClient = httpClientBuilder.build();

    this.vehicleServiceCollectionSdk = new VehicleServiceCollectionSdkService(httpClient, config);
  }

  /**
   * Sets the environment for all API requests.
   *
   * @param environment The environment to use (e.g., DEFAULT, PRODUCTION, STAGING)
   */
  public void setEnvironment(Environment environment) {
    setBaseUrl(environment.getUrl());
  }

  /**
   * Sets the base URL for all API requests.
   *
   * @param baseUrl The base URL to use for API requests
   */
  public void setBaseUrl(String baseUrl) {
    this.config.setBaseUrl(baseUrl);
  }

  /**
   * Sets the API key for all API requests.
   *
   * @param apiKey The API key to use for authentication
   */
  public void setApiKey(String apiKey) {
    ApiKeyAuthConfig apiKeyAuthConfig = this.config.getApiKeyAuthConfig();
    apiKeyAuthConfig.setApiKey(apiKey);
  }

  /**
   * Sets the API key header name for all API requests.
   *
   * @param apiKeyHeader The header name to use for the API key
   */
  public void setApiKeyHeader(String apiKeyHeader) {
    ApiKeyAuthConfig apiKeyAuthConfig = this.config.getApiKeyAuthConfig();
    apiKeyAuthConfig.setApiKeyHeader(apiKeyHeader);
  }
}
// c029837e0e474b76bc487506e8799df5e3335891efe4fb02bda7a1441840310c
