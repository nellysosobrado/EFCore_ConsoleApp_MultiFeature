using ClassLibrary.Models;
using ClassLibrary.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories.RpsGameRepository;

public class RpsGameRepository
{
    private readonly IApplicationDbContext _context;

    public RpsGameRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public void AddGame(Game game)
    {
        _context.Games.Add(game);
        _context.SaveChanges();
    }

    public List<Game> GetAllGames()
    {
        return _context.Games
            .OrderByDescending(g => g.GameDate)
            .ToList();
    }

    public Game GetGameById(int id)
    {
        return _context.Games.Find(id)
            ?? throw new InvalidOperationException("Game not found");
    }

    public int GetTotalGamesPlayed()
    {
        return _context.Games
            .IgnoreQueryFilters() 
            .Count();
    }

    public double CalculateWinPercentage()
    {
        var totalGames = GetTotalGamesPlayed();
        if (totalGames == 0) return 0;

        var wins = _context.Games
            .IgnoreQueryFilters()
            .Count(g => g.Winner == "Win");

        return Math.Round((double)wins / totalGames * 100, 2);
    }

   
}