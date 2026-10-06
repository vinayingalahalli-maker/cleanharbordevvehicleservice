using System.Net.Http.Json;
using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Http;
using VehicleServiceCollectionSdk.Http.Exceptions;
using VehicleServiceCollectionSdk.Http.Extensions;
using VehicleServiceCollectionSdk.Http.Handlers;
using VehicleServiceCollectionSdk.Http.Serialization;
using VehicleServiceCollectionSdk.Models;
using VehicleServiceCollectionSdk.Validation;
using VehicleServiceCollectionSdk.Validation.Extensions;

namespace VehicleServiceCollectionSdk.Services;

/// <summary>
/// Service class providing access to API endpoints for VehicleServiceCollectionSdkService.
/// Inherits HTTP client management, JSON serialization, and streaming capabilities from the base service.
/// Each method corresponds to an API operation and handles request building, execution, and response parsing.
/// </summary>
public class VehicleServiceCollectionSdkService : BaseService
{
    private RequestConfig? _createAVehicleAsyncConfig;
    private RequestConfig? _retrieveAVehicleAsyncConfig;
    private RequestConfig? _updateACollectionAsyncConfig;
    private RequestConfig? _deleteAVehicleAsyncConfig;
    private RequestConfig? _updateVehicleAsyncConfig;
    private RequestConfig? _getAllVehiclesAsyncConfig;

    internal VehicleServiceCollectionSdkService(
        global::VehicleServiceCollectionSdk.Http.Client httpClient
    )
        : base(httpClient) { }

    /// <summary>
    /// Sets method-level configuration for <c>CreateAVehicleAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetCreateAVehicleAsyncConfig(RequestConfig config)
    {
        _createAVehicleAsyncConfig = config;
        return this;
    }

    /// <summary>
    /// Sets method-level configuration for <c>RetrieveAVehicleAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetRetrieveAVehicleAsyncConfig(RequestConfig config)
    {
        _retrieveAVehicleAsyncConfig = config;
        return this;
    }

    /// <summary>
    /// Sets method-level configuration for <c>UpdateACollectionAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetUpdateACollectionAsyncConfig(RequestConfig config)
    {
        _updateACollectionAsyncConfig = config;
        return this;
    }

    /// <summary>
    /// Sets method-level configuration for <c>DeleteAVehicleAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetDeleteAVehicleAsyncConfig(RequestConfig config)
    {
        _deleteAVehicleAsyncConfig = config;
        return this;
    }

    /// <summary>
    /// Sets method-level configuration for <c>UpdateVehicleAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetUpdateVehicleAsyncConfig(RequestConfig config)
    {
        _updateVehicleAsyncConfig = config;
        return this;
    }

    /// <summary>
    /// Sets method-level configuration for <c>GetAllVehiclesAsync</c>.
    /// Method-level config overrides service-level config but is overridden by per-request config.
    /// </summary>
    public VehicleServiceCollectionSdkService SetGetAllVehiclesAsyncConfig(RequestConfig config)
    {
        _getAllVehiclesAsyncConfig = config;
        return this;
    }

    public async global::System.Threading.Tasks.Task<object> CreateAVehicleAsync(
        global::VehicleServiceCollectionSdk.Models.CreateAVehicleRequest? input,
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        var validationResults = new List<FluentValidation.Results.ValidationResult> { };
        var validator = new CreateAVehicleRequestValidator();
        var validationResult = validator.Validate(input);
        validationResults.Add(validationResult);

        var combinedFailures = validationResults.SelectMany(result => result.Errors).ToList();
        if (combinedFailures.Any())
        {
            throw new Http.Exceptions.ValidationException(combinedFailures);
        }

        var resolvedConfig = GetResolvedConfig(_createAVehicleAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Post, "vehicles")
            .SetContentAsJson(input, _jsonSerializerOptions)
            .Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        object result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<object>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }

    public async global::System.Threading.Tasks.Task<object> RetrieveAVehicleAsync(
        string id,
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        var validationResults = new List<FluentValidation.Results.ValidationResult> { };
        var idValidationResult = new StringValidator().ValidateRequired<string>(id);
        if (idValidationResult != null)
        {
            validationResults.Add(idValidationResult);
        }

        var combinedFailures = validationResults.SelectMany(result => result.Errors).ToList();
        if (combinedFailures.Any())
        {
            throw new Http.Exceptions.ValidationException(combinedFailures);
        }

        var resolvedConfig = GetResolvedConfig(_retrieveAVehicleAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Get, "vehicles/{id}")
            .SetPathParameter("id", id)
            .Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        object result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<object>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }

    public async global::System.Threading.Tasks.Task<object> UpdateACollectionAsync(
        global::VehicleServiceCollectionSdk.Models.CreateAVehicleRequest? input,
        string id,
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        var validationResults = new List<FluentValidation.Results.ValidationResult> { };
        var idValidationResult = new StringValidator().ValidateRequired<string>(id);
        if (idValidationResult != null)
        {
            validationResults.Add(idValidationResult);
        }
        ;
        var validator = new CreateAVehicleRequestValidator();
        var validationResult = validator.Validate(input);
        validationResults.Add(validationResult);

        var combinedFailures = validationResults.SelectMany(result => result.Errors).ToList();
        if (combinedFailures.Any())
        {
            throw new Http.Exceptions.ValidationException(combinedFailures);
        }

        var resolvedConfig = GetResolvedConfig(_updateACollectionAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Patch, "vehicles/{id}")
            .SetPathParameter("id", id)
            .SetContentAsJson(input, _jsonSerializerOptions)
            .Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        object result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<object>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }

    public async global::System.Threading.Tasks.Task<string> DeleteAVehicleAsync(
        string id,
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        var validationResults = new List<FluentValidation.Results.ValidationResult> { };
        var idValidationResult = new StringValidator().ValidateRequired<string>(id);
        if (idValidationResult != null)
        {
            validationResults.Add(idValidationResult);
        }

        var combinedFailures = validationResults.SelectMany(result => result.Errors).ToList();
        if (combinedFailures.Any())
        {
            throw new Http.Exceptions.ValidationException(combinedFailures);
        }

        var resolvedConfig = GetResolvedConfig(_deleteAVehicleAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Delete, "vehicles/{id}")
            .SetPathParameter("id", id)
            .Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        string result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<string>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }

    public async global::System.Threading.Tasks.Task<object> UpdateVehicleAsync(
        global::VehicleServiceCollectionSdk.Models.CreateAVehicleRequest? input,
        string id,
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        var validationResults = new List<FluentValidation.Results.ValidationResult> { };
        var idValidationResult = new StringValidator().ValidateRequired<string>(id);
        if (idValidationResult != null)
        {
            validationResults.Add(idValidationResult);
        }
        ;
        var validator = new CreateAVehicleRequestValidator();
        var validationResult = validator.Validate(input);
        validationResults.Add(validationResult);

        var combinedFailures = validationResults.SelectMany(result => result.Errors).ToList();
        if (combinedFailures.Any())
        {
            throw new Http.Exceptions.ValidationException(combinedFailures);
        }

        var resolvedConfig = GetResolvedConfig(_updateVehicleAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Put, "vehicles/{id}")
            .SetPathParameter("id", id)
            .SetContentAsJson(input, _jsonSerializerOptions)
            .Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        object result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<object>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }

    public async global::System.Threading.Tasks.Task<object> GetAllVehiclesAsync(
        RequestConfig? requestConfig = null,
        CancellationToken cancellationToken = default
    )
    {
        var resolvedConfig = GetResolvedConfig(_getAllVehiclesAsyncConfig, requestConfig);

        var request = new RequestBuilder(HttpMethod.Get, "vehicles").Build();

        var response = await ExecuteAsync(request, resolvedConfig, cancellationToken)
            .ConfigureAwait(false);

        // Standard deserialization
        var responseContent = response.Content;
        var responseContentLength = responseContent.Headers.ContentLength;

        object result;
        if (responseContentLength == null || responseContentLength > 0)
        {
            result =
                await responseContent
                    .ReadFromJsonAsync<object>(_jsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new Exception("Failed to deserialize response.");
        }
        else
        {
            // Empty response body - return default instance
            result = default!;
        }

        return result;
    }
}
