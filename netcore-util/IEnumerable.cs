using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// distinct with lambda
        /// </summary>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> lst, Func<T, TKey> keySelector)
        {
            return lst.GroupBy(keySelector).Select(w => w.First());
        }

        /// <summary>
        /// enumerable extension to enumerate itself into an (item, idx) set
        /// </summary>
        public static IEnumerable<(T item, int idx)> WithIndex<T>(this IEnumerable<T> en)
            => en.Select((item, idx) => (item, idx));

        /// <summary>
        /// enumerable extension to enumerate itself into an (item, idx, isLast) set
        /// </summary>
        public static IEnumerable<(T item, int idx, bool isLast)> WithIndexIsLast<T>(this IEnumerable<T> en)
        {
            var enm = en.GetEnumerator();

            var idx = 0;
            var isLast = !enm.MoveNext();
            var item = default(T);
            while (!isLast)
            {
                item = enm.Current;
                isLast = !enm.MoveNext();
                yield return (item, idx, isLast);
                if (isLast) yield break;
                ++idx;
            }
        }

    }

}
