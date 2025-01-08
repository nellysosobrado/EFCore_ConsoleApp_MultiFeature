using Spectre.Console;
using StartUp;
using CalculatorApp.Controllers;

namespace StartUp;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            ShowMenu();
        }
    }

    private static void ShowMenu()
    {
        Console.Clear();

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("[green]Choose an option:[/]")
                .UseConverter(option => option.GetDescription()) 
                .AddChoices(Enum.GetValues<MenuOptions>()));

        switch (option)
        {
            case MenuOptions.StartCalculator:
                StartCalculator();
                break;

            case MenuOptions.StartShapes:
                AnsiConsole.MarkupLine("[yellow]Shapes logic not implemented yet.[/]");
                Console.ReadKey();
                break;

            case MenuOptions.Exit:
                AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
                Environment.Exit(0);
                break;
        }
    }

    private static void StartCalculator()
    {
        var calculatorController = new CalculatorController();
        calculatorController.Start();
    }
}
