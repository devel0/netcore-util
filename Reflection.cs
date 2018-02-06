using System.Linq;

namespace SearchAThing.NETCoreUtil
{

    public static partial class Util
    {

        /// <summary>
        /// copy properties from other object excluding given list of property names
        /// </summary>        
        internal static T CopyFrom<T>(this T obj, T other, bool onlyPrimitives = true, params string[] exclude)
        {
            var type = typeof(T);

            foreach (var p in type.GetProperties().Where(x => (onlyPrimitives ? x.PropertyType.IsPrimitive : true) && !exclude.Contains(x.Name)))
                p.SetValue(obj, p.GetValue(other));

            return obj;
        }

        /// <summary>
        /// copy primitive properties from other object excluding given list of property names
        /// </summary>        
        internal static T CopyFrom<T>(this T obj, T other, params string[] exclude)
        {
            return obj.CopyFrom(other, onlyPrimitives: true, exclude: exclude);
        }

    }

}
