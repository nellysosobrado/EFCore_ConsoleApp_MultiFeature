using ClassLibrary.Enums;
using Spectre.Console;

namespace ShapeApp.Services;

public class UpdateShapeService : IUpdateShapeService
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;

    public UpdateShapeService(
        IShapeOperationService operationService,
        IShapeUIService uiService)
    {
        _operationService = operationService;
        _uiService = uiService;
    }

    public void UpdateShape(int id)
    {
        var existingShape = _operationService.GetShapeById(id);
        var currentParameters = existingShape.GetParameters();
        ShapeType shapeType = existingShape.ShapeType;
        Dictionary<string, double> parameters;

        if (_uiService.ShouldChangeShapeType())
        {
            shapeType = _uiService.GetShapeType();
            var requiredParameters = _operationService.GetRequiredParameters(shapeType);
            parameters = _uiService.GetShapeParameters(requiredParameters);
        }
        else
        {
            var selectedUpdates = _uiService.GetSelectedParametersToUpdate(currentParameters);
            parameters = new Dictionary<string, double>(currentParameters);
            foreach (var update in selectedUpdates)
            {
                parameters[update.Key] = update.Value;
            }
        }

        _operationService.UpdateShape(id, shapeType, parameters);

        var shapes = _operationService.GetShapeHistory();
        var updatedShape = shapes.First(s => s.Id == id);
        _uiService.ShowResult(updatedShape);
    }
    public int GetShapeIdForUpdate()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to update:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
    }

}
