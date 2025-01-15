using Autofac;
//GAME
using GameApp.Controller;
using GameApp.Services;
using GameApp.Interfaces;

//CALC
using CalculatorApp.Services;
using CalculatorApp.Validators;
using CalculatorApp.UI;
using CalculatorApp.Controller;
using CalculatorApp.Interfaces;

//Classlibr
using ClassLibrary.Data;
using ClassLibrary;
using ClassLibrary.Repositories.CalculatorAppRepository;
using ClassLibrary.Repositories.ShapeAppRepository;
using ClassLibrary.Repositories.RpsGameRepository;

//SHAPE
using ShapeApp.Controllers;
using ShapeApp.Services;
using ShapeApp.Validators;
using ShapeApp.Interfaces;
using GameApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;





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
        builder.RegisterType<SquareRootCalculator>().As<ISquareRootCalculator>();
        builder.RegisterType<CalculatorDelete>().As<ICalculatorDelete>();
        builder.RegisterType<CalculatorUpdateService>().As<ICalculatorUpdate>();
        builder.RegisterType<CalculatorParser>().As<ICalculatorParser>();

       

        //SHAPE ======================
        //shape factory
        builder.RegisterType<ShapeFactory>().AsSelf();
        builder.RegisterType<ShapeFactory>().As<IShapeFactory>();

        // Register Shape Services
        builder.RegisterType<ShapeRepository>().AsSelf();
        builder.RegisterType<SaveShapeService>().As<ISaveShapeService>();
        builder.RegisterType<UpdateShapeService>().As<IUpdateShapeService>();
        builder.RegisterType<DeleteShapeService>().As<IDeleteShapeService>();
        builder.RegisterType<ShapeDisplay>().As<IShapeDisplay>();
        builder.RegisterType<ErrorService>().As<IErrorService>();
        builder.RegisterType<InputService>().As<IInputService>();

        //Calculators ======================

        //  Controllers
        builder.RegisterType<CalculatorController>().AsSelf();
        builder.RegisterType<ShapeController>().AsSelf();
        builder.RegisterType<GameController>().AsSelf();

        // Validators
        builder.RegisterType<CalculatorValidator>().AsSelf();
        builder.RegisterType<InputValidator>().AsSelf();
        builder.RegisterType<ShapeValidator>().AsSelf();

        //RpsGame ======================

        //Services
        builder.RegisterType<GameService>().As<IGameService>();
        builder.RegisterType<PlayerInput>().As<IPlayerInput>();
        builder.RegisterType<PlayGame>().As<IPlayGame>();
        builder.RegisterType<GameError>().As<IGameError>();
        builder.RegisterType<DisplayRspGame>().As<IDisplayRspGame>();
        builder.RegisterType<RpsGameRepository>().AsSelf();
   


        return builder.Build();
    }
}
