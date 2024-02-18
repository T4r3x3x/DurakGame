using Server.Entities;

namespace Tests.ServerTests
{
    internal class Helpers
    {
        internal static User AddNewUser(string nickName, ConnectionResources resources)
        {
            Guid guid = Guid.NewGuid();
            User user = new User() { Guid = guid, NickName = nickName };
            resources.Users.Add(guid, user);
            return user;
        }

        internal static Lobby AddNewLobby(User user, ConnectionResources resources)
        {
            Guid guid = new Guid();
            Lobby lobby = new Lobby() { Guid = guid, Name = string.Empty, Owner = user, Players = null, Settings = null };
            resources.Lobbies.Add(guid, lobby);
            return lobby;
        }
    }
}
