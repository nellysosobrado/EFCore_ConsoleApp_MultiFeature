namespace ClassLibrary.Services.Shapes;

public interface IShape
{
    double CalculateArea();
    double CalculatePerimeter();
    Dictionary<string, double> GetParameters();
    void SetParameters(Dictionary<string, double> parameters);
}