using Autofac;

using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.ViewModels;

using Splat;

namespace DurakClient.Setup.DI.LocatorRegisters
{
    internal class LocatorRegister
    {
        internal static void RegisterViewModelFactories(IContainer container)
        {
            Locator.CurrentMutable.Register(container.Resolve<IViewModelFactory<ConnectionViewModel>>);
            Locator.CurrentMutable.Register(container.Resolve<IViewModelFactory<LobbiesViewModel>>);
            Locator.CurrentMutable.Register(container.Resolve<IViewModelFactory<FilterViewModel>>);
            Locator.CurrentMutable.Register(container.Resolve<IViewModelFactory<CreateLobbyViewModel>>);
            Locator.CurrentMutable.Register(container.Resolve<IViewModelFactory<LobbyViewModel>>);
        }
    }
}