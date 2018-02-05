using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace SearchAThing.NETCoreUtil
{

    public static partial class Util
    {

        /// <summary>
        /// create a dynamic object containing given set of properties
        /// </summary>        
        public static dynamic MakeDynamic(params (string name, object value)[] items)
        {
            var fieldeo = new ExpandoObject() as IDictionary<string, object>;

            foreach (var x in items) fieldeo.Add(x.name, x.value);

            return fieldeo as dynamic;
        }

        /// <summary>
        /// convert given dynamic object into a dictionary string,object for its properties
        /// </summary>        
        public static IDictionary<string, object> DynamicMakeDictionary(dynamic obj)
        {
            if (obj is JObject)
                return ((JObject)obj).ToObject<IDictionary<string, object>>();
            else
                return (IDictionary<string, object>)obj;
        }

    }

}
