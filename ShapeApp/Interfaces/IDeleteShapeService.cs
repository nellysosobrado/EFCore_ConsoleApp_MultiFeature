using ClassLibrary.Models;

namespace ShapeApp.Services;

public interface IDeleteShapeService
{
    void SoftDeleteShape(int id);
    void ShowShapes(IEnumerable<Shape> shapes);
    int GetShapeIdForDelete();
    bool ConfirmDeletion();
    void DeleteShape(int id);
}
