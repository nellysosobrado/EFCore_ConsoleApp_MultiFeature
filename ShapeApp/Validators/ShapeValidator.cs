using FluentValidation;
using ClassLibrary.Models;

namespace ShapeApp.Validators;

public class ShapeValidator : AbstractValidator<Shape>
{
    public ShapeValidator()
    {
        RuleFor(x => x.ShapeType)
            .NotEmpty()
            .WithMessage("Shape type is required");

        RuleFor(x => x.Parameters)
            .NotNull()
            .WithMessage("Parameters are required")
            .Must(p => p.All(kvp => kvp.Value > 0))
            .WithMessage("All parameters must be greater than 0");

        RuleFor(x => x.Area)
            .GreaterThan(0)
            .WithMessage("Area must be greater than 0");

        RuleFor(x => x.Perimeter)
            .GreaterThan(0)
            .WithMessage("Perimeter must be greater than 0");

        RuleFor(x => x.CalculationDate)
            .NotNull()
            .WithMessage("Calculation date is required")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Calculation date cannot be in the future");
    }
}