using ClassLibrary.DataAccess;
using ClassLibrary.Enums;
using ClassLibrary.Models;
using ClassLibrary.Services;
using Spectre.Console;

namespace CalculatorApp.Controllers;

public class CalculatorController
{
    private readonly CalculatorService _service;

    public CalculatorController()
    {
        var accessDatabase = new AccessDatabase();
        var dbContext = accessDatabase.GetDbContext();
        _service = new CalculatorService(dbContext);
    }

    public void Start()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[italic yellow]Calculator Menu[/]")
                    .PageSize(4)
                    .AddChoices(new[] {
                        "Calculate",
                        "History",
                        "Main Menu"
                    }));

            switch (choice)
            {
                case "Calculate":
                    PerformCalculation();
                    break;

                case "History":
                    ShowCalculations();
                    break;

                case "Main Menu":
                    return;
            }
        }
    }

    private void PerformCalculation()
    {
        var operand1 = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter the [green]first[/] number:")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]Please enter a valid number[/]"));

        var operand2 = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter the [green]second[/] number:")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]Please enter a valid number[/]"));
         
        var operatorInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an [green]operator[/]")
                .PageSize(5)
                .AddChoices(new[] { "+", "-", "*", "/", "%" }));

        if (!TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
        {
            AnsiConsole.MarkupLine("[red]Error: Invalid operator[/]");
            return;
        }

        double result = calculatorOperator switch
        {
            CalculatorOperator.Add => operand1 + operand2,
            CalculatorOperator.Subtract => operand1 - operand2,
            CalculatorOperator.Multiply => operand1 * operand2,
            CalculatorOperator.Divide => operand2 != 0 ? operand1 / operand2 : double.NaN,
            CalculatorOperator.Modulus => operand1 % operand2,
            _ => throw new InvalidOperationException("Invalid operator")
        };

        var calculation = new Calculator
        {
            Operand1 = operand1,
            Operand2 = operand2,
            Operator = calculatorOperator,
            Result = Math.Round(result, 2),
            CalculationDate = DateTime.Now
        };

        _service.AddCalculation(calculation);

        var panel = new Panel($"{operand1} {operatorInput} {operand2} = {calculation.Result}")
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1)
        };
        panel.Header = new PanelHeader("Result");

        AnsiConsole.Write(panel);
        AnsiConsole.MarkupLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
    {
        switch (input)
        {
            case "+":
                calculatorOperator = CalculatorOperator.Add;
                return true;
            case "-":
                calculatorOperator = CalculatorOperator.Subtract;
                return true;
            case "*":
                calculatorOperator = CalculatorOperator.Multiply;
                return true;
            case "/":
                calculatorOperator = CalculatorOperator.Divide;
                return true;
            case "%":
                calculatorOperator = CalculatorOperator.Modulus;
                return true;
            default:
                calculatorOperator = default;
                return false;
        }
    }

    private void ShowCalculations()
    {
        var calculations = _service.GetAllCalculations();

        var table = new Spectre.Console.Table()
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
        AnsiConsole.MarkupLine("\nPress any key to continue...");
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