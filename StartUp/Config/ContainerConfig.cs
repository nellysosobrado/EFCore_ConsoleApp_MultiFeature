using Autofac;
using CalculatorApp.Services;
//using ClassLibrary.Services;
using CalculatorApp.Validators;
using CalculatorApp.Controllers;
using ClassLibrary.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using ClassLibrary.Services.CalculatorAppServices;
using ClassLibrary;
using ClassLibrary.Repositories.CalculatorAppRepository;


namespace StartUp.Config;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        // Läs in configuration från appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        // Register Database
        builder.Register(c =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(optionsBuilder.Options);
        }).As<IApplicationDbContext>().InstancePerLifetimeScope();

        // Register Services
        builder.RegisterType<CalculatorRepository>().AsSelf();
        builder.RegisterType<SpectreCalculatorUIService>().As<ICalculatorUIService>();
        builder.RegisterType<CalculatorOperationService>().As<ICalculatorOperationService>();

        // Register Controllers
        builder.RegisterType<CalculatorController>().AsSelf();

        // Register Validators
        builder.RegisterType<CalculatorValidator>().AsSelf();
        builder.RegisterType<InputValidator>().AsSelf();

        return builder.Build();
    }
}
