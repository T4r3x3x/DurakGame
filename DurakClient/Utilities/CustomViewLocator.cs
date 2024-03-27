using DurakClient.MVVM.ViewModels;
using DurakClient.MVVM.Views;

using ReactiveUI;

using System;

namespace DurakClient.Utilities
{
    public class CustomViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string? contract = null)
        {
            return viewModel switch
            {
                LobbiesViewModel context => new LobbiesView { DataContext = context },
                LobbyViewModel context => GetLobbyView(context),
                CreateLobbyViewModel context => new CreateLobbyView { DataContext = context },
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
            };
        }

        private IViewFor GetLobbyView(LobbyViewModel viewModel)
        {
            var previousViewModelType = GetTypeOfPreviousViewModel(viewModel.HostScreen);

            if (previousViewModelType == typeof(LobbiesViewModel))
                return new LobbyVisitorView { DataContext = viewModel };

            if (previousViewModelType == typeof(CreateLobbyViewModel))
                return new LobbyOwnerView { DataContext = viewModel };

            throw new ArgumentOutOfRangeException(nameof(viewModel));
        }

        private Type GetTypeOfPreviousViewModel(IScreen hostScreen)
        {
            var count = hostScreen.Router.NavigationStack.Count;
            return hostScreen.Router.NavigationStack[count - 2].GetType();
        }
    }
}