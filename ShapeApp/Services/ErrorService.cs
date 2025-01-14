using ShapeApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Services
{
    public class ErrorService : IErrorService
    {
        public void ShowError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
        }

        public void WaitForKeyPress(string message = "\nPress any key to continue...")
        {
            AnsiConsole.MarkupLine($"[grey]{message}[/]");
            Console.ReadKey(true);
        }
    }
}
