using ClassLibrary.Enums;
using ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;
using ClassLibrary.Models;

namespace CalculatorApp.Services;

public interface ICalculatorOperationService
{
    double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator);
    void SaveCalculation(Calculator calculation);
    
}