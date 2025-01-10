using Autofac;
using CalculatorApp.Services;
using ClassLibrary.DataAccess;
using ClassLibrary.Services;
using CalculatorApp.Validators;
using ClassLibrary.Services.CalculatorAppServices;

namespace Startup;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        // Register Services
        builder.RegisterType<AccessDatabase>().AsSelf();
        builder.RegisterType<CalculatorService>().AsSelf();
        builder.RegisterType<SpectreCalculatorUIService>().As<ICalculatorUIService>();
        builder.RegisterType<CalculatorOperationService>().As<ICalculatorOperationService>();

        // Register Validators
        builder.RegisterType<CalculatorValidator>().AsSelf();
        builder.RegisterType<InputValidator>().AsSelf();

        return builder.Build();
    }
}