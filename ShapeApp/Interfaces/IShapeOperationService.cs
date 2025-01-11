using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface IShapeOperationService
{
    IEnumerable<Shape> GetShapeHistory();
    void SaveShape(ShapeType shapeType, Dictionary<string, double> parameters);
    void UpdateShape(int id, ShapeType shapeType, Dictionary<string, double> parameters);
    void DeleteShape(int id);
    Dictionary<string, double> GetRequiredParameters(ShapeType shapeType);
}