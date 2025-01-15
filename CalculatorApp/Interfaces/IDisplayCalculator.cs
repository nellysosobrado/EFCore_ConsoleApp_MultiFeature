using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface IDisplayCalculator
    {
        void DisplayResult(double result);
        void DisplaySquareRootResults(double sqrtResult1, double sqrtResult2);
        void ClearTable();
    }
}
