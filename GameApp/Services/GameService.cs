using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Data;
using ClassLibrary;

namespace GameApp.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    private readonly Random _random;

    public GameService(ApplicationDbContext context)
    {
        _context = context;
        _random = new Random();
    }

    public GameMove GetComputerMove()
    {
        return (GameMove)_random.Next(0, 3);
    }

    public GameResult DetermineWinner(GameMove playerMove, GameMove computerMove)
    {
        if (playerMove == computerMove)
            return GameResult.Draw;

        return (playerMove, computerMove) switch
        {
            (GameMove.Rock, GameMove.Scissors) => GameResult.Win,
            (GameMove.Paper, GameMove.Rock) => GameResult.Win,
            (GameMove.Scissors, GameMove.Paper) => GameResult.Win,
            _ => GameResult.Loss
        };
    }

    public void SaveGame(Game game)
    {
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

        var wins = _context.Games.Count(g => g.Result == GameResult.Win);
        return (double)wins / totalGames * 100;
    }
}