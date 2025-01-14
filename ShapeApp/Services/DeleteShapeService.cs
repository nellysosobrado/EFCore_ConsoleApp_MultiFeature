// ShapeApp/Services/DeleteShapeService.cs
using ClassLibrary.Models;
using Spectre.Console;

namespace ShapeApp.Services;

public class DeleteShapeService : IDeleteShapeService
{
    private readonly IShapeOperationService _operationService;
    private readonly IShapeUIService _uiService;

    public DeleteShapeService(IShapeOperationService operationService, IShapeUIService uiService)
    {
        _operationService = operationService;
        _uiService = uiService;
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
}
