using ClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
    [Column(TypeName = "jsonb")]
    public string ParametersJson { get; set; } = "{}";

    [Required]
    public DateTime CalculationDate { get; set; }

    [NotMapped]
    public Dictionary<string, double> Parameters
    {
        get => JsonSerializer.Deserialize<Dictionary<string, double>>(ParametersJson) ?? new();
        set => ParametersJson = JsonSerializer.Serialize(value);
    }

    public void UpdateParameters(Dictionary<string, double> parameters)
    {
        Parameters = parameters;
    }
}