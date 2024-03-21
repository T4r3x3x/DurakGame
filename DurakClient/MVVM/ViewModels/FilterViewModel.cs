using DurakClient.MVVM.Models;
using DurakClient.Services.LobbyServices;
using DurakClient.Utilities;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        private readonly ILobbyService _lobbyService;

        [Reactive] public string FilterName { get; set; } = DefaultValues.DefaultName;
        [Reactive] public int MinPlayersCount { get; set; } = DefaultValues.DefaultMinPlayersCount;
        [Reactive] public int MaxPlayersCount { get; set; } = DefaultValues.DefaultMaxPlayersCount;
        [Reactive] public int MinStartCardsCount { get; set; } = DefaultValues.DefaultMinStartCardsCount;
        [Reactive] public int MaxStartCardsCount { get; set; } = DefaultValues.DefaultMaxStartCardsCount;
        [Reactive] public bool IsAllowCommonDeckType { get; set; } = DefaultValues.DefaultAllowCommonDeckType;
        [Reactive] public bool IsAllowExtendedDeckType { get; set; } = DefaultValues.DefaultAllowExtendedDeckType;

        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public IObservable<Filter> Filter { get; }

        public FilterViewModel(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
            ResetCommand = ReactiveCommand.Create(Reset);

            Filter = this.WhenAnyValue(
                     x => x.FilterName,
                     x => x.MinPlayersCount,
                     x => x.MaxPlayersCount,
                     x => x.MinStartCardsCount,
                     x => x.MaxStartCardsCount,
                     x => x.IsAllowCommonDeckType,
                     x => x.IsAllowExtendedDeckType,
                     selector: (a, b, c, d, e, f, g) => new Filter(a, b, c, d, e, f, g));
        }

        private void Reset()
        {
            FilterName = DefaultValues.DefaultName;
            MinPlayersCount = DefaultValues.DefaultMinPlayersCount;
            MaxPlayersCount = DefaultValues.DefaultMaxPlayersCount;
            MinStartCardsCount = DefaultValues.DefaultMinStartCardsCount;
            MaxStartCardsCount = DefaultValues.DefaultMaxStartCardsCount;
            IsAllowCommonDeckType = DefaultValues.DefaultAllowCommonDeckType;
            IsAllowExtendedDeckType = DefaultValues.DefaultAllowExtendedDeckType;
        }
    }
}