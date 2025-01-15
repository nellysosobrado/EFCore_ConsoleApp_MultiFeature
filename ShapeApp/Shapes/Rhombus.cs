namespace ClassLibrary.Services.Shapes;

public class Rhombus : IShape
{
    private double _side;
    private double _height;

    public Dictionary<string, double> GetParameters()
    {
        return new Dictionary<string, double>
        {
            { "Side", _side },
            { "Height", _height }
        };
    }

    public void SetParameters(Dictionary<string, double> parameters)
    {
        _side = parameters["Side"];
        _height = parameters["Height"];
    }

    public double CalculateArea() => _side * _height;
    public double CalculatePerimeter() => 4 * _side;
}