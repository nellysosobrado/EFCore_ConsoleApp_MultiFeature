using GameApp.Services;
using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace GameApp.Controller;

public class GameController
{
    private readonly IGameService _gameService;
    private readonly IGameUIService _uiService;

    public GameController(IGameService gameService, IGameUIService uiService)
    {
        _gameService = gameService;
        _uiService = uiService;
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                var playerMove = _uiService.GetPlayerMove();
                var computerMove = _gameService.GetComputerMove();
                var result = _gameService.DetermineWinner(playerMove, computerMove);

                var game = new Game
                {
                    PlayerMove = playerMove,
                    ComputerMove = computerMove,
                    Result = result,
                    GameDate = DateTime.Now
                };

                _gameService.SaveGame(game);
                var winPercentage = _gameService.CalculateWinPercentage();
                _uiService.ShowGameResult(game, winPercentage);

                _uiService.WaitForKeyPress("\nPress Enter to play again or Esc to view history...");

                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    var history = _gameService.GetGameHistory();
                    _uiService.ShowGameHistory(history);
                    _uiService.WaitForKeyPress();
                }
            }
            catch (Exception ex)
            {
                _uiService.ShowError(ex.Message);
                _uiService.WaitForKeyPress();
            }
        }
    }
}