using Spectre.Console;
using ClassLibrary.Models;
using CalculatorApp.Validators;
using System.Globalization;
using ClassLibrary.Enums.CalculatorAppEnums;

namespace CalculatorApp.Services;

public class SpectreCalculatorUIService : ICalculatorUIService
{
    private readonly InputValidator _inputValidator;

    public SpectreCalculatorUIService()
    {
        _inputValidator = new InputValidator();
    }

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
                    "Update Calculation",
                    "Delete Calculation",
                    "Main Menu"
                }));
    }

    public string ShowMenuAfterCalc()
    {
        return AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("[green]What would you like to do next?[/]")
                         .AddChoices(new[]
                         {
                        "New Calculation",
                        "Calculator Menu"
                         }));

    }
    public string ShowMenuAfterUpdate()
    {
        return AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("[green]What would you like to do next?[/]")
                         .AddChoices(new[]
                         {
                        "Update Calculation",
                        "Calculator Menu",
                         }));

    } 
    public string ShowMenuAfterDelete()
    {
        return AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("[green]What would you like to do next?[/]")
                         .AddChoices(new[]
                         {
                        "Delete a calculation",
                        "Calculator Menu",
                         }));

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
        }
        AnsiConsole.Write(table);
    }

    public void ShowHistory(IEnumerable<Calculator> calculations)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[green]Date[/]").Centered())
            .AddColumn(new TableColumn("[blue]Calculation[/]").Centered())
            .AddColumn(new TableColumn("[magenta]Result[/]").Centered());

        foreach (var calc in calculations)
        {
            string expression;
            if (calc.Operator == CalculatorOperator.SquareRoot)
            {
                var secondResult = Math.Sqrt(calc.SecondNumber);
                expression = $"√{calc.FirstNumber}, √{calc.SecondNumber}";
                table.AddRow(
                    $"[yellow]{calc.Id}[/]",
                    $"[green]{calc.CalculationDate}[/]",
                    $"[blue]{expression}[/]",
                    $"[magenta]{calc.Result}, {Math.Round(secondResult, 2)}[/]"
                );
            }
            else
            {
                expression = $"{calc.FirstNumber} {GetOperatorSymbol(calc.Operator)} {calc.SecondNumber}";
                table.AddRow(
                    $"[yellow]{calc.Id}[/]",
                    $"[green]{calc.CalculationDate}[/]",
                    $"[blue]{expression}[/]",
                    $"[magenta]{calc.Result}[/]"
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
    public void ShowResult(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
    }
}