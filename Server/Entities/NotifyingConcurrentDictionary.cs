﻿using System.Collections;
using System.Collections.Concurrent;

namespace Server.Entities
{
    public class NotifyingConcurrentDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary = new();

        public NotifyingConcurrentDictionary() { }

        public NotifyingConcurrentDictionary(ConcurrentDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public event Action<IEnumerable<KeyValuePair<TKey, TValue>>>? OnDataChanged;

        public bool TryAdd(TKey key, TValue value)
        {
            var result = _dictionary.TryAdd(key, value);
            OnDataChanged?.Invoke(_dictionary.AsEnumerable());
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = _dictionary.TryGetValue(key, out value!);
            return result;
        }

        public bool Remove(TKey key, out TValue value)
        {
            var result = _dictionary.Remove(key, out value!);
            OnDataChanged?.Invoke(_dictionary.AsEnumerable());
            return result;
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
        }

        public int Count => _dictionary.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }
    }
}