# SearchAThing.UtilExt.Sort method
## Sort<TSource, TKey>(ObservableCollection<TSource>, Func<TSource, TKey>, bool)
sort obc

### Signature
```csharp
public static void Sort<TSource, TKey>(ObservableCollection<TSource> obc, Func<TSource, TKey> keySelector, bool descending = False)
```
### Parameters
- `obc`: observable collection to sort
- `keySelector`: (No Description)
- `descending`: if true then sort descending

### Returns
sorted obc ( same obc reference )
### Remarks

