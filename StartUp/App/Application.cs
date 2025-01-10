using Autofac;
using StartUp.Config;
using StartUp.UI;

namespace StartUp.App;

public class Application
{
    private readonly IContainer _container;
    private readonly Menu _menu;

    public Application()
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