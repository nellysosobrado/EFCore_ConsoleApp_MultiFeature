using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace ClassLibrary.Repositories.ShapeAppRepository;

public class ShapeRepository
{
    private readonly IApplicationDbContext _context;

    public ShapeRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public void AddShape(Shape shape)
    {
        _context.Shapes.Add(shape);
        _context.SaveChanges();
    }

    public List<Shape> GetAllShapes()
    {
        return _context.Shapes.ToList();
    }

    public Shape GetShapeById(int id)
    {
        return _context.Shapes.Find(id)
            ?? throw new InvalidOperationException("Shape not found");
    }

    public void UpdateShape(Shape shape)
    {
        var existing = _context.Shapes.Find(shape.Id)
            ?? throw new InvalidOperationException("Shape not found");

        existing.ShapeType = shape.ShapeType;
        existing.Area = shape.Area;
        existing.Perimeter = shape.Perimeter;
        existing.Parameters = shape.Parameters;

        _context.SaveChanges();
    }

    public void DeleteShape(int id)
    {
        var shape = _context.Shapes.Find(id)
            ?? throw new InvalidOperationException("Shape not found");

        _context.Shapes.Remove(shape);
        _context.SaveChanges();
    }

    public void SeedData()
    {
        if (_context.Shapes.Any()) return;

        var shapes = new List<Shape>
        {
            new()
            {
                ShapeType = ShapeType.Rectangle,
                Area = 20,
                Perimeter = 18,
                Parameters = new Dictionary<string, double>
                {
                    { "Width", 4 },
                    { "Height", 5 }
                },
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Parallelogram,
                Area = 24,
                Perimeter = 20,
                Parameters = new Dictionary<string, double>
                {
                    { "Base", 6 },
                    { "Height", 4 },
                    { "Side", 4 }
                },
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Triangle,
                Area = 12,
                Perimeter = 12,
                Parameters = new Dictionary<string, double>
                {
                    { "SideA", 4 },
                    { "SideB", 4 },
                    { "SideC", 4 },
                    { "Height", 6 }
                },
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Rhombus,
                Area = 16,
                Perimeter = 16,
                Parameters = new Dictionary<string, double>
                {
                    { "Side", 4 },
                    { "Height", 4 }
                },
                CalculationDate = DateTime.Now
            }
        };

        _context.Shapes.AddRange(shapes);
        _context.SaveChanges();
    }
}