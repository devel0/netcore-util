using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using static SearchAThing.UtilToolkit;

namespace SearchAThing
{

    public static partial class UtilToolkit
    {
       
        /// <summary>
        /// Anonymous Comparer.
        /// create an IComparer from given comparison expression.
        /// first dummy argument used to infer the type T.
        /// </summary>
        /// <param name="sample">dummy element to infer the type T</param>
        /// <param name="comparison">(a,b) => { -1, 0, 1 }</param>                
        public static IComparer<T> CreateComparer<T>(T sample, Comparison<T> comparison) => Comparer<T>.Create(comparison);

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static IEnumerable<string> GetMemberNamesExt<T>(Expression<Func<T, object>> membersExpr)
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
                        if (arg is MemberExpression expr)
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

                    if (body.Operand is MemberExpression expr)
                        yield return expr.Member.Name;
                }

                yield break;
            }

            throw new NotImplementedException($"expression type: {membersExpr.Body.Type.ToString()}");
        }

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static HashSet<string> GetMemberNames<T>(T obj, Expression<Func<T, object>> membersExpr) =>
            new HashSet<string>(GetMemberNamesExt(membersExpr).ToArray());

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static HashSet<string> GetMemberNames<T>(Expression<Func<T, object>> membersExpr) =>
            new HashSet<string>(GetMemberNamesExt(membersExpr).ToArray());

        /// <summary>
        /// retrieve member name from a functor like `x=>x.membername1
        /// </summary>
        public static string GetMemberName<T>(Expression<Func<T, object>> membersExpr)
        {
            var en = GetMemberNamesExt(membersExpr).GetEnumerator();
            if (!en.MoveNext()) throw new Exception($"can't find member names");
            var res = en.Current;
            if (en.MoveNext()) throw new ArgumentException($"more than one member in expression specified");
            return res;
        }

        /// <summary>
        /// retrieve member name from a functor like `x=>x.membername1
        /// </summary>
        public static string GetMemberName<T>(T obj, Expression<Func<T, object>> membersExpr)
        {
            var en = GetMemberNamesExt(membersExpr).GetEnumerator();
            if (!en.MoveNext()) throw new Exception($"can't find member names");
            var res = en.Current;
            if (en.MoveNext()) throw new ArgumentException($"more than one member in expression specified");
            return res;
        }

        /// <summary>
        /// retrieve name of var at runtime with GetVarName(() => variable)
        /// </summary>        
        public static string GetVarName<T>(Expression<Func<T>> varNameExpression) =>
            ((MemberExpression)varNameExpression.Body).Member.Name;

    }

    public static partial class UtilExt
    {

        /// <summary>
        /// create getter (func) and setter (action) from given lambda expr
        /// </summary>
        public static (Func<T, V> getter, Action<T, V> setter) CreateGetterSetter<T, V>(this Expression<Func<T, V>> expr)
        {
            MemberExpression mExpr;

            if (expr.Body is UnaryExpression cvtExpr)
                mExpr = (MemberExpression)cvtExpr.Operand;
            else
                mExpr = (MemberExpression)expr.Body;

            Func<T, V> getter = expr.Compile();

            Action<T, V> setter = (t, v) => { };

            var paramT = Expression.Parameter(typeof(T));
            var paramV = Expression.Parameter(typeof(V));

            if (mExpr.Member.MemberType == MemberTypes.Field)
            {
                var field = (FieldInfo)mExpr.Member;
                var fExpr = Expression.Field(paramT, field);
                var rExpr = paramV.Type == field.FieldType ? (Expression)paramV : Expression.Convert(paramV, field.FieldType);
                var aExpr = Expression.Assign(fExpr, rExpr);
                setter = Expression.Lambda<Action<T, V>>(aExpr, paramT, paramV).Compile();
            }
            else if (mExpr.Member.MemberType == MemberTypes.Property)
            {
                var prop = (PropertyInfo)mExpr.Member;
                if (!prop.CanWrite) throw new ArgumentException($"readonly prop given; can't build setter");
                var pExpr = paramV.Type == prop.PropertyType ?
                    (Expression)paramV :
                     Expression.Convert(paramV, prop.PropertyType);
                var cExpr = Expression.Call(paramT, prop.SetMethod!, pExpr);
                setter = Expression.Lambda<Action<T, V>>(cExpr, paramT, paramV).Compile();
            }

            return (getter, setter);
        }

    }

}