using ClassLibrary.Enums.CalculatorAppEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ICalculatorUpdate
    {
        void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator);
        void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator, double result);
        (Dictionary<string, double> parameters, string newOperator) GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
    }
}
