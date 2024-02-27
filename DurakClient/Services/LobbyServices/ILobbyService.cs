using DurakClient.MVVM.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurakClient.Services.LobbyServices
{
    public interface ILobbyService
    {
        public Task<List<Lobby>> GetAllLobby();
        public Task CreateLobby(LobbyCreateModel createModel);
        public Task DeleteLobby(Guid lobbyId);
        public Task JoinLobby(Guid lobbyId, string? password);
        public Task LeaveLobby(Guid lobbyId);
        public Task StartGame(Guid lobbyId);
        public Task PrepareToGame(Guid lobbyId);
        public Task KickPlayer(Guid lobbyId, Guid KickingPlayerId);
    }
}