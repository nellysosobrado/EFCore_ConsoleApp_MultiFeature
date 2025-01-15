using System.ComponentModel;

namespace ClassLibrary.Enums.CalculatorAppEnums;

public enum CalculatorMenuOptions
{
    [Description("Calculate")]
    Calculate,

    [Description("History")]
    History,

    [Description("Update Calculation")]
    UpdateCalculation,

    [Description("Delete Calculation")]
    DeleteCalculation,

    [Description("Main Menu")]
    MainMenu,



}