using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Enums.CalculatorAppEnums;

namespace CalculatorApp.Services;

public class CalculationProcessor
{
    private readonly ICalculatorOperationService _operationService;

    public CalculationProcessor(ICalculatorOperationService operationService)
    {
        _operationService = operationService;
    }

    public (double result, bool isSquareRoot) Calculate(double operand1, double operand2, string operatorInput)
    {
        if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
        {
            throw new InvalidOperationException("Invalid operator");
        }

        var result = _operationService.Calculate(operand1, operand2, calculatorOperator);
        return (Math.Round(result, 2), calculatorOperator == CalculatorOperator.SquareRoot);
    }

    public void SaveCalculation(double operand1, double operand2, string operatorInput, double result)
    {
        if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
        {
            throw new InvalidOperationException("Invalid operator");
        }

        var calculation = new Calculator
        {
            FirstNumber = operand1,
            SecondNumber = operand2,
            Operator = calculatorOperator,
            Result = result,
            CalculationDate = DateTime.Now
        };

        _operationService.SaveCalculation(calculation);
    }

    public Calculator GetCalculationById(int id)
    {
        return _operationService.GetCalculationById(id);
    }

    public bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
    {
        return _operationService.TryParseOperator(input, out calculatorOperator);
    }

    public void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator, double result)
    {
        _operationService.UpdateCalculation(id, operand1, operand2, calculatorOperator);
    }
}