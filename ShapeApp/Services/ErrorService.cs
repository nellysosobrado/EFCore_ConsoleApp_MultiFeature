﻿using ShapeApp.Interfaces;
using Spectre.Console;


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
