# SearchAThing.Util.Toolkit.ExecRedirect method
## ExecRedirect(string, IEnumerable<string>, CancellationToken, bool, bool)
start a process in background redirecting standard output, error;
            a cancellation token can be supplied to cancel underlying process
            (this method will not redirect stdout and stderr)

### Signature
```csharp
public static System.Threading.Tasks.Task<System.ValueTuple<int, string, string>> ExecRedirect(string cmd, IEnumerable<string> args, CancellationToken ct, bool sudo = False, bool verbose = False)
```
### Returns

### Remarks

