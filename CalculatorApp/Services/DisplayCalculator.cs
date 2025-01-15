using CalculatorApp.Interfaces;
using CalculatorApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Services
{
    public class DisplayCalculator : IDisplayCalculator
    {
        private readonly CalculatorTable _table;

        public DisplayCalculator(CalculatorTable table)
        {
            _table = table;
        }

        public void DisplayResult(double result)
        {
            _table.UpdateResult(result.ToString());
            _table.Display();
        }

        public void DisplaySquareRootResults(double sqrtResult1, double sqrtResult2)
        {
            _table.UpdateResult($"√{sqrtResult1}, √{sqrtResult2}");
            _table.Display();
        }

        public void ClearTable()
        {
            _table.Clear();
            _table.Display();
        }
    }
}
