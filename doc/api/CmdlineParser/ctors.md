# SearchAThing.CmdlineParser constructors
## CmdlineParser(string, Action<SearchAThing.CmdlineParser>)
construct a command line parser.
            through builder command, flag, argument and nested parser can be added

### Signature
```csharp
public CmdlineParser(string programDescription, Action<SearchAThing.CmdlineParser> builder)
```
### Remarks


<p>&nbsp;</p>
<p>&nbsp;</p>
<hr/>

## CmdlineParser(Action<SearchAThing.CmdlineParser>)
construct a command line parser ( retrieve description from parent parser command that contains this parser ).
            through builder command, flag, argument and nested parser can be added

### Signature
```csharp
public CmdlineParser(Action<SearchAThing.CmdlineParser> builder)
```
### Remarks

