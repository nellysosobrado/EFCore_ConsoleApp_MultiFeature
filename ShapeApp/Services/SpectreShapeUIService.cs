using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Pagination;

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
        var pagination = new Pagination<Shape>(shapes, pageSize: 5);
        var showDeleted = false;

        while (true)
        {
            Console.Clear();
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
                .AddColumn(new TableColumn("[green]Date[/]").Centered())
                .AddColumn(new TableColumn("[blue]Shape[/]").Centered())
                .AddColumn(new TableColumn("[cyan]Parameters[/]").LeftAligned())
                .AddColumn(new TableColumn("[magenta]Area[/]").Centered())
                .AddColumn(new TableColumn("[red]Perimeter[/]").Centered())
                .AddColumn(new TableColumn("[grey]Status[/]").Centered());

            var filteredShapes = showDeleted ? shapes : shapes.Where(s => !s.IsDeleted);
            pagination = new Pagination<Shape>(filteredShapes, pageSize: 5);

            foreach (var shape in pagination.GetCurrentPage())
            {
                var parameters = GetParametersString(shape);
                var statusColor = shape.IsDeleted ? "red" : "green";
                var status = shape.IsDeleted ? "Deleted" : "Active";

                table.AddRow(
                    $"[yellow]{shape.Id}[/]",
                    $"[green]{shape.CalculationDate:yyyy-MM-dd HH:mm:ss}[/]",
                    $"[blue]{shape.ShapeType}[/]",
                    $"[cyan]{parameters}[/]",
                    $"[magenta]{shape.Area:F2}[/]",
                    $"[red]{shape.Perimeter:F2}[/]",
                    $"[{statusColor}]{status}[/]"
                );
            }

            AnsiConsole.Write(table);

            var choices = new List<string>
        {
            showDeleted ? "Show Active Only" : "Show All (Including Deleted)",
            "Next Page",
            "Previous Page",
            "Back to Menu"
        };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select an option:[/]")
                    .AddChoices(choices));

            switch (choice)
            {
                case "Show All (Including Deleted)":
                    showDeleted = true;
                    break;
                case "Show Active Only":
                    showDeleted = false;
                    break;
                case "Next Page":
                    pagination.NextPage();
                    break;
                case "Previous Page":
                    pagination.PreviousPage();
                    break;
                case "Back to Menu":
                    return;
            }
        }
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
        Console.Clear();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[blue]Property[/]").LeftAligned())
            .AddColumn(new TableColumn("[green]Value[/]").RightAligned());

        table.AddRow("[blue]Shape Type[/]", $"[white]{shape.ShapeType}[/]");

        switch (shape.ShapeType)
        {
            case ShapeType.Rectangle:
                table.AddRow("[cyan]Width[/]", $"[white]{shape.Width:F2}[/]");
                table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                break;
            case ShapeType.Parallelogram:
                table.AddRow("[cyan]Base[/]", $"[white]{shape.BaseLength:F2}[/]");
                table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                table.AddRow("[cyan]Side[/]", $"[white]{shape.Side:F2}[/]");
                break;
            case ShapeType.Triangle:
                table.AddRow("[cyan]Side A[/]", $"[white]{shape.SideA:F2}[/]");
                table.AddRow("[cyan]Side B[/]", $"[white]{shape.SideB:F2}[/]");
                table.AddRow("[cyan]Side C[/]", $"[white]{shape.SideC:F2}[/]");
                table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                break;
            case ShapeType.Rhombus:
                table.AddRow("[cyan]Side[/]", $"[white]{shape.Side:F2}[/]");
                table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                break;
        }

        // Lägg till area och omkrets
        table.AddRow("[magenta]Area[/]", $"[white]{shape.Area:F2}[/]");
        table.AddRow("[red]Perimeter[/]", $"[white]{shape.Perimeter:F2}[/]");

        AnsiConsole.Write(table);
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

        RenderParameterTable(requiredParameters, parameters);

        foreach (var param in requiredParameters)
        {
            var value = GetNumberInput($"\n[white]Enter[/] [green]{param.Key}[/]");
            parameters[param.Key] = value;

            RenderParameterTable(requiredParameters, parameters);
        }

        if (!AnsiConsole.Confirm("\n[yellow]Are these values correct?[/]"))
        {
            return GetShapeParameters(requiredParameters);
        }

        return parameters;
    }

    private void RenderParameterTable(Dictionary<string, double> requiredParameters, Dictionary<string, double> currentValues)
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[blue]Parameter[/]").Centered())
            .AddColumn(new TableColumn("[green]Value[/]").Centered());

        foreach (var param in requiredParameters)
        {
            table.AddRow(
                param.Key,
                currentValues.ContainsKey(param.Key)
                    ? $"{currentValues[param.Key]:F2}"
                    : "-"
            );
        }

        AnsiConsole.Write(table);
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
