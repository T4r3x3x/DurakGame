using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DurakClient.Comparers
{
    public class EnumerablesEqualityComparer<T, TEqaultiyComparer> : IEqualityComparer<IEnumerable<T>> where TEqaultiyComparer : IEqualityComparer<T>, new()
    {
        private readonly TEqaultiyComparer _comparer = new();

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (x is null || y is null) return x == y;

            if (x.Count() != y.Count()) return false;

            var xEnumerator = x.GetEnumerator();
            var yEnumerator = y.GetEnumerator();
            while (xEnumerator.MoveNext())
            {
                yEnumerator.MoveNext();
                var xCurrent = xEnumerator.Current;
                var yCurrent = yEnumerator.Current;

                if (!_comparer.Equals(xCurrent, yCurrent))
                    return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] IEnumerable<T> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}