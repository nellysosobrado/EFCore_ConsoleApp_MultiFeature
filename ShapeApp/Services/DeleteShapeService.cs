// ShapeApp/Services/DeleteShapeService.cs
using ClassLibrary.Models;
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

    public bool ConfirmDeletion()
    {
        return _uiService.ConfirmDeletion();
    }
    public int GetShapeIdForDelete()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the shape to delete:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));
    }
}
