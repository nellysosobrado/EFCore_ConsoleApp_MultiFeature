using ClassLibrary.Models;

namespace CalculatorApp.Services;

public interface ICalculatorUIService
{
    string ShowMainMenu();
    string ShowMenuAfterCalc();
    string ShowMenuAfterUpdate();
    string ShowMenuAfterDelete();

    double GetNumberInput(string prompt);
    string GetOperatorInput();
    void ShowCalculations(IEnumerable<Calculator> calculations);
    void ShowResult(double operand1, double operand2, string operatorSymbol, double result);
    void ShowResult(double operand1, double operand2, string operatorSymbol, double result, bool isDeleted);
    void ShowMessage(string message);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
    int GetCalculationIdForUpdate();
    int GetCalculationIdForDelete();
    bool ConfirmDeletion();
    bool ShouldChangeOperator();
    Dictionary<string, double> GetSelectedInputsToUpdate(Dictionary<string, double> currentInputs);
}