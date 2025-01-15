using CalculatorApp.Services;
using FluentValidation;
using Spectre.Console;
using CalculatorApp.UI;
using CalculatorApp.Enums;
using CalculatorApp.Interfaces;


namespace CalculatorApp.Controller;

public class CalculatorController
{
    private readonly ICalculatorDisplay _calculatorDisplay;
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
        _calculatorDisplay = uiService;
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
            Console.Clear();
            var id = _inputService.GetCalculationIdForUpdate();
            var updatedCalc = _calculatorUpdate.GetUpdatedCalculationValues(id);
            if (updatedCalc == null) return;

            _calculatorUpdate.ProcessAndSaveCalculation(updatedCalc);
            _calculatorUpdate.DisplayResults(updatedCalc);
            var choice = _calculatorMenu.ShowMenuAfterUpdate();
            if (choice == UpdateMenuOptions.UpdateAgain)
            {
                UpdateCalculation();
            }
        }
        catch (Exception ex)
        {
            _calculatorDisplay.HandleError(ex);
        }
    }
    
  
    private void DeleteCalculation()
    {
        while (true)
        {
            try
            {
                var calculations = _inputService.GetCalculationHistory();

                var id = _calculatorDelete.GetCalculationIdForDelete();
                if (_calculatorDelete.ConfirmDeletion())
                {
                    try
                    {
                        _calculatorDelete.DeleteCalculation(id);
                        Console.Clear();
                        _calculatorDisplay.ShowMessage("Calculation deleted successfully");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.Clear();
                        _calculatorDisplay.ShowError(ex.Message);
                        _calculatorDisplay.WaitForKeyPress();
                        continue;
                    }
                }
                var choice = _calculatorMenu.ShowMenuAfterDelete();
                if (choice == DeleteMenuOptions.CalculatorMenu)
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                _calculatorDisplay.ShowError(ex.Message);
                _calculatorDisplay.WaitForKeyPress();
            }
        }
    }
    private void PerformCalculation()
    {
        while (true)
        {
            try
            {
                _calculatorDisplay.ClearTable();
                var (operand1, operand2, operatorInput) = _inputService.GetUserInput();

                try
                {
                    var (result, isSquareRoot) = _calculationProcessor.Calculate(operand1, operand2, operatorInput);

                    if (isSquareRoot)
                    {
                        var (firstRoot, secondRoot) = _squareRootCalculator.CalculateRoots(operand1, operand2);
                        _calculatorDisplay.DisplaySquareRootResults(firstRoot, secondRoot);
                        _calculatorDisplay.WaitForKeyPress();
                    }
                    else
                    {
                            _calculatorDisplay.DisplayResult(result);
                        _calculatorDisplay.WaitForKeyPress();
                    }

                    _calculationProcessor.SaveCalculation(operand1, operand2, operatorInput, result);

                    AnsiConsole.WriteLine();
                    var choice = _calculatorMenu.ShowMenuAfterCalc();
                    if (choice == PerformMenuOptions.CalculatorMenu) return;

                    _calculatorDisplay.ClearTable();
                }
                catch (DivideByZeroException)
                {
                    _calculatorDisplay.ShowError("Cannot divide by zero");
                    _calculatorDisplay.WaitForKeyPress();
                    _calculatorDisplay.ClearTable();
                }
            }
            catch (ValidationException ex)
            {
                _calculatorDisplay.ShowError(ex.Message);
                _calculatorDisplay.WaitForKeyPress();
                _calculatorDisplay.ClearTable();
            }
            catch (InvalidOperationException ex)
            {
                _calculatorDisplay.ShowError(ex.Message);
                _calculatorDisplay.WaitForKeyPress();
                _calculatorDisplay.ClearTable();
            }
        }
    }

    private void CalculationHistory()
    {
        var calculations = _inputService.GetCalculationHistory();
        _calculatorDisplay.CalculationHistory(calculations);

        _calculatorDisplay.WaitForKeyPress();
    }
}
