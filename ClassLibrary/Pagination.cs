using Spectre.Console;

namespace ClassLibrary.Pagination;


public class Pagination<T>
{
    private readonly IEnumerable<T> _items;
    private readonly int _pageSize;
    private int _currentPage;

    public Pagination(IEnumerable<T> items, int pageSize = 5)
    {
        _items = items;
        _pageSize = pageSize;
        _currentPage = 1;
    }

    public IEnumerable<T> GetCurrentPage()
    {
        return _items
            .Skip((_currentPage - 1) * _pageSize)
            .Take(_pageSize);
    }

    public void NextPage()
    {
        if (_currentPage < TotalPages)
            _currentPage++;
    }

    public void PreviousPage()
    {
        if (_currentPage > 1)
            _currentPage--;
    }

    public int GetCurrentPageNumber() => _currentPage;
    public int TotalPages => (int)Math.Ceiling(_items.Count() / (double)_pageSize);
    public int GetTotalItems() => _items.Count();
}


public static class PaginationRenderer
{
    public static string ShowPaginationControls<T>(Pagination<T> pagination)
    {
        var choices = new List<string>();

        if (pagination.GetCurrentPageNumber() < pagination.TotalPages)
            choices.Add("Next Page");

        if (pagination.GetCurrentPageNumber() > 1)
            choices.Add("Previous Page");

        choices.Add("Back to Menu");

        var prompt = new SelectionPrompt<string>()
            .Title($"\n[grey]Page {pagination.GetCurrentPageNumber()} of {pagination.TotalPages} " +
                  $"(Total items: {pagination.GetTotalItems()})[/]")
            .AddChoices(choices);

        var choice = AnsiConsole.Prompt(prompt);

        switch (choice)
        {
            case "Next Page":
                pagination.NextPage();
                break;
            case "Previous Page":
                pagination.PreviousPage();
                break;
        }

        return choice;
    }
}
