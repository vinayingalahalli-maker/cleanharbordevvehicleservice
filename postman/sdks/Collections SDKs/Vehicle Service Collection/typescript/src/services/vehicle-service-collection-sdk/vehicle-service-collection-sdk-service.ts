import { z } from 'zod';
import { BaseService } from '../base-service';
import { ContentType, HttpResponse, SdkConfig } from '../../http/types';
import { RequestBuilder } from '../../http/transport/request-builder';
import { SerializationStyle } from '../../http/serialization/base-serializer';
import { ThrowableError } from '../../http/errors/throwable-error';
import { Environment } from '../../http/environment';
import {
  CreateAVehicleRequest,
  createAVehicleRequestRequest,
} from './models/create-a-vehicle-request';

/**
 * Service class for VehicleServiceCollectionSdkService operations.
 * Provides methods to interact with VehicleServiceCollectionSdkService-related API endpoints.
 * All methods return promises and handle request/response serialization automatically.
 */
export class VehicleServiceCollectionSdkService extends BaseService {
  protected createAVehicleConfig?: Partial<SdkConfig>;

  protected retrieveAVehicleConfig?: Partial<SdkConfig>;

  protected updateACollectionConfig?: Partial<SdkConfig>;

  protected deleteAVehicleConfig?: Partial<SdkConfig>;

  protected updateVehicleConfig?: Partial<SdkConfig>;

  protected getAllVehiclesConfig?: Partial<SdkConfig>;

  /**
   * Sets method-level configuration for createAVehicle.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setCreateAVehicleConfig(config: Partial<SdkConfig>): this {
    this.createAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for retrieveAVehicle.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setRetrieveAVehicleConfig(config: Partial<SdkConfig>): this {
    this.retrieveAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for updateACollection.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setUpdateACollectionConfig(config: Partial<SdkConfig>): this {
    this.updateACollectionConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for deleteAVehicle.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setDeleteAVehicleConfig(config: Partial<SdkConfig>): this {
    this.deleteAVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for updateVehicle.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setUpdateVehicleConfig(config: Partial<SdkConfig>): this {
    this.updateVehicleConfig = config;
    return this;
  }

  /**
   * Sets method-level configuration for getAllVehicles.
   * @param config - Partial configuration to override service-level defaults
   * @returns This service instance for method chaining
   */
  setGetAllVehiclesConfig(config: Partial<SdkConfig>): this {
    this.getAllVehiclesConfig = config;
    return this;
  }

  /**
   *
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<any>>} - 201 - Success
   */
  async createAVehicle(
    body: CreateAVehicleRequest,
    requestConfig?: Partial<SdkConfig>,
  ): Promise<any> {
    const resolvedConfig = this.getResolvedConfig(this.createAVehicleConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('POST')
      .setPath('/vehicles')
      .setRequestSchema(createAVehicleRequestRequest)
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.any(),
        contentType: ContentType.Json,
        status: 201,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 400,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 409,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .addHeaderParam({ key: 'Content-Type', value: 'application/json' })
      .addBody(body)
      .build();
    return this.client.callDirect<any>(request);
  }

  /**
   *
   * @param {string} id -
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<any>>} - 200 - Success
   */
  async retrieveAVehicle(id: string, requestConfig?: Partial<SdkConfig>): Promise<any> {
    const resolvedConfig = this.getResolvedConfig(this.retrieveAVehicleConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('GET')
      .setPath('/vehicles/{id}')
      .setRequestSchema(z.any())
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.any(),
        contentType: ContentType.Json,
        status: 200,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 404,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .addPathParam({
        key: 'id',
        value: id,
      })
      .build();
    return this.client.callDirect<any>(request);
  }

  /**
   *
   * @param {string} id -
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<any>>} - 200 - Success
   */
  async updateACollection(
    id: string,
    body: CreateAVehicleRequest,
    requestConfig?: Partial<SdkConfig>,
  ): Promise<any> {
    const resolvedConfig = this.getResolvedConfig(this.updateACollectionConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('PATCH')
      .setPath('/vehicles/{id}')
      .setRequestSchema(createAVehicleRequestRequest)
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.any(),
        contentType: ContentType.Json,
        status: 200,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 400,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .addPathParam({
        key: 'id',
        value: id,
      })
      .addHeaderParam({ key: 'Content-Type', value: 'application/json' })
      .addBody(body)
      .build();
    return this.client.callDirect<any>(request);
  }

  /**
   *
   * @param {string} id -
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<string>>} - 204 - Success
   */
  async deleteAVehicle(id: string, requestConfig?: Partial<SdkConfig>): Promise<string> {
    const resolvedConfig = this.getResolvedConfig(this.deleteAVehicleConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('DELETE')
      .setPath('/vehicles/{id}')
      .setRequestSchema(z.any())
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.string(),
        contentType: ContentType.Json,
        status: 204,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .addPathParam({
        key: 'id',
        value: id,
      })
      .build();
    return this.client.callDirect<string>(request);
  }

  /**
   *
   * @param {string} id -
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<any>>} - Success
   */
  async updateVehicle(
    id: string,
    body: CreateAVehicleRequest,
    requestConfig?: Partial<SdkConfig>,
  ): Promise<any> {
    const resolvedConfig = this.getResolvedConfig(this.updateVehicleConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('PUT')
      .setPath('/vehicles/{id}')
      .setRequestSchema(createAVehicleRequestRequest)
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.any(),
        contentType: ContentType.Json,
        status: 200,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .addPathParam({
        key: 'id',
        value: id,
      })
      .addHeaderParam({ key: 'Content-Type', value: 'application/json' })
      .addBody(body)
      .build();
    return this.client.callDirect<any>(request);
  }

  /**
   *
   * @param {Partial<SdkConfig>} [requestConfig] - The request configuration for retry and validation.
   * @returns {Promise<HttpResponse<any>>} - 200 - Success
   */
  async getAllVehicles(requestConfig?: Partial<SdkConfig>): Promise<any> {
    const resolvedConfig = this.getResolvedConfig(this.getAllVehiclesConfig, requestConfig);
    const request = new RequestBuilder()
      .setConfig(resolvedConfig)
      .setBaseUrl(resolvedConfig)
      .setMethod('GET')
      .setPath('/vehicles')
      .setRequestSchema(z.any())
      .addApiKeyAuth(resolvedConfig?.apiKey, 'X-API-Key', 'header')
      .setRequestContentType(ContentType.Json)
      .addResponse({
        schema: z.any(),
        contentType: ContentType.Json,
        status: 200,
      })
      .addError({
        error: ThrowableError,
        contentType: ContentType.Json,
        status: 500,
      })
      .build();
    return this.client.callDirect<any>(request);
  }
}
