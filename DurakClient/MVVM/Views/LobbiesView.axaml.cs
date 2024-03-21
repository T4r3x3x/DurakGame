using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.MVVM.Views;

public partial class LobbiesView : ReactiveUserControl<LobbiesViewModel>
{
    public LobbiesView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}

