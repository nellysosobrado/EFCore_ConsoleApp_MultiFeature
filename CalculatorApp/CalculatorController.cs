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


    public CalculatorController(
        ICalculatorUIService uiService, 
        ICalculatorOperationService operationService,
        CalculatorMenu calculatorMenu)
    {
        _uiService = uiService;
        _operationService = operationService;
        _calculatorMenu = calculatorMenu;
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
        while(true)
        {
            try
            {
                Console.Clear();
                CalculationHistory();
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

                var choice = _calculatorMenu.ShowMenuAfterUpdate();
                switch (choice)
                {
                    case "Update Calculation":
                        UpdateCalculation(); 
                        break;
                    case "Calculator Menu":
                        return; 
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                _uiService.ShowError(ex.Message);
                var choice = _calculatorMenu.ShowMenuAfterUpdate();
                switch (choice)
                {
                    case "Update Calculation":
                        UpdateCalculation();
                        break;
                    case "Calculator Menu":
                        return;
                }
            }
     
        }
      
    }

    private void DeleteCalculation()
    {
        while (true)
        {
            try
            {
                CalculationHistory();
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
                    case "Delete a calculation":
                        DeleteCalculation();
                        break;
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
                Console.Clear();
                var table = new Table().Border(TableBorder.Square);
                table.AddColumns(
                    new TableColumn("First Number").Centered(),
                    new TableColumn("Operator").Centered(),
                    new TableColumn("Second Number").Centered(),
                    new TableColumn("Result").Centered()
                );
                table.AddRow("", "", "", "");

                AnsiConsole.Clear();
                AnsiConsole.Write(table);

                var operand1 = _uiService.GetNumberInput("first");
                table.UpdateCell(0, 0, operand1.ToString());
                AnsiConsole.Clear();
                AnsiConsole.Write(table);

                var operand2 = _uiService.GetNumberInput("second");
                table.UpdateCell(0, 2, operand2.ToString());
                AnsiConsole.Clear();
                AnsiConsole.Write(table);

                var operatorInput = _uiService.GetOperatorInput();
                table.UpdateCell(0, 1, operatorInput);
                AnsiConsole.Clear();
                AnsiConsole.Write(table);

                if (!_operationService.TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
                {
                    _uiService.ShowError("Invalid operator");
                    _uiService.WaitForKeyPress();
                    continue;
                }

                double result;
                try
                {
                    result = _operationService.Calculate(operand1, operand2, calculatorOperator);
                    result = Math.Round(result, 2);
                    table.UpdateCell(0, 3, result.ToString());
                    AnsiConsole.Clear();
                    AnsiConsole.Write(table);
                }
                catch (DivideByZeroException)
                {
                    _uiService.ShowError("Cannot divide by zero");
                    _uiService.WaitForKeyPress();
                    continue;
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

                AnsiConsole.WriteLine();
                var choice = _calculatorMenu.ShowMenuAfterCalc();
                switch (choice)
                {
                    case "New Calculation":
                        continue;
                    case "Calculator Menu":
                        return;
                }
            }
            catch (ValidationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
            }
            catch (InvalidOperationException ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
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
