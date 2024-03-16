using Autofac;

using DurakClient.MVVM.ViewModels;

using Splat;

namespace DurakClient.Setup.DI.LocatorRegisters
{
    internal class LocatorRegister
    {
        internal static void RegisterViewModelFactories(IContainer container)
        {
            Locator.CurrentMutable.RegisterLazySingleton(container.Resolve<LobbiesViewModel>);
        }
    }
}