using ClassLibrary.Interface;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

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

    public Calculator GetCalculationById(int id)
    {
        return _context.Calculations.Find(id)
            ?? throw new InvalidOperationException("Calculation not found");
    }

    public void UpdateCalculation(Calculator calculator)
    {
        var existing = _context.Calculations.Find(calculator.Id)
            ?? throw new InvalidOperationException("Calculation not found");

        existing.FirstNumber = calculator.FirstNumber;
        existing.SecondNumber = calculator.SecondNumber;
        existing.Operator = calculator.Operator;
        existing.Result = calculator.Result;
        existing.CalculationDate = DateTime.Now;

        _context.SaveChanges();
    }


    public void DeleteCalculation(int id)
    {
        var calculation = _context.Calculations
            .IgnoreQueryFilters()
            .FirstOrDefault(c => c.Id == id)
            ?? throw new InvalidOperationException("Calculation not found");

        if (calculation.IsDeleted)
        {
            throw new InvalidOperationException("This calculation has already been deleted");
        }

        calculation.IsDeleted = true;
        calculation.DeletedAt = DateTime.Now;
        _context.SaveChanges();
    }
    public List<Calculator> GetAllCalculations()
    {
        return _context.Calculations
            .OrderByDescending(c => c.CalculationDate)
            .ToList();
    }
}