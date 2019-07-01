using System;
using System.Linq;
using System.Reflection;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// assign public properties of src to dst object
        /// </summary>        
        public static void Assign<T>(this T src, T dst, Func<PropertyInfo, bool> exclude = null)
        {
            foreach (var item in src.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                if (exclude != null && exclude(item)) continue;
                if (!item.CanRead || !item.CanWrite) continue;
                item.SetValue(dst, item.GetValue(src, null), null);
            }
        }

        /// <summary>
        /// copy properties from other object ; if match functor specified it copies only matched properties
        /// </summary>        
        public static T CopyFrom<T>(this T obj, T other, Func<PropertyInfo, bool> match = null)
        {
            var type = typeof(T);

            foreach (var p in type.GetProperties().Where(x => match == null ? true : match(x)))
                p.SetValue(obj, p.GetValue(other));

            return obj;
        }

        /// <summary>
        /// copy properties from other object excluding those with given names
        /// </summary>        
        public static T CopyFromExclude<T>(this T obj, T other, params string[] exclude_names)
        {
            return obj.CopyFrom(other, (p) => !exclude_names.Any(exclude_name => exclude_name == p.Name));
        }

        /// <summary>
        /// copy properties from other object including only those with given names
        /// </summary>        
        public static T CopyFromInclude<T>(this T obj, T other, params string[] include_names)
        {
            return obj.CopyFrom(other, (p) => include_names.Any(include_name => include_name == p.Name));
        }

    }

}
