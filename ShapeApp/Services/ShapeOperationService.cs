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
    public Shape GetShapeById(int id)
    {
        return _shapeRepository.GetShapeById(id);
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

        var shapeModel = new Shape
        {
            ShapeType = shapeType,
            CalculationDate = DateTime.Now
        };

        switch (shapeType)
        {
            case ShapeType.Rectangle:
                shapeModel.Width = parameters["Width"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Parallelogram:
                shapeModel.BaseLength = parameters["Base"];  
                shapeModel.Height = parameters["Height"];
                shapeModel.Side = parameters["Side"];
                break;
            case ShapeType.Triangle:
                shapeModel.SideA = parameters["SideA"];
                shapeModel.SideB = parameters["SideB"];
                shapeModel.SideC = parameters["SideC"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Rhombus:
                shapeModel.Side = parameters["Side"];
                shapeModel.Height = parameters["Height"];
                break;
        }

        shape.SetParameters(parameters);
        shapeModel.Area = shape.CalculateArea();
        shapeModel.Perimeter = shape.CalculatePerimeter();

        _validator.ValidateAndThrow(shapeModel);
        _shapeRepository.AddShape(shapeModel);
    }


    public void UpdateShape(int id, ShapeType shapeType, Dictionary<string, double> parameters)
    {
        if (!_shapes.TryGetValue(shapeType, out var shape))
        {
            throw new ArgumentException("Invalid shape type");
        }

        var shapeModel = new Shape
        {
            Id = id,
            ShapeType = shapeType,
            CalculationDate = DateTime.Now
        };

        switch (shapeType)
        {
            case ShapeType.Rectangle:
                shapeModel.Width = parameters["Width"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Parallelogram:
                shapeModel.BaseLength = parameters["Base"];
                shapeModel.Height = parameters["Height"];
                shapeModel.Side = parameters["Side"];
                break;
            case ShapeType.Triangle:
                shapeModel.SideA = parameters["SideA"];
                shapeModel.SideB = parameters["SideB"];
                shapeModel.SideC = parameters["SideC"];
                shapeModel.Height = parameters["Height"];
                break;
            case ShapeType.Rhombus:
                shapeModel.Side = parameters["Side"];
                shapeModel.Height = parameters["Height"];
                break;
        }

        shape.SetParameters(parameters);
        shapeModel.Area = shape.CalculateArea();
        shapeModel.Perimeter = shape.CalculatePerimeter();

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