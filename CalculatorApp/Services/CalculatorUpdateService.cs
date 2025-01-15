using CalculatorApp.Interfaces;
using CalculatorApp.Validators;
using ClassLibrary.Enums.CalculatorAppEnums;
using ClassLibrary.Repositories.CalculatorAppRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation;



namespace CalculatorApp.Services
{
    public class CalculatorUpdateService : ICalculatorUpdate
    {
        private readonly CalculatorRepository _calculatorRepository;    
        private readonly CalculatorValidator _validator;    
        private readonly CalculatorOperationService _calculatorOperationService;    

        public CalculatorUpdateService(CalculatorRepository calculatorRepository, CalculatorValidator validator, CalculatorOperationService calculatorOperationService)
        {
            _calculatorRepository = calculatorRepository;
            _validator = validator;
            _calculatorOperationService = calculatorOperationService;
        }

        public void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator)
        {
            var existingCalculation = _calculatorRepository.GetCalculationById(id);

            var result = _calculatorOperationService.Calculate(operand1, operand2, calculatorOperator);

            var updatedCalculation = new Calculator
            {
                Id = id,
                FirstNumber = operand1,
                SecondNumber = operand2,
                Operator = calculatorOperator,
                Result = Math.Round(result, 2),
                CalculationDate = DateTime.Now
            };

            var validationResult = _validator.Validate(updatedCalculation);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            _calculatorRepository.UpdateCalculation(updatedCalculation);
        }
        public void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator, double result)
        {
            UpdateCalculation(id, operand1, operand2, calculatorOperator);
        }

    }
}
