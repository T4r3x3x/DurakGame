using Grpc.Core;

namespace Server.Entities
{
    public static class Resources
    {
        public static Dictionary<Guid, User> Users { get; set; } = new(); //todo клиент может не вызвать disconnect, удалять автоматически через сутки после подключения? (квартц?)
        public static Dictionary<Guid, Lobby> Lobbies { get; set; } = new();

        public static User GetUser(string guidString)
        {
            var guid = ParseGuid(guidString);
            var searchResult = Users.TryGetValue(guid, out var player);
            if (!searchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a player with id: {guid}"));

            return player!;
        }
        //todo написать абстракцию для обоих методов? 
        public static Lobby GetLobby(string guidString)
        {
            var guid = ParseGuid(guidString);
            var searchResult = Lobbies.TryGetValue(guid, out var lobby);
            if (!searchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a player with id: {guid}"));

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
