using Spectre.Console;

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
                new SelectionPrompt<string>()
                    .Title("[green]Choose an option:[/]")
                    .WrapAround(true)
                    .AddChoices(new[] { "Start Calculator", "Exit" }));

            switch (option)
            {
                case "Start Calculator":
                    break;


                case "Exit":
                    AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
                    Environment.Exit(0);
                    break;

                default:
                    AnsiConsole.MarkupLine("[yellow]Invalid option. Try again![/]");
                    break;
            }
        }

      
    }
}
