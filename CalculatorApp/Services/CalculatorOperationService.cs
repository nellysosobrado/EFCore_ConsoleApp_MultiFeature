﻿using ClassLibrary.Models;
using CalculatorApp.Validators;
using FluentValidation;
using ClassLibrary.Enums.CalculatorAppEnums;
using ClassLibrary.Repositories.CalculatorAppRepository;

namespace CalculatorApp.Services;

public class CalculatorOperationService : ICalculatorOperationService
{
    private readonly CalculatorRepository _calculatorRepository;
    private readonly CalculatorValidator _validator;

    public CalculatorOperationService(CalculatorRepository calculatorRepository)
    {
        _calculatorRepository = calculatorRepository;
        _validator = new CalculatorValidator();
    }

   

    public (double firstResult, double secondResult)? CalculateSquareRoots(double operand1, double operand2)
    {
        if (operand1 < 0 || operand2 < 0)
        {
            throw new InvalidOperationException("Cannot calculate square root of negative numbers");
        }
        return (Math.Sqrt(operand1), Math.Sqrt(operand2));
    }

    public double Calculate(double operand1, double operand2, CalculatorOperator calculatorOperator)
    {
        if (calculatorOperator == CalculatorOperator.Divide && operand2 == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero");
        }

        if (calculatorOperator == CalculatorOperator.SquareRoot)
        {
            var results = CalculateSquareRoots(operand1, operand2);
            return results?.firstResult ?? 0; 
        }

        return calculatorOperator switch
        {
            CalculatorOperator.Add => operand1 + operand2,
            CalculatorOperator.Subtract => operand1 - operand2,
            CalculatorOperator.Multiply => operand1 * operand2,
            CalculatorOperator.Divide => operand1 / operand2,
            CalculatorOperator.Modulus => operand1 % operand2,
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }

    public void SaveCalculation(Calculator calculation)
    {
        var validationResult = _validator.Validate(calculation);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        if (double.IsInfinity(calculation.Result) || double.IsNaN(calculation.Result))
        {
            throw new InvalidOperationException("Result is invalid (Infinity or NaN)");
        }

        _calculatorRepository.AddCalculation(calculation);
    }


   

   
    
  
}