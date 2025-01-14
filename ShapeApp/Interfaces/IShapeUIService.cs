using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface IShapeUIService
{
    string ShowMainMenu();

    ShapeType GetShapeType();
    Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters);

    bool ShouldChangeShapeType();
    Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
    int GetShapeIdForUpdate();
    int GetShapeIdForDelete();
    bool ConfirmDeletion();

    double GetNumberInput(string prompt);
    void ShowShapes(IEnumerable<Shape> shapes);
    void ShowResult(Shape shape);

    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
}
