using CalculatorApp.Interfaces;
using CalculatorApp.Validators;
using ClassLibrary.Enums.CalculatorAppEnums;
using ClassLibrary.Extensions;
using ClassLibrary.Repositories.CalculatorAppRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation;
using Spectre.Console;
using CalculatorApp.UI;
using CalculatorApp.Enums;



namespace CalculatorApp.Services
{
    public class CalculatorUpdateService : ICalculatorUpdate
    {
        private readonly CalculatorRepository _calculatorRepository;    
        private readonly CalculatorValidator _validator;    
        private readonly CalculatorOperationService _calculatorOperationService;
        private readonly ICalculatorDisplay _calculatorDisplay;
        private readonly ICalculatorParser _calculatorParser;
        private bool _operatorChanged = false;
        private string _newOperator = string.Empty;


        public CalculatorUpdateService(CalculatorRepository calculatorRepository, 
            CalculatorValidator validator, 
            CalculatorOperationService calculatorOperationService,
            ICalculatorParser calculatorParser,
            ICalculatorDisplay calculatorUIService)
        {
            _calculatorRepository = calculatorRepository;
            _validator = validator;
            _calculatorOperationService = calculatorOperationService;
            _calculatorDisplay = calculatorUIService;
            _calculatorParser = calculatorParser;
        }
        public Dictionary<string, double> GetSelectedInputsToUpdate(Dictionary<string, double> currentInputs)
        {
            var updatedInputs = new Dictionary<string, double>();
            var inputs = currentInputs.Keys.ToList();
            inputs.Add("[green]Confirm[/]");
            inputs.Add("[red]Cancel[/]");

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select input to update:[/]")
                        .AddChoices(inputs));

                if (choice == "[green]Confirm[/]")
                    return updatedInputs;

                if (choice == "[red]Cancel[/]")
                    return currentInputs;

                updatedInputs[choice] = _calculatorDisplay.GetNumberInput(choice);
            }
        }
        public (Dictionary<string, double> parameters, string newOperator) GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters)
        {
            var updatedParameters = new Dictionary<string, double>();
            var parameters = currentParameters.Keys.ToList();
            if (!_operatorChanged) parameters.Add("Change Operator");
            parameters.Add("[green]Confirm[/]");
            parameters.Add("[red]Cancel[/]");

            while (true)
            {
                AnsiConsole.Clear();
                _calculatorDisplay.ShowCurrentParameters(currentParameters, updatedParameters);
                if (_operatorChanged)
                {
                    AnsiConsole.MarkupLine($"\n[blue]New operator:[/] {_newOperator}");
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select parameter to update:[/]")
                        .AddChoices(parameters));

                if (choice == "[green]Confirm[/]")
                {
                    var returnOperator = _operatorChanged ? _newOperator : string.Empty;
                    _operatorChanged = false;
                    _newOperator = string.Empty;
                    return (updatedParameters.Count > 0 ? updatedParameters : currentParameters, returnOperator);
                }

                if (choice == "[red]Cancel[/]")
                {
                    _operatorChanged = false;
                    _newOperator = string.Empty;
                    return (null, string.Empty);
                }

                if (choice == "Change Operator")
                {
                    _newOperator = _calculatorDisplay.GetOperatorInput();
                    _operatorChanged = true;
                    parameters.Remove("Change Operator");
                    continue;
                }

                var newValue = _calculatorDisplay.GetNumberInput($"Enter new value for {choice}");
                updatedParameters[choice] = newValue;
            }
        }

        public void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator)
        {
            var existingCalculation = _calculatorRepository.GetCalculationById(id);

            var result = _calculatorOperationService.Calculate(operand1, operand2, calculatorOperator);

            var updatedCalculation = new Calculator
            {
                Id = id,
                FirstNumber = operand1,
                SecondNumber = operand2,
                Operator = calculatorOperator,
                Result = Math.Round(result, 2),
                CalculationDate = DateTime.Now
            };

            var validationResult = _validator.Validate(updatedCalculation);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            _calculatorRepository.UpdateCalculation(updatedCalculation);
        }
        public void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator, double result)
        {
            UpdateCalculation(id, operand1, operand2, calculatorOperator);
        }
        //
        public Calculator GetUpdatedCalculationValues(int id)
        {
            var calculation = _calculatorRepository.GetCalculationById(id);

            var currentParameters = new Dictionary<string, double>
    {
        { "First Number", calculation.FirstNumber },
        { "Second Number", calculation.SecondNumber }
    };

            var (updatedParameters, newOperator) = GetSelectedParametersToUpdate(currentParameters);
            if (updatedParameters == null) return null;

            calculation.FirstNumber = updatedParameters.TryGetValue("First Number", out var first) ? first : calculation.FirstNumber;
            calculation.SecondNumber = updatedParameters.TryGetValue("Second Number", out var second) ? second : calculation.SecondNumber;

            if (!string.IsNullOrEmpty(newOperator))
            {
                if (!_calculatorParser.TryParseOperator(newOperator, out var calculatorOperator))
                {
                    throw new InvalidOperationException("Invalid operator");
                }
                calculation.Operator = calculatorOperator;
            }

            return calculation;
        }

        public void ProcessAndSaveCalculation(Calculator calc)
        {
            var operatorSymbol = _calculatorDisplay.GetOperatorSymbol(calc.Operator);
            var result = _calculatorOperationService.Calculate(calc.FirstNumber, calc.SecondNumber, calc.Operator); 
            calc.Result = result;
            calc.CalculationDate = DateTime.Now;
            UpdateCalculation(calc.Id, calc.FirstNumber, calc.SecondNumber, calc.Operator, calc.Result);
        }


        public void DisplayResults(Calculator calc)
        {
            var operatorSymbol = _calculatorDisplay.GetOperatorSymbol(calc.Operator);
            _calculatorDisplay.ShowResultSimple(calc.FirstNumber, calc.SecondNumber, operatorSymbol, calc.Result);
            _calculatorDisplay.ShowMessage("\n[green]Calculation updated successfully![/]");
        }
       


    }
}
