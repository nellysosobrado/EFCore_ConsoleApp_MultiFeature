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
                .Title("[italic yellow]Calculator Menu[/]")
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
                .PageSize(5)
                .AddChoices(new[] { "+", "-", "*", "/", "%" }));
    }

    public void ShowResult(double operand1, double operand2, string operatorSymbol, double result)
    {
        var panel = new Panel($"{operand1} {operatorSymbol} {operand2} = {result}")
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1)
        };
        panel.Header = new PanelHeader("Result");

        AnsiConsole.Write(panel);
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
            table.AddRow(
                calc.CalculationDate.ToString(),
                $"{calc.Operand1} {GetOperatorSymbol(calc.Operator)} {calc.Operand2}",
                calc.Result.ToString()
            );
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
        _ => "?"
    };
}