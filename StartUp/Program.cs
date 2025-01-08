using Spectre.Console;
using StartUp;

namespace StartUp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("[green]Choose an option:[/]")
                    .UseConverter(option => option.GetDescription()) 
                    .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (option)
            {
                case MenuOptions.StartCalculator:
                    break;

                case MenuOptions.StartShapes:
                    break;

                case MenuOptions.Exit:
                    AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
