# SearchAThing.UtilExt.GetMemberNamesExt method
## GetMemberNamesExt<T>(Expression<System.Func<T, object>>)
retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`

### Signature
```csharp
public static System.Collections.Generic.IEnumerable<string> GetMemberNamesExt<T>(Expression<System.Func<T, object>> membersExpr)
```
### Returns

### Remarks

