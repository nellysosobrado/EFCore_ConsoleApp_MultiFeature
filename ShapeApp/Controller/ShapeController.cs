﻿using ClassLibrary.Models;
using Spectre.Console;
using ClassLibrary.Enums;
using ShapeApp.Enums;
using ClassLibrary.Extensions;
using ShapeApp.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShapeApp.Interfaces;

namespace ShapeApp.Controllers;

public class ShapeController
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;
    private readonly IUpdateShapeService _updateShapeService;
    private readonly IDeleteShapeService _deleteShapeService;
    private readonly IShapeDisplay _shapeDisplay;



    public ShapeController(
        IShapeOperationService operationService,
        IShapeUIService uiService,
        IUpdateShapeService updateShapeService,
        IDeleteShapeService deleteShapeService,
        IShapeDisplay shapeDisplay)
    {
        _operationService = operationService;
        _uiService = uiService;
        _updateShapeService = updateShapeService;
        _deleteShapeService = deleteShapeService;
        _shapeDisplay = shapeDisplay;
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
        _shapeDisplay.ShowResult(latestShape);
    }

    private void ShowShapes()
    {
        var shapes = _operationService.GetShapeHistory();
        _shapeDisplay.ShowShapes(shapes);
        _uiService.WaitForKeyPress();
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
        var shapes = _operationService.GetShapeHistory();

        var id = _deleteShapeService.GetShapeIdForDelete();

        if (_deleteShapeService.ConfirmDeletion())
        {
            _deleteShapeService.DeleteShape(id);
        }
    }
}