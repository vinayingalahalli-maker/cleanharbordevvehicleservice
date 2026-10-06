namespace VehicleServiceCollectionSdk.Validation;

using FluentValidation;
using FluentValidation.Results;

/// <summary>
/// Fluent validator for nullable floating-point (double) values with support for range and multiple-of constraints.
/// Provides chainable methods for adding validation rules to numeric properties.
/// </summary>
public class FloatValidator : AbstractValidator<double?>
{
    private readonly string _name;

    public FloatValidator(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Adds an exclusive minimum constraint (value must be strictly greater than min). Null values are not validated.
    /// </summary>
    /// <param name="min">The exclusive minimum value.</param>
    /// <returns>This validator instance for method chaining.</returns>
    public FloatValidator WithGreaterThan(double min)
    {
        When(
            num => num != null,
            () =>
            {
                RuleFor(num => num).Must(num => num > min).WithName(_name);
            }
        );
        return this;
    }

    /// <summary>
    /// Adds an inclusive minimum constraint (value must be greater than or equal to min). Null values are not validated.
    /// </summary>
    /// <param name="min">The inclusive minimum value.</param>
    /// <returns>This validator instance for method chaining.</returns>
    public FloatValidator WithGreaterThanOrEqualTo(double min)
    {
        When(
            num => num != null,
            () =>
            {
                RuleFor(num => num).GreaterThanOrEqualTo(min).WithName(_name);
            }
        );
        return this;
    }

    /// <summary>
    /// Adds an exclusive maximum constraint (value must be strictly less than max). Null values are not validated.
    /// </summary>
    /// <param name="max">The exclusive maximum value.</param>
    /// <returns>This validator instance for method chaining.</returns>
    public FloatValidator WithLessThan(double max)
    {
        When(
            num => num != null,
            () =>
            {
                RuleFor(num => num).Must(num => num < max).WithName(_name);
            }
        );
        return this;
    }

    /// <summary>
    /// Adds an inclusive maximum constraint (value must be less than or equal to max). Null values are not validated.
    /// </summary>
    /// <param name="max">The inclusive maximum value.</param>
    /// <returns>This validator instance for method chaining.</returns>
    public FloatValidator WithLessThanOrEqualTo(double max)
    {
        When(
            num => num != null,
            () =>
            {
                RuleFor(num => num).LessThanOrEqualTo(max).WithName(_name);
            }
        );
        return this;
    }

    /// <summary>
    /// Adds a multipleOf constraint (value must be evenly divisible by multipleOf). Null values are not validated.
    /// </summary>
    /// <remarks>
    /// `%` on a double is not the arithmetic remainder: 0.3 % 0.1 is 0.09999999999999998 in IEEE-754,
    /// so an equality test against 0 rejects 0.3, 0.7, 1.0 and 2.4 against multipleOf 0.1. The
    /// quotient is compared to its nearest integer instead, with a tolerance loose enough for the
    /// ~7 significant digits a float-width value carries and far tighter than the gap to the next
    /// multiple.
    /// </remarks>
    /// <param name="multipleOf">The divisor that the value must be a multiple of.</param>
    /// <returns>This validator instance for method chaining.</returns>
    public FloatValidator WithMultipleOf(double multipleOf)
    {
        When(
            num => num != null,
            () =>
            {
                RuleFor(num => num)
                    .Must(value =>
                        value == null
                        || Math.Abs(value.Value / multipleOf - Math.Round(value.Value / multipleOf))
                            < 1e-6
                    )
                    .WithName(_name)
                    .WithMessage($"'{_name}' must be a multiple of {multipleOf}.");
            }
        );
        return this;
    }
}
