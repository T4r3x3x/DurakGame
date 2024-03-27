using DurakClient.MVVM.ViewModels;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class LobbyViewModelFactory : IViewModelFactory<LobbyViewModel>
    {
        private readonly ILobbyService _lobbyService;

        public LobbyViewModelFactory(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public LobbyViewModel GetViewModel(IScreen hostScreen)
        {
            return new LobbyViewModel(_lobbyService, hostScreen);
        }
    }
}
