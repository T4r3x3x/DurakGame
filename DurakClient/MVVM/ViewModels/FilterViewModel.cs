using DurakClient.MVVM.Models;
using DurakClient.Utilities;
using DurakClient.Utilities.Ranges;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        private readonly Filter _filter;

        [Reactive] public IntRange PlayersCountRange { get; set; } = DefaultValuesProvider.PlayersCountRange;
        [Reactive] public IntRange CardsStartCountRange { get; set; } = DefaultValuesProvider.CardsStartCountRange;
        [Reactive] public string FilterName { get; set; } = DefaultValuesProvider.DefaultFilterName;
        [Reactive] public bool IsAllowCommonDeckType { get; set; } = DefaultValuesProvider.DefaultAllowCommonDeckType;
        [Reactive] public bool IsAllowExtendedDeckType { get; set; } = DefaultValuesProvider.DefaultAllowExtendedDeckType;

        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public IObservable<Filter> Filter { get; }

        public FilterViewModel()
        {
            ResetCommand = ReactiveCommand.Create(Reset);
            _filter = new Filter(
                FilterName,
                PlayersCountRange.Min,
                PlayersCountRange.Max,
                CardsStartCountRange.Min,
                CardsStartCountRange.Max,
                IsAllowCommonDeckType,
                IsAllowExtendedDeckType);

            Filter = this.WhenAnyValue(
                     x => x.FilterName,
                     x => x.PlayersCountRange.Min,
                     x => x.PlayersCountRange.Max,
                     x => x.CardsStartCountRange.Min,
                     x => x.CardsStartCountRange.Max,
                     x => x.IsAllowCommonDeckType,
                     x => x.IsAllowExtendedDeckType,
                     selector: (a, b, c, d, e, f, g) =>
                     {
                         UpdateFilter(a, b, c, d, e, f, g);
                         return _filter;
                     });
        }

        private void UpdateFilter(string name, int minPlayerCount, int maxPlayersCount, int minStartCardsCount,
            int maxStartCardsCount, bool isAllowedCommonDeckType, bool isAllowedExtenededDeckType)
        {
            _filter.FilterName = name;
            _filter.MinPlayersCount = minPlayerCount;
            _filter.MaxPlayersCount = maxPlayersCount;
            _filter.MinStartCardsCount = minStartCardsCount;
            _filter.MaxStartCardsCount = maxStartCardsCount;
            _filter.IsAllowCommonDeckType = isAllowedCommonDeckType;
            _filter.IsAllowExtendedDeckType = isAllowedExtenededDeckType;
        }

        private void Reset()
        {
            FilterName = DefaultValuesProvider.DefaultFilterName;
            PlayersCountRange = DefaultValuesProvider.PlayersCountRange;
            CardsStartCountRange = DefaultValuesProvider.CardsStartCountRange;
            IsAllowCommonDeckType = DefaultValuesProvider.DefaultAllowCommonDeckType;
            IsAllowExtendedDeckType = DefaultValuesProvider.DefaultAllowExtendedDeckType;
        }
    }
}