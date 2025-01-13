using ClassLibrary.Enums;

namespace CalculatorApp.Services;

public class SquareRootCalculator
{
    public (double firstRoot, double secondRoot) CalculateRoots(double operand1, double operand2)
    {
        return (
            Math.Round(Math.Sqrt(operand1), 2),
            Math.Round(Math.Sqrt(operand2), 2)
        );
    }
}