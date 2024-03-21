using Grpc.Core;

using Server.Services;

namespace Server.Entities
{
    public class ConnectionResources
    {
        public NotifyingConcurrentDictionary<Guid, User> Users { get; set; } = new(); //todo клиент может не вызвать disconnect, удалять автоматически через сутки после подключения? (квартц?)
        public NotifyingConcurrentDictionary<Guid, Lobby> Lobbies { get; set; } = new();

        public ConnectionResources()
        {
            Lobbies = LobbiesCreatorForTest();
        }

        private NotifyingConcurrentDictionary<Guid, Lobby> LobbiesCreatorForTest()
        {
            var result = new NotifyingConcurrentDictionary<Guid, Lobby>();
            for (int i = 0; i < 10; i++)
            {
                var guid = Guid.NewGuid();
                var lobby = new Lobby()
                {
                    Guid = guid,
                    Name = i.ToString(),
                    Owner = null,
                    Players = new List<User>(),
                    Password = i % 2 == 0 ? null : i.ToString(),
                    Settings = new()
                    {
                        DeckType = i % 2 == 0 ? GameEngine.Entities.SystemEntites.DeckType.Common : GameEngine.Entities.SystemEntites.DeckType.Extended,
                        PlayersCount = i,
                        PlayersStartCardsCount = i
                    }
                };
                for (int j = 0; j < i; j++)
                    lobby.Players.Add(new User()
                    {
                        Guid = guid,
                        NickName = j.ToString()
                    });
                result.TryAdd(guid, lobby);
            }
            return result;
        }

        public User GetUser(string guidString)
        {
            var guid = ParseGuid(guidString);
            var searchResult = Users.TryGetValue(guid, out var player);
            if (!searchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a player with id: {guid}"));

            return player!;
        }
        //todo написать абстракцию для обоих методов? 
        public Lobby GetLobby(string guidString)
        {
            var guid = ParseGuid(guidString);
            var searchResult = Lobbies.TryGetValue(guid, out var lobby);
            if (!searchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a lobby with id: {guid}"));

            return lobby!;
        }

        public static Guid ParseGuid(string line)
        {
            Guid guid;
            var parseResult = Guid.TryParse(line, out guid);

            if (!parseResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find an object with id: {guid}"));
            return guid;
        }
    }
}
