using AutoMapper;

using Connections.Services;

using GameEngine.Entities.SystemEntites;
using GameEngine.Factories;
using GameEngine.Shufflers;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.CustomCollections;
using Server.Entities;

namespace Server.Services
{
    public class LobbyService : Connections.Services.LobbyService.LobbyServiceBase
    {
        private readonly IMapper _mapper;
        //   private readonly GameService _gameService;
        private readonly ConnectionResources _resources;
        private readonly ILogger<LobbyService> _logger;
        private readonly LobbyListResponce _lobbyListResponce = new();
        private LobbyStateResponce _lobbyStateResponce = new();

        private static readonly Empty s_empty = new();

        public LobbyService(ILogger<LobbyService> logger, IMapper mapper, /*GameService gameService,*/ ConnectionResources resources)
        {
            _logger = logger;
            _mapper = mapper;
            // _gameService = gameService;
            _resources = resources;
            LobbyListResponceUpdate(_resources.Lobbies);
            _resources.Lobbies.DataChanged += LobbyListResponceUpdate;
        }

        public override Task<CreateLobbyResponce> CreateLobby(CreateLobbyRequest request, ServerCallContext context)
        {
            GameSettings settings = _mapper.Map<GameSettings>(request.GameSettings);

            var creator = _resources.GetUser(request.CreatorId);
            creator.ReadyStatus = true;

            Lobby lobby = new Lobby()
            {
                Guid = Guid.NewGuid(),
                Owner = creator,
                Name = request.Name,
                Password = request.Password,
                Players = new PlayersList(settings.PlayersCount),
                Settings = settings
            };
            lobby.Players.TryAdd(creator);
            _resources.Lobbies.TryAdd(lobby.Guid, lobby);

            _logger.LogInformation($"Lobby {lobby.Guid} has been created!");

            var responce = new CreateLobbyResponce() { LobbyId = lobby.Guid.ToString(), IsSuccessefully = true };
            return Task.FromResult(responce);
        }

        public override Task<Empty> DeleteLobby(ActionRequest request, ServerCallContext context)
        {
            var guid = ConnectionResources.ParseGuid(request.LobbyId);
            _resources.Lobbies.Remove(guid, out var deletedLobby);

            _logger.LogInformation($"Lobby {guid} has been deleted!");

            return Task.FromResult(s_empty);
        }

        public override async Task GetLobbiesStream(Empty request, IServerStreamWriter<LobbyListResponce> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                responseStream.WriteAsync(_lobbyListResponce);
                await Task.Delay(10);
            }
        }

        public override Task<JoinResponce> JoinLobby(JoinRequest request, ServerCallContext context)
        {
            var lobbyId = ConnectionResources.ParseGuid(request.ActionRequest.LobbyId);
            var playerId = ConnectionResources.ParseGuid(request.ActionRequest.SenderId);

            var playerSearchResult = _resources.Users.TryGetValue(playerId, out var player);
            if (!playerSearchResult)
                throw new RpcException(new(StatusCode.NotFound, $"Can't find a player with id: {playerId}"));

            var lobbySearchResult = _resources.Lobbies.TryGetValue(lobbyId, out var lobby);
            if (!lobbySearchResult)
                throw new RpcException(new(StatusCode.NotFound, $"Can't find a lobby with id: {lobbyId}"));

            if (request.Password != lobby!.Password)
                throw new RpcException(new(StatusCode.PermissionDenied, $"Wrong password!"));

            var addingResult = lobby!.Players.TryAdd(player!);
            if (!addingResult)
                throw new RpcException(new(StatusCode.ResourceExhausted, "Lobby is already full!"));


            _logger.LogInformation($"Player {playerId} has been joined to lobby {lobbyId}!");

            var responce = new JoinResponce() { LobbyId = lobbyId.ToString(), IsSuccessefully = true };
            return Task.FromResult(responce);
        }

        public override Task<Empty> KickPlayer(KickPlayerRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.ActionRequest.LobbyId);
            var kickingPlayer = _resources.GetUser(request.KickingPlayerId);
            var kicker = _resources.GetUser(request.ActionRequest.SenderId);

            if (kicker != lobby!.Owner)
                throw new RpcException(new(StatusCode.PermissionDenied, "You are not onwer of the lobby!"));

            var kickingResult = lobby!.Players.TryRemove(kickingPlayer!);
            if (!kickingResult)
                throw new RpcException(new(StatusCode.NotFound, "This player is not into lobby"));

            _logger.LogInformation($"Player {kicker.Guid} kicked a player {kickingPlayer.Guid} for lobby {lobby.Guid}!");

            return Task.FromResult(s_empty);
        }

        public override Task<Empty> LeaveLobby(ActionRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.LobbyId);
            var player = _resources.GetUser(request.SenderId);

            var leavingResult = lobby!.Players.TryRemove(player!);
            if (!leavingResult)
                throw new RpcException(new(StatusCode.NotFound, "This player is not into lobby"));

            _logger.LogInformation($"Player {player.Guid} leaved lobby {lobby.Guid}!");

            return Task.FromResult(s_empty);
        }

        public override Task<Empty> SetReadyStatus(ReadyStatusRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.ActionRequest.LobbyId);
            var player = _resources.GetUser(request.ActionRequest.SenderId);
            lobby.Players.Where(x => x == player).Single().ReadyStatus = request.Status;

            _logger.LogInformation($"Player {player.Guid} prepared to game in lobby {lobby.Guid}!");

            return Task.FromResult(s_empty);
        }

        public override Task<Empty> StartGame(ActionRequest request, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.LobbyId);
            var AreEverybodyIsReady = lobby.Players.Where(x => x.ReadyStatus).Count() == lobby.Players.Count();

            if (!AreEverybodyIsReady)
                return Task.FromResult(new Empty());

            var gameFactory = new GameFactory();
            lobby.Game = gameFactory.GetGameManager(lobby.Settings, new FisherYatesShuffler<GameEngine.Entities.GameEntities.Card>());
            //  _gameService.StartGame(lobby.Game);

            _logger.LogInformation($"Game has been started in lobby {lobby.Guid}!");

            return Task.FromResult(s_empty);
        }

        public override async Task GetLobbyStateStream(LobbyStateRequest request, IServerStreamWriter<LobbyStateResponce> responseStream, ServerCallContext context)
        {
            var lobby = _resources.GetLobby(request.Id);
            lobby.Players.OnDataChanged += LobbyStateResponceUpdate;
            _lobbyStateResponce.Players.AddRange(lobby.Players.Select(x =>
                new PlayerResponce()
                {
                    Nickname = x.NickName,
                    AreReady = x.ReadyStatus
                }));
            while (!context.CancellationToken.IsCancellationRequested)
            {
                responseStream.WriteAsync(_lobbyStateResponce);
                await Task.Delay(10);
            }
            lobby.Players.OnDataChanged -= LobbyStateResponceUpdate;
        }

        private void LobbyStateResponceUpdate(IEnumerable<User> users)
        {
            _lobbyStateResponce.Players.Clear();
            _lobbyStateResponce.Players.AddRange(users.Select(x =>
                new PlayerResponce()
                {
                    Nickname = x.NickName,
                    AreReady = x.ReadyStatus
                }));
            //NOTE: костыль, не работает, так как вызывается только у игроков в лобби,
            //игроки, которые просматривают все лобби не увидят изменения
        }

        private void LobbyListResponceUpdate(IEnumerable<KeyValuePair<Guid, Lobby>> lobbies)
        {
            _lobbyListResponce.LobbyList.Clear();
            _lobbyListResponce.LobbyList
                .AddRange(_resources.Lobbies
                .Where(x => !x.Value.IsFull)
                .Select(x => _mapper.Map<LobbyResponce>(x.Value)
                ));
        }
    }
}