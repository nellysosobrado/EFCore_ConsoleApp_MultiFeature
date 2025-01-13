using System.ComponentModel;

namespace ClassLibrary.Enums;

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