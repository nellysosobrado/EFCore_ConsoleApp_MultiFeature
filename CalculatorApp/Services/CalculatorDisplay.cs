using Spectre.Console;
using System.Globalization;
using ClassLibrary.Enums.CalculatorAppEnums;

using CalculatorApp.UI;

namespace CalculatorApp.Services;

public class CalculatorDisplay : ICalculatorDisplay
{
    private readonly CalculatorTable _table;
    private readonly CalculatorMenu _calculatorMenu;
    private bool _showDeleteButton;
    private bool _operatorChanged = false;
    private string _newOperator = string.Empty;
    private const int PageSize = 10;

    private readonly InputValidator _inputValidator;

    public CalculatorDisplay(InputValidator inputValidator,
        CalculatorMenu calculatorMenu,
        CalculatorTable calculatorTable)
    {
        _inputValidator = inputValidator;
        _table = calculatorTable;
        _calculatorMenu = calculatorMenu;
    }
    public void HandleError(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"An error occurred: {ex.Message}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        _calculatorMenu.ShowMainMenu();
    }
    public void DisplayResult(double result)
    {
        _table.UpdateResult(result.ToString());
        _table.Display();
    }

    public void DisplaySquareRootResults(double sqrtResult1, double sqrtResult2)
    {
        _table.UpdateResult($"√{sqrtResult1}, √{sqrtResult2}");
        _table.Display();
    }

    public void ClearTable()
    {
        _table.Clear();
        _table.Display();
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

    public void DisplayCalculationsPage(List<Calculator> calculations, int page)
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


    public void CalculationHistory(IEnumerable<Calculator> calculations )
    {
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
    
    public void SearchById(List<Calculator> calculations)
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

   
    public string GetOperatorSymbol(CalculatorOperator op) => op switch
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

    public void ShowCurrentParameters(Dictionary<string, double> current, Dictionary<string, double> updated)
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
    
    
}