using ReactiveUI.Fody.Helpers;

namespace DurakClient.MVVM.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        [Reactive] public string? FilterName { get; set; }
        [Reactive] public int MinPlayersCount { get; set; } = 2;
        [Reactive] public int MaxPlayersCount { get; set; } = 6;
        [Reactive] public int MinStartCardsCount { get; set; } = 3;
        [Reactive] public int MaxStartCardsCount { get; set; } = 7;
    }
}