using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories.Implementations
{
    internal class CreateLobbyViewModelFactory : IViewModelFactory<CreateLobbyViewModel>
    {
        public CreateLobbyViewModel GetViewModel(IScreen hotScreen) => new CreateLobbyViewModel();
    }
}