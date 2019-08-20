﻿using System;
using System.Collections.Generic;
using System.Globalization;
using static System.Math;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// Returns true if two numbers are equals using a default tolerance of 1e-6 about the smaller one.
        /// </summary>        
        public static bool EqualsAutoTol(this double x, double y, double precision = 1e-6)
        {
            return Abs(x - y) <= Abs(Min(x, y) * precision);
        }

        /// <summary>
        /// Round the given value using the multiple basis
        /// </summary>        
        public static double MRound(this double value, double multiple)
        {
            if (Abs(multiple) < double.Epsilon) return value;

            var p = Round(value / multiple);

            return Truncate(p) * multiple;
        }


        /// <summary>
        /// Round the given value using the multiple basis
        /// if null return null
        /// </summary>        
        public static double? MRound(this double? value, double multiple)
        {
            if (value.HasValue)
                return value.Value.MRound(multiple);
            else
                return null;
        }

        /// <summary>
        /// Round the given value using the multiple basis
        /// </summary>        
        public static double MRound(this double value, double? multiple)
        {
            if (multiple.HasValue)
                return value.MRound(multiple.Value);
            else
                return value;
        }

        /// <summary>
        /// convert given angle(rad) to degree
        /// </summary>        
        public static double ToDeg(this double angleRad)
        {
            return angleRad / PI * 180.0;
        }

        /// <summary>
        /// convert given angle(grad) to radians
        /// </summary>        
        public static double ToRad(this double angleGrad)
        {
            return angleGrad / 180.0 * PI;
        }

        /// <summary>
        /// Return an invariant string representation rounded to given dec.        
        public static string Stringify(this double x, int dec)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", Round(x, dec));
        }

        /// <summary>
        /// Magnitude of given number. (eg. 190 -> 1.9e2 -> 2)
        /// (eg. 0.0034 -> 3.4e-3 -> -3)
        /// </summary>        
        public static int Magnitude(this double value)
        {
            var a = Abs(value);

            if (a < double.Epsilon) return 0;

            return (int)Floor(Log10(a));
        }

        /// <summary>
        /// Measure precision distance between two given number.
        /// for example two big numbers 1234567890123.0 and 1234567890023
        /// have difference of 100 but a precision difference of about 1e-10.
        /// This is an instrumentation function that is not to be used outside its scope,
        /// it will help to understand loss of precision between two numbers represented with different storage.
        /// For example this could useful to compare if an import-export tool produce results comparable to other previous versions
        /// because there can be approximations around 1e-12 and 1e-15 due to different format and providers.  
        /// While in general to compare measurements a tolerance have to be used and EqualsTol method, so that
        /// 1234567890123.0d.EqualsTol(1e-10, 1234567890023) is false because diff is 100.
        /// </summary>
        public static double PrecisionDifference(this double a, double b)
        {            
            var ka = a.Magnitude();
            var kb = b.Magnitude();

            var qa = a * Pow(10, -ka);
            var qb = b * Pow(10, -kb);

            return Abs(qa - qb) + (ka == kb ? 0 : Pow(10, Abs(ka - kb)));
        }

        /// <summary>
        /// Invariant culture double parse
        /// </summary>        
        public static double InvDoubleParse(this string str)
        {
            return double.Parse(str, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// parse string that represent number without knowing current culture
        /// so that it can parse "1.2" or "1,2" equivalent to 1.2
        /// it will throw error more than one dot or comma found
        /// </summary>        
        public static double SmartDoubleParse(this string str)
        {
            int dotCnt = 0;
            int commaCnt = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                var c = str[i];
                if (c == '.') ++dotCnt;
                else if (c == ',') ++commaCnt;
            }
            if (dotCnt == 0 && commaCnt == 0) return double.Parse(str, CultureInfo.InvariantCulture);
            if (dotCnt == 1 && commaCnt == 0) return double.Parse(str, CultureInfo.InvariantCulture);
            if (commaCnt == 1 && dotCnt == 0) return double.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
            throw new Exception($"unable to smart parse double from string \"{str}\"");
        }

        /// <summary>
        /// Mean of given numbers
        /// </summary>  
        public static double Mean(this IEnumerable<double> set)
        {
            var v = 0.0;
            int cnt = 0;
            foreach (var x in set) { v += x; ++cnt; }
            return v / cnt;
        }

        /// <summary>
        /// format number so that show given significant digits. (eg. 2.03 with significantDigits=4 create "2.0300")
        /// </summary>  
        public static string ToString(this double d, int significantDigits)
        {
            var decfmt = "#".Repeat(significantDigits);
            return string.Format(CultureInfo.InvariantCulture, "{0:0." + decfmt + "}", d);
        }

        public static bool EqualsTol(this double x, double tol, double y)
        {
            return Abs(x - y) <= tol;
        }

        public static bool GreatThanTol(this double x, double tol, double y)
        {
            return x > y && !x.EqualsTol(tol, y);
        }

        public static bool GreatThanOrEqualsTol(this double x, double tol, double y)
        {
            return x > y || x.EqualsTol(tol, y);
        }

        public static bool LessThanTol(this double x, double tol, double y)
        {
            return x < y && !x.EqualsTol(tol, y);
        }

        public static bool LessThanOrEqualsTol(this double x, double tol, double y)
        {
            return x < y || x.EqualsTol(tol, y);
        }

        public static int CompareTol(this double x, double tol, double y)
        {
            if (x.EqualsTol(tol, y)) return 0;
            if (x < y) return -1;
            return 1; // x > y
        }

        /// <summary>
        /// eval if a number fits in given range
        /// eg.
        /// - "[0, 10)" are numbers from 0 (included) to 10 (excluded)
        /// - "[10, 20]" are numbers from 10 (included) to 20 (included)
        /// - "(30,)" are numbers from 30 (excluded) to +infinity
        /// </summary>        
        public static bool IsInRange(this double nr, double tol, string range)
        {
            var s = range.Trim();
            var fromIncluded = s.StartsWith("[");
            var toIncluded = s.EndsWith("]");
            var ss = s.TrimStart('[', '(').TrimEnd(']').TrimEnd(')').Split(',');
            var from = ss[0].Trim().Length == 0 ? new double?() : double.Parse(ss[0], CultureInfo.InvariantCulture);
            var to = ss[1].Trim().Length == 0 ? new double?() : double.Parse(ss[1], CultureInfo.InvariantCulture);

            if (!from.HasValue && !to.HasValue) return true;

            var contains = true;

            if (from.HasValue)
            {
                if (fromIncluded)
                    contains = contains && nr.GreatThanOrEqualsTol(tol, from.Value);
                else
                    contains = contains && nr.GreatThanTol(tol, from.Value);
            }

            if (to.HasValue)
            {
                if (toIncluded)
                    contains = contains && nr.LessThanOrEqualsTol(tol, to.Value);
                else
                    contains = contains && nr.LessThanTol(tol, to.Value);
            }

            return contains;
        }

        /// <summary>
        /// returns 1.0 if n>=0
        /// -1 otherwise
        /// </summary>        
        public static double Sign(this int n)
        {
            if (n >= 0) return 1.0;
            return -1.0;
        }

        /// <summary>
        /// returns 1.0 if n>=0
        /// -1 otherwise
        /// </summary>        
        public static double Sign(this double n)
        {
            if (n >= 0) return 1.0;
            return -1.0;
        }

    }

}
