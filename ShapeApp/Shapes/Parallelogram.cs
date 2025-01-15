namespace ClassLibrary.Services.Shapes;

public class Parallelogram : IShape
{
    private double _base;
    private double _height;
    private double _side;

    public Dictionary<string, double> GetParameters()
    {
        return new Dictionary<string, double>
        {
            { "Base", _base },
            { "Height", _height },
            { "Side", _side }
        };
    }

    public void SetParameters(Dictionary<string, double> parameters)
    {
        _base = parameters["Base"];
        _height = parameters["Height"];
        _side = parameters["Side"];
    }

    public double CalculateArea() => _base * _height;
    public double CalculatePerimeter() => 2 * (_base + _side);
}