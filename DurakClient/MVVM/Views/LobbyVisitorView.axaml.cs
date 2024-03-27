using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.MVVM.Views
{
    public partial class LobbyVisitorView : ReactiveUserControl<LobbyViewModel>
    {
        public LobbyVisitorView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}