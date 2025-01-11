using ClassLibrary.Models;
using Spectre.Console;
using ClassLibrary.Enums;
//using ShapeApp.Interfaces;
using ShapeApp.Enums;
using ShapeApp.Extensions;
using ShapeApp.Services;

namespace ShapeApp.Controllers;

public class ShapeController
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;

    public ShapeController(
        IShapeOperationService operationService,
        IShapeUIService uiService)
    {
        _operationService = operationService;
        _uiService = uiService;
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ShapeMenuOptions>()
                        .Title("[green]Shape Calculator Menu[/]")
                        .UseConverter(option => option.GetDescription())
                        .AddChoices(Enum.GetValues<ShapeMenuOptions>()));

                switch (option)
                {
                    case ShapeMenuOptions.NewCalculation:
                        CalculateShape();
                        break;
                    case ShapeMenuOptions.ViewHistory:
                        ShowShapes();
                        break;
                    case ShapeMenuOptions.UpdateCalculation:
                        UpdateShape();
                        break;
                    case ShapeMenuOptions.DeleteCalculation:
                        DeleteShape();
                        break;
                    case ShapeMenuOptions.MainMenu:
                        return;
                }
            }
            catch (Exception ex)
            {
                _uiService.ShowError(ex.Message);
            }
            finally
            {
                _uiService.WaitForKeyPress();
            }
        }
    }

    private void CalculateShape()
    {
        var shapeType = _uiService.GetShapeType();
        var requiredParameters = _operationService.GetRequiredParameters(shapeType);
        var parameters = _uiService.GetShapeParameters(requiredParameters);

        _operationService.SaveShape(shapeType, parameters);

        var shapes = _operationService.GetShapeHistory();
        var latestShape = shapes.First();
        _uiService.ShowResult(latestShape);
    }

    private void ShowShapes()
    {
        var shapes = _operationService.GetShapeHistory();
        _uiService.ShowShapes(shapes);
    }

    private void UpdateShape()
    {
        ShowShapes();
        var id = _uiService.GetShapeIdForUpdate();

        var shapeType = _uiService.GetShapeType();
        var requiredParameters = _operationService.GetRequiredParameters(shapeType);
        var parameters = _uiService.GetShapeParameters(requiredParameters);

        _operationService.UpdateShape(id, shapeType, parameters);

        var shapes = _operationService.GetShapeHistory();
        var updatedShape = shapes.First(s => s.Id == id);
        _uiService.ShowResult(updatedShape);
    }

    private void DeleteShape()
    {
        ShowShapes();
        var id = _uiService.GetShapeIdForDelete();

        if (_uiService.ConfirmDeletion())
        {
            _operationService.DeleteShape(id);
            AnsiConsole.MarkupLine("[green]Shape deleted successfully[/]");
        }
    }
}