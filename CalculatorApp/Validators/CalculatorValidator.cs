using FluentValidation;
using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace CalculatorApp.Validators;

public class CalculatorValidator : AbstractValidator<Calculator>
{
    public CalculatorValidator()
    {
        RuleFor(x => x.Operand1)
            .NotNull()
            .WithMessage("First operand is required");

        RuleFor(x => x.Operand2)
            .NotNull()
            .WithMessage("Second operand is required")
            .Must((calculator, operand2) =>
                calculator.Operator != CalculatorOperator.Divide || operand2 != 0)
            .WithMessage("Cannot divide by zero");

        RuleFor(x => x.Operator)
            .IsInEnum()
            .WithMessage("Invalid operator");

  
    }
}