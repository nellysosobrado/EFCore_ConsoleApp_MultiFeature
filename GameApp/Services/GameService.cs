using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary;

namespace GameApp.Services;

public class GameService : IGameService
{
    private readonly IApplicationDbContext _context;
    private readonly Random _random;

    public GameService(IApplicationDbContext context)
    {
        _context = context;
        _random = new Random();
    }

    public GameMove GetComputerMove()
    {
        return (GameMove)_random.Next(0, 3);
    }

    public string DetermineWinner(GameMove playerMove, GameMove computerMove)
    {
        if (playerMove == computerMove)
            return "Draw";

        return (playerMove, computerMove) switch
        {
            (GameMove.Rock, GameMove.Scissors) => "Player",
            (GameMove.Paper, GameMove.Rock) => "Player",
            (GameMove.Scissors, GameMove.Paper) => "Player",
            _ => "Computer"
        };
    }

    public void SaveGame(Game game)
    {
        var totalGames = _context.Games.Count();
        var playerWins = _context.Games.Count(g => g.Winner == "Player");

        if (totalGames == 0)
        {
            game.AverageWinRate = game.Winner == "Player" ? 100.0 : 0.0;
        }
        else
        {
            if (game.Winner == "Player")
                playerWins++;

            game.AverageWinRate = (double)playerWins / (totalGames + 1) * 100;
        }

        _context.Games.Add(game);
        _context.SaveChanges();
    }

    public IEnumerable<Game> GetGameHistory()
    {
        return _context.Games
            .OrderByDescending(g => g.GameDate)
            .ToList();
    }

    public double CalculateWinPercentage()
    {
        var totalGames = _context.Games.Count();
        if (totalGames == 0) return 0;

        var wins = _context.Games.Count(g => g.Winner == "Player");
        return (double)wins / totalGames * 100;
    }
}