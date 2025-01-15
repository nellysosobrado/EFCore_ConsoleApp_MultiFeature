using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.UITool;
using GameApp.Interfaces;
using ClassLibrary.Enums.RpsGameEnums;

namespace GameApp.Services;

public class PlayerInput : IPlayerInput
{
    public GameMove GetPlayerMove()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<GameMove>()
                .Title("[green]Choose your move:[/]")
                .AddChoices(Enum.GetValues<GameMove>()));
    }
    public bool ShouldPlayAgain()
    {
        Console.Clear();
        var playAgain = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(new[] {
                    "Play Again",
                    "Back to Game Menu"
                }));

        return playAgain != "Back to Game Menu";
    }

    public void WaitForKeyPress(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine($"[grey]{message}[/]");
        Console.ReadKey(true);
    }
}
