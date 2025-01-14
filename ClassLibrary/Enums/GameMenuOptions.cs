using System.ComponentModel;

namespace ClassLibrary.Enums;

public enum GameMenuOptions
{
    [Description("Play Game")]
    PlayGame,

    [Description("View History")]
    ViewHistory,

    [Description("Main Menu")]
    MainMenu
}