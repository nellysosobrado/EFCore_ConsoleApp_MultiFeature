﻿using ClassLibrary.Enums;
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
        private readonly IErrorService _errorService;

        public InputService(IErrorService errorService)
        {
            _errorService = errorService;
        }

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
        public Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters)
        {
            var parameters = new Dictionary<string, double>();
            RenderParameterTable(requiredParameters, parameters);

            foreach (var param in requiredParameters)
            {
                try
                {
                    var value = GetNumberInput($"\n[white]Enter[/] [green]{param.Key}[/]");
                    parameters[param.Key] = value;
                    RenderParameterTable(requiredParameters, parameters);
                }
                catch (Exception ex)
                {
                    _errorService.ShowError(ex.Message);
                }
            }

            if (!AnsiConsole.Confirm("\n[yellow]Are these values correct?[/]"))
            {
                return GetShapeParameters(requiredParameters);
            }

            return parameters;
        }

        private void RenderParameterTable(Dictionary<string, double> requiredParameters, Dictionary<string, double> currentValues)
        {
            AnsiConsole.Clear();

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[blue]Parameter[/]").Centered())
                .AddColumn(new TableColumn("[green]Value[/]").Centered());

            foreach (var param in requiredParameters)
            {
                table.AddRow(
                    param.Key,
                    currentValues.ContainsKey(param.Key)
                        ? $"{currentValues[param.Key]:F2}"
                        : "-"
                );
            }

            AnsiConsole.Write(table);
        }
       
    }
}
