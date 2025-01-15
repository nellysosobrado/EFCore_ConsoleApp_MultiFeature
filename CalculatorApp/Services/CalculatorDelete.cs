using CalculatorApp.Interfaces;
using ClassLibrary.Repositories.CalculatorAppRepository;
using Spectre.Console;


namespace CalculatorApp.Services
{
    public class CalculatorDelete : ICalculatorDelete
    {
        private readonly CalculatorRepository _calculatorRepository;

        public CalculatorDelete(CalculatorRepository calculatorRepository)
        {
            _calculatorRepository = calculatorRepository;
        }

        public int GetCalculationIdForDelete()
        {
            return AnsiConsole.Ask<int>("Enter the [green]ID[/] of the calculation to delete:");
        }

        public bool ConfirmDeletion()
        {
            Console.Clear();
            return AnsiConsole.Confirm("Are you sure you want to delete this calculation?");
        }
        public void DeleteCalculation(int id)
        {
            _calculatorRepository.DeleteCalculation(id);
        }

    }
}
