using Autofac;
using CalculatorApp.Services;
using CalculatorApp.Validators;
using ClassLibrary.Data;
using GameApp.Controller;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClassLibrary;
using ClassLibrary.Repositories.CalculatorAppRepository;
using ClassLibrary.Repositories.ShapeAppRepository;
using ShapeApp.Controllers;
using GameApp.Services;
using ShapeApp.Services;
using ShapeApp.Validators;
using Microsoft.Extensions.Options;
using CalculatorApp.UI;
using ShapeApp.Interfaces;
using GameApp.Interfaces;
using CalculatorApp.Controller;
using CalculatorApp.Interfaces;


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

        builder.RegisterType<CalculatorOperationService>()
    .AsSelf()  
    .As<ICalculatorOperationService>(); 


        //repository
        builder.RegisterType<CalculatorRepository>().AsSelf();
        // Register Calculator Services
        builder.RegisterType<CalculationInputService>().As<ICalculationInputService>();
        builder.RegisterType<CalculatorTable>().AsSelf().SingleInstance();
        builder.RegisterType<CalculatorDisplay>().As<ICalculatorDisplay>();
        builder.RegisterType<CalculatorOperationService>().As<ICalculatorOperationService>();
        builder.RegisterType<CalculatorMenu>().AsSelf();
        builder.RegisterType<CalculationProcessor>().AsSelf();
        builder.RegisterType<CalculationInputService>().AsSelf();
        builder.RegisterType<SquareRootCalculator>().AsSelf();
        builder.RegisterType<CalculatorDelete>().As<ICalculatorDelete>();
        builder.RegisterType<CalculatorUpdateService>().As<ICalculatorUpdate>();
       


        //shape factory
        builder.RegisterType<ShapeFactory>().AsSelf();

        // Register Shape Services
        builder.RegisterType<ShapeRepository>().AsSelf();
        builder.RegisterType<SaveShapeService>().As<ISaveShapeService>();
        builder.RegisterType<UpdateShapeService>().As<IUpdateShapeService>();
        builder.RegisterType<DeleteShapeService>().As<IDeleteShapeService>();
        builder.RegisterType<ShapeDisplay>().As<IShapeDisplay>();
        builder.RegisterType<ErrorService>().As<IErrorService>();
        builder.RegisterType<InputService>().As<IInputService>();

        // Register Controllers
        builder.RegisterType<CalculatorController>().AsSelf();
        builder.RegisterType<ShapeController>().AsSelf();
        builder.RegisterType<GameController>().AsSelf();

        // Register Validators
        builder.RegisterType<CalculatorValidator>().AsSelf();
        builder.RegisterType<InputValidator>().AsSelf();
        builder.RegisterType<ShapeValidator>().AsSelf();

        // Register Game Services
        builder.RegisterType<GameService>().As<IGameService>();
        builder.RegisterType<PlayerInput>().As<IPlayerInput>();
        builder.RegisterType<PlayGame>().As<IPlayGame>();
        builder.RegisterType<GameError>().As<IGameError>();
        builder.RegisterType<DisplayRspGame>().As<IDisplayRspGame>();

        // Registrera ShapeFactory
        builder.RegisterType<ShapeFactory>().As<IShapeFactory>();


        return builder.Build();
    }
}
