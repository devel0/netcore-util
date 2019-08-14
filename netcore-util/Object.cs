using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.FormattableString;
using static System.Math;

namespace SearchAThing
{

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