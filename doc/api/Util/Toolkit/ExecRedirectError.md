# SearchAThing.Util.Toolkit.ExecRedirectError method
## ExecRedirectError(string, IEnumerable<string>, CancellationToken, bool, bool)
start a process in background redirecting standard error;
            a cancellation token can be supplied to cancel underlying process

### Signature
```csharp
public static System.Threading.Tasks.Task<System.ValueTuple<int, string, string>> ExecRedirectError(string cmd, IEnumerable<string> args, CancellationToken ct, bool sudo = False, bool verbose = False)
```
### Returns

### Remarks

