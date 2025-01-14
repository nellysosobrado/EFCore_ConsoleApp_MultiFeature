using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;

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
            .AddColumn(new TableColumn("[cyan]Parameters[/]").LeftAligned())
            .AddColumn(new TableColumn("[magenta]Area[/]").Centered())
            .AddColumn(new TableColumn("[red]Perimeter[/]").Centered());

        foreach (var shape in shapes)
        {
            var parameters = GetParametersString(shape);

            table.AddRow(
                $"[yellow]{shape.Id}[/]",
                $"[green]{shape.CalculationDate:yyyy-MM-dd HH:mm:ss}[/]",
                $"[blue]{shape.ShapeType}[/]",
                $"[cyan]{parameters}[/]",
                $"[magenta]{shape.Area:F2}[/]",
                $"[red]{shape.Perimeter:F2}[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    private string GetParametersString(Shape shape)
    {
        var parameters = new List<string>();

        switch (shape.ShapeType)
        {
            case ShapeType.Rectangle:
                parameters.Add($"Width: {shape.Width:F2}");
                parameters.Add($"Height: {shape.Height:F2}");
                break;
            case ShapeType.Parallelogram:
                parameters.Add($"Base: {shape.BaseLength:F2}");
                parameters.Add($"Height: {shape.Height:F2}");
                parameters.Add($"Side: {shape.Side:F2}");
                break;
            case ShapeType.Triangle:
                parameters.Add($"SideA: {shape.SideA:F2}");
                parameters.Add($"SideB: {shape.SideB:F2}");
                parameters.Add($"SideC: {shape.SideC:F2}");
                parameters.Add($"Height: {shape.Height:F2}");
                break;
            case ShapeType.Rhombus:
                parameters.Add($"Side: {shape.Side:F2}");
                parameters.Add($"Height: {shape.Height:F2}");
                break;
        }

        return string.Join("\n", parameters);
    }

    public void ShowResult(Shape shape)
    {
        var parameters = GetParametersString(shape);

        AnsiConsole.MarkupLine($"\n[blue]Shape Type:[/] {shape.ShapeType}");
        AnsiConsole.MarkupLine($"[cyan]Parameters:[/]\n{parameters}");
        AnsiConsole.MarkupLine($"[magenta]Area:[/] {shape.Area:F2}");
        AnsiConsole.MarkupLine($"[red]Perimeter:[/] {shape.Perimeter:F2}");

        WaitForKeyPress();
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

        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]Shape Parameters Input[/]").RuleStyle("grey"));

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[blue]Parameter[/]").Centered())
            .AddColumn(new TableColumn("[green]Value[/]").Centered())
            .AddColumn(new TableColumn("[grey]Status[/]").Centered());

        // Först visa alla parametrar som "Pending"
        foreach (var param in requiredParameters)
        {
            table.AddRow(
                param.Key,
                "-",
                "[yellow]Pending[/]"
            );
        }

        AnsiConsole.Write(table);

        // Nu samla in värden och uppdatera tabellen efter varje inmatning
        foreach (var param in requiredParameters)
        {
            var value = AnsiConsole.Prompt(
                new TextPrompt<double>($"\n[green]Enter {param.Key}:[/]")
                    .PromptStyle("blue")
                    .ValidationErrorMessage("[red]Please enter a valid number greater than 0[/]")
                    .Validate(number =>
                    {
                        if (number <= 0)
                            return ValidationResult.Error("[red]Value must be greater than 0[/]");
                        return ValidationResult.Success();
                    }));

            parameters[param.Key] = value;

            // Rensa skärmen och rita om tabellen med uppdaterade värden
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Shape Parameters Input[/]").RuleStyle("grey"));

            table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[blue]Parameter[/]").Centered())
                .AddColumn(new TableColumn("[green]Value[/]").Centered())
                .AddColumn(new TableColumn("[grey]Status[/]").Centered());

            foreach (var p in requiredParameters)
            {
                if (parameters.ContainsKey(p.Key))
                {
                    table.AddRow(
                        p.Key,
                        $"{parameters[p.Key]:F2}",
                        "[green]Completed[/]"
                    );
                }
                else
                {
                    table.AddRow(
                        p.Key,
                        "-",
                        "[yellow]Pending[/]"
                    );
                }
            }

            AnsiConsole.Write(table);
        }

        // Visa slutlig bekräftelse
        if (!AnsiConsole.Confirm("\n[yellow]Are these values correct?[/]"))
        {
            return GetShapeParameters(requiredParameters);
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
        AnsiConsole.MarkupLine($"[red]{message}[/]");
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
            new ConfirmationPrompt("Are you sure you want to delete this shape?")
                .ShowChoices()
                .ShowDefaultValue());
    }

    public bool ShouldChangeShapeType()
    {
        return AnsiConsole.Prompt(
            new ConfirmationPrompt("Do you want to change the shape type?")
                .ShowChoices()
                .ShowDefaultValue());
    }

    public Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters)
    {
        var updatedParameters = new Dictionary<string, double>();

        AnsiConsole.MarkupLine("[blue]Current parameters:[/]");
        foreach (var param in currentParameters)
        {
            AnsiConsole.MarkupLine($"{param.Key}: [cyan]{param.Value:F2}[/]");
        }

        foreach (var param in currentParameters)
        {
            if (AnsiConsole.Prompt(
                new ConfirmationPrompt($"Do you want to update {param.Key}?")
                    .ShowChoices()
                    .ShowDefaultValue()))
            {
                updatedParameters[param.Key] = GetNumberInput($"Enter new value for {param.Key}");
            }
            else
            {
                updatedParameters[param.Key] = param.Value;
            }
        }

        return updatedParameters;
    }
}
