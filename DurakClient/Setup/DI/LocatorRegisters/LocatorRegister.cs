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
            Locator.CurrentMutable.RegisterLazySingleton(container.Resolve<IViewModelFactory<ConnectionViewModel>>);
            Locator.CurrentMutable.RegisterLazySingleton(container.Resolve<IViewModelFactory<LobbiesViewModel>>);
            Locator.CurrentMutable.RegisterLazySingleton(container.Resolve<IViewModelFactory<FilterViewModel>>);
            Locator.CurrentMutable.RegisterLazySingleton(container.Resolve<IViewModelFactory<CreateLobbyViewModel>>);
        }
    }
}