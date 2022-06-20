using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnitsNet;
using static System.Math;
using static System.FormattableString;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// Returns true if two numbers are equals using a default tolerance of 1e-6 about the smaller one.
        /// </summary>        
        public static bool EqualsAutoTol(this double x, double y, double precision = 1e-6) =>
            Abs(x - y) <= Abs(Min(x, y) * precision);

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
        /// convert given angle(rad) to deg
        /// </summary>
        /// <param name="angleRad">angle (rad)</param>
        /// <returns>angle (deg)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDeg(this double angleRad) => angleRad / PI * 180.0;

        /// <summary>
        /// convert given angle(deg) to rad
        /// </summary>
        /// <param name="angleGrad">angle (deg)</param>
        /// <returns>angle (radians)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToRad(this double angleGrad) => angleGrad / 180.0 * PI;

        /// <summary>
        /// convert given angle(deg) to rad
        /// </summary>
        /// <param name="angleDeg">angle (deg)</param>
        /// <returns>angle (radians)</returns>
        public static float ToRad(this float angleDeg) => angleDeg / 180f * ((float)PI);

        /// <summary>
        /// convert given angle(rad) to deg
        /// </summary>
        /// <param name="angleRad">angle (rad)</param>
        /// <returns>angle (deg)</returns>
        public static float ToDeg(this float angleRad) => angleRad / (float)PI * 180f;

        /// <summary>
        /// Return an invariant string representation rounded to given dec.        
        public static string Stringify(this double x, int dec) => Invariant($"{Round(x, dec)}");

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
        public static double InvDoubleParse(this string str) => double.Parse(str, CultureInfo.InvariantCulture);

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

        /// <summary>
        /// true if ( |x-y| <= tol )
        /// </summary>        
        public static bool EqualsTol(this double x, double tol, double y)
        {
            var d = UnitsNet.Length.FromCentimeters(3);

            return Abs(x - y) <= tol;
        }

        /// <summary>
        /// true if ( |x-y| <= tol )
        /// </summary>        
        public static bool EqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.EqualsTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
        }

        /// <summary>
        /// true if (x > y) AND NOT ( |x-y| <= tol )
        /// </summary>        
        public static bool GreatThanTol(this double x, double tol, double y) => x > y && !x.EqualsTol(tol, y);

        /// <summary>
        /// true if (x > y) AND NOT ( |x-y| <= tol )
        /// </summary>        
        public static bool GreatThanTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.GreatThanTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
        }

        /// <summary>
        /// true if (x > y) AND ( |x-y| <= tol )
        /// </summary>        
        public static bool GreatThanOrEqualsTol(this double x, double tol, double y) => x > y || x.EqualsTol(tol, y);

        /// <summary>
        /// true if (x > y) AND ( |x-y| <= tol )
        /// </summary>     
        public static bool GreatThanOrEqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.GreatThanOrEqualsTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
        }

        /// <summary>
        /// true if (x < y) AND NOT ( |x-y| <= tol )
        /// </summary>     
        public static bool LessThanTol(this double x, double tol, double y) => x < y && !x.EqualsTol(tol, y);

        /// <summary>
        /// true if (x < y) AND NOT ( |x-y| <= tol )
        /// </summary>     
        public static bool LessThanTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.LessThanTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
        }

        /// <summary>
        /// true if (x < y) AND ( |x-y| <= tol )
        /// </summary>     
        public static bool LessThanOrEqualsTol(this double x, double tol, double y) => x < y || x.EqualsTol(tol, y);

        /// <summary>
        /// true if (x < y) AND ( |x-y| <= tol )
        /// </summary>     
        public static bool LessThanOrEqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.LessThanOrEqualsTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
        }

        public static int CompareTol(this double x, double tol, double y)
        {
            if (x.EqualsTol(tol, y)) return 0;
            if (x < y) return -1;
            return 1; // x > y
        }

        public static int CompareTol(this IQuantity x, IQuantity tol, IQuantity y)
        {
            var bu = x.QuantityInfo.BaseUnitInfo.Value;

            return x.ToUnit(bu).Value.CompareTol(tol.ToUnit(bu).Value, y.ToUnit(bu).Value);
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
        public static double Sign(this int n) => (n >= 0) ? 1d : -1d;

        /// <summary>
        /// returns 1.0 if n>=0
        /// -1 otherwise
        /// </summary>        
        public static double Sign(this double n) => (n >= 0) ? 1d : -1d;

        /// <summary>
        /// returns 0,+1,-1 depending on the sign.
        /// (0) : if given number EqualsTol(zeroTol, 0)
        /// (+1) : if given number positive;
        /// (-1) : if given number negative;                
        /// </summary>
        /// <param name="n">number to test</param>
        /// <param name="zeroTol">tolerance to consider it zero</param>
        /// <returns>0,+1,-1</returns>
        public static int Sign(this double n, double zeroTol)
        {
            if (n.EqualsTol(zeroTol, 0)) return 0;
            if (n > 0) return 1;
            return -1;
        }

        /// <summary>
        /// return clamped number between [min,max] interval
        /// </summary>
        /// <param name="n">number</param>
        /// <param name="min">min value admissible</param>
        /// <param name="max">max value admissible</param>
        /// <returns>n if between [min,max] otherwise min when n less than min or max when n great than max</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float n, float min, float max) => n < min ? min : (n > max) ? max : n;

        /// <summary>
        /// return clamped number between [min,max] interval
        /// </summary>
        /// <param name="n">number</param>
        /// <param name="min">min value admissible</param>
        /// <param name="max">max value admissible</param>
        /// <returns>n if between [min,max] otherwise min when n less than min or max when n great than max</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double n, double min, double max) => n < min ? min : (n > max) ? max : n;

    }

}
