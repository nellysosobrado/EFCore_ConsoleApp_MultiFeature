using ClassLibrary.Enums;
using ClassLibrary.Models;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Interfaces;
using ShapeApp.Validators;
using Spectre.Console;
using FluentValidation;
using System;

namespace ShapeApp.Services;

public class UpdateShapeService : IUpdateShapeService
{
    private readonly IShapeDisplay _shapeDisplay;
    private readonly IInputService _inputService;
    private readonly ShapeValidator _shapeValidator;
    private readonly ShapeRepository _shapeRepository;
    private readonly ShapeFactory _shapeFactory;

    public UpdateShapeService(
        IShapeDisplay shapeDisplay,
        IInputService inputService,
        ShapeValidator shapeValidator,
        ShapeRepository shapeRepository,
        ShapeFactory shapeFactory
       )
    {
        _shapeDisplay = shapeDisplay;
        _inputService = inputService;
        _shapeValidator = shapeValidator;
        _shapeRepository = shapeRepository;
        _shapeFactory = shapeFactory;
    }

    public void UpdateShape(int id)
    {
        var existingShape = _inputService.GetShapeById(id);
        var currentParameters = existingShape.GetParameters();
        ShapeType shapeType = existingShape.ShapeType;
        Dictionary<string, double> parameters;

        if (ShouldChangeShapeType())
        {
            shapeType = _inputService.GetShapeType();
            var requiredParameters = _inputService.GetRequiredParameters(shapeType);
            parameters = _inputService.GetShapeParameters(requiredParameters);
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

        UpdateShape(id, shapeType, parameters);

        var shapes = _shapeDisplay.GetShapeHistory();
        var updatedShape = shapes.First(s => s.Id == id);
        _shapeDisplay.ShowResult(updatedShape);
    }
    public int GetShapeIdForUpdate()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to update:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
    }
    public bool ShouldChangeShapeType()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new ConfirmationPrompt("Do you want to change the shape type?")
                .ShowChoices()
                .ShowDefaultValue());
    }
    public Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters)
    {
        Console.Clear();
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
                updatedParameters[param.Key] = _inputService.GetNumberInput($"Enter new value for {param.Key}");
            }
            else
            {
                updatedParameters[param.Key] = param.Value;
            }
        }

        return updatedParameters;
    }
    public void UpdateShape(int id, ShapeType shapeType, Dictionary<string, double> parameters)
    {
        var shape = _shapeFactory.CreateShape(shapeType);
        shape.SetParameters(parameters);

        var shapeModel = new Shape
        {
            Id = id,
            ShapeType = shapeType,
            CalculationDate = DateTime.Now
        };

        switch (shapeType)
        {
            case ShapeType.Rectangle:
                shapeModel.Width = parameters["Width"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Parallelogram:
                shapeModel.BaseLength = parameters["Base"];
                shapeModel.Height = parameters["Height"];
                shapeModel.Side = parameters["Side"];
                break;
            case ShapeType.Triangle:
                shapeModel.SideA = parameters["SideA"];
                shapeModel.SideB = parameters["SideB"];
                shapeModel.SideC = parameters["SideC"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Rhombus:
                shapeModel.Side = parameters["Side"];
                shapeModel.Height = parameters["Height"];
                break;
        }

        shapeModel.Area = shape.CalculateArea();
        shapeModel.Perimeter = shape.CalculatePerimeter();

        _shapeValidator.ValidateAndThrow(shapeModel);
        _shapeRepository.UpdateShape(shapeModel);
    }

}
