using ClassLibrary.Models;
using ClassLibrary.Services.Shapes;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Validators;
using FluentValidation;
using System.Drawing;

namespace ShapeApp.Services;

public class ShapeOperationService : IShapeOperationService
{
    private readonly ShapeRepository _shapeRepository;
    private readonly ShapeValidator _validator;
    private readonly Dictionary<string, IShape> _shapes;

    public ShapeOperationService(ShapeRepository shapeRepository, ShapeValidator validator)
    {
        _shapeRepository = shapeRepository;
        _validator = validator;
        _shapes = new Dictionary<string, IShape>
        {
            { "Rectangle", new Rectangle() },
            { "Parallelogram", new Parallelogram() },
            { "Triangle", new Triangle() },
            { "Rhombus", new Rhombus() }
        };
    }

    public IEnumerable<Shape> GetShapeHistory()
    {
        return _shapeRepository.GetAllShapes()
            .OrderByDescending(s => s.CalculationDate);
    }

    public void SaveShape(string shapeType, Dictionary<string, double> parameters)
    {
        if (!_shapes.TryGetValue(shapeType, out var shape))
        {
            throw new ArgumentException("Invalid shape type");
        }

        shape.SetParameters(parameters);

        var shapeModel = new Shape
        {
            ShapeType = shapeType,
            Parameters = parameters,
            Area = shape.CalculateArea(),
            Perimeter = shape.CalculatePerimeter(),
            CalculationDate = DateTime.Now
        };

        _validator.ValidateAndThrow(shapeModel);
        _shapeRepository.AddShape(shapeModel);
    }

    public void UpdateShape(int id, string shapeType, Dictionary<string, double> parameters)
    {
        if (!_shapes.TryGetValue(shapeType, out var shape))
        {
            throw new ArgumentException("Invalid shape type");
        }

        shape.SetParameters(parameters);

        var shapeModel = new Shape
        {
            Id = id,
            ShapeType = shapeType,
            Parameters = parameters,
            Area = shape.CalculateArea(),
            Perimeter = shape.CalculatePerimeter(),
            CalculationDate = DateTime.Now
        };

        _validator.ValidateAndThrow(shapeModel);
        _shapeRepository.UpdateShape(shapeModel);
    }

    public void DeleteShape(int id)
    {
        _shapeRepository.DeleteShape(id);
    }

    public Dictionary<string, double> GetRequiredParameters(string shapeType)
    {
        if (!_shapes.TryGetValue(shapeType, out var shape))
        {
            throw new ArgumentException("Invalid shape type");
        }

        return shape.GetParameters();
    }
}