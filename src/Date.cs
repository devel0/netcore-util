using System;

namespace SearchAThing.Util
{

    public static partial class Util
    {

        /// <summary>
        /// if given dt has unspecified kind rectifies to UTC without any conversion
        /// </summary>        
        public static DateTime UnspecifiedAsUTCDateTime(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            else
                return dt;
        }

    }

}
