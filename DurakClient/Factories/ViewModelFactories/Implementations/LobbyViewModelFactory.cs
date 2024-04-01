using DurakClient.MVVM.ViewModels;
using DurakClient.Services;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class LobbyViewModelFactory : IViewModelFactory<LobbyViewModel>
    {
        private readonly ILobbyService _lobbyService;
        private readonly Resources _resources;

        public LobbyViewModelFactory(ILobbyService lobbyService, Resources resources)
        {
            _lobbyService = lobbyService;
            _resources = resources;
        }

        public LobbyViewModel GetViewModel(IScreen hostScreen)
        {
            return new LobbyViewModel(_lobbyService, hostScreen);
        }
    }
}