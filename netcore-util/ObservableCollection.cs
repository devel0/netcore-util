using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SearchAThing.Util
{

    public static partial class Util
    {

        /// <summary>
        /// sort obc
        /// </summary>
        /// <param name="obc">observable collection to sort</param>
        /// <param name="descending">if true then sort descending</param>
        /// <returns>sorted obc ( same obc reference )</returns>
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> obc,
            Func<TSource, TKey> keySelector, bool descending = false)
        {
            List<TSource> lst = null;
            if (descending)
                lst = obc.OrderByDescending(keySelector).ToList();
            else
                lst = obc.OrderBy(keySelector).ToList();

            obc.Clear();
            foreach (var x in lst)
            {
                obc.Add(x);
            }
        }

    }

}