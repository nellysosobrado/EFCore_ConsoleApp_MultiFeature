using ClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Interfaces
{
    public interface IInputService
    {
        double GetNumberInput(string prompt);
        ShapeType GetShapeType();
        Dictionary<string, double> GetShapeParameters(Dictionary<string, double> requiredParameters);
    }
}
