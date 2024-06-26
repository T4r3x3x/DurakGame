﻿using AutoMapper;

using Connections.Services;

using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

namespace Server.Services
{
    public class GameService : Connections.Services.GameService.GameServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ConnectionResources _resources;
        private readonly ILogger<GameService> _logger;

        private static readonly Empty s_empty = new Empty();

        public GameService(IMapper mapper, ConnectionResources resources, ILogger<GameService> logger)
        {
            _mapper = mapper;
            _resources = resources;
            _logger = logger;
        }

        public async Task StartGame(Game game) => game.StartGame();

        public override Task<Empty> EndTurn(ActionRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.LobbyId, request.LobbyId);
            game!.EndTurn(player);
            return Task.FromResult(s_empty);
        }

        public override Task<Empty> GiveUp(ActionRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.LobbyId, request.LobbyId);
            game!.GiveUp(player);
            return Task.FromResult(s_empty);
        }

        public override Task<Empty> StartGame(GameId request, ServerCallContext context)
        {
            return base.StartGame(request, context);
        }

        public override Task<Empty> ThrowAttackCard(ThrowAttackCardRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.ActionRequest.LobbyId, request.ActionRequest.LobbyId);
            Card card = FindCard(request.Card, player);
            game!.ThrowAttackCard(player, card);
            return Task.FromResult(s_empty);
        }

        public override Task<Empty> ThrowDeffenceCard(ThrowDefenceCardRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.ActionRequest.LobbyId, request.ActionRequest.LobbyId);
            Card card = FindCard(request.Card, player);
            game.ThrowDeffenceCard(player, card, request.Position);
            return Task.FromResult(s_empty);
        }
        private Card FindCard(CardMessage messageCard, Player player)
        {
            var playingCard = _mapper.Map<Card>(messageCard);
            var card = player.Cards.Where(x => x == playingCard).SingleOrDefault();
            if (card == null)
                new RpcException(new Status(StatusCode.NotFound, $"Can't find a card [{messageCard.Rank},{messageCard.Suit}]"));

            return card!;
        }

        private (Game, Player) GetGameEntities(string lobbyId, string userId)
        {
            var lobby = _resources.GetLobby(lobbyId);
            var user = _resources.GetUser(userId);
            var player = GetGameSidePlayer(lobby, user);

            if (lobby.Game == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a game in this lobby: {lobbyId}"));

            return (lobby.Game, player);
        }

        private Player GetGameSidePlayer(Lobby lobby, User user)
        {
            var index = lobby.Players.IndexOf(user);
            var gamePlayer = lobby.Game!.Players[index];

            return gamePlayer;
        }
    }
}