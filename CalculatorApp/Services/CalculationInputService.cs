using CalculatorApp.UI;
using ClassLibrary.Enums;

namespace CalculatorApp.Services;

public class CalculationInputService
{
    private readonly ICalculatorUIService _uiService;
    private readonly CalculatorTable _table;

    public CalculationInputService(ICalculatorUIService uiService)
    {
        _uiService = uiService;
        _table = new CalculatorTable();
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

    public void DisplayResult(double result)
    {
        _table.UpdateResult(result.ToString());
        _table.Display();
    }

    public void DisplaySquareRootResults(double sqrtResult1, double sqrtResult2)
    {
        _table.UpdateResult($"√{sqrtResult1}, √{sqrtResult2}");
        _table.Display();
    }

    public void ClearTable()
    {
        _table.Clear();
        _table.Display();
    }
}