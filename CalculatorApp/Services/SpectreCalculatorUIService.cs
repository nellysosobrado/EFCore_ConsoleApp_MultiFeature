using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace CalculatorApp.Services;

public class SpectreCalculatorUIService : ICalculatorUIService
{
    public string ShowMainMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Calculator Menu[/]")
                .PageSize(4)
                .AddChoices(new[] {
                    "Calculate",
                    "History",
                    "Main Menu"
                }));
    }

    public double GetNumberInput(string prompt)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<double>($"Enter the [green]{prompt}[/] number:")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]Please enter a valid number[/]"));
    }

    public string GetOperatorInput()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an [green]operator[/]")
                .PageSize(6)
                .AddChoices(new[] { "+", "-", "*", "/", "%", "√" }));
    }

    public void ShowResult(double operand1, double operand2, string operatorSymbol, double result)
    {
        Console.Clear();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[italic green]\nResult[/]")
            .AddColumn("Expression")
            .AddColumn("Result");

        if (operatorSymbol == "√")
        {
            table.AddRow($"√{operand1}", $"{Math.Round(result, 2)}");
            table.AddRow($"√{operand2}", $"{Math.Round(Math.Sqrt(operand2), 2)}");
        }
        else
        {
            table.AddRow($"{operand1} {operatorSymbol} {operand2}", $"{Math.Round(result, 2)}");
        }        AnsiConsole.Write(table);
    }

    public void ShowHistory(IEnumerable<Calculator> calculations)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("Date").Centered())
            .AddColumn(new TableColumn("Calculation").Centered())
            .AddColumn(new TableColumn("Result").Centered());

        foreach (var calc in calculations)
        {
            string expression;
            if (calc.Operator == CalculatorOperator.SquareRoot)
            {
                var secondResult = Math.Sqrt(calc.Operand2);
                expression = $"√{calc.Operand1}, √{calc.Operand2}";
                table.AddRow(
                    calc.CalculationDate.ToString(),
                    expression,
                    $"{calc.Result}, {Math.Round(secondResult, 2)}"
                );
            }
            else
            {
                expression = $"{calc.Operand1} {GetOperatorSymbol(calc.Operator)} {calc.Operand2}";
                table.AddRow(
                    calc.CalculationDate.ToString(),
                    expression,
                    calc.Result.ToString()
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
}