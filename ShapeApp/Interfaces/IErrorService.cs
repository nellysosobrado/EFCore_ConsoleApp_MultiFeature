using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Interfaces
{
    public interface IErrorService
    {
        void ShowError(string message);
        void WaitForKeyPress(string message = "\nPress any key to continue...");
    }
}
