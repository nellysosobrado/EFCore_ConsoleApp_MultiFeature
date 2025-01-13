using Spectre.Console;

namespace CalculatorApp.UI;

public class CalculatorTable
{
    private Table _table;

    public CalculatorTable()
    {
        InitializeTable();
    }

    private void InitializeTable()
    {
        _table = new Table().Border(TableBorder.Square);
        _table.AddColumns(
            new TableColumn("First Number").Centered(),
            new TableColumn("Operator").Centered(),
            new TableColumn("Second Number").Centered(),
            new TableColumn("Result").Centered()
        );
        _table.AddRow("", "", "", "");
    }

    public void Clear()
    {
        InitializeTable();
        Display();
    }

    public void Display()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(_table);
    }

    public void UpdateFirstNumber(string value) => _table.UpdateCell(0, 0, value);
    public void UpdateOperator(string value) => _table.UpdateCell(0, 1, value);
    public void UpdateSecondNumber(string value) => _table.UpdateCell(0, 2, value);
    public void UpdateResult(string value) => _table.UpdateCell(0, 3, value);
}