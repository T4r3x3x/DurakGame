using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;

using ReactiveUI;

using System;

namespace DurakClient.Utilities
{
    public class CustomViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
        {
            LobbyViewModel context => new LobbyView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
