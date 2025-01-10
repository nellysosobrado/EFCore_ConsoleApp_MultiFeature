using Autofac;
using Spectre.Console;
using StartUp;
using CalculatorApp.Controllers;
using Startup;

namespace StartUp;

internal class Program
{
    static void Main(string[] args)
    {
        var container = ContainerConfig.Configure();

        while (true)
        {
            ShowMenu(container);
        }
    }

    private static void ShowMenu(IContainer container)
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
                StartCalculator(container);
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

    private static void StartCalculator(IContainer container)
    {
        using (var scope = container.BeginLifetimeScope())
        {
            var calculatorController = scope.Resolve<CalculatorController>();
            calculatorController.Start();
        }
    }
}
