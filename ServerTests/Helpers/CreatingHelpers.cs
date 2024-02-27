using GameEngine.Entities.SystemEntites;

using Server.Entities;

namespace ServerTests.Helpers
{
    internal class CreatingHelpers
    {
        internal static User AddNewUser(string nickName, ConnectionResources resources)
        {
            Guid guid = Guid.NewGuid();
            User user = new User() { Guid = guid, NickName = nickName };
            resources.Users.Add(guid, user);
            return user;
        }

        internal static Lobby AddNewLobby(User user, ConnectionResources resources, GameSettings settings, string name = "", string password = "")
        {
            Guid guid = Guid.NewGuid();
            Lobby lobby = new Lobby() { Guid = guid, Name = name, Owner = user, Password = password, Players = new(), Settings = settings };
            resources.Lobbies.Add(guid, lobby);
            return lobby;
        }
    }
}
