using System;
using System.Collections;
using System.Collections.Generic;

namespace SearchAThing
{

    public interface IReadOnlyHashSet<T> : IReadOnlyCollection<T>
    {
        public bool Contains(T x);
    }

    /// <summary>
    /// enclose hashset to a readonly collection
    /// </summary>
    public class ReadOnlyHashSet<T> : IReadOnlyHashSet<T>
    {
        HashSet<T> hs;

        public ReadOnlyHashSet(HashSet<T> hs)
        {
            this.hs = hs;
        }

        public int Count => hs.Count;

        public bool Contains(T x) => hs.Contains(x);

        public IEnumerator<T> GetEnumerator() => hs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => hs.GetEnumerator();

    }


}
