using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ICalculationProcessor
    {
        public (double result, bool isSquareRoot) Calculate(double operand1, double operand2, string operatorInput);
        public void SaveCalculation(double operand1, double operand2, string operatorInput, double result);
    }
}
