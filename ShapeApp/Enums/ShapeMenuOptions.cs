using System.ComponentModel;

namespace ShapeApp.Enums;

public enum ShapeMenuOptions
{
    [Description("1. New Calculation")]
    NewCalculation,

    [Description("2. View History")]
    ViewHistory,

    [Description("3. Update Calculation")]
    UpdateCalculation,

    [Description("4. Delete Calculation")]
    DeleteCalculation,

    [Description("5. Main Menu")]
    MainMenu
}