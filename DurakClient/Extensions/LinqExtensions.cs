using System;
using System.Collections.Generic;
using System.Linq;

namespace DurakClient.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> collection, bool condition, Func<T, bool> predicate)
        {
            if (condition)
                collection.Where(predicate);
            return collection;
        }
    }
}