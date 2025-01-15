using ClassLibrary.Enums;
using ClassLibrary.Enums.CalculatorAppEnums;
using ClassLibrary.Models;

namespace CalculatorApp.Services;

public interface ICalculatorOperationService
{
    bool TryParseOperator(string input, out CalculatorOperator calculatorOperator);
    double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator);
    void SaveCalculation(Calculator calculation);
    void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator);
    void DeleteCalculation(int id);
}