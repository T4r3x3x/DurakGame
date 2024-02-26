using AutoMapper;

using Connections.Services;

using System;
using System.Threading.Tasks;

using static Connections.Services.GameService;

namespace DurakClient.Services
{
    public class GameService
    {
        private readonly GameServiceClient _gameService;
        private readonly IMapper _mapper;
        private readonly Guid _userId, _lobbyId;
        private readonly ActionRequest _actionRequest;

        public GameService(GameServiceClient gameService, IMapper mapper, Guid userId, Guid lobbyId)
        {
            _gameService = gameService;
            _mapper = mapper;
            _userId = userId;
            _lobbyId = lobbyId;

            _actionRequest = new()
            {
                LobbyId = lobbyId.ToString(),
                SenderId = _userId.ToString(),
            };
        }

        public async Task EndTurn()
        {
            await _gameService.EndTurnAsync(_actionRequest);
        }
        public async Task GiveUp()
        {
            await _gameService.GiveUpAsync(_actionRequest);
        }
        public async Task ThrowDeffenceCard(Card card, int position)
        {
            var requset = new ThrowDefenceCardRequest()
            {
                ActionRequest = _actionRequest,
                Card = card,
                Position = position
            };
            await _gameService.ThrowDeffenceCardAsync(requset);
        }
        public async Task ThrowAttackCard(Card card)
        {
            var request = new ThrowAttackCardRequest()
            {
                ActionRequest = _actionRequest,
                Card = card
            };
            await _gameService.ThrowAttackCardAsync(request);
        }
    }
}