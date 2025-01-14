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
    private readonly IShapeFactory _shapeFactory;

    public ShapeOperationService(
        ShapeRepository shapeRepository,
        ShapeValidator validator,
        IShapeFactory shapeFactory)
    {
        _shapeRepository = shapeRepository;
        _validator = validator;
        _shapeFactory = shapeFactory;
    }
    public Shape GetShapeById(int id)
    {
        return _shapeRepository.GetShapeById(id);
    }


   

    public void SaveShape(ShapeType shapeType, Dictionary<string, double> parameters)
    {
        var shape = _shapeFactory.CreateShape(shapeType);
        shape.SetParameters(parameters);

        var shapeModel = new Shape
        {
            ShapeType = shapeType,
            CalculationDate = DateTime.Now
        };

        // Sätt specifika parametrar baserat på formtyp
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

        // Beräkna area och omkrets
        shapeModel.Area = shape.CalculateArea();
        shapeModel.Perimeter = shape.CalculatePerimeter();

        _validator.ValidateAndThrow(shapeModel);
        _shapeRepository.AddShape(shapeModel);
    }


  

   
    public Dictionary<string, double> GetRequiredParameters(ShapeType shapeType)
    {
        var shape = _shapeFactory.CreateShape(shapeType);
        return shape.GetParameters();
    }
}