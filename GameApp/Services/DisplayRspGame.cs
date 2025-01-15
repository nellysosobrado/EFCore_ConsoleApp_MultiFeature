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
            Console.Clear();

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Player Move")
                .AddColumn("Computer Move")
                .AddColumn("Result");

            var resultColor = game.Winner switch
            {
                "Win" => "green",
                "Loss" => "red",
                _ => "yellow"
            };

            table.AddRow(
                game.PlayerMove.ToString(),
                game.ComputerMove.ToString(),
                $"[{resultColor}]{game.Winner}[/]"
            );

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\nWin Rate (all games): [blue]{winPercentage:F1}%[/]");
        }

        public void HandleGameError(Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
        }
        public void ShowGameHistory(IEnumerable<Game> history)
        {
            var pagination = new Pagination<Game>(history, pageSize: 5);

            while (true)
            {
                Console.Clear();
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
                    .AddColumn(new TableColumn("[green]Date[/]").Centered())
                    .AddColumn(new TableColumn("[blue]Player[/]").Centered())
                    .AddColumn(new TableColumn("[cyan]Computer[/]").Centered())
                    .AddColumn(new TableColumn("[magenta]Result[/]").Centered());

                foreach (var game in pagination.GetCurrentPage())
                {
                    var resultColor = game.Winner switch
                    {
                        "Win" => "green",
                        "Loss" => "red",
                        _ => "yellow"
                    };

                    table.AddRow(
                        $"[white]{game.Id}[/]",
                        $"[white]{game.GameDate:yyyy-MM-dd HH:mm:ss}[/]",
                        $"[white]{game.PlayerMove}[/]",
                        $"[white]{game.ComputerMove}[/]",
                        $"[{resultColor}]{game.Winner}[/]"
                    );
                }

                AnsiConsole.Write(table);

                var currentWinRate = history.Any()
                    ? history.First().AverageWinRate
                    : 0;

                AnsiConsole.MarkupLine($"\nWin Rate (all games): [blue]{currentWinRate:F1}%[/]");

                var choice = PaginationRenderer.ShowPaginationControls(pagination);
                if (choice == "Back to Menu")
                {
                    break;
                }
            }
        }
    }
}
