using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}`
        /// </summary>
        public static IEnumerable<string> GetMemberNames<T>(this Expression<Func<T, object>> exclude)
        {            
            var body = (System.Linq.Expressions.NewExpression)exclude.Body;

            foreach (var arg in body.Arguments)
            {
                var am = arg as MemberExpression;
                yield return am.Member.Name;
            }
        }

    }

}