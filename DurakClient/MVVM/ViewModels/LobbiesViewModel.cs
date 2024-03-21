using DurakClient.Extensions;
using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.Models;
using DurakClient.Services.LobbyServices;

using GameEngine.Entities.SystemEntites;

using ReactiveUI;

using Splat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbiesViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly ILobbyService _lobbyService;

        public IObservable<IEnumerable<Lobby>> Lobbies { get; }
        public FilterViewModel FilterViewModel { get; set; }
        public CreateLobbyViewModel CreateLobbyViewModel { get; set; } = new();
        public string UrlPathSegment { get; } = "Lobbies list";
        public IScreen HostScreen { get; }


        public LobbiesViewModel(IScreen hostScreen, ILobbyService lobbyService, FilterViewModel filterViewModel = null)
        {
            HostScreen = hostScreen;
            _lobbyService = lobbyService;
            FilterViewModel = filterViewModel ?? Locator.Current.GetService<IViewModelFactory<FilterViewModel>>()!.GetViewModel(hostScreen);
            Lobbies = FilterViewModel.Filter.CombineLatest(_lobbyService.Lobbies, resultSelector: FilterLobby);
            _lobbyService.StartListiningLobbies();

        }

        private IEnumerable<Lobby> FilterLobby(Filter filter, IEnumerable<Lobby> lobbies)
        {
            return lobbies.WhereIf(!string.IsNullOrWhiteSpace(filter.FilterName), lobby => lobby.Name == filter.FilterName)

            .Where(lobby => lobby.Settings.PlayersCount.isInRange(filter.MinPlayersCount, filter.MaxPlayersCount))

            .Where(lobby => lobby.Settings.PlayersStartCardsCount.isInRange(filter.MinStartCardsCount, filter.MaxStartCardsCount))

            .WhereIf(!filter.IsAllowCommonDeckType, lobby => lobby.Settings.DeckType != DeckType.Common)

            .WhereIf(!filter.IsAllowExtendedDeckType, lobby => lobby.Settings.DeckType != DeckType.Extended);
        }
    }
}