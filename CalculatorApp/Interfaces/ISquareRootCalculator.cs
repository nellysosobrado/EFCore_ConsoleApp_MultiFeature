using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ISquareRootCalculator
    {
        (double firstRoot, double secondRoot) CalculateRoots(double operand1, double operand2);
        (double firstResult, double secondResult)? CalculateSquareRoots(double operand1, double operand2);

    }
}
