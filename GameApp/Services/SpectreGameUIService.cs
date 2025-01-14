using Spectre.Console;
using ClassLibrary.Models;
using ClassLibrary.Enums;

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
        var resultColor = game.Winner switch
        {
            "Player" => "green",
            "Computer" => "red",
            _ => "yellow"
        };

        AnsiConsole.MarkupLine($"\nYour move: [blue]{game.PlayerMove}[/]");
        AnsiConsole.MarkupLine($"Computer's move: [blue]{game.ComputerMove}[/]");
        AnsiConsole.MarkupLine($"Winner: [{resultColor}]{game.Winner}[/]");
        AnsiConsole.MarkupLine($"Current win percentage: [cyan]{winPercentage:F2}%[/]");
        AnsiConsole.MarkupLine($"Average win rate: [cyan]{game.AverageWinRate:F2}%[/]");
    }

    public void ShowGameHistory(IEnumerable<Game> games)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[yellow]Date[/]").Centered())
            .AddColumn(new TableColumn("[blue]Your Move[/]").Centered())
            .AddColumn(new TableColumn("[blue]Computer's Move[/]").Centered())
            .AddColumn(new TableColumn("[green]Winner[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Avg Win Rate[/]").Centered());

        foreach (var game in games)
        {
            var resultColor = game.Winner switch
            {
                "Player" => "green",
                "Computer" => "red",
                _ => "yellow"
            };

            table.AddRow(
                game.GameDate.ToString("yyyy-MM-dd HH:mm:ss"),
                game.PlayerMove.ToString(),
                game.ComputerMove.ToString(),
                $"[{resultColor}]{game.Winner}[/]",
                $"{game.AverageWinRate:F2}%"
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
