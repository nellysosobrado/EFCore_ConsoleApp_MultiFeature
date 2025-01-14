using ClassLibrary.Models;
using ClassLibrary.Extensions;
using ClassLibrary.Enums;
using Spectre.Console;
using GameApp.Interfaces;

namespace GameApp.Controller;

public class GameController
{
    private readonly IGameService _gameService;
    private readonly IGameUIService _uiService;
    private readonly IPlayGame _playGame;

    public GameController(IGameService gameService, IGameUIService uiService, IPlayGame playGame)
    {
        _gameService = gameService;
        _uiService = uiService;
        _playGame = playGame;
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

            if (!_playGame.ShouldPlayAgain())
            {
                return;
            }
        }
        catch (Exception ex)
        {
            _playGame.HandleGameError(ex);
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