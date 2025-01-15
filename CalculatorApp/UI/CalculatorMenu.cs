using Spectre.Console;
using CalculatorApp.Enums;
using ClassLibrary.Extensions;

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

    //public string ShowMenuAfterCalc()
    //{
    //    Console.Clear();
        
    //    return AnsiConsole.Prompt(
    //        new SelectionPrompt<string>()
    //            .AddChoices(new[]
    //            {
    //                "New Calculation",
    //                "Calculator Menu"
    //            }));
        
    //}

    public string ShowMenuAfterUpdate()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]What would you like to do next?[/]")
                .AddChoices(new[]
                {
                    "Update Calculation",
                    "Calculator Menu"
                }));
    }

    //public string ShowMenuAfterDelete()
    //{
    //    return AnsiConsole.Prompt(
    //        new SelectionPrompt<string>()
    //            .AddChoices(new[]
    //            {
    //                "Back",
    //                "Calculator Menu"
    //            }));
    //}
    public DeleteMenuOptions ShowMenuAfterDelete()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<DeleteMenuOptions>()
                .Title("[green]What would you like to do next?[/]")
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<DeleteMenuOptions>()));
    }
    public PerformMenuOptions ShowMenuAfterCalc()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<PerformMenuOptions>()
                .Title("[green]What would you like to do next?[/]")
                .UseConverter(opt => opt.GetDescription())
                .AddChoices(Enum.GetValues<PerformMenuOptions>()));
    }

}