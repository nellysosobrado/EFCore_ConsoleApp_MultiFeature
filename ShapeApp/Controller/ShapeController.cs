using ClassLibrary.Models;
using Spectre.Console;
using ClassLibrary.Enums;
using ShapeApp.Enums;
using ClassLibrary.Extensions;
using ShapeApp.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            Console.Clear();
            try
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ShapeMenuOptions>()
                        .Title("[italic yellow]Shape Calculator Menu[/]")
                        .WrapAround(true)
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
        _uiService.WaitForKeyPress();
    }

    private void UpdateShape()
    {
        while (true)
        {
            try
            {
                ShowShapes();
                var id = _uiService.GetShapeIdForUpdate();
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

                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ShapeUpdateMenuOptions>()
                        .Title("[green]What would you like to do next?[/]")
                        .UseConverter(option => option.GetDescription())
                        .AddChoices(Enum.GetValues<ShapeUpdateMenuOptions>()));

                switch (option)
                {
                    case ShapeUpdateMenuOptions.UpdateCalculation:
                        continue;
                    case ShapeUpdateMenuOptions.CalculatorMenu:
                        return;
                }
            }
            catch (Exception ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
            }
        }
    }

    private void DeleteShape()
    {
        ShowShapes();
        var id = _uiService.GetShapeIdForDelete();

        if (_uiService.ConfirmDeletion())
        {
            _operationService.DeleteShape(id);
            AnsiConsole.MarkupLine("[green]Shape deleted successfully[/]");
            _uiService.WaitForKeyPress();
        }
    }
}