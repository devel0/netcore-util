using System.Linq;

namespace SearchAThing
{

    public static partial class Util
    {

        /// <summary>
        /// copy properties from other object excluding given list of property names
        /// </summary>        
        public static T CopyFrom<T>(this T obj, T other, params string[] exclude)
        {
            var type = typeof(T);

            foreach (var p in type.GetProperties().Where(x => !exclude.Contains(x.Name)))
                p.SetValue(obj, p.GetValue(other));

            return obj;
        }        

    }

}
