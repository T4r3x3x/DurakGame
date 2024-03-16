using DurakClient.MVVM.ViewModels;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class LobbiesViewModelFactory : IViewModelFactory<LobbiesViewModel>
    {
        private readonly ILobbyService _lobbyService;

        public LobbiesViewModelFactory(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public LobbiesViewModel GetViewModel(IScreen hotScreen) => new LobbiesViewModel(hotScreen, _lobbyService);
    }
}