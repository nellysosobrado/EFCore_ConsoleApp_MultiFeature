using ClassLibrary.Enums;
using ClassLibrary.Models;
using CalculatorApp.Services;
using FluentValidation;
using ClassLibrary.Enums.CalculatorAppEnums;
using Spectre.Console;
using CalculatorApp.UI;
using CalculatorApp.Enums;

namespace CalculatorApp.Controllers;

public class CalculatorController
{
    private readonly ICalculatorUIService _uiService;
    private readonly ICalculatorOperationService _operationService;
    private readonly CalculatorMenu _calculatorMenu;
    private readonly CalculationProcessor _calculationProcessor;
    private readonly CalculationInputService _inputService; 
    private readonly SquareRootCalculator _squareRootCalculator;




    public CalculatorController(
        ICalculatorUIService uiService, 
        ICalculatorOperationService operationService,
        CalculatorMenu calculatorMenu,
        CalculationProcessor calculationProcessor,
        CalculationInputService calculationInputService,
        SquareRootCalculator squareRootCalculator)
    {
        _uiService = uiService;
        _operationService = operationService;
        _calculatorMenu = calculatorMenu;
        _calculationProcessor = calculationProcessor;
        _inputService = calculationInputService;
        _squareRootCalculator = squareRootCalculator;
    }

    public void Start()
    {
        while (true)
        {
            var choice = _calculatorMenu.ShowMainMenu();

            switch (choice)
            {
                case CalculatorMenuOptions.Calculate:
                    PerformCalculation();
                    break;

                case CalculatorMenuOptions.History:
                    CalculationHistory();
                    break;

                case CalculatorMenuOptions.UpdateCalculation:
                    UpdateCalculation();
                    break;

                case CalculatorMenuOptions.DeleteCalculation:
                    DeleteCalculation();
                    break;

                case CalculatorMenuOptions.MainMenu:
                    return;
            }
        }
    }
    private void UpdateCalculation()
    {
        try
        {
            var id = _uiService.GetCalculationIdForUpdate();
            var calculation = _calculationProcessor.GetCalculationById(id);

            var currentParameters = new Dictionary<string, double>
            {
                { "First Number", calculation.FirstNumber },
                { "Second Number", calculation.SecondNumber }
            };

            var (updatedParameters, newOperator) = _uiService.GetSelectedParametersToUpdate(currentParameters);

            if (updatedParameters == null)
            {
                return;
            }

            var firstNumber = updatedParameters.TryGetValue("First Number", out var first) ? first : calculation.FirstNumber;
            var secondNumber = updatedParameters.TryGetValue("Second Number", out var second) ? second : calculation.SecondNumber;

            var operatorInput = !string.IsNullOrEmpty(newOperator) ? newOperator : GetOperatorSymbol(calculation.Operator);
            if (!_calculationProcessor.TryParseOperator(operatorInput, out var calculatorOperator))
            {
                throw new InvalidOperationException("Invalid operator");
            }

            var result = _calculationProcessor.Calculate(firstNumber, secondNumber, operatorInput).result;
            _calculationProcessor.UpdateCalculation(id, firstNumber, secondNumber, calculatorOperator, result);

            _uiService.ShowResultSimple(firstNumber, secondNumber, operatorInput, result);
            _uiService.ShowMessage("\n[green]Calculation updated successfully![/]");

            var choice = _calculatorMenu.ShowMenuAfterUpdate();
            if (choice == "Update Calculation")
                UpdateCalculation();
        }
        catch (Exception ex)
        {
            _uiService.ShowError(ex.Message);
            _uiService.WaitForKeyPress();
        }
    }
    private string GetOperatorSymbol(CalculatorOperator op)
    {
        return op switch
        {
            CalculatorOperator.Add => "+",
            CalculatorOperator.Subtract => "-",
            CalculatorOperator.Multiply => "*",
            CalculatorOperator.Divide => "/",
            CalculatorOperator.Modulus => "%",
            CalculatorOperator.SquareRoot => "√",
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
    private void DeleteCalculation()
    {
        while (true)
        {
            try
            {
                var calculations = _operationService.GetCalculationHistory();
                _uiService.CalculationHistory(calculations, showDeleteButton: true);

                var id = _uiService.GetCalculationIdForDelete();

                if (_uiService.ConfirmDeletion())
                {
                    try
                    {
                        _operationService.DeleteCalculation(id);
                        Console.Clear();
                        _uiService.ShowMessage("Calculation deleted successfully");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.Clear();
                        _uiService.ShowError(ex.Message);
                        _uiService.WaitForKeyPress();
                        continue;
                    }
                }

                var choice = _calculatorMenu.ShowMenuAfterDelete();
                switch (choice)
                {
                    case "Back":
                        continue;
                    case "Calculator Menu":
                        return;
                }
            }
            catch (Exception ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
            }
        }
    }
    private void PerformCalculation()
    {
        while (true)
        {
            try
            {
                var (operand1, operand2, operatorInput) = _inputService.GetUserInput();

                try
                {
                    var (result, isSquareRoot) = _calculationProcessor.Calculate(operand1, operand2, operatorInput);

                    if (isSquareRoot)
                    {
                        var (firstRoot, secondRoot) = _squareRootCalculator.CalculateRoots(operand1, operand2);
                        _inputService.DisplaySquareRootResults(firstRoot, secondRoot);
                        _uiService.WaitForKeyPress();
                    }
                    else
                    {
                        _inputService.DisplayResult(result);
                        _uiService.WaitForKeyPress();
                    }

                    _calculationProcessor.SaveCalculation(operand1, operand2, operatorInput, result);

                    AnsiConsole.WriteLine();
                    var choice = _calculatorMenu.ShowMenuAfterCalc();
                    if (choice == "Calculator Menu") return;

                    _inputService.ClearTable();
                }
                catch (DivideByZeroException)
                {
                    _uiService.ShowError("Cannot divide by zero");
                    _uiService.WaitForKeyPress();
                    _inputService.ClearTable();
                }
            }
            catch (ValidationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _inputService.ClearTable();
            }
            catch (InvalidOperationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _inputService.ClearTable();
            }
        }
    }
 
    private void CalculationHistory()
    {
        var calculations = _operationService.GetCalculationHistory();
        _uiService.CalculationHistory(calculations);

        _uiService.WaitForKeyPress();
    }
}
