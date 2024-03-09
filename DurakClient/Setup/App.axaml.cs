using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;
using DurakClient.Utilities;

using Grpc.Net.Client;

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
        var channel = GrpcChannel.ForAddress("https://localhost:5058");
        var client = new Connections.Services.ConnectionService.ConnectionServiceClient(channel);
        Locator.CurrentMutable.RegisterConstant<IScreen>(new ConnectionViewModel(new Services.ConnectionServices.ConnectionService(client), new()));
        Locator.CurrentMutable.RegisterLazySingleton(() => new CustomViewLocator(), typeof(IViewLocator));


        new ConnectionView { DataContext = Locator.Current.GetService<IScreen>() }.Show();


        base.OnFrameworkInitializationCompleted();
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        throw new NotImplementedException();
    }
}