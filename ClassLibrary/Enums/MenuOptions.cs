﻿using System.ComponentModel;

namespace ClassLibrary.Enums;

public enum MenuOptions
{
    [Description("Calculator")]
    StartCalculator,

    [Description("Shapes")]
    StartShapes,

    [Description("Rock Paper Scissors")]
    StartGame,

    [Description("Exit")]
    Exit
}