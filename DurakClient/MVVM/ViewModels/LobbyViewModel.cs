using DurakClient.MVVM.Models;
using DurakClient.Services.LobbyServices;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbyViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly ILobbyService _lobbyService;

        public IObservable<IEnumerable<Player>> Players { get; }
        public ReactiveCommand<Unit, Unit> KickPlayerCommand { get; }
        public ReactiveCommand<Unit, Unit> GetReadyCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveLobbyCommand { get; }
        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteLobbyCommand { get; }

        public string? UrlPathSegment => "Lobby view";
        public IScreen HostScreen { get; }

        public LobbyViewModel(ILobbyService lobbyService, IScreen hostScreen)
        {
            _lobbyService = lobbyService;
            HostScreen = hostScreen;

            Players = _lobbyService.Players;
        }
    }
}