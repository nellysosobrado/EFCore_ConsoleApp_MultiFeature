using GameApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;

namespace GameApp.Services
{
    public class PlayGame : IPlayGame
    {
        private readonly IGameService _gameService;
        private readonly IPlayerInput _uiService;
        private readonly IDisplayRspGame _displayGame;

        public PlayGame(
            IGameService gameService,
            IPlayerInput uiService, 
            IDisplayRspGame displayGame)
        {
            _gameService = gameService;
            _uiService = uiService;
            _displayGame = displayGame;
        }



        public Game CreateGame()
        {
            var playerMove = _uiService.GetPlayerMove();
            var computerMove = _gameService.GetComputerMove();
            var winner = _gameService.DetermineWinner(playerMove, computerMove);

            return new ClassLibrary.Models.Game
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
            _displayGame.ShowGameResult(game, winPercentage);
            _uiService.WaitForKeyPress();
        }

        public bool ShouldPlayAgain()
        {
            Console.Clear();
            var playAgain = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] {
                    "Play Again",
                    "Back to Game Menu"
                    }));

            return playAgain != "Back to Game Menu";
        }

       
    }
}
