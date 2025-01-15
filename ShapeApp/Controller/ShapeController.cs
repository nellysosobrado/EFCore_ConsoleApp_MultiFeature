using Spectre.Console;
using ClassLibrary.Extensions;
using ShapeApp.Services;
using ShapeApp.Interfaces;
using ClassLibrary.Enums.ShapeAppEnums;

namespace ShapeApp.Controllers;

public class ShapeController
{
    private readonly ISaveShapeService _operationService;
    private readonly IUpdateShapeService _updateShapeService;
    private readonly IDeleteShapeService _deleteShapeService;
    private readonly IShapeDisplay _shapeDisplay;
    private readonly IErrorService _errorService;
    private readonly IInputService _inputService;

    public ShapeController(
        ISaveShapeService operationService,
        IUpdateShapeService updateShapeService,
        IDeleteShapeService deleteShapeService,
        IShapeDisplay shapeDisplay,
        IErrorService errorService,
        IInputService inputService
        )
    {
        _operationService = operationService;
        _updateShapeService = updateShapeService;
        _deleteShapeService = deleteShapeService;
        _shapeDisplay = shapeDisplay;
        _errorService = errorService;
        _inputService = inputService;
    }

    public void ShapeAppMenu()
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
                        ShapeHistory();
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
                _errorService.ShowError(ex.Message);
                _errorService.WaitForKeyPress();
            }
        }
    }

    private void CalculateShape()
    {
        var shapeType = _inputService.GetShapeType();
        var requiredParameters = _inputService.GetRequiredParameters(shapeType);
        var parameters = _inputService.GetShapeParameters(requiredParameters);

        _operationService.SaveShape(shapeType, parameters);

        var shapes = _shapeDisplay.GetShapeHistory();
        var latestShape = shapes.First();
        _shapeDisplay.ShowResult(latestShape);
    }

    private void ShapeHistory()
    {
        var shapes = _shapeDisplay.GetShapeHistory();
        _shapeDisplay.ShapeHistoryDisplay(shapes);
        _errorService.WaitForKeyPress();
    }
    private void UpdateShape()
    {
        while (true)
        {
            try
            {
                var id = _updateShapeService.GetShapeIdForUpdate();
                _updateShapeService.UpdateShape(id);

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ShapeUpdateMenuOptions>()
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
                _errorService.ShowError(ex.Message);
                _errorService.WaitForKeyPress();
            }
        }
    }


    private void DeleteShape()
    {
        var shapes = _shapeDisplay.GetShapeHistory();

        var id = _deleteShapeService.GetShapeIdForDelete();

        if (_deleteShapeService.ConfirmDeletion())
        {
            _deleteShapeService.DeleteShape(id);
        }
    }
}