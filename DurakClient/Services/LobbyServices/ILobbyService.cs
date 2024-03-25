using DurakClient.MVVM.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurakClient.Services.LobbyServices
{
    public interface ILobbyService
    {
        public IObservable<IEnumerable<Lobby>> Lobbies { get; }
        public void StartListiningLobbies();
        public void StopListiningLobbies();
        public Task<bool> CreateLobby(CreateLobbyModel createModel);
        public Task DeleteLobby(Guid lobbyId);
        public Task<bool> JoinLobby(Guid lobbyId, string? password);
        public Task LeaveLobby(Guid lobbyId);
        public Task StartGame(Guid lobbyId);
        public Task PrepareToGame(Guid lobbyId);
        public Task KickPlayer(Guid lobbyId, Guid KickingPlayerId);
    }
}