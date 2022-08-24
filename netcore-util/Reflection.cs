using System;
using System.Linq;
using System.Reflection;

namespace SearchAThing
{

    public delegate (bool include, bool customValue, object valueIfCustom) CopyFromCustomDelegate(PropertyInfo pi, object val);

    public static partial class UtilToolkit
    {

        /// <summary>
        /// assign public properties of src to dst object
        /// </summary>        
        public static void Assign<T>(T src, T dst, Func<PropertyInfo, bool> exclude = null)
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
        /// <param name="obj">destination</param>
        /// <param name="other">source</param>
        /// <param name="match">if specified allow to copy only matching props</param>        
        public static T CopyFrom<T>(T obj, T other, Func<PropertyInfo, bool>? match = null)
        {
            var type = typeof(T);

            foreach (var p in type.GetProperties().Where(x => match == null ? true : match(x)))
                p.SetValue(obj, p.GetValue(other));

            return obj;
        }

        /// <summary>
        /// copy properties from other object ; a custom non null delegate function can be passed to specify
        /// if include a property and if to assign a custom value ( useful for complex, array types )
        /// </summary>        
        public static T CopyFromCustom<T>(T obj, T other, CopyFromCustomDelegate custom = null)
        {
            var type = typeof(T);

            foreach (var p in type.GetProperties())
            {
                var val = p.GetValue(other);

                if (custom != null)
                {
                    var q = custom(p, val);
                    if (q.include)
                    {
                        p.SetValue(obj, q.customValue ? q.valueIfCustom : val);
                    }
                }
                else
                    p.SetValue(obj, val);
            }

            return obj;
        }

        /// <summary>
        /// copy properties from other object excluding those with given names
        /// </summary>        
        public static T CopyFromExclude<T>(T obj, T other, params string[] exclude_names) =>
            CopyFrom(obj, other, (p) => !exclude_names.Any(exclude_name => exclude_name == p.Name));

        /// <summary>
        /// copy properties from other object including only those with given names
        /// </summary>        
        public static T CopyFromInclude<T>(T obj, T other, params string[] include_names) =>
        CopyFrom(obj, other, (p) => include_names.Any(include_name => include_name == p.Name));

    }

}
