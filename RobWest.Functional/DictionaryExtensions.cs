using System;
using System.Collections.Generic;

namespace RobWest.Functional
{
    public static class DictionaryExtensions
    {
        public static Func<TKey, TValue> ToLookupWithDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> @this,
            TValue defaultValue) =>
            x => 
                @this.ContainsKey(x) 
                    ? @this[x]
                    : defaultValue;
    }
}