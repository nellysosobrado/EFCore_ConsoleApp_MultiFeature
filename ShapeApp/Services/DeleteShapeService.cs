using ClassLibrary.Models;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Interfaces;
using Spectre.Console;

namespace ShapeApp.Services;

public class DeleteShapeService : IDeleteShapeService
{
    private readonly IShapeOperationService _operationService;
    private readonly ShapeRepository _shapeRepository;
    private readonly IShapeDisplay _shapeDisplay;
    private readonly IErrorService _errorService;

    public DeleteShapeService(IShapeOperationService operationService,
        ShapeRepository shapeRepository,
        IShapeDisplay shapeDisplay,
        IErrorService errorService)
    {
        _operationService = operationService;
        _shapeRepository = shapeRepository;
        _shapeDisplay = shapeDisplay;
        _errorService = errorService;
    }

    public void DeleteShape(int id)
    {
        _operationService.DeleteShape(id);
        AnsiConsole.MarkupLine("[green]Shape deleted successfully[/]");
        _errorService.WaitForKeyPress();
    }

    public void ShowShapes(IEnumerable<Shape> shapes)
    {
        _shapeDisplay.ShowShapes(shapes);
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

                _shapeRepository.GetShapeById(id);
                return id;
            }
            catch (InvalidOperationException ex)
            {
                _errorService.ShowError(ex.Message);
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
