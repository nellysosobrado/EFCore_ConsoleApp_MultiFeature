﻿using ClassLibrary.Models;
using ClassLibrary.Repositories.ShapeAppRepository;
using Spectre.Console;

namespace ShapeApp.Services;

public class DeleteShapeService : IDeleteShapeService
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;
    private readonly ShapeRepository _shapeRepository;

    public DeleteShapeService(IShapeOperationService operationService, IShapeUIService uiService, ShapeRepository shapeRepository)
    {
        _operationService = operationService;
        _uiService = uiService;
        _shapeRepository = shapeRepository;
    }

    public void DeleteShape(int id)
    {
        _operationService.DeleteShape(id);
        AnsiConsole.MarkupLine("[green]Shape deleted successfully[/]");
        _uiService.WaitForKeyPress();
    }

    public void ShowShapes(IEnumerable<Shape> shapes)
    {
        _uiService.ShowShapes(shapes);
    }
    public int GetShapeIdForDelete()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                var id = AnsiConsole.Prompt(
                    new TextPrompt<int>("[green]Enter the ID of the shape to delete:[/]")
                        .ValidationErrorMessage("[red]Please enter a valid ID[/]"));

                // Verify that the shape exists and isn't already deleted
                _shapeRepository.GetShapeById(id);
                return id;
            }
            catch (InvalidOperationException ex)
            {
                _uiService.ShowError(ex.Message);
                if (!AnsiConsole.Confirm("Would you like to try another ID?"))
                    throw new OperationCanceledException("Delete cancelled.");
            }
        }
    }
    public bool ConfirmDeletion()
    {
        return AnsiConsole.Prompt(
            new ConfirmationPrompt("Are you sure you want to delete this shape?")
                .ShowChoices()
                .ShowDefaultValue());
    }
}