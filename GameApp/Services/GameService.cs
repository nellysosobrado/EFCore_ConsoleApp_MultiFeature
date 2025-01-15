using ClassLibrary.Models;
using ClassLibrary.Repositories.RpsGameRepository;
using GameApp.Interfaces;
using ClassLibrary.Enums.RpsGameEnums;

namespace GameApp.Services;

public class GameService : IGameService
{
    private readonly RpsGameRepository _repository;
    private readonly Random _random = new();

    public GameService(RpsGameRepository repository)
    {
        _repository = repository;
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
            (GameMove.Rock, GameMove.Scissors) => "Win",
            (GameMove.Paper, GameMove.Rock) => "Win",
            (GameMove.Scissors, GameMove.Paper) => "Win",
            _ => "Loss"
        };
    }

    public void SaveGame(Game game)
    {
        game.AverageWinRate = CalculateWinPercentage();
        _repository.AddGame(game);
    }

    public IEnumerable<Game> GetGameHistory()
    {
        return _repository.GetAllGames();
    }

    public double CalculateWinPercentage()
    {
        return _repository.CalculateWinPercentage();
    }
}