using ClassLibrary.Models;
using ClassLibrary.Pagination;
using GameApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApp.Services
{
    public class DisplayRspGame : IDisplayRspGame
    {
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
            var pagination = new Pagination<Game>(games, pageSize: 5);

            while (true)
            {
                Console.Clear();
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("[yellow]Date[/]").Centered())
                    .AddColumn(new TableColumn("[blue]Your Move[/]").Centered())
                    .AddColumn(new TableColumn("[blue]Computer's Move[/]").Centered())
                    .AddColumn(new TableColumn("[green]Winner[/]").Centered())
                    .AddColumn(new TableColumn("[cyan]Avg Win Rate[/]").Centered());

                foreach (var game in pagination.GetCurrentPage())
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

                var choice = PaginationRenderer.ShowPaginationControls(pagination);

                if (choice == "Back to Menu")
                    break;
            }
        }
    }
}
