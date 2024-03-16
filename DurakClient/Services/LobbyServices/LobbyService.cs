using AutoMapper;

using Connections.Services;

using DurakClient.MVVM.Models;

using Google.Protobuf.WellKnownTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using static Connections.Services.LobbyService;

namespace DurakClient.Services.LobbyServices
{
    public class LobbyService : ILobbyService
    {
        private readonly LobbyServiceClient _lobbyService;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        private static readonly Empty s_empty = new Empty();

        private readonly BehaviorSubject<List<Lobby>> _lobbySubject;

        public LobbyService(LobbyServiceClient clientService, Resources resources, IMapper mapper)
        {
            _lobbyService = clientService;
            _resources = resources;
            _mapper = mapper;
        }

        public async Task<List<Lobby>> GetAllLobby()
        {
            var responce = await _lobbyService.GetAllLobiesAsync(s_empty);
            return responce.LobbyList_
                                    .Select(x => _mapper.Map<Lobby>(x))
                                    .ToList();
        }

        public async Task CreateLobby(LobbyCreateModel createModel)
        {
            var createRequest = _mapper.Map<LobbyCreateRequest>(createModel);
            var responceStream = _lobbyService.CreateLobby(createRequest);


            //todo listining stream


        }

        public async Task DeleteLobby(Guid lobbyId)
        {
            var actionRequest = GetActionRequest(lobbyId);
            await _lobbyService.DeleteLobbyAsync(actionRequest);
        }
        public async Task JoinLobby(Guid lobbyId, string? password)
        {
            var actionRequest = GetActionRequest(lobbyId);

            var joinRequest = new JoinRequest() { ActionRequest = actionRequest, Password = password };
            var responceStream = _lobbyService.JoinLobby(joinRequest);


            //todo listining stream


        }

        public async Task LeaveLobby(Guid lobbyId)
        {
            var actionRequest = GetActionRequest(lobbyId);
            await _lobbyService.LeaveLobbyAsync(actionRequest);
        }

        public async Task StartGame(Guid lobbyId)
        {
            var actionRequest = GetActionRequest(lobbyId);
            await _lobbyService.StartGameAsync(actionRequest);
        }

        public async Task PrepareToGame(Guid lobbyId)
        {
            var actionRequest = GetActionRequest(lobbyId);
            await _lobbyService.PrepareToGameAsync(actionRequest);
        }

        public async Task KickPlayer(Guid lobbyId, Guid KickingPlayerId)
        {
            ActionRequest actionRequest = GetActionRequest(lobbyId);
            var request = new KickPlayerRequest()
            {
                ActionRequest = actionRequest,
                KickingPlayerId = KickingPlayerId.ToString()
            };

            await _lobbyService.KickPlayerAsync(request);
        }

        private ActionRequest GetActionRequest(Guid lobbyId)
        {
            return new ActionRequest
            {
                LobbyId = lobbyId.ToString(),
                SenderId = _resources.Guid.ToString()
            };
        }
    }
}