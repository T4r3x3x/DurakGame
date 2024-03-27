using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.MVVM.Views
{
    public partial class LobbyOwnerView : ReactiveUserControl<LobbyViewModel>
    {
        public LobbyOwnerView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
