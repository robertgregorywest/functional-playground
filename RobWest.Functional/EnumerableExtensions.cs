using System;
using System.Collections.Generic;
using static RobWest.Functional.F;

namespace RobWest.Functional
{
    public static class EnumerableExtensions
    {
        public static Option<T> Lookup<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            foreach (var t in @this)
            {
                if (predicate(t)) return Some(t);
            }
            return None;
        }
    }
}