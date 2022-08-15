using System;
using System.Collections.Generic;

namespace SearchAThing
{

    /// <summary>
    /// useful if need to store quick tuple values into a list or dictionary and allowing further modification;
    /// without this retrieved tuple will a copy-value and tuple in collection remains unmodified.
    /// </summary>
    public class ValueObj<T>
    {
        public T Value { get; set; }
        public ValueObj(T x) { Value = x; }
    }

    public static partial class UtilExt
    {

        /// <summary>
        /// Allow to tranform the object into other type.        
        /// eg. `intvar.Fn((x) => (x == 0) ? "zero" : "non-zero")`
        /// </summary>
        public static U Fn<T, U>(this T obj, Func<T, U> fn) => fn(obj);

        /// <summary>
        /// Allow to apply some action on the object inline returning the same object.
        /// 
        /// eg `dxf.Entities.Add(new Line3D().DxfEntity.Act(ent => ent.Color = AciColor.Red))`        
        /// </summary>                
        public static T Act<T>(this T obj, Action<T> setter)
        {            
            setter(obj);
            return obj;
        }

    }

    public static partial class UtilToolkit
    {

        /// <summary>
        /// returns true if only one of given objects is null;
        /// returns false if all objects null or all objects not null;
        /// </summary>
        public static bool XorNull(object a, object b) => a == null ^ b == null;

    }

}