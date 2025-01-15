using CalculatorApp.Interfaces;
using ClassLibrary.Enums;

namespace CalculatorApp.Services;

public class SquareRootCalculator : ISquareRootCalculator
{
    public (double firstRoot, double secondRoot) CalculateRoots(double operand1, double operand2)
    {
        return (
            Math.Round(Math.Sqrt(operand1), 2),
            Math.Round(Math.Sqrt(operand2), 2)
        );
    }

    public (double firstResult, double secondResult)? CalculateSquareRoots(double operand1, double operand2)
    {
        if (operand1 < 0 || operand2 < 0)
        {
            throw new InvalidOperationException("Cannot calculate square root of negative numbers");
        }
        return (Math.Sqrt(operand1), Math.Sqrt(operand2));
    }
}