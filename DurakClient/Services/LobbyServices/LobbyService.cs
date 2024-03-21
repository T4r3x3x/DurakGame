using AutoMapper;

using Connections.Services;

using DurakClient.Comparers;
using DurakClient.MVVM.Models;

using Google.Protobuf.WellKnownTypes;

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

        private static readonly EnumerablesEqualityComparer<LobbyResponce, LobbyResponceEqualityComparer> s_comparer = new();
        private static readonly Empty s_empty = new Empty();

        public IObservable<IEnumerable<Lobby>> Lobbies { get; }

        public LobbyService(LobbyServiceClient clientService, Resources resources, IMapper mapper)
        {
            _lobbyService = clientService;
            _resources = resources;
            _mapper = mapper;
            Lobbies = _lobbiesResponce.DistinctUntilChanged(s_comparer)
                .Select(x => x
                .Select(x => _mapper.Map<Lobby>(x)));
        }
        private async Task<IEnumerable<Lobby>> GetLobbiesForTest()
        {
            var a = new List<Lobby>() {
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "First",
                    HasPassword = false,
                    JoinedPlayersCount = 0,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Common,
                        PlayersCount = 2,
                        PlayersStartCardsCount = 3
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                },
                new Lobby()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Second",
                    HasPassword = true,
                    JoinedPlayersCount = 3,
                    Settings = new()
                    {
                        DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = 4,
                        PlayersStartCardsCount = 5
                    }
                }};
            return a;
        }

        public async void StartListiningLobbies()
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

        public void StopListiningLobbies()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}