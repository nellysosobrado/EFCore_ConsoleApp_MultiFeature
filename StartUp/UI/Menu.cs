using Autofac;
using ShapeApp.Controllers;
using Spectre.Console;
using ClassLibrary.Extensions;
using GameApp.Controller;
using CalculatorApp.Controller;
using ClassLibrary.Enums.StartUpEnums;

namespace StartUp.UI;

public class Menu
{
    private readonly IContainer _container;
    private bool _isRunning = true;

    public Menu(IContainer container)
    {
        _container = container;
    }

    public void Show()
    {
        while (_isRunning)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("[green]Choose an option:[/]")
                    .UseConverter(option => option.GetDescription())
                    .AddChoices(Enum.GetValues<MenuOptions>()));

            HandleMenuOption(option);
        }
    }

    private void HandleMenuOption(MenuOptions option)
    {
        switch (option)
        {
            case MenuOptions.StartCalculator:
                StartCalculator();
                break;

            case MenuOptions.StartShapes:
                StartShapes();
                break;

            case MenuOptions.StartGame:
                StartGame();
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

    private void StartShapes()
    {
        using (var scope = _container.BeginLifetimeScope())
        {
            var shapeController = scope.Resolve<ShapeController>();
            shapeController.ShapeAppMenu();
        }
    }
    private void StartGame()
    {
        using (var scope = _container.BeginLifetimeScope())
        {
            var gameController = scope.Resolve<GameController>();
            gameController.Start();
        }
    }
}