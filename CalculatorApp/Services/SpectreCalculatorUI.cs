using Spectre.Console;
using ClassLibrary.Models;
using CalculatorApp.Validators;
using System.Globalization;
using ClassLibrary.Enums.CalculatorAppEnums;
using CalculatorApp.Enums;
using CalculatorApp.Enums;
using ClassLibrary.Extensions;


using CalculatorApp.UI;

namespace CalculatorApp.Services;

public class SpectreCalculatorUI : ICalculatorUIService
{
    private const int PageSize = 10;
    private bool _showDeleteButton;
    private readonly InputValidator _inputValidator;
    private readonly CalculatorMenu _calculatorMenu;
    private bool _operatorChanged = false;
    private string _newOperator = string.Empty;

    public SpectreCalculatorUI(InputValidator inputValidator, CalculatorMenu calculatorMenu)
    {
        _inputValidator = inputValidator;
        _calculatorMenu = calculatorMenu;
    }

   

    public void ShowMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
    }


    public double GetNumberInput(string prompt)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>($"Enter the [green]{prompt}[/] number:");

            var validationResult = _inputValidator.Validate(input);
            if (validationResult.IsValid)
            {
                return double.Parse(input, CultureInfo.InvariantCulture);
            }

            AnsiConsole.MarkupLine($"[red]{validationResult.Errors[0].ErrorMessage}[/]");
        }
    }

    public string GetOperatorInput()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an [green]operator[/]")
                .PageSize(6)
                .AddChoices(new[] { "+", "-", "*", "/", "%", "√" }));
    }
    public void ShowResult(double operand1, double operand2, string operatorSymbol, double result, bool isDeleted = false)
    {
        Console.Clear();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[italic green]\nResult[/]")
            .AddColumn("Expression")
            .AddColumn("Result")
            .AddColumn("Status");

        if (operatorSymbol == "√")
        {
            table.AddRow(
                $"√{operand1}",
                $"{Math.Round(result, 2)}",
                isDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]"
            );
            table.AddRow(
                $"√{operand2}",
                $"{Math.Round(Math.Sqrt(operand2), 2)}",
                isDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]"
            );
        }
        else
        {
            table.AddRow(
                $"{operand1} {operatorSymbol} {operand2}",
                $"{Math.Round(result, 2)}",
                isDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]"
            );
        }
        AnsiConsole.Write(table);
    }
    public void ShowResultSimple(double operand1, double operand2, string operatorSymbol, double result)
    {
        Console.Clear();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[italic green]\nResult[/]")
            .AddColumn("Calculation")
            .AddColumn("Result");

        if (operatorSymbol == "√")
        {
            table.AddRow(
                $"√{operand1}",
                $"{Math.Round(result, 2)}"
            );
            table.AddRow(
                $"√{operand2}",
                $"{Math.Round(Math.Sqrt(operand2), 2)}"
            );
        }
        else
        {
            table.AddRow(
                $"{operand1} {operatorSymbol} {operand2}",
                $"{Math.Round(result, 2)}"
            );
        }
        AnsiConsole.Write(table);
    }
    
    public void CalculationHistory(IEnumerable<Calculator> calculations, bool showDeleteButton = false)
    {
        _showDeleteButton = showDeleteButton;
        var allCalculations = calculations.ToList();
        var totalPages = (int)Math.Ceiling(allCalculations.Count / (double)PageSize);
        var currentPage = 1;

        while (true)
        {
            AnsiConsole.Clear();
            DisplayCalculationsPage(allCalculations, currentPage);

            if (totalPages <= 1 && !_showDeleteButton)
            {
                WaitForKeyPress("\nPress any key to return to menu...");
                break;
            }

            var choices = new List<string> { "Search by ID" };
            if (_showDeleteButton) choices.Add("[red]Delete Calculation[/]");
            if (currentPage > 1) choices.Add("Previous Page");
            if (currentPage < totalPages) choices.Add("Next Page");
            choices.Add("Return to Menu");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"\n[blue]Page {currentPage}/{totalPages}[/]")
                    .AddChoices(choices));

            switch (choice)
            {
                case "Search by ID":
                    SearchById(allCalculations);
                    break;
                case "[red]Delete Calculation[/]":
                        return;
                case "Previous Page":
                    currentPage--;
                    break;
                case "Next Page":
                    currentPage++;
                    break;
                case "Return to Menu":
                    return;
            }
        }
    }

    private void SearchById(List<Calculator> calculations)
    {
        AnsiConsole.Clear();
        var id = AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter calculation ID:[/]")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Please enter a valid number[/]"));

        var calculation = calculations.FirstOrDefault(c => c.Id == id);

        if (calculation == null)
        {
            AnsiConsole.MarkupLine("[red]No calculation found with that ID[/]");
            WaitForKeyPress();
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[green]Created[/]").Centered())
            .AddColumn(new TableColumn("[blue]Calculation[/]").Centered())
            .AddColumn(new TableColumn("[magenta]Result[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Status[/]").Centered())
            .AddColumn(new TableColumn("[red]Deleted At[/]").Centered());

        string expression;
        if (calculation.Operator == CalculatorOperator.SquareRoot)
        {
            var secondResult = Math.Sqrt(calculation.SecondNumber);
            expression = $"√{calculation.FirstNumber}, √{calculation.SecondNumber}";
            table.AddRow(
                $"[white]{calculation.Id}[/]",
                $"[white]{calculation.CalculationDate}[/]",
                $"[white]{expression}[/]",
                $"[white]{calculation.Result}, {Math.Round(secondResult, 2)}[/]",
                calculation.IsDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]",
                calculation.IsDeleted ? $"[white]{calculation.DeletedAt}[/]" : "-"
            );
        }
        else
        {
            expression = $"{calculation.FirstNumber} {GetOperatorSymbol(calculation.Operator)} {calculation.SecondNumber}";
            table.AddRow(
                $"[white]{calculation.Id}[/]",
                $"[white]{calculation.CalculationDate}[/]",
                $"[white]{expression}[/]",
                $"[white]{calculation.Result}[/]",
                calculation.IsDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]",
                calculation.IsDeleted ? $"[white]{calculation.DeletedAt}[/]" : "-"
            );
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
        WaitForKeyPress();
    }

    private void DisplayCalculationsPage(List<Calculator> calculations, int page)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[green]Created[/]").Centered())
            .AddColumn(new TableColumn("[blue]Calculation[/]").Centered())
            .AddColumn(new TableColumn("[magenta]Result[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Status[/]").Centered())
            .AddColumn(new TableColumn("[red]Deleted At[/]").Centered());

        var pageCalculations = calculations
            .Skip((page - 1) * PageSize)
            .Take(PageSize);

        foreach (var calc in pageCalculations)
        {
            string expression;
            if (calc.Operator == CalculatorOperator.SquareRoot)
            {
                var secondResult = Math.Sqrt(calc.SecondNumber);
                expression = $"√{calc.FirstNumber}, √{calc.SecondNumber}";
                table.AddRow(
                    $"[white]{calc.Id}[/]",
                    $"[white]{calc.CalculationDate}[/]",
                    $"[white]{expression}[/]",
                    $"[white]{calc.Result}, {Math.Round(secondResult, 2)}[/]",
                    calc.IsDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]",
                    calc.IsDeleted ? $"[white]{calc.DeletedAt}[/]" : "-"
                );
            }
            else
            {
                expression = $"{calc.FirstNumber} {GetOperatorSymbol(calc.Operator)} {calc.SecondNumber}";
                table.AddRow(
                    $"[white]{calc.Id}[/]",
                    $"[white]{calc.CalculationDate}[/]",
                    $"[white]{expression}[/]",
                    $"[white]{calc.Result}[/]",
                    calc.IsDeleted ? "[red]Deleted[/]" : "[green]Not Deleted[/]",
                    calc.IsDeleted ? $"[white]{calc.DeletedAt}[/]" : "-"
                );
            }
        }

        AnsiConsole.Write(table);
    }

    private string GetOperatorSymbol(CalculatorOperator op) => op switch
    {
        CalculatorOperator.Add => "+",
        CalculatorOperator.Subtract => "-",
        CalculatorOperator.Multiply => "*",
        CalculatorOperator.Divide => "/",
        CalculatorOperator.Modulus => "%",
        CalculatorOperator.SquareRoot => "√",
        _ => throw new ArgumentException("Invalid operator")
    };

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }

    public void WaitForKeyPress(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine(message);
        Console.ReadKey();
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

            updatedInputs[choice] = GetNumberInput(choice);
        }
    }
    

    private void ShowCurrentParameters(Dictionary<string, double> current, Dictionary<string, double> updated)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[blue]Parameter[/]").Centered())
            .AddColumn(new TableColumn("[yellow]Current Value[/]").Centered())
            .AddColumn(new TableColumn("[green]New Value[/]").Centered());

        foreach (var param in current)
        {
            table.AddRow(
                param.Key,
                param.Value.ToString(),
                updated.ContainsKey(param.Key) ? updated[param.Key].ToString() : "-"
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
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
            ShowCurrentParameters(currentParameters, updatedParameters);
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
                _newOperator = GetOperatorInput();
                _operatorChanged = true;
                parameters.Remove("Change Operator");
                continue;
            }

            var newValue = GetNumberInput($"Enter new value for {choice}");
            updatedParameters[choice] = newValue;
        }
    }
}