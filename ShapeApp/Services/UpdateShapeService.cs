using ClassLibrary.Enums;
using ShapeApp.Interfaces;
using Spectre.Console;

namespace ShapeApp.Services;

public class UpdateShapeService : IUpdateShapeService
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;
    private readonly IShapeDisplay _shapeDisplay;

    public UpdateShapeService(
        IShapeOperationService operationService,
        IShapeUIService uiService,
        IShapeDisplay shapeDisplay)
    {
        _operationService = operationService;
        _uiService = uiService;
        _shapeDisplay = shapeDisplay;
    }

    public void UpdateShape(int id)
    {
        var existingShape = _operationService.GetShapeById(id);
        var currentParameters = existingShape.GetParameters();
        ShapeType shapeType = existingShape.ShapeType;
        Dictionary<string, double> parameters;

        if (ShouldChangeShapeType())
        {
            shapeType = _uiService.GetShapeType();
            var requiredParameters = _operationService.GetRequiredParameters(shapeType);
            parameters = _uiService.GetShapeParameters(requiredParameters);
        }
        else
        {
            var selectedUpdates = GetSelectedParametersToUpdate(currentParameters);
            parameters = new Dictionary<string, double>(currentParameters);
            foreach (var update in selectedUpdates)
            {
                parameters[update.Key] = update.Value;
            }
        }

        _operationService.UpdateShape(id, shapeType, parameters);

        var shapes = _operationService.GetShapeHistory();
        var updatedShape = shapes.First(s => s.Id == id);
        _shapeDisplay.ShowResult(updatedShape);
    }
    public int GetShapeIdForUpdate()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to update:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
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
                updatedParameters[param.Key] = _uiService.GetNumberInput($"Enter new value for {param.Key}");
            }
            else
            {
                updatedParameters[param.Key] = param.Value;
            }
        }

        return updatedParameters;
    }

}
