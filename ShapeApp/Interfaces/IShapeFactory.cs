using ClassLibrary.Enums;
using ClassLibrary.Services.Shapes;

public interface IShapeFactory
{
    IShape CreateShape(ShapeType shapeType);
}
