using DurakClient.MVVM.ViewModels;
using DurakClient.Services;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class CreateLobbyViewModelFactory : IViewModelFactory<CreateLobbyViewModel>
    {
        private readonly ILobbyService _lobbyService;
        private readonly Resources _resources;

        public CreateLobbyViewModelFactory(ILobbyService lobbyService, Resources resources)
        {
            _lobbyService = lobbyService;
            _resources = resources;
        }

        public CreateLobbyViewModel GetViewModel(IScreen hotScreen) => new CreateLobbyViewModel(_lobbyService, _resources);
    }
}