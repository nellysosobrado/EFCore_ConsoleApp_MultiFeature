using CalculatorApp.Interfaces;
using CalculatorApp.UI;
using ClassLibrary.Enums.CalculatorAppEnums;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Services
{
    public class DisplayCalculator : IDisplayCalculator
    {
        private readonly CalculatorTable _table;
        private bool _showDeleteButton;
        private bool _operatorChanged = false;
        private string _newOperator = string.Empty;
        private const int PageSize = 10;
        private readonly ICalculatorUIService _calculatorUI;



        public DisplayCalculator(CalculatorTable table, ICalculatorUIService spectreCalculatorUI)
        {
            _table = table;
            _calculatorUI = spectreCalculatorUI;
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
                    expression = $"{calc.FirstNumber} {_calculatorUI.GetOperatorSymbol(calc.Operator)} {calc.SecondNumber}";
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
                    _calculatorUI.WaitForKeyPress("\nPress any key to return to menu...");
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
                        _calculatorUI.SearchById(allCalculations);
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


    }
}
