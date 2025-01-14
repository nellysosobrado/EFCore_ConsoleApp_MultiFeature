using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface IUpdateShapeService
{
    void UpdateShape(int id);
    int GetShapeIdForUpdate();
    bool ShouldChangeShapeType();
    Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
    void UpdateShape(int id, ShapeType shapeType, Dictionary<string, double> parameters);
}
