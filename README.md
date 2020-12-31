# netcore-util

[![NuGet Badge](https://buildstats.info/nuget/netcore-util)](https://www.nuget.org/packages/netcore-util/)

.NET core utilities

- [API Documentation](https://devel0.github.io/netcore-util/api/SearchAThing.UtilExt.html)
- [Changelog](https://github.com/devel0/netcore-util/commits/master)

<hr/>

- [Quickstart](#quickstart)
- [Unit tests](#unit-tests)
- [How this project was built](#how-this-project-was-built)

<hr/>

## Quickstart

- [nuget package](https://www.nuget.org/packages/netcore-util/)

- [extension methods](https://devel0.github.io/netcore-util/api/SearchAThing.UtilExt.html)

```csharp
using SearchAThing;
```

- [toolkit methods](https://devel0.github.io/netcore-util/api/SearchAThing.UtilToolkit.html)

```csharp
using static SearchAThing.UtilToolkit;
```

## Examples

### with-index-is-last

- [WithIndexIsLast](https://devel0.github.io/netcore-util/api/SearchAThing.UtilExt.html#SearchAThing_UtilExt_WithIndexIsLast__1_IEnumerable___0__)

```csharp
using SearchAThing;
using System.Linq;

namespace with_index_is_last
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new[] { 1, 2, 4 };

            var last = 0d;

            // q2 : sum of all elements except last ( save last into `last` var )
            var q2 = q.WithIndexIsLast().Select(w =>
            {
                if (w.isLast)
                {
                    last = w.item;
                    return 0;
                }
                return w.item;
            }).Sum();

            if (q2 == 3 && last == 4)
                System.Console.WriteLine($"tests succeeded");
            else
                System.Console.WriteLine($"tests failed");
        }
    }
}
```

### exec

- [Exec]()

```csharp
using System.Threading;
using System.Threading.Tasks;
using static SearchAThing.UtilToolkit;
using SearchAThing;
using System.Linq;

namespace exec
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var q = await Exec("ls", new[] { "-la", "/etc/hosts" }, CancellationToken.None,
                    sudo: false,
                    redirectStdout: true,
                    redirectStderr: false,
                    verbose: false);

                if (q.ExitCode == 0)
                {
                    var line = q.Output.Lines().First();
                    System.Console.WriteLine(line);

                    var ss = line.Split(' ');

                    System.Console.WriteLine($"perm: {ss[0]}");
                    System.Console.WriteLine($"owner: {ss[2]}");
                    System.Console.WriteLine($"group: {ss[3]}");
                    System.Console.WriteLine($"size: {ss[4]}");
                }

                // RESULT:
                //
                // -rw-r--r-- 1 root root 218 May 11  2020 /etc/hosts
                // perm: -rw-r--r--
                // owner: root
                // group: root
                // size: 218

            }).Wait();
        }
    }
}
```

### exec-bash-redirect

- [ExecBashRedirect]()

```csharp
using System.Threading;
using System.Threading.Tasks;
using static SearchAThing.UtilToolkit;

namespace exec
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var q = await ExecBashRedirect("i=0; while (($i < 5)); do echo $i; let i=$i+1; done",
                    CancellationToken.None,
                    sudo: false,                    
                    verbose: false);

                if (q.ExitCode == 0)
                {
                    System.Console.WriteLine($"output[{q.Output}]");                    
                }

                // RESULT:
                //
                // output[0
                // 1
                // 2
                // 3
                // 4

                // ]
                
            }).Wait();
        }
    }
}
```

## Unit tests

```sh
dotnet test
```

- to debug from vscode just run debug test from code lens balloon

## How this project was built

```sh
mkdir netcore-util
cd netcore-util

dotnet new sln
dotnet new classlib -n netcore-util

cd netcore-util
dotnet add package Newtonsoft.Json --version 11.0.2
dotnet add package Microsoft.CSharp --version 4.5.0
cd ..

dotnet new xunit -n test
cd test
dotnet add reference ../netcore-util/netcore-util.csproj
cd ..

dotnet sln netcore-util.sln add netcore-util/netcore-util.csproj
dotnet sln netcore-util.sln add test/test.csproj
dotnet restore
dotnet build
dotnet test test/test.csproj
```
