using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Math;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// enumerable extension to enumerate itself into an (item, idx) set
        /// </summary>
        public static IEnumerable<(T item, int idx)> WithIndex<T>(this IEnumerable<T> en)
            => en.Select((item, idx) => (item, idx)); 

    }

}
