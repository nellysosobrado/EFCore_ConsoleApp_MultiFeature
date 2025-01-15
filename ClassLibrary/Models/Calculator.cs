using ClassLibrary.Enums.CalculatorAppEnums.CalculatorEnums;
using System.ComponentModel.DataAnnotations;

public class Calculator
{
    [Key]
    public int Id { get; set; }

    [Required]
    public double FirstNumber { get; set; }

    [Required]
    public double SecondNumber { get; set; }

    [Required]
    public double Result { get; set; }

    [Required]
    public CalculatorOperator Operator { get; set; }

    [Required]
    public DateTime CalculationDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
