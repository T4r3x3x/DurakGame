using DurakClient.MVVM.ViewModels;
using DurakClient.Services;
using DurakClient.Services.ConnectionServices;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class ConnectionViewModelFactory : IViewModelFactory<ConnectionViewModel>
    {
        private readonly IConnectionService _connectionService;
        private readonly Resources _resources;

        public ConnectionViewModelFactory(IConnectionService connectionService, Resources resources)
        {
            _connectionService = connectionService;
            _resources = resources;
        }

        public ConnectionViewModel GetViewModel(IScreen hotScreen) => new ConnectionViewModel(_connectionService, _resources);
    }
}