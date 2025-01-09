using ClassLibrary.Enums;
using ClassLibrary.Models;

namespace CalculatorApp.Services;

public interface ICalculatorOperationService
{
    bool TryParseOperator(string input, out CalculatorOperator calculatorOperator);
    double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator);
    void SaveCalculation(Calculator calculation);
    IEnumerable<Calculator> GetCalculationHistory();
}