using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static IEnumerable<string> GetMemberNames<T>(this Expression<Func<T, object>> membersExpr)
        {
            {
                var q = membersExpr.Body as MemberExpression;
                if (q != null)
                {
                    yield return q.Member.Name;

                    yield break;
                }
            }

            {
                var q = membersExpr.Body as NewExpression;
                if (q != null)
                {
                    var body = (NewExpression)membersExpr.Body;

                    foreach (var arg in body.Arguments)
                    {
                        var expr = arg as MemberExpression;

                        yield return expr.Member.Name;
                    }

                    yield break;
                }
            }

            {
                var q = membersExpr.Body as UnaryExpression;
                if (q != null)
                {
                    var body = (UnaryExpression)membersExpr.Body;

                    var expr = body.Operand as MemberExpression;

                    yield return expr.Member.Name;
                }

                yield break;
            }

            throw new NotImplementedException($"expression type: {membersExpr.Body.Type.ToString()}");
        }

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static HashSet<string> GetMemberNames<T>(this T obj, Expression<Func<T, object>> membersExpr)
        {
            return membersExpr.GetMemberNames().ToHashSet();
        }

        /// <summary>
        /// retrieve member name from a functor like `x=>x.membername1
        /// </summary>
        public static string GetMemberName<T>(this T obj, Expression<Func<T, object>> membersExpr)
        {
            var en = membersExpr.GetMemberNames().GetEnumerator();
            if (!en.MoveNext()) throw new Exception($"can't find member names");
            var res = en.Current;
            if (en.MoveNext()) throw new ArgumentException($"more than one member in expression specified");
            return res;
        }

        /// <summary>
        /// create getter (func) and setter (action) from given lambda expr
        /// </summary>
        public static (Func<T, V> getter, Action<T, V> setter) CreateGetterSetter<T, V>(this Expression<Func<T, V>> expr)
        {            
            MemberExpression mExpr = null;

            var cvtExpr = expr.Body as UnaryExpression;
            if (cvtExpr != null)
                mExpr = cvtExpr.Operand as MemberExpression;
            else
                mExpr = expr.Body as MemberExpression;

            Func<T, V> getter = expr.Compile();

            Action<T, V> setter = null;

            var paramT = Expression.Parameter(typeof(T));
            var paramV = Expression.Parameter(typeof(V));

            if (mExpr.Member.MemberType == MemberTypes.Field)
            {
                var field = mExpr.Member as FieldInfo;
                var fExpr = Expression.Field(paramT, field);
                var rExpr = paramV.Type == field.FieldType ? (Expression)paramV : Expression.Convert(paramV, field.FieldType);
                var aExpr = Expression.Assign(fExpr, rExpr);
                setter = Expression.Lambda<Action<T, V>>(aExpr, paramT, paramV).Compile();
            }
            else if (mExpr.Member.MemberType == MemberTypes.Property)
            {
                var prop = mExpr.Member as PropertyInfo;
                if (!prop.CanWrite) throw new ArgumentException($"readonly prop given; can't build setter");
                var pExpr = paramV.Type == prop.PropertyType ? (Expression)paramV : Expression.Convert(paramV, prop.PropertyType);
                var cExpr = Expression.Call(paramT, prop.SetMethod, pExpr);
                setter = Expression.Lambda<Action<T, V>>(cExpr, paramT, paramV).Compile();
            }

            return (getter, setter);
        }

    }

}