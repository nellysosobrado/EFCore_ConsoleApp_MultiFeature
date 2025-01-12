using Autofac;
using ClassLibrary;
using ClassLibrary.Data;
using ClassLibrary.Repositories.ShapeAppRepository;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using StartUp.Config;
using StartUp.UI;

namespace StartUp.App;

public class Application
{
    private readonly IContainer _container;
    private readonly Menu _menu;

    public Application()
    {
        try
        {
            _container = ContainerConfig.Configure();
            InitializeDatabase();
            _menu = new Menu(_container);
        }
        catch (Exception ex)
        {
            HandleStartupError(ex);
        }
    }

    private void InitializeDatabase()
    {
        using var scope = _container.BeginLifetimeScope();
        var context = scope.Resolve<IApplicationDbContext>();

        if (context is DbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.Database.CanConnect())
            {
                scope.Resolve<ShapeRepository>().SeedData();
            }
            else
            {
                throw new Exception("Database connection failed. Check connection string and SQL Server.");
            }
        }
    }

    private void HandleStartupError(Exception ex)
    {
        AnsiConsole.MarkupLine("[red]Startup failed:[/]");
        AnsiConsole.WriteLine(ex.Message);
        Console.ReadKey();
        Environment.Exit(1);
    }

    public void Run()
    {
        while (true) _menu.Show();
    }
}
