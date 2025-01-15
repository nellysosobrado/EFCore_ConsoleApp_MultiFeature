using CalculatorApp.Enums;
using ClassLibrary.Models;

namespace CalculatorApp.Services;

public interface ICalculatorUIService
{

    double GetNumberInput(string prompt);
    string GetOperatorInput();
    void CalculationHistory(IEnumerable<Calculator> calculations, bool showDeleteButton = false);
    void ShowResult(double operand1, double operand2, string operatorSymbol, double result, bool isDeleted = false);

    void ShowResultSimple(double operand1, double operand2, string operatorSymbol, double result);

    void ShowMessage(string message);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
    
    Dictionary<string, double> GetSelectedInputsToUpdate(Dictionary<string, double> currentInputs);
    (Dictionary<string, double> parameters, string newOperator) GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
}