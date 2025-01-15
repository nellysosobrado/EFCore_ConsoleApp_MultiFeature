using CalculatorApp.Interfaces;
using CalculatorApp.UI;
using ClassLibrary.Enums;
using ClassLibrary.Repositories.CalculatorAppRepository;

namespace CalculatorApp.Services;

public class CalculationInputService : ICalculationInputService
{
    private readonly ICalculatorUIService _uiService;
    private readonly CalculatorTable _table;
    private readonly CalculatorOperationService _operationService;
    private readonly CalculatorRepository _calculatorRepository;

    public CalculationInputService(ICalculatorUIService uiService, 
        CalculatorTable calculatorTable,
        CalculatorOperationService operationService,
        CalculatorRepository calculatorRepository)
    {
        _uiService = uiService;
        _table = calculatorTable;
        _operationService = operationService;
        _calculatorRepository = calculatorRepository;
    }

    public (double operand1, double operand2, string operatorInput) GetUserInput()
    {
        _table.Display();

        var operand1 = _uiService.GetNumberInput("first");
        _table.UpdateFirstNumber(operand1.ToString());
        _table.Display();

        var operand2 = _uiService.GetNumberInput("second");
        _table.UpdateSecondNumber(operand2.ToString());
        _table.Display();

        var operatorInput = _uiService.GetOperatorInput();
        _table.UpdateOperator(operatorInput);
        _table.Display();

        return (operand1, operand2, operatorInput);
    }

    public Calculator GetCalculationById(int id)
    {
        return _calculatorRepository.GetCalculationById(id);
    }

    
}