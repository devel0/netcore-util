# SearchAThing.Util.Toolkit.Exec method
## Exec(string, IEnumerable<string>, CancellationToken, bool, bool, bool, bool)
start a process in background redirecting standard output, error;
            a cancellation token can be supplied to cancel underlying process

### Signature
```csharp
public static System.Threading.Tasks.Task<System.ValueTuple<int, string, string>> Exec(string cmd, IEnumerable<string> args, CancellationToken ct, bool sudo = False, bool redirectStdout = True, bool redirectStderr = True, bool verbose = False)
```
### Returns

### Remarks

