using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Enums.CalculatorAppEnums;
using CalculatorApp.Interfaces;

namespace CalculatorApp.Services;

public class CalculationProcessor
{
    private readonly ICalculatorParser _calculatorParser;
    private readonly CalculatorOperationService _calculatorOperationService;


    public CalculationProcessor(
        ICalculatorParser calculatorParser, CalculatorOperationService calculatorOperationService)
    {
        _calculatorParser = calculatorParser;
        _calculatorOperationService = calculatorOperationService;
    }

    public (double result, bool isSquareRoot) Calculate(double operand1, double operand2, string operatorInput)
    {
        if (!_calculatorParser.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
        {
            throw new InvalidOperationException("Invalid operator");
        }

        var result = _calculatorOperationService.Calculate(operand1, operand2, calculatorOperator);
        return (Math.Round(result, 2), calculatorOperator == CalculatorOperator.SquareRoot);
    }

    public void SaveCalculation(double operand1, double operand2, string operatorInput, double result)
    {
        if (!_calculatorParser.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
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

        _calculatorOperationService.SaveCalculation(calculation);
    }

   

    public bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
    {
        return _calculatorParser.TryParseOperator(input, out calculatorOperator);
    }

   
}