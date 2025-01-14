using ClassLibrary.Models;

namespace ShapeApp.Services;

public interface IDeleteShapeService
{
    void DeleteShape(int id);
    void ShowShapes(IEnumerable<Shape> shapes);
    int GetShapeIdForDelete();
    bool ConfirmDeletion();
}
