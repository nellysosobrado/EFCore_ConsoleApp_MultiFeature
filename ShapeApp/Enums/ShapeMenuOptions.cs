using System.ComponentModel;

namespace ShapeApp.Enums;

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