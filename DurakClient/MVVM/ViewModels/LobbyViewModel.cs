using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.Models;
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
        private readonly IViewModelFactory<LobbiesViewModel> _factory;
        private bool _readyStatus = false;

        public bool ReadyStatus
        {
            get => _readyStatus;
            set
            {
                this.RaiseAndSetIfChanged(ref _readyStatus, value);
                SetReadyStatus(value);
            }
        }
        public IObservable<IEnumerable<Player>> Players { get; }
        public ReactiveCommand<Guid, Unit> KickPlayerCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveLobbyCommand { get; }
        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteLobbyCommand { get; }
        public string? UrlPathSegment => "Lobby";
        public IScreen HostScreen { get; }

        public LobbyViewModel(ILobbyService lobbyService, IScreen hostScreen, IViewModelFactory<LobbiesViewModel> factory = null!)
        {
            _lobbyService = lobbyService;
            HostScreen = hostScreen;
            _factory = factory ?? Locator.Current.GetService<IViewModelFactory<LobbiesViewModel>>()!;

            LeaveLobbyCommand = ReactiveCommand.Create(LeaveLobby);
            DeleteLobbyCommand = ReactiveCommand.Create(DeleteLobby);
            StartGameCommand = ReactiveCommand.Create(StartGame);
            KickPlayerCommand = ReactiveCommand.Create<Guid>(KickPlayer);

            Players = _lobbyService.Players;

            _lobbyService.StartListiningLobbyState();
        }

        private void KickPlayer(Guid kickingPlayerId) => _lobbyService.KickPlayer(kickingPlayerId);

        private void StartGame() => _lobbyService.StartGame();

        private void SetReadyStatus(bool readyStatus) => _lobbyService.SetReadyStatus(readyStatus);

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