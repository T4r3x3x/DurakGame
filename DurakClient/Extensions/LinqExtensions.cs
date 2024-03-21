using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DurakClient.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> collection, bool condition, Func<T, bool> predicate)
        {
            if (condition)
                return collection.Where(predicate);
            return collection;
        }
        //public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> collection, bool condition, Func<T, string, bool> predicate)
        //{
        //    if (condition)
        //        collection.Where(predicate);
        //    return collection;
        //}
        public static Subject<IEnumerable<T>> WhereIf<T>(this Subject<IEnumerable<T>> collection, bool condition, Func<IEnumerable<T>, bool> predicate)
        {
            if (condition)
                collection.Where(predicate);
            return collection;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in source)
                result.Add(item);
            return result;
        }
    }
}