using CalculatorApp.Interfaces;
using CalculatorApp.UI;
using ClassLibrary.Enums;
using ClassLibrary.Repositories.CalculatorAppRepository;
using Spectre.Console;

namespace CalculatorApp.Services;

public class CalculationInputService : ICalculationInputService
{
    private readonly ICalculatorDisplay _displayCalculator;
    private readonly CalculatorTable _table;
    private readonly CalculatorOperationService _operationService;
    private readonly CalculatorRepository _calculatorRepository;

    public CalculationInputService(ICalculatorDisplay uiService, 
        CalculatorTable calculatorTable,
        CalculatorOperationService operationService,
        CalculatorRepository calculatorRepository)
    {
        _displayCalculator = uiService;
        _table = calculatorTable;
        _operationService = operationService;
        _calculatorRepository = calculatorRepository;
    }

    public (double operand1, double operand2, string operatorInput) GetUserInput()
    {
        _table.Display();

        var operand1 = _displayCalculator.GetNumberInput("first");
        _table.UpdateFirstNumber(operand1.ToString());
        _table.Display();

        var operand2 = _displayCalculator.GetNumberInput("second");
        _table.UpdateSecondNumber(operand2.ToString());
        _table.Display();

        var operatorInput = _displayCalculator.GetOperatorInput();
        _table.UpdateOperator(operatorInput);
        _table.Display();

        return (operand1, operand2, operatorInput);
    }

    public Calculator GetCalculationById(int id)
    {
        return _calculatorRepository.GetCalculationById(id);
    }

    public IEnumerable<Calculator> GetCalculationHistory()
    {
        return _calculatorRepository.GetAllCalculations();
    }
    public bool ShouldChangeOperator()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Would you like to change the operator?[/]")
                .AddChoices("Yes", "No")) == "Yes";
    }
   public int GetCalculationIdForUpdate()
{
    while (true)
    {
        var id = AnsiConsole.Prompt(
            new TextPrompt<int>("[green]Enter the ID of the calculation to update:[/]")
                .ValidationErrorMessage("[red]Please enter a valid ID[/]"));

        var calculation = _calculatorRepository.GetCalculationById(id);
        
        if (calculation == null)
        {
            _displayCalculator.ShowError("No calculation found with that ID.");
            continue;
        }

        if (calculation.IsDeleted)
        {
            _displayCalculator.ShowError("Cannot update a deleted calculation.");
            continue;
        }

        return id;
    }
}

    


}