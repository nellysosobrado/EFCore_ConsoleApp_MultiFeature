using ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;
using ClassLibrary.Models;
using ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;


namespace CalculatorApp.Services;

public interface ICalculatorDisplay
{

    double GetNumberInput(string prompt);
    string GetOperatorInput();
    string GetOperatorSymbol(CalculatorOperator op);
    void SearchById(List<Calculator> calculations);

    void ShowMessage(string message);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
    

    void ShowCurrentParameters(Dictionary<string, double> current, Dictionary<string, double> updated);
    void DisplayResult(double result);
    void DisplaySquareRootResults(double sqrtResult1, double sqrtResult2);
    void ClearTable();
    void ShowResult(double operand1, double operand2, string operatorSymbol, double result, bool isDeleted = false);
    void ShowResultSimple(double operand1, double operand2, string operatorSymbol, double result);
    void CalculationHistory(IEnumerable<Calculator> calculations);
    void DisplayCalculationsPage(List<Calculator> calculations, int page);
    void HandleError(Exception ex);

}