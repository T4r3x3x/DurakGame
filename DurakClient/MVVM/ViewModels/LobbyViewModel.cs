using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.Models;
using DurakClient.Services;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

using Splat;

using System;
using System.Collections.Generic;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbyViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly ILobbyService _lobbyService;
        private readonly Resources _resources;
        private readonly IViewModelFactory<LobbiesViewModel> _factory;

        public IObservable<IEnumerable<Player>> Players { get; }
        public ReactiveCommand<Guid, Unit> KickPlayerCommand { get; }
        public ReactiveCommand<Unit, Unit> GetReadyCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveLobbyCommand { get; }
        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteLobbyCommand { get; }

        public string? UrlPathSegment => "Lobby";
        public IScreen HostScreen { get; }

        public LobbyViewModel(ILobbyService lobbyService, Resources resources, IScreen hostScreen, IViewModelFactory<LobbiesViewModel> factory = null!)
        {
            _lobbyService = lobbyService;
            _resources = resources;
            HostScreen = hostScreen;
            _factory = factory ?? Locator.Current.GetService<IViewModelFactory<LobbiesViewModel>>()!;

            LeaveLobbyCommand = ReactiveCommand.Create(LeaveLobby);
            DeleteLobbyCommand = ReactiveCommand.Create(DeleteLobby);
            GetReadyCommand = ReactiveCommand.Create(GetReady);
            StartGameCommand = ReactiveCommand.Create(StartGame);
            KickPlayerCommand = ReactiveCommand.Create<Guid>(KickPlayer);

            Players = _lobbyService.Players;
            Players.Subscribe((_) => Loop(_));
            _lobbyService.StartListiningLobbyState();
        }

        private void Loop(IEnumerable<Player> players)
        {

        }

        private void KickPlayer(Guid kickingPlayerId) => _lobbyService.KickPlayer(kickingPlayerId);

        private void StartGame() => _lobbyService.StartGame();

        private void GetReady() => _lobbyService.PrepareToGame();

        private void DeleteLobby()
        {
            _lobbyService.DeleteLobby();
            NavigateBackToLobbies();
        }

        private void LeaveLobby()
        {
            _lobbyService.LeaveLobby();
            NavigateBackToLobbies();
        }

        private void NavigateBackToLobbies()
        {
            _lobbyService.StopListining();
            var lobbiesVm = _factory.GetViewModel(HostScreen);
            HostScreen.Router.NavigateAndReset.Execute(lobbiesVm);
        }
    }
}