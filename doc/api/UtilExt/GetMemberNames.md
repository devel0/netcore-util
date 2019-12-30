# SearchAThing.UtilExt.GetMemberNames method
## GetMemberNames<T>(Expression<System.Func<T, object>>)
retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`

### Signature
```csharp
public static System.Collections.Generic.IEnumerable<string> GetMemberNames<T>(Expression<System.Func<T, object>> membersExpr)
```
### Returns

### Remarks


<p>&nbsp;</p>
<p>&nbsp;</p>
<hr/>

## GetMemberNames<T>(T, Expression<System.Func<T, object>>)
retrieve list of member names from a functor like `x=>new {x.membername1, x.membername2, ...}` or `x=>x.membername`

### Signature
```csharp
public static System.Collections.Generic.HashSet<string> GetMemberNames<T>(T obj, Expression<System.Func<T, object>> membersExpr)
```
### Returns

### Remarks

