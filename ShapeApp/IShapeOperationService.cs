using ClassLibrary.Models;

namespace ShapeApp.Services;

public interface IShapeOperationService
{
    IEnumerable<Shape> GetShapeHistory();
    void SaveShape(string shapeType, Dictionary<string, double> parameters);
    void UpdateShape(int id, string shapeType, Dictionary<string, double> parameters);
    void DeleteShape(int id);
    Dictionary<string, double> GetRequiredParameters(string shapeType);
}