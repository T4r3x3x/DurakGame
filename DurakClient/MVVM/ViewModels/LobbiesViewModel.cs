using DurakClient.Extensions;
using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.Models;
using DurakClient.Services.LobbyServices;

using GameEngine.Entities.SystemEntites;

using MsBox.Avalonia;

using ReactiveUI;

using Splat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbiesViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly ILobbyService _lobbyService;
        public FilterViewModel FilterViewModel { get; }
        public CreateLobbyViewModel CreateLobbyViewModel { get; }
        public IObservable<IEnumerable<Lobby>> Lobbies { get; }
        public string UrlPathSegment { get; } = "Lobbies list";
        public IScreen HostScreen { get; }

        public ReactiveCommand<(Guid, string?), Unit> JoinLobbyCommand { get; set; }

        public LobbiesViewModel(IScreen hostScreen, ILobbyService lobbyService, CreateLobbyViewModel createLobbyViewModel = null!, FilterViewModel filterViewModel = null!)
        {
            HostScreen = hostScreen;
            _lobbyService = lobbyService;

            CreateLobbyViewModel = createLobbyViewModel ?? Locator.Current.GetService<IViewModelFactory<CreateLobbyViewModel>>()!.GetViewModel(hostScreen);
            FilterViewModel = filterViewModel ?? Locator.Current.GetService<IViewModelFactory<FilterViewModel>>()!.GetViewModel(hostScreen);

            JoinLobbyCommand = ReactiveCommand.Create<(Guid, string?)>(async (guid) => await JoinLobby(guid));

            Lobbies = FilterViewModel.Filter.CombineLatest(_lobbyService.Lobbies, resultSelector: FilterLobbies);

            _lobbyService.StartListiningLobbies();
        }

        private async Task JoinLobby((Guid lobbyId, string? password) param)
        {
            var joinResult = await _lobbyService.JoinLobby(param.lobbyId, param.password);
            if (!joinResult)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Error", "Inputed password is incorrect", MsBox.Avalonia.Enums.ButtonEnum.Ok);
                await messageBox.ShowAsync();
            }

            //todo goto next controller 
        }

        private IEnumerable<Lobby> FilterLobbies(Filter filter, IEnumerable<Lobby> lobbies)
        {
            return lobbies.WhereIf(!string.IsNullOrWhiteSpace(filter.FilterName), lobby => lobby.Name == filter.FilterName)

            .Where(lobby => lobby.Settings.PlayersCount.isInRange(filter.MinPlayersCount, filter.MaxPlayersCount))

            .Where(lobby => lobby.Settings.PlayersStartCardsCount.isInRange(filter.MinStartCardsCount, filter.MaxStartCardsCount))

            .WhereIf(!filter.IsAllowCommonDeckType, lobby => lobby.Settings.DeckType != DeckType.Common)

            .WhereIf(!filter.IsAllowExtendedDeckType, lobby => lobby.Settings.DeckType != DeckType.Extended);
        }
    }
}