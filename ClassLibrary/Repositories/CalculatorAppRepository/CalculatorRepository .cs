using ClassLibrary.Models;

namespace ClassLibrary.Repositories.CalculatorAppRepository;

public class CalculatorRepository
{
    private readonly IApplicationDbContext _context;

    public CalculatorRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public void AddCalculation(Calculator calculator)
    {
        _context.Calculations.Add(calculator);
        _context.SaveChanges();
    }

    public List<Calculator> GetAllCalculations()
    {
        return _context.Calculations.ToList();
    }

    public Calculator GetCalculationById(int id)
    {
        return _context.Calculations.Find(id)
            ?? throw new InvalidOperationException("Calculation not found");
    }

    public void UpdateCalculation(Calculator calculator)
    {
        var existing = _context.Calculations.Find(calculator.Id)
            ?? throw new InvalidOperationException("Calculation not found");

        existing.Operand1 = calculator.Operand1;
        existing.Operand2 = calculator.Operand2;
        existing.Operator = calculator.Operator;
        existing.Result = calculator.Result;

        _context.SaveChanges();
    }

    public void DeleteCalculation(int id)
    {
        var calculation = _context.Calculations.Find(id)
            ?? throw new InvalidOperationException("Calculation not found");

        _context.Calculations.Remove(calculation);
        _context.SaveChanges();
    }
}