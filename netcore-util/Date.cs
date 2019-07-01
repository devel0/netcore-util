using System;

namespace SearchAThing
{

    public static partial class UtilExt
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
