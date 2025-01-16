using GameApp.Interfaces;
using ClassLibrary.Models;

namespace GameApp.Services;

public class PlayGame : IPlayGame
{
    private readonly IGameService _gameService;
    private readonly IPlayerInput _playerInput;
    private readonly IDisplayRspGame _displayGame;

    public PlayGame(
        IGameService gameService,
        IPlayerInput uiService,
        IDisplayRspGame displayGame)
    {
        _gameService = gameService;
        _playerInput = uiService;
        _displayGame = displayGame;
    }

    public Game CreateGame()
    {
        var playerMove = _playerInput.GetPlayerMove();
        var computerMove = _gameService.GetComputerMove();
        var winner = _gameService.DetermineWinner(playerMove, computerMove);

        return new Game
        {
            PlayerMove = playerMove,
            ComputerMove = computerMove,
            Winner = winner,
            GameDate = DateTime.Now
        };
    }

    public void ProcessGameResult(Game game)
    {
        _gameService.SaveGame(game);
        var winPercentage = _gameService.CalculateWinPercentage();
        game.AverageWinRate = winPercentage;
        _displayGame.ShowGameResult(game, winPercentage);
        _playerInput.WaitForKeyPress();
    }
}