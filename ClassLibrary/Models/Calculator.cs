﻿using ClassLibrary.Enums.CalculatorAppEnums;
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
}
