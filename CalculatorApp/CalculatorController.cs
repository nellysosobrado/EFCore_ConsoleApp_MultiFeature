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

    public CalculatorController(
        ICalculatorUIService uiService, 
        ICalculatorOperationService operationService)
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
        while(true)
        {
            try
            {
                Console.Clear();
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

                var choice = _uiService.ShowMenuAfterUpdate();
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
                var choice = _uiService.ShowMenuAfterUpdate();
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
                ShowCalculations();
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

                var choice = _uiService.ShowMenuAfterDelete();
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
                var operand1 = _uiService.GetNumberInput("first");
                var operand2 = _uiService.GetNumberInput("second");
                var operatorInput = _uiService.GetOperatorInput();

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
                    Result = Math.Round(result, 2),
                    CalculationDate = DateTime.Now
                };

                _operationService.SaveCalculation(calculation);
                _uiService.ShowResultSimple(operand1, operand2, operatorInput, calculation.Result);

                var choice = _uiService.ShowMenuAfterCalc();
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

    private void ShowCalculations()
    {
        var calculations = _operationService.GetCalculationHistory();
        _uiService.ShowCalculations(calculations);

        _uiService.WaitForKeyPress();
    }
}
