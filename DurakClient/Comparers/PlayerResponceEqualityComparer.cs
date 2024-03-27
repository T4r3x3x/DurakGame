using Connections.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DurakClient.Comparers
{
    internal class PlayerResponceEqualityComparer : IEqualityComparer<PlayerResponce>
    {
        public bool Equals(PlayerResponce? x, PlayerResponce? y) => x?.Nickname == y?.Nickname && x?.AreReady == y?.AreReady;

        public int GetHashCode([DisallowNull] PlayerResponce obj)
        {
            throw new NotImplementedException();
        }
    }
}