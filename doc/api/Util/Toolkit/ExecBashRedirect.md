# SearchAThing.Util.Toolkit.ExecBashRedirect method
## ExecBashRedirect(string, CancellationToken, bool, bool)
start a bash process in background redirecting standard output, error;
            given script can contains pipe and other shell related redirections.
            a cancellation token can be supplied to cancel underlying process
            (this method will not redirect stdout and stderr)

### Signature
```csharp
public static System.Threading.Tasks.Task<System.ValueTuple<int, string, string>> ExecBashRedirect(string script, CancellationToken ct, bool sudo = False, bool verbose = False)
```
### Returns

### Remarks

