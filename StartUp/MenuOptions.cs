using System.ComponentModel;

namespace StartUp;

public enum MenuOptions
{
    [Description("Start Calculator App")]
    StartCalculator,

    [Description("Start Shapes App")]
    StartShapes,

    [Description("Exit Application")]
    Exit
}
