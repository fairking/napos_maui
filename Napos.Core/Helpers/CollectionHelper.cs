using System;
using System.Collections.Generic;

namespace Napos.Core.Helpers
{
    public static class CollectionHelper
    {
        public static void ForEachElement<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var el in collection)
                action.Invoke(el);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            values.ForEachElement(x => collection.Add(x));
        }
    }
}
