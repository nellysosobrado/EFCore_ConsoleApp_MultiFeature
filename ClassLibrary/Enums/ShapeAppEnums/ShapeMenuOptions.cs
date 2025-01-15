using System.ComponentModel;

namespace ClassLibrary.Enums.ShapeAppEnums;

public enum ShapeMenuOptions
{
    [Description("New Calculation")]
    NewCalculation,

    [Description("View History")]
    ViewHistory,

    [Description("Update Calculation")]
    UpdateCalculation,

    [Description("Delete Calculation")]
    DeleteCalculation,

    [Description("Main Menu")]
    MainMenu
}