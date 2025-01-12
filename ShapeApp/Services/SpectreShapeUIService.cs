using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ShapeApp.Validators;
using ShapeApp.Extensions;

namespace ShapeApp.Services;

public class SpectreShapeUIService : IShapeUIService
{
    public string ShowMainMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Shape Calculator[/]")
                .AddChoices(new[]
                {
                    "1. New Calculation",
                    "2. View History",
                    "3. Update Calculation",
                    "4. Delete Calculation",
                    "5. Main Menu"
                }));
    }

    public void ShowShapes(IEnumerable<Shape> shapes)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[green]Date[/]").Centered())
            .AddColumn(new TableColumn("[blue]Shape[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Parameters[/]").Centered())
            .AddColumn(new TableColumn("[magenta]Area[/]").Centered())
            .AddColumn(new TableColumn("[red]Perimeter[/]").Centered());

        foreach (var shape in shapes)
        {
            var parameters = string.Join(", ",
                shape.GetParameters().Select(p => $"{p.Key}: {p.Value:F2}"));

            table.AddRow(
                $"[yellow]{shape.Id}[/]",
                $"[green]{shape.CalculationDate}[/]",
                $"[blue]{shape.ShapeType}[/]",
                $"[cyan]{parameters}[/]",
                $"[magenta]{shape.Area:F2}[/]",
                $"[red]{shape.Perimeter:F2}[/]"
            );
        }

        AnsiConsole.Write(table);
    }


    public void ShowResult(Shape shape)
    {
        var parameters = string.Join(", ",
            shape.GetParameters().Select(p => $"{p.Key}: {p.Value:F2}"));

        AnsiConsole.MarkupLine($"\n[blue]Shape Type:[/] {shape.ShapeType}");
        AnsiConsole.MarkupLine($"[cyan]Parameters:[/] {parameters}");
        AnsiConsole.MarkupLine($"[magenta]Area:[/] {shape.Area:F2}");
        AnsiConsole.MarkupLine($"[red]Perimeter:[/] {shape.Perimeter:F2}");
    }

    public ShapeType GetShapeType()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<ShapeType>()
                .Title("[green]Select a shape:[/]")
                .UseConverter(type => type.ToString())
                .AddChoices(Enum.GetValues<ShapeType>()));
    }

    public Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters)
    {
        var parameters = new Dictionary<string, double>();

        foreach (var param in requiredParameters)
        {
            parameters[param.Key] = GetNumberInput($"Enter {param.Key}");
        }

        return parameters;
    }

    public double GetNumberInput(string prompt)
    {
        while (true)
        {
            try
            {
                return AnsiConsole.Prompt(
                    new TextPrompt<double>($"[green]{prompt}:[/]")
                        .ValidationErrorMessage("[red]Please enter a valid number[/]")
                        .Validate(n => n > 0 ? ValidationResult.Success() : ValidationResult.Error("Value must be greater than 0")));
            }
            catch (Exception)
            {
                ShowError("Invalid input. Please try again.");
            }
        }
    }

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message.EscapeMarkup()}[/]");
    }


    public void WaitForKeyPress(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine($"[grey]{message}[/]");
        Console.ReadKey(true);
    }

    public int GetShapeIdForUpdate()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to update:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
    }

    public int GetShapeIdForDelete()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to delete:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
    }

    public bool ConfirmDeletion()
    {
        return AnsiConsole.Prompt(
            new ConfirmationPrompt("Are you sure you want to delete this shape?"));
    }
    public Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters)
    {
        var updatedParameters = new Dictionary<string, double>();
        var parameters = currentParameters.Keys.ToList();
        parameters.Add("[green]Confirm[/]");
        parameters.Add("[red]Cancel[/]");

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select parameter to update:[/]")
                    .AddChoices(parameters));

            if (choice == "[green]Confirm[/]")
                return updatedParameters;

            if (choice == "[red]Cancel[/]")
                return currentParameters;

            updatedParameters[choice] = GetNumberInput($"Enter new value for {choice}");
        }
    }
    public bool ShouldChangeShapeType()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<bool>()
                .Title("[green]Would you like to change the shape type?[/]")
                .AddChoices(true, false)
                .UseConverter(b => b ? "Yes" : "No"));
    }

}