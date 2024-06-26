﻿using GameEngine.Entities.SystemEntites;

using Server.Entities;
using Server.Services;

namespace ServerTests.Helpers
{
    internal class CreatingHelpers
    {
        internal static User AddNewUser(string nickName, ConnectionResources resources)
        {
            Guid guid = Guid.NewGuid();
            User user = new User() { Guid = guid, NickName = nickName };
            resources.Users.TryAdd(guid, user);
            return user;
        }

        internal static Lobby AddNewLobby(User user, ConnectionResources resources, GameSettings settings, string name = "", string password = "")
        {
            Guid guid = Guid.NewGuid();
            Lobby lobby = new Lobby()
            {
                Guid = guid,
                Name = name,
                Owner = user,
                Password = password,
                Players = new(settings.PlayersCount),
                Settings = settings
            };
            resources.Lobbies.TryAdd(guid, lobby);
            return lobby;
        }
    }
}
