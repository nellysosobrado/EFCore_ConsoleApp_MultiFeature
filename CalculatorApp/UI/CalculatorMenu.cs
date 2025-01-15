using Spectre.Console;
using CalculatorApp.Enums;
using ClassLibrary.Extensions;
using ClassLibrary.Enums.CalculatorAppEnums;

namespace CalculatorApp.UI;

public class CalculatorMenu
{
    public CalculatorMenuOptions ShowMainMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<CalculatorMenuOptions>()
                .Title("[italic yellow]Calculator Menu[/]")
                .WrapAround(true)
                .PageSize(5)
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<CalculatorMenuOptions>()));
    }
    public DeleteMenuOptions ShowMenuAfterDelete()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<DeleteMenuOptions>()
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<DeleteMenuOptions>()));
    }
    public PerformMenuOptions ShowMenuAfterCalc()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<PerformMenuOptions>()
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<PerformMenuOptions>()));
    }
    public UpdateMenuOptions ShowMenuAfterUpdate()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<UpdateMenuOptions>()
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<UpdateMenuOptions>()));
    }

}