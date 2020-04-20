using System;
using System.Linq;
using System.Collections.Generic;

namespace BehavioralLink
{
    internal static class Enumerables
    {
        public static IEnumerable<T> Where<T, A>(this IEnumerable<T> items, Func<T, A, bool> filter, A a) =>
            items.Where(item => filter(item, a));

        public static IEnumerable<T> Iterate<T, A>(this IEnumerable<T> items, Action<T, A> action, A a)
        {
            foreach (var item in items)
            {
                action(item, a);
                yield return item;
            }
        }

        public static void EvaluateAndIgnore<T>(this IEnumerable<T> items)
        {
            items.ToList();
        }
    }
}