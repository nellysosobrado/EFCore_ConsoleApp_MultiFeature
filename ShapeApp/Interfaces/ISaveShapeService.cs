using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface ISaveShapeService
{
    void SaveShape(ShapeType shapeType, Dictionary<string, double> parameters);
}