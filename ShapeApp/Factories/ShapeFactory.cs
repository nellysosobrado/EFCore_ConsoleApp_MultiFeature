using ClassLibrary.Enums;
using ClassLibrary.Services.Shapes;

public class ShapeFactory : IShapeFactory
{
    public IShape CreateShape(ShapeType shapeType)
    {
        return shapeType switch
        {
            ShapeType.Rectangle => new Rectangle(),
            ShapeType.Parallelogram => new Parallelogram(),
            ShapeType.Triangle => new Triangle(),
            ShapeType.Rhombus => new Rhombus(),
            _ => throw new ArgumentException("Invalid shape type")
        };
    }
}