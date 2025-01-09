using ClassLibrary.Enums.CalculatorAppEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Calculator
    {
        public int Id { get; set; }
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public CalculatorOperator Operator { get; set; }
        public double Result { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.Now;
    }
}
