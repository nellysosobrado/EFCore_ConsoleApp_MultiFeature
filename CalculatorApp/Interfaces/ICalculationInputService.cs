using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ICalculationInputService
    {
        Calculator GetCalculationById(int id);
        (double operand1, double operand2, string operatorInput) GetUserInput();
        IEnumerable<Calculator> GetCalculationHistory();
        bool ShouldChangeOperator();
        int GetCalculationIdForUpdate();

    }
}
