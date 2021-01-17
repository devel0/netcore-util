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

        /// <summary>
        /// enumerate given items returning a tuple with nullable ( for first hit ) prev element
        /// </summary>
        public static IEnumerable<(T? prev, T item)> WithPrevPrimitive<T>(this IEnumerable<T> en) where T : struct
        {
            var enm = en.GetEnumerator();

            T? prev = null;
            var item = default(T);
            while (enm.MoveNext())
            {
                item = enm.Current;
                yield return (prev, item);
                prev = item;
            }
        }

        /// <summary>
        /// enumerate given items returning a tuple with null ( for first hit ) prev element
        /// </summary>
        public static IEnumerable<(T prev, T item)> WithPrev<T>(this IEnumerable<T> en) where T : class
        {
            var enm = en.GetEnumerator();

            T prev = null;
            var item = default(T);
            while (enm.MoveNext())
            {
                item = enm.Current;
                yield return (prev, item);
                prev = item;
            }
        }

        /*
        // compiler err:
        // A nullable type parameter must be known to be a value type or non-nullable reference type. Consider adding a 'class', 'struct', or type constraint.        

        /// <summary>
        /// enumerate given items returning a tuple with null ( for first hit ) prev element
        /// </summary>
        public static IEnumerable<(T? prev, T item)> WithPrevX<T>(this IEnumerable<T> en)
        {
            var enm = en.GetEnumerator();

            T prev = null;
            var item = default(T);
            while (enm.MoveNext())
            {
                item = enm.Current;
                yield return (prev, item);
                prev = item;
            }
        }
        */

        /// <summary>
        /// enumerate given items returning a tuple with null ( for last hit ) next element
        /// </summary>
        /// <remarks>        
        /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0002.cs)
        /// </remarks>
        public static IEnumerable<(T item, T? next, bool isLast)> WithNextPrimitive<T>(this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : struct
        {
            var enm = en.GetEnumerator();

            T? first = null;
            T? prev = null;
            var item = default(T?);
            var isLast = !enm.MoveNext();
            while (!isLast)
            {
                item = enm.Current;
                isLast = !enm.MoveNext();

                if (first == null)
                {
                    first = prev = item;
                    if (isLast) yield return (item.Value, repeatFirstAtEnd ? first : null, true);
                }
                else
                {
                    yield return (prev.Value, item, false);
                    if (isLast)
                    {
                        yield return (item.Value, repeatFirstAtEnd ? first : null, true);
                        yield break;
                    }
                    prev = item;
                }
            }
        }

        /// <summary>
        /// enumerate given items returning a tuple (prev,item,next,isLast) 
        /// with prev=null for first element
        /// with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
        /// </summary>                
        /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
        /// <remarks>
        /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0004.cs)
        /// </remarks>
        public static IEnumerable<(T? prev, T item, T? next, bool isLast)> WithPrevNextPrimitive<T>(
            this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : struct
        {
            foreach (var x in en.WithNextPrimitive(repeatFirstAtEnd).WithPrevPrimitive())
            {
                var item = x.item.item;
                T? prev = x.prev.HasValue ? x.prev.Value.item : default(T?);
                T? next = x.item.next;
                var isLast = x.item.isLast;

                yield return (prev, item, next, isLast);
            }
        }

        /// <summary>
        /// enumerate given items returning a tuple (item,next,isLast) with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
        /// </summary>                
        /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
        /// <remarks>
        /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0001.cs)
        /// </remarks>
        public static IEnumerable<(T item, T next, bool isLast)> WithNext<T>(this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : class
        {
            var enm = en.GetEnumerator();

            T first = null;
            T prev = null;
            var item = default(T);
            var isLast = !enm.MoveNext();
            while (!isLast)
            {
                item = enm.Current;
                isLast = !enm.MoveNext();

                if (first == null)
                {
                    first = prev = item;
                    if (isLast) yield return (item, repeatFirstAtEnd ? first : null, true);
                }
                else
                {
                    yield return (prev, item, false);
                    if (isLast)
                    {
                        yield return (item, repeatFirstAtEnd ? first : null, true);
                        yield break;
                    }
                    prev = item;
                }
            }
        }

        /// <summary>
        /// enumerate given items returning a tuple (prev,item,next,isLast) 
        /// with prev=null for first element
        /// with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
        /// </summary>                
        /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
        /// <remarks>
        /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0003.cs)
        /// </remarks>
        public static IEnumerable<(T prev, T item, T next, bool isLast)> WithPrevNext<T>(
            this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : class
        {
            foreach (var x in en.WithNext(repeatFirstAtEnd).WithPrevPrimitive())
            {
                var item = x.item.item;
                T prev = x.prev.HasValue ? x.prev.Value.item : null;
                T next = x.item.next;
                var isLast = x.item.isLast;

                yield return (prev, item, next, isLast);
            }
        }

    }

}
