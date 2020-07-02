using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
namespace SearchAThing
{

    public static partial class UtilToolkit
    {

        /// <summary>
        /// create an expando object by copying given src
        /// </summary>        
        public static ExpandoObject ToExpando(object src)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            var type = src.GetType();

            foreach (var property in type.GetProperties()) expando.Add(property.Name, property.GetValue(src));

            return expando as ExpandoObject;
        }

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
