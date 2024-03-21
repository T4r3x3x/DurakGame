using Connections.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DurakClient.Comparers
{
    internal class LobbyResponceEnumerableEqualityComparer : IEqualityComparer<List<LobbyResponce>>
    {
        private readonly LobbyResponceEqualityComparer _comparer = new();

        public bool Equals(List<LobbyResponce>? x, List<LobbyResponce>? y)
        {
            if (x is null || y is null) return x == y;

            if (x.Count != y.Count) return false;

            for (int i = 0; i < x.Count; i++)
                if (!_comparer.Equals(x[i], y[i]))
                    return false;

            return true;
        }

        public int GetHashCode([DisallowNull] List<LobbyResponce> obj)
        {
            throw new NotImplementedException();
        }
    }
}