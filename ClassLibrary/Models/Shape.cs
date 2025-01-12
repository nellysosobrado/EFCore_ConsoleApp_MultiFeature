using ClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models;

[Table("Shapes")]
public class Shape
{
    [Key]
    public int Id { get; set; }

    [Required]
    public ShapeType ShapeType { get; set; }

    [Required]
    [Precision(18, 2)]
    public double Area { get; set; }

    [Required]
    [Precision(18, 2)]
    public double Perimeter { get; set; }

    [Required]
    public DateTime CalculationDate { get; set; }

    [Precision(18, 2)]
    public double? Width { get; set; }

    [Precision(18, 2)]
    public double? Height { get; set; }

    // Parallelogram & Rhombus parameters
    [Precision(18, 2)]
    public double? Side { get; set; }

    [Column("Base")]
    [Precision(18, 2)]
    public double? BaseLength { get; set; }  // Named BaseLength since Base is a reserved word

    // Triangle parameters
    [Precision(18, 2)]
    public double? SideA { get; set; }

    [Precision(18, 2)]
    public double? SideB { get; set; }

    [Precision(18, 2)]
    public double? SideC { get; set; }

    public Dictionary<string, double> GetParameters()
    {
        var parameters = new Dictionary<string, double>();

        switch (ShapeType)
        {
            case ShapeType.Rectangle:
                if (Width.HasValue) parameters.Add(nameof(Width), Width.Value);
                if (Height.HasValue) parameters.Add(nameof(Height), Height.Value);
                break;

            case ShapeType.Parallelogram:
                if (BaseLength.HasValue) parameters.Add("Base", BaseLength.Value);
                if (Height.HasValue) parameters.Add(nameof(Height), Height.Value);
                if (Side.HasValue) parameters.Add(nameof(Side), Side.Value);
                break;

            case ShapeType.Triangle:
                if (SideA.HasValue) parameters.Add(nameof(SideA), SideA.Value);
                if (SideB.HasValue) parameters.Add(nameof(SideB), SideB.Value);
                if (SideC.HasValue) parameters.Add(nameof(SideC), SideC.Value);
                if (Height.HasValue) parameters.Add(nameof(Height), Height.Value);
                break;

            case ShapeType.Rhombus:
                if (Side.HasValue) parameters.Add(nameof(Side), Side.Value);
                if (Height.HasValue) parameters.Add(nameof(Height), Height.Value);
                break;
        }

        return parameters;
    }

    public void SetParameters(Dictionary<string, double> parameters)
    {
        foreach (var param in parameters)
        {
            switch (param.Key)
            {
                case "Width": Width = param.Value; break;
                case "Height": Height = param.Value; break;
                case "Side": Side = param.Value; break;
                case "Base": BaseLength = param.Value; break;
                case "SideA": SideA = param.Value; break;
                case "SideB": SideB = param.Value; break;
                case "SideC": SideC = param.Value; break;
            }
        }
    }
}