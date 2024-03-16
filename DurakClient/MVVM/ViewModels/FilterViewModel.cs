using DurakClient.Extensions;
using DurakClient.MVVM.Models;
using DurakClient.Utilities;

using GameEngine.Entities.SystemEntites;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DurakClient.MVVM.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        private List<Lobby> _lobbies;

        [Reactive] public string FilterName { get; set; } = DefaultValues.DefaultName;
        [Reactive] public int MinPlayersCount { get; set; } = DefaultValues.DefaultMinPlayersCount;
        [Reactive] public int MaxPlayersCount { get; set; } = DefaultValues.DefaultMaxPlayersCount;
        [Reactive] public int MinStartCardsCount { get; set; } = DefaultValues.DefaultMinStartCardsCount;
        [Reactive] public int MaxStartCardsCount { get; set; } = DefaultValues.DefaultMaxStartCardsCount;
        [Reactive] public bool IsAllowCommonDeckType { get; set; } = DefaultValues.DefaultAllowCommonDeckType;
        [Reactive] public bool IsAllowExtendedDeckType { get; set; } = DefaultValues.DefaultAllowExtendedDeckType;
        public ReactiveCommand<Unit, Unit> ResetCommand { get; set; }
        public IObservable<IEnumerable<Lobby>> Lobbies { get; init; }


        public FilterViewModel()
        {
            ResetCommand = ReactiveCommand.Create(Reset);
            Lobbies = this.WhenAnyValue(
                x => x.FilterName,
                x => x.MinPlayersCount,
                x => x.MaxPlayersCount,
                x => x.MinStartCardsCount,
                x => x.MaxStartCardsCount,
                x => x.IsAllowCommonDeckType,
                x => x.IsAllowExtendedDeckType,
                FilterLobby).
                Throttle(TimeSpan.FromSeconds(1));
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

        private IEnumerable<Lobby> FilterLobby(string name, int minPlayersCount, int maxPlayersCount, int minStartCardsCount,
            int maxStartCardsCount, bool isAllowCommonDeckType, bool isAllowExtendedDeckType)
        {
            return _lobbies.
                WhereIf(!string.IsNullOrWhiteSpace(FilterName), lobby => lobby.Name == name).
                Where(lobby => lobby.Settings.PlayersCount >= minPlayersCount && lobby.Settings.PlayersCount <= maxPlayersCount).
                Where(lobby => lobby.Settings.PlayersStartCardsCount >= minStartCardsCount && lobby.Settings.PlayersStartCardsCount <= maxStartCardsCount).
                WhereIf(isAllowCommonDeckType, lobby => lobby.Settings.DeckType == DeckType.Common).
                WhereIf(isAllowExtendedDeckType, lobby => lobby.Settings.DeckType == DeckType.Extended);
        }
    }
}