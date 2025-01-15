using CalculatorApp.Interfaces;
using ClassLibrary.Enums.CalculatorAppEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Services
{
    public class CalculatorParser : ICalculatorParser
    {
        public bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
        {
            switch (input)
            {
                case "+":
                    calculatorOperator = CalculatorOperator.Add;
                    return true;
                case "-":
                    calculatorOperator = CalculatorOperator.Subtract;
                    return true;
                case "*":
                    calculatorOperator = CalculatorOperator.Multiply;
                    return true;
                case "/":
                    calculatorOperator = CalculatorOperator.Divide;
                    return true;
                case "%":
                    calculatorOperator = CalculatorOperator.Modulus;
                    return true;
                case "√":
                    calculatorOperator = CalculatorOperator.SquareRoot;
                    return true;
                default:
                    calculatorOperator = default;
                    return false;
            }
        }
        
    }
}
