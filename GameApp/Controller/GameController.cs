using GameApp.Services;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Extensions;
using GameApp.Enums;
using Spectre.Console;

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
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<GameMenuOptions>()
                    .Title("[green]Rock Paper Scissors Menu[/]")
                    .UseConverter(option => option.GetDescription())
                    .AddChoices(Enum.GetValues<GameMenuOptions>()));

            switch (option)
            {
                case GameMenuOptions.PlayGame:
                    PlayGame();
                    break;
                case GameMenuOptions.ViewHistory:
                    ShowHistory();
                    break;
                case GameMenuOptions.MainMenu:
                    return;
            }
        }
    }

    private void PlayGame()
    {
        try
        {
            var playerMove = _uiService.GetPlayerMove();
            var computerMove = _gameService.GetComputerMove();
            var result = _gameService.DetermineWinner(playerMove, computerMove);

            var game = new ClassLibrary.Models.Game
            {
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Result = result,
                GameDate = DateTime.Now
            };

            _gameService.SaveGame(game);
            var winPercentage = _gameService.CalculateWinPercentage();
            _uiService.ShowGameResult(game, winPercentage);
            _uiService.WaitForKeyPress();
        }
        catch (Exception ex)
        {
            _uiService.ShowError(ex.Message);
            _uiService.WaitForKeyPress();
        }
    }

    private void ShowHistory()
    {
        try
        {
            var history = _gameService.GetGameHistory();
            _uiService.ShowGameHistory(history);
            _uiService.WaitForKeyPress();
        }
        catch (Exception ex)
        {
            _uiService.ShowError(ex.Message);
            _uiService.WaitForKeyPress();
        }
    }
}