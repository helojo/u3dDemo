namespace fastJSON
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class SafeDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _Dictionary;
        private readonly object _Padlock;

        public SafeDictionary()
        {
            this._Padlock = new object();
            this._Dictionary = new Dictionary<TKey, TValue>();
        }

        public SafeDictionary(int capacity)
        {
            this._Padlock = new object();
            this._Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public void Add(TKey key, TValue value)
        {
            object obj2 = this._Padlock;
            lock (obj2)
            {
                if (!this._Dictionary.ContainsKey(key))
                {
                    this._Dictionary.Add(key, value);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            object obj2 = this._Padlock;
            lock (obj2)
            {
                return this._Dictionary.TryGetValue(key, out value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                object obj2 = this._Padlock;
                lock (obj2)
                {
                    return this._Dictionary[key];
                }
            }
            set
            {
                object obj2 = this._Padlock;
                lock (obj2)
                {
                    this._Dictionary[key] = value;
                }
            }
        }
    }
}

