using ClassLibrary.Models;
using ClassLibrary.Services.Shapes;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Validators;
using FluentValidation;
using ClassLibrary.Enums;

namespace ShapeApp.Services;

public class ShapeOperationService : IShapeOperationService
{
    private readonly ShapeRepository _shapeRepository;
    private readonly ShapeValidator _validator;
    private readonly Dictionary<ShapeType, IShape> _shapes;

    public ShapeOperationService(ShapeRepository shapeRepository, ShapeValidator validator)
    {
        _shapeRepository = shapeRepository;
        _validator = validator;
        _shapes = new Dictionary<ShapeType, IShape>
        {
            { ShapeType.Rectangle, new Rectangle() },
            { ShapeType.Parallelogram, new Parallelogram() },
            { ShapeType.Triangle, new Triangle() },
            { ShapeType.Rhombus, new Rhombus() }
        };
    }

    public IEnumerable<Shape> GetShapeHistory()
    {
        return _shapeRepository.GetAllShapes()
            .OrderByDescending(s => s.CalculationDate);
    }

    public void SaveShape(ShapeType shapeType, Dictionary<string, double> parameters)
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

    public void UpdateShape(int id, ShapeType shapeType, Dictionary<string, double> parameters)
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

    public Dictionary<string, double> GetRequiredParameters(ShapeType shapeType)
    {
        if (!_shapes.TryGetValue(shapeType, out var shape))
        {
            throw new ArgumentException("Invalid shape type");
        }

        return shape.GetParameters();
    }
}