using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Interface;

namespace ClassLibrary.Repositories.ShapeAppRepository;

public class ShapeRepository
{
    private readonly IApplicationDbContext _context;

    public ShapeRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    public List<Shape> GetAllShapes()
    {
        return _context.Shapes
            .OrderByDescending(s => s.CalculationDate)
            .ToList();
    }


    public Shape GetShapeById(int id)
    {
        return _context.Shapes
            .FirstOrDefault(s => s.Id == id && !s.IsDeleted)
            ?? throw new InvalidOperationException("Shape not found");
    }

    public void SoftDeleteShape(int id)
    {
        var shape = _context.Shapes.Find(id)
            ?? throw new InvalidOperationException("Shape not found");

        shape.IsDeleted = true;
        shape.DeletedAt = DateTime.Now;

        _context.SaveChanges();
    }

    public void AddShape(Shape shape)
    {
        _context.Shapes.Add(shape);
        _context.SaveChanges();
    }



    public void UpdateShape(Shape shape)
    {
        var existing = _context.Shapes.Find(shape.Id)
            ?? throw new InvalidOperationException("Shape not found");

        existing.ShapeType = shape.ShapeType;
        existing.Area = shape.Area;
        existing.Perimeter = shape.Perimeter;
        existing.CalculationDate = shape.CalculationDate;

        switch (shape.ShapeType)
        {
            case ShapeType.Rectangle:
                existing.Width = shape.Width;
                existing.Height = shape.Height;
                break;
            case ShapeType.Parallelogram:
                existing.BaseLength = shape.BaseLength;
                existing.Height = shape.Height;
                existing.Side = shape.Side;
                break;
            case ShapeType.Triangle:
                existing.SideA = shape.SideA;
                existing.SideB = shape.SideB;
                existing.SideC = shape.SideC;
                existing.Height = shape.Height;
                break;
            case ShapeType.Rhombus:
                existing.Side = shape.Side;
                existing.Height = shape.Height;
                break;
        }

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
                Width = 4,
                Height = 5,
                Area = 20,
                Perimeter = 18,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Parallelogram,
                BaseLength = 6,
                Height = 4,
                Side = 4,
                Area = 24,
                Perimeter = 20,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Triangle,
                SideA = 4,
                SideB = 4,
                SideC = 4,
                Height = 6,
                Area = 12,
                Perimeter = 12,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Rhombus,
                Side = 4,
                Height = 4,
                Area = 16,
                Perimeter = 16,
                CalculationDate = DateTime.Now
            }
        };

        _context.Shapes.AddRange(shapes);
        _context.SaveChanges();
    }
}