using Avalonia;
using Avalonia.Markup.Xaml;

using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;
using DurakClient.Services.ConnectionServices;
using DurakClient.Utilities;

using Grpc.Net.Client;

using ReactiveUI;

using Splat;

using static Connections.Services.ConnectionService;

namespace DurakClient.Setup;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5058");
        var client = new ConnectionServiceClient(channel);
        Locator.CurrentMutable.RegisterConstant<IScreen>(new ConnectionViewModel(new ConnectionService(client)));
        Locator.CurrentMutable.RegisterLazySingleton(() => new CustomViewLocator(), typeof(IViewLocator));


        new ConnectionView { DataContext = Locator.Current.GetService<IScreen>() }.Show();
        base.OnFrameworkInitializationCompleted();
    }
}
