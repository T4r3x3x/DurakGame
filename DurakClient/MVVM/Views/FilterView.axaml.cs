using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.MVVM.Views
{
    public partial class FilterView : ReactiveUserControl<FilterViewModel>
    {
        public FilterView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
