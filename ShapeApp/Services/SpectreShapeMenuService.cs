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

        public ShapeType GetShapeType()
        {
            return _inputService.GetShapeType();
        }

        public Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters)
        {
            var parameters = new Dictionary<string, double>();
            RenderParameterTable(requiredParameters, parameters);

            foreach (var param in requiredParameters)
            {
                try
                {
                    var value = _inputService.GetNumberInput($"\n[white]Enter[/] [green]{param.Key}[/]");
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
