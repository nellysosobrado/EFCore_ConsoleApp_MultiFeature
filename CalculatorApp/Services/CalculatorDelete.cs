using CalculatorApp.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Services
{
    public class CalculatorDelete : ICalculatorDelete
    {
        public int GetCalculationIdForDelete()
        {
            return AnsiConsole.Ask<int>("Enter the [green]ID[/] of the calculation to delete:");
        }

        public bool ConfirmDeletion()
        {
            Console.Clear();
            return AnsiConsole.Confirm("Are you sure you want to delete this calculation?");
        }
    }
}
