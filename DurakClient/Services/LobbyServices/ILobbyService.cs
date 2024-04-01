using DurakClient.MVVM.Models;
using DurakClient.Results;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurakClient.Services.LobbyServices
{
    public interface ILobbyService
    {
        public IObservable<IEnumerable<Lobby>> Lobbies { get; }
        public IObservable<IEnumerable<Player>> Players { get; }
        public Task StartListiningLobbies();
        public Task StartListiningLobbyState();
        public void StopListining();
        public Task<bool> CreateLobby(CreateLobbyModel createModel);
        public Task DeleteLobby();
        public Task<JoinResult> JoinLobby(Guid lobbyId, string? password);
        public Task LeaveLobby();
        public Task StartGame();
        public Task PrepareToGame();
        public Task KickPlayer(Guid KickingPlayerId);
    }
}