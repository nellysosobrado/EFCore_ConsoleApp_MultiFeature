using ClassLibrary.Enums.CalculatorAppEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ICalculatorParser
    {
        bool TryParseOperator(string input, out CalculatorOperator calculatorOperator);
    }
}
