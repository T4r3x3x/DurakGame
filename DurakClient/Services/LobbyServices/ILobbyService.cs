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
        public Task StartListiningLobbyState(Guid lobbyId);
        public void StopListiningLobbies();
        public Task<bool> CreateLobby(CreateLobbyModel createModel);
        public Task DeleteLobby(Guid lobbyId);
        public Task<JoinResult> JoinLobby(Guid lobbyId, string? password);
        public Task LeaveLobby(Guid lobbyId);
        public Task StartGame(Guid lobbyId);
        public Task PrepareToGame(Guid lobbyId);
        public Task KickPlayer(Guid lobbyId, Guid KickingPlayerId);
    }
}