# netcore-util ( command line parser )

[![NuGet Badge](https://buildstats.info/nuget/netcore-util)](https://www.nuget.org/packages/netcore-util/)

.NET core utilities

<hr/>

- [install](README.md#install)
- [features](#features)
- [examples](#examples)

<hr/>

## Features

- multi level command ( eg. nested ) with inherited flags and usage
- command, flags, parameters

## Examples

### [cmdline-parser-01](examples/cmdline-parser-01/Program.cs)

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-01/bin/Debug/netcoreapp3.0/cmdline-parser-01.dll
```

Result:

```

Usage: cmdline-parser-01 FLAGS file1 file2

sample program

Optional flags:

  -i        print filename stats

Parameters:

  [file1]   1th pathfilename to test
  [file2]   2th pathfilename to test

```

<hr/>

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-01/bin/Debug/netcoreapp3.0/cmdline-parser-01.dll -i /etc/hosts /etc/resolv.conf
```

Result:

```
flag_i:True
file1:/etc/hosts
file2:/etc/resolv.conf
Filesize [/etc/hosts] = 252
Filesize [/etc/resolv.conf] = 39
```

### [cmdline-parser-02](examples/cmdline-parser-02/Program.cs)

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-02/bin/Debug/netcoreapp3.0/cmdline-parser-02.dll -i /etc/hosts /etc/resolv.conf
```

Result:

```
Filesize [/etc/hosts] = 252
Filesize [/etc/resolv.conf] = 39
```

<hr/>

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-02/bin/Debug/netcoreapp3.0/cmdline-parser-02.dll -i /etc/hosts /etc/resolv.conf -h
```

> *TODO* here builtin help option -h, --help should not work after ending parameters ( this wrong behavior actually appears also using dotnet for example `dotnet run --project examples/cmdline-parser-02 -i -h /etc/hosts` )

Result:

```

Usage: cmdline-parser-02 FLAGS files...

sample program

Optional flags:

  -i         print file size

Parameters:

  files...   file to test

```

### [cmdline-parser-03](examples/cmdline-parser-03/Program.cs)

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-03/bin/Debug/netcoreapp3.0/cmdline-parser-03.dll
```

Result:

```
ERROR: missing mandatory flag [mfsv]
ERROR: missing mandatory parameter [files]

Usage: cmdline-parser-03 FLAGS files...

sample program

Optional flags:

  -fs                                 flag short
  -fsv=VAL                            flag short with value
  --fl                                flag long
  -fsl,--flag-short-long              flag short long
  -fslv,--flag-short-long-val=VAL     flag short long

Mandatory flags:

  -mfsv=VAL                           mandatory flag short with value
  -mfslv,--mflag-short-long-val=VAL   flag short long

Parameters:

  files...                            file to test

```

<hr/>

Execution:

```sh
otnet build && dotnet examples/cmdline-parser-03/bin/Debug/netcoreapp3.0/cmdline-parser-03.dll -fs -fsl=10 --flag-short-long -fslv 33 -mfsv=90 -mfslv=12 a b c
```

Result:

```
TYPE             SHORT-NAME   LONG-NAME              DESCRIPTION                       MANDATORY   MATCHES   VALUE            
------------------------------------------------------------------------------------------------------------------------------
flag             fs                                  flag short                        False       True      -fs              
flag             fsv                                 flag short with value             False       False                      
flag             mfsv                                mandatory flag short with value   True        True      90               
flag                          fl                     flag long                         False       False                      
flag             fsl          flag-short-long        flag short long                   False       True      --flag-short-long
flag             fslv         flag-short-long-val    flag short long                   False       True      33               
flag             mfslv        mflag-short-long-val   flag short long                   True        True      12               
parameterArray   files                               file to test                      True        True      [ "a","b","c" ]  
```

### [cmdline-parser-04](examples/cmdline-parser-04/Program.cs)

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-04/bin/Debug/netcoreapp3.0/cmdline-parser-04.dll
```

Result:

```

Usage: cmdline-parser-04 COMMAND FLAGS

sample program

Commands:

  cmd1   command 1 with no sub commands
  cmd2   command 2 with sub commands

Optional flags:

  -b     base option

```

<hr/>

Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-04/bin/Debug/netcoreapp3.0/cmdline-parser-04.dll cmd2 -b
```

Result:

```
ERROR: must specify a command

Usage: cmdline-parser-04 cmd2 COMMAND FLAGS

invoke cmd2 sub commands

Commands:

  cmd-a   command a
  cmd-b   command b

Optional flags:

  -o2     option on cmd2

(inherited) Optional flags:

  -b      base option

```

<hr/>


Execution:

```sh
dotnet build && dotnet examples/cmdline-parser-04/bin/Debug/netcoreapp3.0/cmdline-parser-04.dll cmd2 cmd-b -b -o2
```

Result:

```
TYPE      SHORT-NAME   LONG-NAME   DESCRIPTION                      MANDATORY   MATCHES   VALUE
-----------------------------------------------------------------------------------------------
flag      o2                       option on cmd2                   False       True      -o2  
command   cmd-a                    command a                        False       False          
command   cmd-b                    command b                        False       True           
flag      b                        base option                      False       True      -b   
command   cmd1                     command 1 with no sub commands   False       False          
command   cmd2                     command 2 with sub commands      False       True 
```
