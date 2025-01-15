namespace ClassLibrary.Services.Shapes;

public class Triangle : IShape
{
    private double _sideA;
    private double _sideB;
    private double _sideC;
    private double _height;

    public Dictionary<string, double> GetParameters()
    {
        return new Dictionary<string, double>
        {
            { "SideA", _sideA },
            { "SideB", _sideB },
            { "SideC", _sideC },
            { "Height", _height }
        };
    }

    public void SetParameters(Dictionary<string, double> parameters)
    {
        _sideA = parameters["SideA"];
        _sideB = parameters["SideB"];
        _sideC = parameters["SideC"];
        _height = parameters["Height"];
    }

    public double CalculateArea() => (_sideA * _height) / 2;
    public double CalculatePerimeter() => _sideA + _sideB + _sideC;
}