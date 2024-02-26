using Avalonia;
using Avalonia.Markup.Xaml;

using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;
using DurakClient.Utilities;

using ReactiveUI;

using Splat;

namespace DurakClient.Setup;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Locator.CurrentMutable.RegisterConstant<IScreen>(new MainViewModel());
        Locator.CurrentMutable.RegisterLazySingleton(() => new CustomViewLocator(), typeof(IViewLocator));


        new MainWindow { DataContext = Locator.Current.GetService<IScreen>() }.Show();
        base.OnFrameworkInitializationCompleted();
    }
}
