namespace VehicleServiceCollectionSdk.Validation;

using FluentValidation;
using FluentValidation.Results;
using VehicleServiceCollectionSdk.Models;

/// <summary>
/// FluentValidation validator for global::VehicleServiceCollectionSdk.Models.CreateAVehicleRequest model.
/// Defines validation rules for required fields, formats, ranges, and constraints based on the API schema.
/// Automatically validates instances during request serialization and response deserialization.
/// </summary>
public class CreateAVehicleRequestValidator
    : AbstractValidator<global::VehicleServiceCollectionSdk.Models.CreateAVehicleRequest>
{
    public CreateAVehicleRequestValidator() { }
}
