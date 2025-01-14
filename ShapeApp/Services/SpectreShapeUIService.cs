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
