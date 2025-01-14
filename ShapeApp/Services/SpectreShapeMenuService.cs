using ClassLibrary.Enums;
using ShapeApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Services
{
    public class SpectreShapeMenuService : IShapeMenuService
    {
        private readonly IInputService _inputService;
        private readonly IErrorService _errorService;

        public SpectreShapeMenuService(IInputService inputService, IErrorService errorService)
        {
            _inputService = inputService;
            _errorService = errorService;
        }

        public string ShowMainMenu()
        {
            AnsiConsole.Clear();
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Shape Calculator[/]")
                    .AddChoices(new[]
                    {
                    "1. New Calculation",
                    "2. View History",
                    "3. Update Calculation",
                    "4. Delete Calculation",
                    "5. Main Menu"
                    }));
        }

        
    }
}
