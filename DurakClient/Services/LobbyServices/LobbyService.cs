﻿using AutoMapper;

using Connections.Services;

using DurakClient.Comparers;
using DurakClient.MVVM.Models;
using DurakClient.Results;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using static Connections.Services.LobbyService;

namespace DurakClient.Services.LobbyServices
{
    public class LobbyService : ILobbyService
    {
        private readonly LobbyServiceClient _lobbyService;
        private readonly IMapper _mapper;
        private readonly Resources _resources;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly BehaviorSubject<List<LobbyResponce>> _lobbiesResponce = new([]);
        private readonly BehaviorSubject<LobbyStateResponce> _lobbyStateResponce = new(new());

        private static readonly EnumerablesEqualityComparer<LobbyResponce, LobbyResponceEqualityComparer> s_lobbyResponceComparer = new();
        private static readonly EnumerablesEqualityComparer<PlayerResponce, PlayerResponceEqualityComparer> s_playerResponceComparer = new();
        private static readonly Empty s_empty = new Empty();

        public IObservable<IEnumerable<Player>> Players { get; }
        public IObservable<IEnumerable<Lobby>> Lobbies { get; }

        public LobbyService(LobbyServiceClient clientService, Resources resources, IMapper mapper)
        {
            _lobbyService = clientService;
            _resources = resources;
            _mapper = mapper;

            Lobbies = _lobbiesResponce.DistinctUntilChanged(s_lobbyResponceComparer)
                .Select(x => x
                .Select(x => _mapper.Map<Lobby>(x)));
            Players = _lobbyStateResponce.Select(x => x?.Players).DistinctUntilChanged(s_playerResponceComparer)
                .Select(x => x
                .Select(x => _mapper.Map<Player>(x)));
        }

        public async Task StartListiningLobbies()
        {
            var token = _cancellationTokenSource.Token;
            var responce = _lobbyService.GetLobbiesStream(s_empty, cancellationToken: token);
            var stream = responce.ResponseStream;
            while (await stream.MoveNext(token))
            {
                var message = stream.Current;
                _lobbiesResponce.OnNext(message.LobbyList.ToList());
            }
        }

        public async Task<bool> CreateLobby(CreateLobbyModel createModel)
        {
            var createRequest = _mapper.Map<CreateLobbyRequest>(createModel);
            var responce = await _lobbyService.CreateLobbyAsync(createRequest);

            if (responce.IsSuccessefully)
                _resources.LobbyId = Guid.Parse(responce.LobbyId);

            return responce.IsSuccessefully;
        }

        public async Task DeleteLobby()
        {
            var actionRequest = GetActionRequest(_resources.LobbyId);
            await _lobbyService.DeleteLobbyAsync(actionRequest);
        }

        public async Task<JoinResult> JoinLobby(Guid lobbyId, string? password)
        {
            var actionRequest = GetActionRequest(lobbyId);

            var joinRequest = new JoinRequest() { ActionRequest = actionRequest, Password = password };
            try
            {
                var responce = _lobbyService.JoinLobby(joinRequest);

                if (responce.IsSuccessefully)
                    _resources.LobbyId = Guid.Parse(responce.LobbyId);

                return JoinResult.Success;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.PermissionDenied)
            {
                return JoinResult.WrongPassword;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                return JoinResult.LobbyNotFound;
            }
            catch
            {
                return JoinResult.UnkownException;
            }
        }

        public async Task StartListiningLobbyState()
        {
            var token = _cancellationTokenSource.Token;
            var lobbyStateRequest = new LobbyStateRequest() { Id = _resources.LobbyId.ToString() };
            var responceStream = _lobbyService.GetLobbyStateStream(lobbyStateRequest, cancellationToken: token).ResponseStream;
            while (await responceStream.MoveNext(token))
            {
                var message = responceStream.Current;
                _lobbyStateResponce.OnNext(message);
            }
        }

        public async Task LeaveLobby()
        {
            var actionRequest = GetActionRequest(_resources.LobbyId);
            await _lobbyService.LeaveLobbyAsync(actionRequest);
        }

        public async Task StartGame()
        {
            var actionRequest = GetActionRequest(_resources.LobbyId);
            await _lobbyService.StartGameAsync(actionRequest);
        }

        public async Task PrepareToGame()
        {
            var actionRequest = GetActionRequest(_resources.LobbyId);
            await _lobbyService.PrepareToGameAsync(actionRequest);
        }

        public async Task KickPlayer(Guid KickingPlayerId)
        {
            ActionRequest actionRequest = GetActionRequest(_resources.LobbyId);
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
                SenderId = _resources.PlayerId.ToString()
            };
        }

        public void StopListining()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.TryReset();
        }
    }
}