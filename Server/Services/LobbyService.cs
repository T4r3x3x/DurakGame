using AutoMapper;

using Connections.Services;

using GameEngine.Entities.SystemEntites;
using GameEngine.Factories;
using GameEngine.Shufflers;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

using System.Collections.ObjectModel;

namespace Server.Services
{
    public class LobbyService : Connections.Services.LobbyService.LobbyServiceBase
    {
        private readonly IMapper _mapper;
        private readonly GameService _gameService;
        private readonly ConnectionResources _resources;
        private readonly ILogger<LobbyService> _logger;

        private static readonly Empty s_empty = new();

        public LobbyService(ILogger<LobbyService> logger, IMapper mapper, GameService gameService, ConnectionResources resources)
        {
            _logger = logger;
            _mapper = mapper;
            _gameService = gameService;
            _resources = resources;
        }

        public override async Task CreateLobby(LobbyCreateRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            GameSettings settings = _mapper.Map<GameSettings>(request.Settings);

            var creator = _resources.GetUser(request.CreatorId);

            Lobby lobby = new Lobby()
            {
                Guid = new Guid(),
                Owner = creator,
                Name = request.Name,
                Password = request.Password,
                Players = new ObservableCollection<User>(),
                Settings = settings
            };
            lobby.Players.Add(creator);
            _resources.Lobbies.Add(lobby.Guid, lobby);

            var lobbyState = _mapper.Map<LobbyState>(lobby);
            while (!context.CancellationToken.IsCancellationRequested)
                await responseStream.WriteAsync(lobbyState);
        }

        public override Task<Empty> DeleteLobby(ActionRequest request, ServerCallContext context)
        {
            var guid = ConnectionResources.ParseGuid(request.LobbyId);
            _resources.Lobbies.Remove(guid);
            return Task.FromResult(s_empty);
        }

        public override Task<LobbyList> GetAllLobies(Empty request, ServerCallContext context)
        {
            LobbyList lobbyList = new LobbyList();
            lobbyList.LobbyList_.AddRange(_resources.Lobbies.Select(x => _mapper.Map<LobbyModel>(x.Value)));
            return Task.FromResult(lobbyList);
        }

        public override async Task JoinLobby(JoinRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            var lobbyId = ConnectionResources.ParseGuid(request.ActionRequest.LobbyId);
            var playerId = ConnectionResources.ParseGuid(request.ActionRequest.SenderId);

            var playerSearchResult = _resources.Users.TryGetValue(playerId, out var player);
            if (!playerSearchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a player with id: {playerId}"));

            var lobbySearchResult = _resources.Lobbies.TryGetValue(lobbyId, out var lobby);
            if (!lobbySearchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a lobby with id: {lobbyId}"));

            if (request.Password != lobby!.Password)
                throw new RpcException(new Status(StatusCode.PermissionDenied, $"Wrong password!"));

            lobby!.Players.Add(player!);

            var lobbyState = _mapper.Map<LobbyState>(lobby);
            while (!context.CancellationToken.IsCancellationRequested)
                await responseStream.WriteAsync(lobbyState);
        }

        public override Task<Empty> KickPlayer(KickPlayerRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.ActionRequest.LobbyId);
            var kickingPlayer = _resources.GetUser(request.KickingPlayerId);
            var kicker = _resources.GetUser(request.ActionRequest.SenderId);

            if (kicker != lobby!.Owner)
                throw new RpcException(new Status(StatusCode.PermissionDenied, "You are not onwer of the lobby!"));

            lobby!.Players.Remove(kickingPlayer!);

            return Task.FromResult(s_empty);
        }

        public override Task<Empty> LeaveLobby(ActionRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.LobbyId);
            var player = _resources.GetUser(request.SenderId);

            lobby!.Players.Remove(player!);
            return Task.FromResult(s_empty);
        }

        public override Task<Empty> PrepareToGame(ActionRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.LobbyId);
            var player = _resources.GetUser(request.SenderId);
            lobby.Players.Where(x => x == player).Single().AreReady = true;

            return Task.FromResult(s_empty);
        }

        public override Task<Empty> StartGame(ActionRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.LobbyId);
            var AreEverybodyIsReady = lobby.Players.Where(x => x.AreReady).Count() == lobby.Players.Count();

            if (!AreEverybodyIsReady)
                return Task.FromResult(new Empty());

            var gameFactory = new GameFactory();
            lobby.Game = gameFactory.GetGameManager(lobby.Settings, new FisherYatesShuffler<GameEngine.Entities.GameEntities.Card>());

            _gameService.StartGame(lobby.Game);
            return Task.FromResult(s_empty);
        }
    }
}