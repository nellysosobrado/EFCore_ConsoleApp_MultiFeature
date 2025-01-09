using ClassLibrary.Enums;
using ClassLibrary.Models;
using ClassLibrary.Services;
using CalculatorApp.Validators;
using FluentValidation;

namespace CalculatorApp.Services;

public class CalculatorOperationService : ICalculatorOperationService
{
    private readonly CalculatorService _service;
    private readonly CalculatorValidator _validator;

    public CalculatorOperationService(CalculatorService service)
    {
        _service = service;
        _validator = new CalculatorValidator();
    }

    public bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
    {
        switch (input)
        {
            case "+":
                calculatorOperator = CalculatorOperator.Add;
                return true;
            case "-":
                calculatorOperator = CalculatorOperator.Subtract;
                return true;
            case "*":
                calculatorOperator = CalculatorOperator.Multiply;
                return true;
            case "/":
                calculatorOperator = CalculatorOperator.Divide;
                return true;
            case "%":
                calculatorOperator = CalculatorOperator.Modulus;
                return true;
            default:
                calculatorOperator = default;
                return false;
        }
    }

    public double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator)
    {
        return calculatorOperator switch
        {
            CalculatorOperator.Add => operand1 + operand2,
            CalculatorOperator.Subtract => operand1 - operand2,
            CalculatorOperator.Multiply => operand1 * operand2,
            CalculatorOperator.Divide => operand2 != 0 ? operand1 / operand2 : double.NaN,
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

        _service.AddCalculation(calculation);
    }

    public IEnumerable<Calculator> GetCalculationHistory()
    {
        return _service.GetAllCalculations();
    }
}