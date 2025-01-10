using Autofac;
using CalculatorApp.Controllers;
using Spectre.Console;

namespace StartUp;

public class Menu
{
    private readonly IContainer _container;

    public Menu(IContainer container)
    {
        _container = container;
    }

    public void Show()
    {
        Console.Clear();

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("[green]Choose an option:[/]")
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<MenuOptions>()));

        HandleMenuOption(option);
    }

    private void HandleMenuOption(MenuOptions option)
    {
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

    private void StartCalculator()
    {
        using (var scope = _container.BeginLifetimeScope())
        {
            var calculatorController = scope.Resolve<CalculatorController>();
            calculatorController.Start();
        }
    }
}