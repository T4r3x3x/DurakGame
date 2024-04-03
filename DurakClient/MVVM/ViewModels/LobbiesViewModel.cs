using DurakClient.Extensions;
using DurakClient.Factories.ViewModelFactories;
using DurakClient.MessageBoxes;
using DurakClient.MVVM.Models;
using DurakClient.Results;
using DurakClient.Services.LobbyServices;

using GameEngine.Entities.SystemEntites;

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
        private readonly IViewModelFactory<LobbyViewModel> _lobbyViewModelFactory;
        private readonly IViewModelFactory<CreateLobbyViewModel> _createLobbyViewModelFactory;

        public FilterViewModel FilterViewModel { get; }
        public IObservable<IEnumerable<Lobby>> Lobbies { get; }
        public string UrlPathSegment { get; } = "Lobbies";
        public IScreen HostScreen { get; }

        public ReactiveCommand<(Guid, string?), Unit> JoinLobbyCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateLobbyCommand { get; set; }

        public LobbiesViewModel(IScreen hostScreen, ILobbyService lobbyService, FilterViewModel filterViewModel = null!,
            IViewModelFactory<CreateLobbyViewModel> createLobbyViewModelFactory = null!, IViewModelFactory<LobbyViewModel> lobbyViewModelFactory = null!)
        {
            HostScreen = hostScreen;
            _lobbyService = lobbyService;

            FilterViewModel = filterViewModel ?? Locator.Current.GetService<IViewModelFactory<FilterViewModel>>()!.GetViewModel(hostScreen);
            _createLobbyViewModelFactory = createLobbyViewModelFactory ?? Locator.Current.GetService<IViewModelFactory<CreateLobbyViewModel>>()!;
            _lobbyViewModelFactory = lobbyViewModelFactory ?? Locator.Current.GetService<IViewModelFactory<LobbyViewModel>>()!;

            JoinLobbyCommand = ReactiveCommand.Create<(Guid, string?)>(async (guid) => await JoinLobby(guid));
            CreateLobbyCommand = ReactiveCommand.Create(CreateLobby);

            Lobbies = FilterViewModel.Filter.CombineLatest(_lobbyService.Lobbies, resultSelector: FilterLobbies);

            _lobbyService.StartListiningLobbies();
        }

        private void CreateLobby()
        {
            _lobbyService.StopListining();
            var viewModel = _createLobbyViewModelFactory.GetViewModel(HostScreen);
            HostScreen.Router.Navigate.Execute(viewModel);
        }

        private async Task JoinLobby((Guid lobbyId, string? password) param)
        {
            var joinResult = await _lobbyService.JoinLobby(param.lobbyId, param.password);

            if (joinResult.Status == JoinResultStatus.Success)
            {
                _lobbyService.StopListining();
                var viewModel = _lobbyViewModelFactory.GetViewModel(HostScreen);
                await HostScreen.Router.Navigate.Execute(viewModel);
            }
            else
                await HandleFailedJoinRequest(joinResult);
        }

        private async Task HandleFailedJoinRequest(JoinResult joinResult) => await MessageBoxHelper.ShowErrorMessageBoxAsync(joinResult.ToString());

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