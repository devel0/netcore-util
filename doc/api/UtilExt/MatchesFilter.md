# SearchAThing.UtilExt.MatchesFilter method
## MatchesFilter(IEnumerable<string>, string, bool)
Checks whatever fields matches given filter all words in any of inputs.
            ex. fields={ "abc", "de" } filter="a" results: true
            ex. fields={ "abc", "de" } filter="a d" results: true
            ex. fields={ "abc", "de" } filter="a f" results: false
            autoskips null fields check;
            returns true if filter empty

### Signature
```csharp
public static bool MatchesFilter(IEnumerable<string> fields, string filter, bool ignoreCase = True)
```
### Returns

### Remarks

