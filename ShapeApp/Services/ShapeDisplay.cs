using ClassLibrary.Enums;
using ClassLibrary.Models;
using ClassLibrary.Pagination;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Services
{
    public class ShapeDisplay : IShapeDisplay
    {
        private readonly IErrorService _errorService;
        private readonly ShapeRepository _shapeRepository;
        public ShapeDisplay(IErrorService errorService, ShapeRepository shapeRepository)
        {
            _errorService = errorService;
            _shapeRepository = shapeRepository;
        }
        public void ShowResult(Shape shape)
        {
            Console.Clear();
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[blue]Property[/]").LeftAligned())
                .AddColumn(new TableColumn("[green]Value[/]").RightAligned());

            table.AddRow("[blue]Shape Type[/]", $"[white]{shape.ShapeType}[/]");

            switch (shape.ShapeType)
            {
                case ShapeType.Rectangle:
                    table.AddRow("[cyan]Width[/]", $"[white]{shape.Width:F2}[/]");
                    table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                    break;
                case ShapeType.Parallelogram:
                    table.AddRow("[cyan]Base[/]", $"[white]{shape.BaseLength:F2}[/]");
                    table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                    table.AddRow("[cyan]Side[/]", $"[white]{shape.Side:F2}[/]");
                    break;
                case ShapeType.Triangle:
                    table.AddRow("[cyan]Side A[/]", $"[white]{shape.SideA:F2}[/]");
                    table.AddRow("[cyan]Side B[/]", $"[white]{shape.SideB:F2}[/]");
                    table.AddRow("[cyan]Side C[/]", $"[white]{shape.SideC:F2}[/]");
                    table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                    break;
                case ShapeType.Rhombus:
                    table.AddRow("[cyan]Side[/]", $"[white]{shape.Side:F2}[/]");
                    table.AddRow("[cyan]Height[/]", $"[white]{shape.Height:F2}[/]");
                    break;
            }

            // Lägg till area och omkrets
            table.AddRow("[magenta]Area[/]", $"[white]{shape.Area:F2}[/]");
            table.AddRow("[red]Perimeter[/]", $"[white]{shape.Perimeter:F2}[/]");

            AnsiConsole.Write(table);
            _errorService.WaitForKeyPress();
        }

        private string GetParametersString(Shape shape)
        {
            var parameters = new List<string>();

            switch (shape.ShapeType)
            {
                case ShapeType.Rectangle:
                    parameters.Add($"Width: {shape.Width:F2}");
                    parameters.Add($"Height: {shape.Height:F2}");
                    break;
                case ShapeType.Parallelogram:
                    parameters.Add($"Base: {shape.BaseLength:F2}");
                    parameters.Add($"Height: {shape.Height:F2}");
                    parameters.Add($"Side: {shape.Side:F2}");
                    break;
                case ShapeType.Triangle:
                    parameters.Add($"SideA: {shape.SideA:F2}");
                    parameters.Add($"SideB: {shape.SideB:F2}");
                    parameters.Add($"SideC: {shape.SideC:F2}");
                    parameters.Add($"Height: {shape.Height:F2}");
                    break;
                case ShapeType.Rhombus:
                    parameters.Add($"Side: {shape.Side:F2}");
                    parameters.Add($"Height: {shape.Height:F2}");
                    break;
            }

            return string.Join("\n", parameters);
        }

        public void ShowShapes(IEnumerable<Shape> shapes)
        {
            var pagination = new Pagination<Shape>(shapes, pageSize: 5);
            var showDeleted = false;

            while (true)
            {
                Console.Clear();
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
                    .AddColumn(new TableColumn("[green]Date[/]").Centered())
                    .AddColumn(new TableColumn("[blue]Shape[/]").Centered())
                    .AddColumn(new TableColumn("[cyan]Parameters[/]").LeftAligned())
                    .AddColumn(new TableColumn("[magenta]Area[/]").Centered())
                    .AddColumn(new TableColumn("[red]Perimeter[/]").Centered())
                    .AddColumn(new TableColumn("[grey]Status[/]").Centered());

                var filteredShapes = showDeleted ? shapes : shapes.Where(s => !s.IsDeleted);
                pagination = new Pagination<Shape>(filteredShapes, pageSize: 5);

                foreach (var shape in pagination.GetCurrentPage())
                {
                    var parameters = GetParametersString(shape);
                    var statusColor = shape.IsDeleted ? "red" : "green";
                    var status = shape.IsDeleted ? "Deleted" : "Not deleted";

                    table.AddRow(
                        $"[yellow]{shape.Id}[/]",
                        $"[green]{shape.CalculationDate:yyyy-MM-dd HH:mm:ss}[/]",
                        $"[blue]{shape.ShapeType}[/]",
                        $"[cyan]{parameters}[/]",
                        $"[magenta]{shape.Area:F2}[/]",
                        $"[red]{shape.Perimeter:F2}[/]",
                        $"[{statusColor}]{status}[/]"
                    );
                }

                AnsiConsole.Write(table);

                var choices = new List<string>
        {
            showDeleted ? "Show Active Only" : "Show All (Including Deleted)",
            "Next Page",
            "Previous Page",
            "Back to Menu"
        };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select an option:[/]")
                        .AddChoices(choices));

                switch (choice)
                {
                    case "Show All (Including Deleted)":
                        showDeleted = true;
                        break;
                    case "Show Active Only":
                        showDeleted = false;
                        break;
                    case "Next Page":
                        pagination.NextPage();
                        break;
                    case "Previous Page":
                        pagination.PreviousPage();
                        break;
                    case "Back to Menu":
                        return;
                }
            }
        }
        public IEnumerable<Shape> GetShapeHistory()
        {
            return _shapeRepository.GetAllShapes()
                .OrderByDescending(s => s.CalculationDate);
        }

    }
}
