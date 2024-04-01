using Autofac;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;
using DurakClient.Setup.DI.ContainerRegisters;
using DurakClient.Setup.DI.LocatorRegisters;
using DurakClient.Utilities;

using ReactiveUI;

using Splat;

using System;

namespace DurakClient.Setup;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //  desktop.Exit += OnExit;
        }

        var container = new ContainerBuilder()
            .RegisterDependecies()
            .Build();

        LocatorRegister.RegisterViewModelFactories(container);
        Locator.CurrentMutable.RegisterLazySingleton(() => new CustomViewLocator(), typeof(IViewLocator));

        var vm = container.Resolve<IViewModelFactory<ConnectionViewModel>>().GetViewModel(null!);
        Locator.CurrentMutable.RegisterConstant<IScreen>(vm);
        new ConnectionView { DataContext = Locator.Current.GetService<IScreen>() }.Show();

        base.OnFrameworkInitializationCompleted();
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        throw new NotImplementedException();
    }
}