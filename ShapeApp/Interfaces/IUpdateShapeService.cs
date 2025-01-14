namespace ShapeApp.Services;

public interface IUpdateShapeService
{
    void UpdateShape(int id);
    int GetShapeIdForUpdate();
    bool ShouldChangeShapeType();
    Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
}
