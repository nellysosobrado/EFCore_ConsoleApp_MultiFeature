using Autofac;
using CalculatorApp.Services;
using ClassLibrary.DataAccess;
using ClassLibrary.Services;
using CalculatorApp.Validators;
using CalculatorApp.Controllers;
using ClassLibrary.Data;
using ClassLibrary.Services.CalculatorAppServices;

namespace Startup;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        // Register Database
        builder.RegisterType<AccessDatabase>().AsSelf();
        builder.Register(c =>
        {
            var accessDatabase = c.Resolve<AccessDatabase>();
            return accessDatabase.GetDbContext();
        }).As<ApplicationDbContext>();

        // Register Services
        builder.RegisterType<CalculatorService>().AsSelf();
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
