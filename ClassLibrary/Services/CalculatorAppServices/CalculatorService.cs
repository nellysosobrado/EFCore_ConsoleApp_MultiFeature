using ClassLibrary.Data;
using ClassLibrary.Models;

namespace ClassLibrary.Services.CalculatorAppServices;

public class CalculatorService
{
    private readonly ApplicationDbContext _context;

    public CalculatorService(ApplicationDbContext context)
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
}
