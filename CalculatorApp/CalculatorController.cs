using ClassLibrary.DataAccess;
using ClassLibrary.Enums;
using ClassLibrary.Models;
using ClassLibrary.Services;

namespace CalculatorApp.Controllers;

public class CalculatorController
{
    private readonly CalculatorService _service;

    public CalculatorController()
    {
        var accessDatabase = new AccessDatabase();
        var dbContext = accessDatabase.GetDbContext();
        _service = new CalculatorService(dbContext);
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Calc");
            Console.WriteLine("1. Calculate");
            Console.WriteLine("2. Histroy");
            Console.WriteLine("3. Main Menu");
            Console.Write(">");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PerformCalculation();
                    break;

                case "2":
                    ShowCalculations();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("ERROR.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void PerformCalculation()
    {
        Console.Write("First number: ");
        var operand1 = double.Parse(Console.ReadLine());

        Console.Write("Second Number ");
        var operand2 = double.Parse(Console.ReadLine());

        Console.Write("Choose (+, -, *, /, %): ");
        var operatorInput = Console.ReadLine();

        if (!TryParseOperator(operatorInput, out CalculatorOperator calculatorOperator))
        {
            Console.WriteLine("Error, invalid operator");
            Console.ReadKey();
            return;
        }

        double result = calculatorOperator switch
        {
            CalculatorOperator.Add => operand1 + operand2,
            CalculatorOperator.Subtract => operand1 - operand2,
            CalculatorOperator.Multiply => operand1 * operand2,
            CalculatorOperator.Divide => operand2 != 0 ? operand1 / operand2 : double.NaN,
            CalculatorOperator.Modulus => operand1 % operand2,
            _ => throw new InvalidOperationException("Invalid operator")
        };

        var calculation = new Calculator
        {
            Operand1 = operand1,
            Operand2 = operand2,
            Operator = calculatorOperator,
            Result = Math.Round(result, 2),
            CalculationDate = DateTime.Now
        };

        _service.AddCalculation(calculation);

        Console.WriteLine($"Result: {calculation.Result}");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }
    private bool TryParseOperator(string input, out CalculatorOperator calculatorOperator)
    {
        switch (input)
        {
            case "+":
                calculatorOperator = CalculatorOperator.Add;
                return true;
            case "-":
                calculatorOperator = CalculatorOperator.Subtract;
                return true;
            case "*":
                calculatorOperator = CalculatorOperator.Multiply;
                return true;
            case "/":
                calculatorOperator = CalculatorOperator.Divide;
                return true;
            case "%":
                calculatorOperator = CalculatorOperator.Modulus;
                return true;
            default:
                calculatorOperator = default;
                return false;
        }
    }

    private void ShowCalculations()
    {
        var calculations = _service.GetAllCalculations();
        foreach (var calc in calculations)
        {
            Console.WriteLine($"{calc.CalculationDate}: {calc.Operand1} {calc.Operator} {calc.Operand2} = {calc.Result}");
        }

        Console.WriteLine("Press any key to continue to main menu..");
        Console.ReadKey();
    }
}
