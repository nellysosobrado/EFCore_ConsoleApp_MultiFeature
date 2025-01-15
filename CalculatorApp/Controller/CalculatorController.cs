using ClassLibrary.Enums;
using ClassLibrary.Models;
using CalculatorApp.Services;
using FluentValidation;
using ClassLibrary.Enums.CalculatorAppEnums;
using Spectre.Console;
using CalculatorApp.UI;
using CalculatorApp.Enums;
using ClassLibrary.Extensions;
using CalculatorApp.Interfaces;


namespace CalculatorApp.Controller;

public class CalculatorController
{
    private readonly ICalculatorUIService _uiService;
    private readonly ICalculatorOperationService _operationService;
    private readonly CalculatorMenu _calculatorMenu;
    private readonly CalculationProcessor _calculationProcessor;
    private readonly ICalculationInputService _inputService;
    private readonly SquareRootCalculator _squareRootCalculator;
    private readonly IDisplayCalculator _displayCalculator;
    private readonly ICalculatorDelete _calculatorDelete;
    private readonly ICalculatorUpdate _calculatorUpdate;




    public CalculatorController(
        ICalculatorUIService uiService,
        ICalculatorOperationService operationService,
        CalculatorMenu calculatorMenu,
        CalculationProcessor calculationProcessor,
        ICalculationInputService calculationInputService,
        SquareRootCalculator squareRootCalculator,
        IDisplayCalculator displayCalculator,
        ICalculatorDelete calculatorDelete,
        ICalculatorUpdate calculatorUpdate


        )
    {
        _uiService = uiService;
        _operationService = operationService;
        _calculatorMenu = calculatorMenu;
        _calculationProcessor = calculationProcessor;
        _inputService = calculationInputService;
        _squareRootCalculator = squareRootCalculator;
        _displayCalculator = displayCalculator;
        _calculatorDelete = calculatorDelete;
        _calculatorUpdate = calculatorUpdate;
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
            var id = _inputService.GetCalculationIdForUpdate();
            var calculation = _inputService.GetCalculationById(id);

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
            _calculatorUpdate.UpdateCalculation(id, firstNumber, secondNumber, calculatorOperator, result);

            _displayCalculator.ShowResultSimple(firstNumber, secondNumber, operatorInput, result);
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
                var calculations = _inputService.GetCalculationHistory();
                _displayCalculator.CalculationHistory(calculations, showDeleteButton: true);

                var id = _calculatorDelete.GetCalculationIdForDelete();
                if (_calculatorDelete.ConfirmDeletion())
                {
                    try
                    {
                        _calculatorDelete.DeleteCalculation(id);
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
                _displayCalculator.ClearTable();
                var (operand1, operand2, operatorInput) = _inputService.GetUserInput();

                try
                {
                    var (result, isSquareRoot) = _calculationProcessor.Calculate(operand1, operand2, operatorInput);

                    if (isSquareRoot)
                    {
                        var (firstRoot, secondRoot) = _squareRootCalculator.CalculateRoots(operand1, operand2);
                        _displayCalculator.DisplaySquareRootResults(firstRoot, secondRoot);
                        _uiService.WaitForKeyPress();
                    }
                    else
                    {
                        _displayCalculator.DisplayResult(result);
                        _uiService.WaitForKeyPress();
                    }

                    _calculationProcessor.SaveCalculation(operand1, operand2, operatorInput, result);

                    AnsiConsole.WriteLine();
                    var choice = _calculatorMenu.ShowMenuAfterCalc();
                    if (choice == "Calculator Menu") return;

                    _displayCalculator.ClearTable();
                }
                catch (DivideByZeroException)
                {
                    _uiService.ShowError("Cannot divide by zero");
                    _uiService.WaitForKeyPress();
                    _displayCalculator.ClearTable();
                }
            }
            catch (ValidationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _displayCalculator.ClearTable();
            }
            catch (InvalidOperationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _displayCalculator.ClearTable();
            }
        }
    }

    private void CalculationHistory()
    {
        var calculations = _inputService.GetCalculationHistory();
        _displayCalculator.CalculationHistory(calculations);

        _uiService.WaitForKeyPress();
    }
}
