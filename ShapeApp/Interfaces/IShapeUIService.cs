using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public interface IShapeUIService
{
    string ShowMainMenu();

    ShapeType GetShapeType();
    Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters);

    //Dictionary<string, double> GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);

    double GetNumberInput(string prompt);

    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
}
