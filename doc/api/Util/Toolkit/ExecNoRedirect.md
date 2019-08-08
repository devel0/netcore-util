# SearchAThing.Util.Toolkit.ExecNoRedirect method
## ExecNoRedirect(string, IEnumerable<string>, CancellationToken, bool)
start a process in background redirecting standard output, error;
            a cancellation token can be supplied to cancel underlying process
            (this method will not redirect stdout and stderr)

### Signature
```csharp
public static System.Threading.Tasks.Task<System.ValueTuple<int, string, string>> ExecNoRedirect(string cmd, IEnumerable<string> args, CancellationToken ct, bool sudo = False)
```
### Returns

### Remarks

