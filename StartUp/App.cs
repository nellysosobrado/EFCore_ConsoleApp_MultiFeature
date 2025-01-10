using Autofac;
using Startup;

namespace StartUp;

public class App
{
    private readonly IContainer _container;
    private readonly Menu _menu;

    public App()
    {
        _container = ContainerConfig.Configure();
        _menu = new Menu(_container);
    }

    public void Run()
    {
        while (true)
        {
            _menu.Show();
        }
    }
}