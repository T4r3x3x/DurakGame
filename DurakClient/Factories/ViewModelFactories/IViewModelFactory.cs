using DurakClient.MVVM.ViewModels;

using ReactiveUI;

namespace DurakClient.Factories.ViewModelFactories
{
    public interface IViewModelFactory<T> where T : ViewModelBase
    {
        T GetViewModel(IScreen hotScreen);
    }
}
