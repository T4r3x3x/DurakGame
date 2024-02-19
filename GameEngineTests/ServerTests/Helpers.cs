﻿using Server.Entities;

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

        internal static Lobby AddNewLobby(User user, ConnectionResources resources, string name = "", string password = "")
        {
            Guid guid = Guid.NewGuid();
            Lobby lobby = new Lobby() { Guid = guid, Name = name, Owner = user, Password = password, Players = new(), Settings = null };
            resources.Lobbies.Add(guid, lobby);
            return lobby;
        }
    }
}
