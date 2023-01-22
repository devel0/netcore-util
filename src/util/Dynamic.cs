using System.Dynamic;

namespace SearchAThing.Util;

public static partial class Toolkit
{

#pragma warning disable CS8619
    /// <summary>
    /// create an expando object by copying given src
    /// </summary>        
    public static ExpandoObject? ToExpando(object src)
    {
        IDictionary<string, object> expando = (new ExpandoObject())!;

        var type = src.GetType();

        foreach (var property in type.GetProperties())
        {
            var v = property.GetValue(src);
            if (v != null)
                expando.Add(property.Name, v);
        }

        return (ExpandoObject)expando;
    }
#pragma warning restore CS8619

    /// <summary>
    /// create a dynamic object containing given set of properties
    /// </summary>        
    public static dynamic MakeDynamic(params (string name, object value)[] items)
    {
        var fieldeo = new ExpandoObject() as IDictionary<string, object>;

        foreach (var x in items) fieldeo.Add(x.name, x.value);

        return fieldeo as dynamic;
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// convert given dynamic object into a dictionary string,object for its properties
    /// </summary>        
    public static IDictionary<string, object>? DynamicMakeDictionary(dynamic obj)
    {
        if (obj is Newtonsoft.Json.Linq.JObject)
            return ((Newtonsoft.Json.Linq.JObject)obj).ToObject<IDictionary<string, object>>();
        else
            return (IDictionary<string, object>)obj;
    }

    /// <summary>
    /// return dynamic array from given [[xx],[yy],...] json array
    /// </summary>        
    public static dynamic GetJsonArray(this string jsonDumps) => ((dynamic)Newtonsoft.Json.Linq.JObject.Parse($"{{a:{jsonDumps}}}")).a;
#endif

}