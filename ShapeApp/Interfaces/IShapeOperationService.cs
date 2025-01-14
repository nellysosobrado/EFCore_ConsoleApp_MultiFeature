using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface IShapeOperationService
{
    void SaveShape(ShapeType shapeType, Dictionary<string, double> parameters);
    //Dictionary<string, double> GetRequiredParameters(ShapeType shapeType);
}