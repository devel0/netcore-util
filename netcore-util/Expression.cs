using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace SearchAThing
{

    public static partial class UtilExt
    {

         /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`
        /// </summary>
        public static IEnumerable<string> GetMemberNames<T>(this Expression<Func<T, object>> membersExpr)
        {
            if (membersExpr.Body is NewExpression)
            {
                var body = (NewExpression)membersExpr.Body;

                foreach (var arg in body.Arguments)
                {
                    var expr = arg as MemberExpression;

                    yield return expr.Member.Name;
                }
            }
            else if (membersExpr.Body is UnaryExpression)
            {
                var body = (UnaryExpression)membersExpr.Body;

                var expr = body.Operand as MemberExpression;

                yield return expr.Member.Name;
            }
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
            if (en.MoveNext()) throw new Exception($"more than one member in expression specified");
            return res;
        }

    }

}