using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Interfaces
{
    public interface ICalculatorDelete
    {
        int GetCalculationIdForDelete();
        bool ConfirmDeletion();
    }
}
