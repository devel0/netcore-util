﻿using System;

namespace SearchAThing
{

    /// <summary>
    /// useful if need to store quick tuple values into a list or dictionary and allowing further modification;
    /// without this retrieved tuple will a copy-value and tuple in collection remains unmodified.
    /// </summary>
    public class ValueObj<T>
    {
        public T Value { get; set; }
        public ValueObj(T x) { Value = x; }
    }

    public static partial class UtilExt
    {

        /// <summary>
        /// useful to decide action checking for nullity when object retrieval
        /// can consume cpu;
        /// eg. myobj.Eval((x) => (x == null) ? "" : x)
        /// </summary>
        public static U Eval<T, U>(this T obj, Func<T, U> fn)
        {
            return fn(obj);
        }

    }

}