namespace ClassLibrary.Services.Shapes;

public class Rectangle : IShape
{
    private double _width;
    private double _height;

    public Dictionary<string, double> GetParameters()
    {
        return new Dictionary<string, double>
        {
            { "Width", _width },
            { "Height", _height }
        };
    }

    public void SetParameters(Dictionary<string, double> parameters)
    {
        _width = parameters["Width"];
        _height = parameters["Height"];
    }

    public double CalculateArea() => _width * _height;
    public double CalculatePerimeter() => 2 * (_width + _height);
}