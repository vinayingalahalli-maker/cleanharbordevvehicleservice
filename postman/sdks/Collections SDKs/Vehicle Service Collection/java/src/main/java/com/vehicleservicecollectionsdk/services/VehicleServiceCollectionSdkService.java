package com.vehicleservicecollectionsdk.services;

import com.fasterxml.jackson.core.type.TypeReference;
import com.vehicleservicecollectionsdk.config.RequestConfig;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;
import com.vehicleservicecollectionsdk.exceptions.ApiError;
import com.vehicleservicecollectionsdk.http.Environment;
import com.vehicleservicecollectionsdk.http.HttpMethod;
import com.vehicleservicecollectionsdk.http.ModelConverter;
import com.vehicleservicecollectionsdk.http.VehicleServiceCollectionSdkResponse;
import com.vehicleservicecollectionsdk.http.util.RequestBuilder;
import com.vehicleservicecollectionsdk.models.CreateAVehicleRequest;
import java.util.Optional;
import java.util.concurrent.CompletableFuture;
import lombok.NonNull;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

/**
 * VehicleServiceCollectionSdkService Service
 */
public class VehicleServiceCollectionSdkService extends BaseService {

  private RequestConfig createAVehicleConfig;
  private RequestConfig retrieveAVehicleConfig;
  private RequestConfig updateACollectionConfig;
  private RequestConfig deleteAVehicleConfig;
  private RequestConfig updateVehicleConfig;
  private RequestConfig getAllVehiclesConfig;

  /**
   * Constructs a new instance of VehicleServiceCollectionSdkService.
   *
   * @param httpClient The HTTP client to use for requests
   * @param config The SDK configuration
   */
  public VehicleServiceCollectionSdkService(
    @NonNull OkHttpClient httpClient,
    VehicleServiceCollectionSdkConfig config
  ) {
    super(httpClient, config);
  }

  /**
   * Sets method-level configuration for {@code createAVehicle}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setCreateAVehicleConfig(RequestConfig config) {
    this.createAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for {@code retrieveAVehicle}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setRetrieveAVehicleConfig(RequestConfig config) {
    this.retrieveAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for {@code updateACollection}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setUpdateACollectionConfig(RequestConfig config) {
    this.updateACollectionConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for {@code deleteAVehicle}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setDeleteAVehicleConfig(RequestConfig config) {
    this.deleteAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for {@code updateVehicle}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setUpdateVehicleConfig(RequestConfig config) {
    this.updateVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for {@code getAllVehicles}.
   * Method-level overrides take precedence over service-level configuration but are
   * overridden by request-level configurations.
   *
   * @param config The configuration overrides to apply at the method level
   * @return This service instance for method chaining
   */
  public VehicleServiceCollectionSdkService setGetAllVehiclesConfig(RequestConfig config) {
    this.getAllVehiclesConfig = config;
    return this;
  }

  /**
   * Method createAVehicle
   * POST /vehicles
   *
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object createAVehicle(@NonNull CreateAVehicleRequest createAVehicleRequest)
    throws ApiError {
    return this.createAVehicle(createAVehicleRequest, null);
  }

  /**
   * Method createAVehicle
   * POST /vehicles
   *
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object createAVehicle(
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse().createAVehicle(createAVehicleRequest, requestConfig).getData();
  }

  /**
   * Method createAVehicle
   * POST /vehicles
   *
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> createAVehicleAsync(
    @NonNull CreateAVehicleRequest createAVehicleRequest
  ) throws ApiError {
    return this.createAVehicleAsync(createAVehicleRequest, null);
  }

  /**
   * Method createAVehicle
   * POST /vehicles
   *
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> createAVehicleAsync(
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse()
      .createAVehicleAsync(createAVehicleRequest, requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildCreateAVehicleRequest(
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig resolvedConfig
  ) {
    return new RequestBuilder(
      HttpMethod.POST,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .setJsonContent(createAVehicleRequest)
      .build();
  }

  /**
   * Method retrieveAVehicle
   * GET /vehicles/{id}
   *
   * @param id String
   * @return response of {@code Object}
   */
  public Object retrieveAVehicle(@NonNull String id) throws ApiError {
    return this.retrieveAVehicle(id, null);
  }

  /**
   * Method retrieveAVehicle
   * GET /vehicles/{id}
   *
   * @param id String
   * @return response of {@code Object}
   */
  public Object retrieveAVehicle(@NonNull String id, RequestConfig requestConfig) throws ApiError {
    return withRawResponse().retrieveAVehicle(id, requestConfig).getData();
  }

  /**
   * Method retrieveAVehicle
   * GET /vehicles/{id}
   *
   * @param id String
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> retrieveAVehicleAsync(@NonNull String id) throws ApiError {
    return this.retrieveAVehicleAsync(id, null);
  }

  /**
   * Method retrieveAVehicle
   * GET /vehicles/{id}
   *
   * @param id String
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> retrieveAVehicleAsync(
    @NonNull String id,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse()
      .retrieveAVehicleAsync(id, requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildRetrieveAVehicleRequest(@NonNull String id, RequestConfig resolvedConfig) {
    return new RequestBuilder(
      HttpMethod.GET,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles/{id}"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .setPathParameter("id", id)
      .build();
  }

  /**
   * Method updateACollection
   * PATCH /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object updateACollection(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest
  ) throws ApiError {
    return this.updateACollection(id, createAVehicleRequest, null);
  }

  /**
   * Method updateACollection
   * PATCH /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object updateACollection(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse().updateACollection(id, createAVehicleRequest, requestConfig).getData();
  }

  /**
   * Method updateACollection
   * PATCH /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> updateACollectionAsync(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest
  ) throws ApiError {
    return this.updateACollectionAsync(id, createAVehicleRequest, null);
  }

  /**
   * Method updateACollection
   * PATCH /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> updateACollectionAsync(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse()
      .updateACollectionAsync(id, createAVehicleRequest, requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildUpdateACollectionRequest(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig resolvedConfig
  ) {
    return new RequestBuilder(
      HttpMethod.PATCH,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles/{id}"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .setPathParameter("id", id)
      .setJsonContent(createAVehicleRequest)
      .build();
  }

  /**
   * Method deleteAVehicle
   * DELETE /vehicles/{id}
   *
   * @param id String
   * @return response of {@code String}
   */
  public String deleteAVehicle(@NonNull String id) throws ApiError {
    return this.deleteAVehicle(id, null);
  }

  /**
   * Method deleteAVehicle
   * DELETE /vehicles/{id}
   *
   * @param id String
   * @return response of {@code String}
   */
  public String deleteAVehicle(@NonNull String id, RequestConfig requestConfig) throws ApiError {
    return withRawResponse().deleteAVehicle(id, requestConfig).getData();
  }

  /**
   * Method deleteAVehicle
   * DELETE /vehicles/{id}
   *
   * @param id String
   * @return response of {@code CompletableFuture<String>}
   */
  public CompletableFuture<String> deleteAVehicleAsync(@NonNull String id) throws ApiError {
    return this.deleteAVehicleAsync(id, null);
  }

  /**
   * Method deleteAVehicle
   * DELETE /vehicles/{id}
   *
   * @param id String
   * @return response of {@code CompletableFuture<String>}
   */
  public CompletableFuture<String> deleteAVehicleAsync(
    @NonNull String id,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse()
      .deleteAVehicleAsync(id, requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildDeleteAVehicleRequest(@NonNull String id, RequestConfig resolvedConfig) {
    return new RequestBuilder(
      HttpMethod.DELETE,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles/{id}"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .setPathParameter("id", id)
      .build();
  }

  /**
   * Method updateVehicle
   * PUT /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object updateVehicle(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest
  ) throws ApiError {
    return this.updateVehicle(id, createAVehicleRequest, null);
  }

  /**
   * Method updateVehicle
   * PUT /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code Object}
   */
  public Object updateVehicle(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse().updateVehicle(id, createAVehicleRequest, requestConfig).getData();
  }

  /**
   * Method updateVehicle
   * PUT /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> updateVehicleAsync(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest
  ) throws ApiError {
    return this.updateVehicleAsync(id, createAVehicleRequest, null);
  }

  /**
   * Method updateVehicle
   * PUT /vehicles/{id}
   *
   * @param id String
   * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> updateVehicleAsync(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig requestConfig
  ) throws ApiError {
    return withRawResponse()
      .updateVehicleAsync(id, createAVehicleRequest, requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildUpdateVehicleRequest(
    @NonNull String id,
    @NonNull CreateAVehicleRequest createAVehicleRequest,
    RequestConfig resolvedConfig
  ) {
    return new RequestBuilder(
      HttpMethod.PUT,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles/{id}"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .setPathParameter("id", id)
      .setJsonContent(createAVehicleRequest)
      .build();
  }

  /**
   * Method getAllVehicles
   * GET /vehicles
   *
   * @return response of {@code Object}
   */
  public Object getAllVehicles() throws ApiError {
    return this.getAllVehicles(null);
  }

  /**
   * Method getAllVehicles
   * GET /vehicles
   *
   * @return response of {@code Object}
   */
  public Object getAllVehicles(RequestConfig requestConfig) throws ApiError {
    return withRawResponse().getAllVehicles(requestConfig).getData();
  }

  /**
   * Method getAllVehicles
   * GET /vehicles
   *
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> getAllVehiclesAsync() throws ApiError {
    return this.getAllVehiclesAsync(null);
  }

  /**
   * Method getAllVehicles
   * GET /vehicles
   *
   * @return response of {@code CompletableFuture<Object>}
   */
  public CompletableFuture<Object> getAllVehiclesAsync(RequestConfig requestConfig)
    throws ApiError {
    return withRawResponse()
      .getAllVehiclesAsync(requestConfig)
      .thenApply(response -> response.getData());
  }

  private Request buildGetAllVehiclesRequest(RequestConfig resolvedConfig) {
    return new RequestBuilder(
      HttpMethod.GET,
      resolveBaseUrl(resolvedConfig, Environment.DEFAULT),
      "vehicles"
    )
      .setApiKeyAuth(resolveApiKeyAuthConfig(resolvedConfig))
      .build();
  }

  /**
   * Returns an accessor whose methods mirror this service but return the full HTTP response
   * (status code, headers, and raw body) wrapped alongside the parsed data.
   *
   * @return An accessor exposing raw-response variants of this service's methods
   */
  public WithRawResponse withRawResponse() {
    return new WithRawResponse();
  }

  /**
   * Per-call accessor exposing raw-response variants of {@link VehicleServiceCollectionSdkService}'s methods.
   * Reuses the enclosing service's request builders and configuration.
   */
  public class WithRawResponse {

    /**
     * Method createAVehicle
     * POST /vehicles
     *
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> createAVehicle(
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.createAVehicle(createAVehicleRequest, null);
    }

    /**
     * Method createAVehicle
     * POST /vehicles
     *
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> createAVehicle(
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(createAVehicleConfig, requestConfig);
      Request request = buildCreateAVehicleRequest(createAVehicleRequest, resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
      );
    }

    /**
     * Method createAVehicle
     * POST /vehicles
     *
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> createAVehicleAsync(
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.createAVehicleAsync(createAVehicleRequest, null);
    }

    /**
     * Method createAVehicle
     * POST /vehicles
     *
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> createAVehicleAsync(
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(createAVehicleConfig, requestConfig);
      Request request = buildCreateAVehicleRequest(createAVehicleRequest, resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
        );
      });
    }

    /**
     * Method retrieveAVehicle
     * GET /vehicles/{id}
     *
     * @param id String
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> retrieveAVehicle(@NonNull String id)
      throws ApiError {
      return this.retrieveAVehicle(id, null);
    }

    /**
     * Method retrieveAVehicle
     * GET /vehicles/{id}
     *
     * @param id String
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> retrieveAVehicle(
      @NonNull String id,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(retrieveAVehicleConfig, requestConfig);
      Request request = buildRetrieveAVehicleRequest(id, resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
      );
    }

    /**
     * Method retrieveAVehicle
     * GET /vehicles/{id}
     *
     * @param id String
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> retrieveAVehicleAsync(
      @NonNull String id
    ) throws ApiError {
      return this.retrieveAVehicleAsync(id, null);
    }

    /**
     * Method retrieveAVehicle
     * GET /vehicles/{id}
     *
     * @param id String
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> retrieveAVehicleAsync(
      @NonNull String id,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(retrieveAVehicleConfig, requestConfig);
      Request request = buildRetrieveAVehicleRequest(id, resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
        );
      });
    }

    /**
     * Method updateACollection
     * PATCH /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> updateACollection(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.updateACollection(id, createAVehicleRequest, null);
    }

    /**
     * Method updateACollection
     * PATCH /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> updateACollection(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(updateACollectionConfig, requestConfig);
      Request request = buildUpdateACollectionRequest(id, createAVehicleRequest, resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
      );
    }

    /**
     * Method updateACollection
     * PATCH /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> updateACollectionAsync(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.updateACollectionAsync(id, createAVehicleRequest, null);
    }

    /**
     * Method updateACollection
     * PATCH /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> updateACollectionAsync(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(updateACollectionConfig, requestConfig);
      Request request = buildUpdateACollectionRequest(id, createAVehicleRequest, resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
        );
      });
    }

    /**
     * Method deleteAVehicle
     * DELETE /vehicles/{id}
     *
     * @param id String
     * @return response of {@code VehicleServiceCollectionSdkResponse<String>}
     */
    public VehicleServiceCollectionSdkResponse<String> deleteAVehicle(@NonNull String id)
      throws ApiError {
      return this.deleteAVehicle(id, null);
    }

    /**
     * Method deleteAVehicle
     * DELETE /vehicles/{id}
     *
     * @param id String
     * @return response of {@code VehicleServiceCollectionSdkResponse<String>}
     */
    public VehicleServiceCollectionSdkResponse<String> deleteAVehicle(
      @NonNull String id,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(deleteAVehicleConfig, requestConfig);
      Request request = buildDeleteAVehicleRequest(id, resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.toBodyString(bodyBytes)
      );
    }

    /**
     * Method deleteAVehicle
     * DELETE /vehicles/{id}
     *
     * @param id String
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<String>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<String>> deleteAVehicleAsync(
      @NonNull String id
    ) throws ApiError {
      return this.deleteAVehicleAsync(id, null);
    }

    /**
     * Method deleteAVehicle
     * DELETE /vehicles/{id}
     *
     * @param id String
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<String>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<String>> deleteAVehicleAsync(
      @NonNull String id,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(deleteAVehicleConfig, requestConfig);
      Request request = buildDeleteAVehicleRequest(id, resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.toBodyString(bodyBytes)
        );
      });
    }

    /**
     * Method updateVehicle
     * PUT /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> updateVehicle(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.updateVehicle(id, createAVehicleRequest, null);
    }

    /**
     * Method updateVehicle
     * PUT /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> updateVehicle(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(updateVehicleConfig, requestConfig);
      Request request = buildUpdateVehicleRequest(id, createAVehicleRequest, resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
      );
    }

    /**
     * Method updateVehicle
     * PUT /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> updateVehicleAsync(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest
    ) throws ApiError {
      return this.updateVehicleAsync(id, createAVehicleRequest, null);
    }

    /**
     * Method updateVehicle
     * PUT /vehicles/{id}
     *
     * @param id String
     * @param createAVehicleRequest {@link CreateAVehicleRequest} Request Body
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> updateVehicleAsync(
      @NonNull String id,
      @NonNull CreateAVehicleRequest createAVehicleRequest,
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(updateVehicleConfig, requestConfig);
      Request request = buildUpdateVehicleRequest(id, createAVehicleRequest, resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
        );
      });
    }

    /**
     * Method getAllVehicles
     * GET /vehicles
     *
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> getAllVehicles() throws ApiError {
      return this.getAllVehicles(null);
    }

    /**
     * Method getAllVehicles
     * GET /vehicles
     *
     * @return response of {@code VehicleServiceCollectionSdkResponse<Object>}
     */
    public VehicleServiceCollectionSdkResponse<Object> getAllVehicles(RequestConfig requestConfig)
      throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(getAllVehiclesConfig, requestConfig);
      Request request = buildGetAllVehiclesRequest(resolvedConfig);
      Response response = execute(request, resolvedConfig);
      byte[] bodyBytes = ModelConverter.readBytes(response);
      return new VehicleServiceCollectionSdkResponse<>(
        response,
        bodyBytes,
        ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
      );
    }

    /**
     * Method getAllVehicles
     * GET /vehicles
     *
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> getAllVehiclesAsync()
      throws ApiError {
      return this.getAllVehiclesAsync(null);
    }

    /**
     * Method getAllVehicles
     * GET /vehicles
     *
     * @return response of {@code CompletableFuture<VehicleServiceCollectionSdkResponse<Object>>}
     */
    public CompletableFuture<VehicleServiceCollectionSdkResponse<Object>> getAllVehiclesAsync(
      RequestConfig requestConfig
    ) throws ApiError {
      RequestConfig resolvedConfig = getResolvedConfig(getAllVehiclesConfig, requestConfig);
      Request request = buildGetAllVehiclesRequest(resolvedConfig);
      CompletableFuture<Response> futureResponse = executeAsync(request, resolvedConfig);
      return futureResponse.thenApplyAsync(response -> {
        byte[] bodyBytes = ModelConverter.readBytes(response);
        return new VehicleServiceCollectionSdkResponse<>(
          response,
          bodyBytes,
          ModelConverter.convert(bodyBytes, new TypeReference<Object>() {})
        );
      });
    }
  }
}
