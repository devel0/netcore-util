using Newtonsoft.Json;

namespace SearchAThing.Util;

public static partial class Toolkit
{
    public static string ToJson(object o) => JsonConvert.SerializeObject(o);
}