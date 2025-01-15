using CalculatorApp.Enums;
using ClassLibrary.Models;
using ClassLibrary.Enums.CalculatorAppEnums;


namespace CalculatorApp.Services;

public interface ICalculatorUIService
{

    double GetNumberInput(string prompt);
    string GetOperatorInput();

    void ShowMessage(string message);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
    
    Dictionary<string, double> GetSelectedInputsToUpdate(Dictionary<string, double> currentInputs);
    (Dictionary<string, double> parameters, string newOperator) GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
    string GetOperatorSymbol(CalculatorOperator op);
    void SearchById(List<Calculator> calculations);
}