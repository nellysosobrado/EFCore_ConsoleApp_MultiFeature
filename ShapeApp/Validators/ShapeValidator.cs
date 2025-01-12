using FluentValidation;
using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Validators;

public class ShapeValidator : AbstractValidator<Shape>
{
    public ShapeValidator()
    {

        RuleFor(x => x.Area)
            .GreaterThan(0)
            .WithMessage("Area must be greater than 0");

        RuleFor(x => x.Perimeter)
            .GreaterThan(0)
            .WithMessage("Perimeter must be greater than 0");

        When(x => x.ShapeType == ShapeType.Rectangle, () =>
        {
            RuleFor(x => x.Width)
                .NotNull()
                .WithMessage("Width is required for Rectangle")
                .GreaterThan(0)
                .WithMessage("Width must be greater than 0");

            RuleFor(x => x.Height)
                .NotNull()
                .WithMessage("Height is required for Rectangle")
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0");
        });

        When(x => x.ShapeType == ShapeType.Parallelogram, () =>
        {
            RuleFor(x => x.BaseLength)
                .NotNull()
                .WithMessage("Base is required for Parallelogram")
                .GreaterThan(0)
                .WithMessage("Base must be greater than 0");

            RuleFor(x => x.Height)
                .NotNull()
                .WithMessage("Height is required for Parallelogram")
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0");

            RuleFor(x => x.Side)
                .NotNull()
                .WithMessage("Side is required for Parallelogram")
                .GreaterThan(0)
                .WithMessage("Side must be greater than 0");
        });

        When(x => x.ShapeType == ShapeType.Triangle, () =>
        {
            RuleFor(x => x.SideA)
                .NotNull()
                .WithMessage("Side A is required for Triangle")
                .GreaterThan(0)
                .WithMessage("Side A must be greater than 0");

            RuleFor(x => x.SideB)
                .NotNull()
                .WithMessage("Side B is required for Triangle")
                .GreaterThan(0)
                .WithMessage("Side B must be greater than 0");

            RuleFor(x => x.SideC)
                .NotNull()
                .WithMessage("Side C is required for Triangle")
                .GreaterThan(0)
                .WithMessage("Side C must be greater than 0");

            RuleFor(x => x.Height)
                .NotNull()
                .WithMessage("Height is required for Triangle")
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0");
        });

        When(x => x.ShapeType == ShapeType.Rhombus, () =>
        {
            RuleFor(x => x.Side)
                .NotNull()
                .WithMessage("Side is required for Rhombus")
                .GreaterThan(0)
                .WithMessage("Side must be greater than 0");

            RuleFor(x => x.Height)
                .NotNull()
                .WithMessage("Height is required for Rhombus")
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0");
        });
    }
}