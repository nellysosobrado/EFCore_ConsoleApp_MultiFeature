using ClassLibrary.Enums;
using ClassLibrary.Models;
using CalculatorApp.Services;
using FluentValidation;
using ClassLibrary.Enums.CalculatorAppEnums;
using Spectre.Console;

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

                case "Update Calculation":
                    UpdateCalculation();
                    break;

                case "Delete Calculation":
                    DeleteCalculation();
                    break;

                case "Main Menu":
                    return;
            }
        }
    }

    private void UpdateCalculation()
    {
        try
        {
            ShowCalculations();
            var id = _uiService.GetCalculationIdForUpdate();

            var operand1 = _uiService.GetNumberInput("first");
            var operand2 = _uiService.GetNumberInput("second");
            var operatorInput = _uiService.GetOperatorInput();

            if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
            {
                _uiService.ShowError("Invalid operator");
                return;
            }

            _operationService.UpdateCalculation(id, operand1, operand2, calculatorOperator);
            _uiService.ShowResult(operand1, operand2, operatorInput,
                _operationService.Calculate(operand1, operand2, calculatorOperator));
        }
        catch (Exception ex)
        {
            _uiService.ShowError(ex.Message);
        }
        finally
        {
            _uiService.WaitForKeyPress();
        }
    }

    private void DeleteCalculation()
    {
        try
        {
            ShowCalculations();
            var id = _uiService.GetCalculationIdForDelete();

            if (_uiService.ConfirmDeletion())
            {
                _operationService.DeleteCalculation(id);
                _uiService.ShowResult("Calculation deleted successfully");
            }
        }
        catch (Exception ex)
        {
            _uiService.ShowError(ex.Message);
        }
        finally
        {
            _uiService.WaitForKeyPress();
        }
    }
    private void PerformCalculation()
    {
        while (true)
        {
            try
            {
                var operand1 = _uiService.GetNumberInput("first");
                var operand2 = _uiService.GetNumberInput("second");
                var operatorInput = _uiService.GetOperatorInput();

                if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
                {
                    _uiService.ShowError("Invalid operator");
                    continue;
                }

                double result;
                try
                {
                    result = _operationService.Calculate(operand1, operand2, calculatorOperator);
                }
                catch (DivideByZeroException)
                {
                    _uiService.ShowError("Cannot divide by zero");
                    continue;
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

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]What would you like to do next?[/]")
                        .AddChoices(new[]
                        {
                        "New Calculation",
                        "Calculator Menu",
                        "Main Menu"
                        }));

                switch (choice)
                {
                    case "New Calculation":
                        continue;
                    case "Calculator Menu":
                        return;
                    case "Main Menu":
                        Start();
                        return;
                }
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
    }

    private void ShowCalculations()
    {
        var calculations = _operationService.GetCalculationHistory();
        _uiService.ShowHistory(calculations);
        _uiService.WaitForKeyPress();
    }
}
