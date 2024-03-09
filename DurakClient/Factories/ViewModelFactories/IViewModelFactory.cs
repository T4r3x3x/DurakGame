using DurakClient.MVVM.ViewModels;

namespace DurakClient.Factories.ViewModelFactories
{
    public interface IViewModelFactory<T> where T : ViewModelBase
    {
        T GetViewModel();
    }
}
