using ClassLibrary.Models;
using Spectre.Console;

namespace CalculatorApp.Services;

public interface ICalculatorUIService
{
    string ShowMainMenu();
    double GetNumberInput(string prompt);
    string GetOperatorInput();
    void ShowResult(double operand1, double operand2, string operatorSymbol, double result);
    void ShowHistory(IEnumerable<Calculator> calculations);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");

}