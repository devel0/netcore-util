using System.Collections.Generic;
using System.Globalization;
using static System.Math;

namespace SearchAThing.Util
{

    public static partial class Util
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
        /// Invariant culture double parse
        /// </summary>        
        public static double InvDoubleParse(this string str)
        {
            return double.Parse(str, CultureInfo.InvariantCulture);
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
