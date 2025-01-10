using ClassLibrary.Enums;
using ClassLibrary.Models;
using CalculatorApp.Services;
using FluentValidation;
using ClassLibrary.Enums.CalculatorAppEnums;

namespace CalculatorApp.Controllers;

public class CalculatorController
{
    private readonly ICalculatorUIService _uiService;
    private readonly ICalculatorOperationService _operationService;

    public CalculatorController(ICalculatorUIService uiService, ICalculatorOperationService operationService)
    {
        _uiService = uiService;
        _operationService = operationService;
    }

    public void Start()
    {
        while (true)
        {
            var choice = _uiService.ShowMainMenu();

            switch (choice)
            {
                case "Calculate":
                    PerformCalculation();
                    break;

                case "History":
                    ShowCalculations();
                    break;

                case "Main Menu":
                    return;
            }
        }
    }

    private void PerformCalculation()
    {
        try
        {
            var operand1 = _uiService.GetNumberInput("first");
            var operand2 = _uiService.GetNumberInput("second");
            var operatorInput = _uiService.GetOperatorInput();

            if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
            {
                _uiService.ShowError("Invalid operator");
                return;
            }

            double result;
            try
            {
                result = _operationService.Calculate(operand1, operand2, calculatorOperator);
            }
            catch (DivideByZeroException)
            {
                _uiService.ShowError("Cannot divide by zero");
                return;
            }

            var calculation = new Calculator
            {
                Operand1 = operand1,
                Operand2 = operand2,
                Operator = calculatorOperator,
                Result = Math.Round(result, 2),
                CalculationDate = DateTime.Now
            };

            _operationService.SaveCalculation(calculation);
            _uiService.ShowResult(operand1, operand2, operatorInput, calculation.Result);
        }
        catch (ValidationException ex)
        {
            _uiService.ShowError(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _uiService.ShowError(ex.Message);
        }
        finally
        {
            _uiService.WaitForKeyPress();
        }
    }

    private void ShowCalculations()
    {
        var calculations = _operationService.GetCalculationHistory();
        _uiService.ShowHistory(calculations);
        _uiService.WaitForKeyPress();
    }
}
