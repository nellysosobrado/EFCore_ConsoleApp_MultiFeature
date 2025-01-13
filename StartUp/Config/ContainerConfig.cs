using Autofac;
using CalculatorApp.Services;
using CalculatorApp.Validators;
using CalculatorApp.Controllers;
using ClassLibrary.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClassLibrary;
using ClassLibrary.Repositories.CalculatorAppRepository;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Controllers;
using ShapeApp.Services;
using ShapeApp.Validators;
using Microsoft.Extensions.Options;
using CalculatorApp.UI;


namespace StartUp.Config;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Register DbContext
        builder.Register(c =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(optionsBuilder.Options);
        })
        .As<IApplicationDbContext>()
        .InstancePerLifetimeScope();

        //repository
        builder.RegisterType<CalculatorRepository>().AsSelf();
        // Register Calculator Services

        builder.RegisterType<SpectreCalculatorUIService>().As<ICalculatorUIService>();
        builder.RegisterType<CalculatorOperationService>().As<ICalculatorOperationService>();
        builder.RegisterType<CalculatorMenu>().AsSelf();
        builder.RegisterType<CalculationProcessor>().AsSelf();
        builder.RegisterType<CalculationInputService>().AsSelf();
        builder.RegisterType<SquareRootCalculator>().AsSelf();


        // Register Shape Services
        builder.RegisterType<ShapeRepository>().AsSelf();
        builder.RegisterType<SpectreShapeUIService>().As<IShapeUIService>();
        builder.RegisterType<ShapeOperationService>().As<IShapeOperationService>();

        // Register Controllers
        builder.RegisterType<CalculatorController>().AsSelf();
        builder.RegisterType<ShapeController>().AsSelf();

        // Register Validators
        builder.RegisterType<CalculatorValidator>().AsSelf();
        builder.RegisterType<InputValidator>().AsSelf();
        builder.RegisterType<ShapeValidator>().AsSelf();

        return builder.Build();
    }
}
