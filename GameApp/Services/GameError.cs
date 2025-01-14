using GameApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApp.Services
{
    public class GameError : IGameError
    {
        private readonly IGameUIService _uiService;

        public GameError(IGameUIService uiService)
        {
            _uiService = uiService;
        }

        public void HandleGameError(Exception ex)
        {
            ShowError(ex.Message);
            _uiService.WaitForKeyPress();
        }
        public void ShowError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
        }
    }
}
