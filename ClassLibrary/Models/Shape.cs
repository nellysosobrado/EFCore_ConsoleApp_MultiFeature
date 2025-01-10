using ClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models;

public class Shape
{
    public int Id { get; set; }
    public ShapeType ShapeType { get; set; }
    public double Area { get; set; }
    public double Perimeter { get; set; }
    public Dictionary<string, double> Parameters { get; set; } = new();
    public DateTime CalculationDate { get; set; }
}