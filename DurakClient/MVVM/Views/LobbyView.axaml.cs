using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.MVVM.Views;

public partial class LobbyView : ReactiveUserControl<LobbiesViewModel>
{
    public LobbyView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}

