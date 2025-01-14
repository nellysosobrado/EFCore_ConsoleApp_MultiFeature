﻿using Autofac;
using CalculatorApp.Services;
using CalculatorApp.Validators;
using CalculatorApp.Controllers;
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

        builder.RegisterType<SpectreCalculatorUI>().As<ICalculatorUIService>();
        builder.RegisterType<CalculatorOperationService>().As<ICalculatorOperationService>();
        builder.RegisterType<CalculatorMenu>().AsSelf();
        builder.RegisterType<CalculationProcessor>().AsSelf();
        builder.RegisterType<CalculationInputService>().AsSelf();
        builder.RegisterType<SquareRootCalculator>().AsSelf();


        // Register Shape Services
        builder.RegisterType<ShapeRepository>().AsSelf();
        builder.RegisterType<ShapeOperationService>().As<IShapeOperationService>();
        builder.RegisterType<UpdateShapeService>().As<IUpdateShapeService>();
        builder.RegisterType<DeleteShapeService>().As<IDeleteShapeService>();
        builder.RegisterType<ShapeDisplay>().As<IShapeDisplay>();
        builder.RegisterType<ErrorService>().As<IErrorService>();
        builder.RegisterType<InputService>().As<IInputService>();
        builder.RegisterType<SpectreShapeMenuService>().As<IShapeMenuService>();

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
        builder.RegisterType<SpectreGameUIService>().As<IGameUIService>();

        // Registrera ShapeFactory
        builder.RegisterType<ShapeFactory>().As<IShapeFactory>();


        return builder.Build();
    }
}
