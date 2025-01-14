using System.ComponentModel;

namespace StartUp.UI;

public enum MenuOptions
{
    [Description("Start Calculator App")]
    StartCalculator,

    [Description("Start Shapes App")]
    StartShapes,

    [Description("Start Rock Paper Scissors")]
    StartGame,

    [Description("Exit Application")]
    Exit
}
