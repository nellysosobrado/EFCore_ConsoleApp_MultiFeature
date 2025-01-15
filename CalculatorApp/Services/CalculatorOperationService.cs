using CalculatorApp.Validators;
using FluentValidation;
using ClassLibrary.Repositories.CalculatorAppRepository;
using CalculatorApp.Interfaces;
using ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;

namespace CalculatorApp.Services;

public class CalculatorOperationService : ICalculatorOperationService
{
    private readonly CalculatorRepository _calculatorRepository;
    private readonly CalculatorValidator _validator;
    private readonly ISquareRootCalculator _squareRootCalculator;

    public CalculatorOperationService(
        CalculatorRepository calculatorRepository,
        ISquareRootCalculator squareRootCalculator,
        CalculatorValidator calculatorValidator
        )
    {
        _validator = calculatorValidator;
        _calculatorRepository = calculatorRepository;
        _squareRootCalculator = squareRootCalculator;
    }

    public double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator)
    {
        if (calculatorOperator == CalculatorOperator.Divide && operand2 == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero");
        }

        if (calculatorOperator == CalculatorOperator.Modulus && operand2 == 0)
        {
            throw new DivideByZeroException("Cannot calculate modulus with zero");
        }

        if (calculatorOperator == CalculatorOperator.SquareRoot)
        {
            var results = _squareRootCalculator.CalculateSquareRoots(operand1, operand2);
            return results?.firstResult ?? 0;
        }

        return calculatorOperator switch
        {
            CalculatorOperator.Add => operand1 + operand2,
            CalculatorOperator.Subtract => operand1 - operand2,
            CalculatorOperator.Multiply => operand1 * operand2,
            CalculatorOperator.Divide => operand1 / operand2,
            CalculatorOperator.Modulus => operand1 % operand2,
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }

    public void SaveCalculation(Calculator calculation)
    {
        var validationResult = _validator.Validate(calculation);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        if (double.IsInfinity(calculation.Result) || double.IsNaN(calculation.Result))
        {
            throw new InvalidOperationException("Result is invalid (Infinity or NaN)");
        }

        _calculatorRepository.AddCalculation(calculation);
    }
  
}