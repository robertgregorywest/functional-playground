using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobWest.Functional
{
    public class Pattern<R> : IEnumerable
    {
        IList<(Type type, Func<object, R> func)> funcs = new List<(Type, Func<object, R>)>();

        IEnumerator IEnumerable.GetEnumerator() => funcs.GetEnumerator();

        public void Add<T>(Func<T, R> func) => funcs.Add((typeof(T), o => func((T)o)));

        public Pattern<R> Default(Func<R> func)
        {
            Add((object _) => func());
            return this;
        }

        public Pattern<R> Default(R val)
        {
            Add((object _) => val);
            return this;
        }

        public R Match(object value)
        {
            Func<object, R> matchingDel = null;
            try
            {
                matchingDel = funcs.First(InputArgMatchesTypeOf(value)).func;
            }
            catch(InvalidOperationException)
            {
                throw new NonExhaustivePattern();
            }

            return matchingDel(value);
        }

        private static Func<(Type type, Func<object, R> func), bool> InputArgMatchesTypeOf(object value)
            => tuple => tuple.type.GetTypeInfo().IsInstanceOfType(value);
    }
    
    public class NonExhaustivePattern : Exception { }
}