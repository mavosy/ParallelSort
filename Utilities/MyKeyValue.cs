using System;
using System.Collections.Generic;

namespace Utilities
{
    public class MyKeyValue<K, V> : IComparable<MyKeyValue<K, V>> where K : IComparable<K>
    {
        public K Key { get; protected set; }
        public V Value { get; set; }

        public MyKeyValue(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public int CompareTo(MyKeyValue<K, V> other)
        {
            return Key.CompareTo(other.Key);
        }
    }
}
