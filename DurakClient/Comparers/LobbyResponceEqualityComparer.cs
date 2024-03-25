using Connections.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DurakClient.Comparers
{
    internal class LobbyResponceEqualityComparer : IEqualityComparer<LobbyResponce>
    {
        public bool Equals(LobbyResponce? x, LobbyResponce? y)
        {
            if (x is null || y is null) return x == y;
            return x.Id == y.Id && x.JoindePlayersCount == y.JoindePlayersCount;
        }

        public int GetHashCode([DisallowNull] LobbyResponce obj)
        {
            throw new NotImplementedException();
        }
    }
}
