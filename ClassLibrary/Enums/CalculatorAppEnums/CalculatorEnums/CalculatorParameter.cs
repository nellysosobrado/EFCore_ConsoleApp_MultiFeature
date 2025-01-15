using System.ComponentModel;

namespace ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;

public enum CalculatorParameter
{
    [Description("First Number")]
    FirstNumber,

    [Description("Second Number")]
    SecondNumber,

    [Description("Result")]
    Result,

    [Description("Operator")]
    Operator
}