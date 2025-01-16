using System.ComponentModel;

namespace ClassLibrary.Enums.RpsGameEnums;

public enum GameMenuOptions
{
    [Description("Play Game Against Computer")]
    PlayGame,

    [Description("View History")]
    ViewHistory,

    [Description("Main Menu")]
    MainMenu
}