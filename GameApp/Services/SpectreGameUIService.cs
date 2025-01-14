using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GameApp.Services;

public class SpectreGameUIService : IGameUIService
{
    public GameMove GetPlayerMove()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<GameMove>()
                .Title("[green]Choose your move:[/]")
                .AddChoices(Enum.GetValues<GameMove>()));
    }

    public void ShowGameResult(Game game, double winPercentage)
    {
        var resultColor = game.Result switch
        {
            GameResult.Win => "green",
            GameResult.Loss => "red",
            _ => "yellow"
        };

        AnsiConsole.MarkupLine($"\nYour move: [blue]{game.PlayerMove}[/]");
        AnsiConsole.MarkupLine($"Computer's move: [blue]{game.ComputerMove}[/]");
        AnsiConsole.MarkupLine($"Result: [{resultColor}]{game.Result}[/]");
        AnsiConsole.MarkupLine($"Win percentage: [cyan]{winPercentage:F2}%[/]");
    }

    public void ShowGameHistory(IEnumerable<Game> games)
    {
        var table = new Spectre.Console.Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]Date[/]").Centered())
            .AddColumn(new TableColumn("[blue]Your Move[/]").Centered())
            .AddColumn(new TableColumn("[blue]Computer's Move[/]").Centered())
            .AddColumn(new TableColumn("[green]Result[/]").Centered());

        foreach (var game in games)
        {
            var resultColor = game.Result switch
            {
                GameResult.Win => "green",
                GameResult.Loss => "red",
                _ => "yellow"
            };

            table.AddRow(
                game.GameDate.ToString("yyyy-MM-dd HH:mm:ss"),
                game.PlayerMove.ToString(),
                game.ComputerMove.ToString(),
                $"[{resultColor}]{game.Result}[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }

    public void WaitForKeyPress(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine($"[grey]{message}[/]");
        Console.ReadKey(true);
    }
}