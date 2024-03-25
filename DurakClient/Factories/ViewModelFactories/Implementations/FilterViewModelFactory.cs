using DurakClient.MVVM.ViewModels;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class FilterViewModelFactory : IViewModelFactory<FilterViewModel>
    {
        private readonly ILobbyService _lobbyService;

        public FilterViewModelFactory(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public FilterViewModel GetViewModel(IScreen hotScreen) => new FilterViewModel();
    }
}
