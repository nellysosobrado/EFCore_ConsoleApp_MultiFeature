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
    private readonly ICalculatorDisplay _uiService;
    private readonly CalculatorMenu _calculatorMenu;
    private readonly CalculationProcessor _calculationProcessor;
    private readonly ICalculationInputService _inputService;
    private readonly ISquareRootCalculator _squareRootCalculator;
    private readonly ICalculatorDelete _calculatorDelete;
    private readonly ICalculatorUpdate _calculatorUpdate;




    public CalculatorController(
        ICalculatorDisplay uiService,
        ICalculatorOperationService operationService,
        CalculatorMenu calculatorMenu,
        CalculationProcessor calculationProcessor,
        ICalculationInputService calculationInputService,
        ISquareRootCalculator squareRootCalculator,
        ICalculatorDelete calculatorDelete,
        ICalculatorUpdate calculatorUpdate


        )
    {
        _uiService = uiService;
        _calculatorMenu = calculatorMenu;
        _calculationProcessor = calculationProcessor;
        _inputService = calculationInputService;
        _squareRootCalculator = squareRootCalculator;
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
            var updatedCalc = _calculatorUpdate.GetUpdatedCalculationValues(id);
            if (updatedCalc == null) return;

            _calculatorUpdate.ProcessAndSaveCalculation(updatedCalc);
            _calculatorUpdate.DisplayResults(updatedCalc);
            HandleAfterUpdateChoice();
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }
    private void HandleAfterUpdateChoice()
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<CalculatorMenuOptions>()
                .Title("[yellow]What would you like to do next?[/]")
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<CalculatorMenuOptions>()));

        switch (choice)
        {
            case CalculatorMenuOptions.Calculate:
                UpdateCalculation();
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
            default:
                _uiService.ShowError("Invalid choice. Returning to calculator menu...");
                break;
        }
    }

    private void HandleError(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"An error occurred: {ex.Message}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        _calculatorMenu.ShowMainMenu();
    }

    //private string GetOperatorSymbol(CalculatorOperator op)
    //{
    //    return op switch
    //    {
    //        CalculatorOperator.Add => "+",
    //        CalculatorOperator.Subtract => "-",
    //        CalculatorOperator.Multiply => "*",
    //        CalculatorOperator.Divide => "/",
    //        CalculatorOperator.Modulus => "%",
    //        CalculatorOperator.SquareRoot => "√",
    //        _ => throw new InvalidOperationException("Invalid operator")
    //    };
    //}
    private void DeleteCalculation()
    {
        while (true)
        {
            try
            {
                var calculations = _inputService.GetCalculationHistory();
                _uiService.CalculationHistory(calculations, showDeleteButton: true);

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
                _uiService.ClearTable();
                var (operand1, operand2, operatorInput) = _inputService.GetUserInput();

                try
                {
                    var (result, isSquareRoot) = _calculationProcessor.Calculate(operand1, operand2, operatorInput);

                    if (isSquareRoot)
                    {
                        var (firstRoot, secondRoot) = _squareRootCalculator.CalculateRoots(operand1, operand2);
                        _uiService.DisplaySquareRootResults(firstRoot, secondRoot);
                        _uiService.WaitForKeyPress();
                    }
                    else
                    {
                            _uiService.DisplayResult(result);
                        _uiService.WaitForKeyPress();
                    }

                    _calculationProcessor.SaveCalculation(operand1, operand2, operatorInput, result);

                    AnsiConsole.WriteLine();
                    var choice = _calculatorMenu.ShowMenuAfterCalc();
                    if (choice == "Calculator Menu") return;

                    _uiService.ClearTable();
                }
                catch (DivideByZeroException)
                {
                    _uiService.ShowError("Cannot divide by zero");
                    _uiService.WaitForKeyPress();
                    _uiService.ClearTable();
                }
            }
            catch (ValidationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _uiService.ClearTable();
            }
            catch (InvalidOperationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
                _uiService.ClearTable();
            }
        }
    }

    private void CalculationHistory()
    {
        var calculations = _inputService.GetCalculationHistory();
        _uiService.CalculationHistory(calculations);

        _uiService.WaitForKeyPress();
    }
}
