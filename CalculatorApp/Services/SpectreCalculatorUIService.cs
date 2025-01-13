using Spectre.Console;
using ClassLibrary.Models;
using CalculatorApp.Validators;
using System.Globalization;
using ClassLibrary.Enums.CalculatorAppEnums;
using CalculatorApp.Enums;
using CalculatorApp.Enums;
using CalculatorApp.Extensions;
using CalculatorApp.UI;

namespace CalculatorApp.Services;

public class SpectreCalculatorUIService : ICalculatorUIService
{
    private readonly InputValidator _inputValidator;
    private readonly CalculatorMenu _calculatorMenu;

    public SpectreCalculatorUIService(InputValidator inputValidator, CalculatorMenu calculatorMenu)
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

    public void CalculationHistory(IEnumerable<Calculator> calculations)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[green]Created[/]").Centered())
            .AddColumn(new TableColumn("[blue]Calculation[/]").Centered())
            .AddColumn(new TableColumn("[magenta]Result[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Status[/]").Centered())
            .AddColumn(new TableColumn("[red]Deleted At[/]").Centered());

        foreach (var calc in calculations)
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

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }

    public void WaitForKeyPress(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine(message);
        Console.ReadKey();
    }

    private string GetOperatorSymbol(CalculatorOperator op) => op switch
    {
        CalculatorOperator.Add => "+",
        CalculatorOperator.Subtract => "-",
        CalculatorOperator.Multiply => "*",
        CalculatorOperator.Divide => "/",
        CalculatorOperator.Modulus => "%",
        CalculatorOperator.SquareRoot => "√",
        _ => "?"
    };

    public int GetCalculationIdForUpdate()
    {
        return AnsiConsole.Ask<int>("Enter the [green]ID[/] of the calculation to update:");
    }

    public int GetCalculationIdForDelete()
    {
        return AnsiConsole.Ask<int>("Enter the [green]ID[/] of the calculation to delete:");
    }

    public bool ConfirmDeletion()
    {
        Console.Clear();
        return AnsiConsole.Confirm("Are you sure you want to delete this calculation?");
    }

    public bool ShouldChangeOperator()
    {
        return AnsiConsole.Confirm("Do you want to change the operator?");
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
}