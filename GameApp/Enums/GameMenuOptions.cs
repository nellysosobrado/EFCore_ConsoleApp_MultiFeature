using System.ComponentModel;

namespace GameApp.Enums;

public enum GameMenuOptions
{
    [Description("1. Play Game")]
    PlayGame,

    [Description("2. View History")]
    ViewHistory,

    [Description("3. Main Menu")]
    MainMenu
}