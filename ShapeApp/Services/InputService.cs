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
    public class InputService : IInputService
    {
        public double GetNumberInput(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<double>($"[green]{prompt}:[/]")
                    .ValidationErrorMessage("[red]Please enter a valid number[/]")
                    .Validate(n => n > 0 ? ValidationResult.Success() : ValidationResult.Error("Value must be greater than 0")));
        }

        public ShapeType GetShapeType()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<ShapeType>()
                    .Title("[green]Select a shape:[/]")
                    .UseConverter(type => type.ToString())
                    .AddChoices(Enum.GetValues<ShapeType>()));
        }
    }
}
