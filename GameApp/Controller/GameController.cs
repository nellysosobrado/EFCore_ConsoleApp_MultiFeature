using ClassLibrary.Models;
using ClassLibrary.Extensions;
using Spectre.Console;
using GameApp.Interfaces;
using ClassLibrary.Enums.RpsGameEnums;

namespace GameApp.Controller;

public class GameController
{
    private readonly IGameService _gameService;
    private readonly IPlayerInput _playerInput;
    private readonly IPlayGame _playGame;
    private readonly IGameError _gameError;
    private readonly IDisplayRspGame _displayGame;

    public GameController(IGameService gameService, 
        IPlayerInput uiService, 
        IPlayGame playGame, 
        IGameError gameError, 
        IDisplayRspGame displayGame)
    {
        _gameService = gameService;
        _playerInput = uiService;
        _playGame = playGame;
        _gameError = gameError;
        _displayGame = displayGame;
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
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
            var game = _playGame.CreateGame();
            _playGame.ProcessGameResult(game);

            if (!_playerInput.ShouldPlayAgain())
            {
                return;
            }
        }
        catch (Exception ex)
        {
            _gameError.HandleGameError(ex);
        }
    }


    private void ShowHistory()
    {
        try
        {
            var history = _gameService.GetGameHistory();
            _displayGame.ShowGameHistory(history);
            _playerInput.WaitForKeyPress();
        }
        catch (Exception ex)
        {
            _gameError.ShowError(ex.Message);
            _playerInput.WaitForKeyPress();
        }
    }
}