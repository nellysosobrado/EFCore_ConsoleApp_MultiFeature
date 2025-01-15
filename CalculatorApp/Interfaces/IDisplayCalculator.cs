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
        void ShowResult(double operand1, double operand2, string operatorSymbol, double result, bool isDeleted = false);
        void ShowResultSimple(double operand1, double operand2, string operatorSymbol, double result);
        void CalculationHistory(IEnumerable<Calculator> calculations, bool showDeleteButton = false);
        void DisplayCalculationsPage(List<Calculator> calculations, int page);

    }
}
