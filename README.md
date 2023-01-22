# netcore-util

[![NuGet Badge](https://buildstats.info/nuget/netcore-util)](https://www.nuget.org/packages/netcore-util/)

.NET core utilities

- [API Documentation](https://devel0.github.io/netcore-util/html/annotated.html)
- [Changelog](https://github.com/devel0/netcore-util/commits/master)

<hr/>

<!-- TOC -->
* [Quickstart](#quickstart)
* [Unit tests](#unit-tests)
* [How this project was built](#how-this-project-was-built)
<!-- TOCEND -->

<hr/>

## Quickstart

```sh
dotnet new console --use-program-main -n test
cd test
dotnet add package netcore-util
dotnet run
```

- copy [usings.util.cs](src/ext/usings.util.cs) global usings to the source folder

- [extension methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_ext.html)

```csharp
using SearchAThing.Util;
```

- [toolkit methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_toolkit.html)

```csharp
using static SearchAThing.Util.Toolkit;
```

## Unit tests

```sh
dotnet test
```

- to debug from vscode just run debug test from code lens balloon

## Examples

### exec-bash-redirect

```csharp
namespace SearchAThing.Util.Examples;

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
```

## How this project was built

```sh
mkdir netcore-util
cd netcore-util

dotnet new sln

mkdir -p examples src/util

cd src/util
dotnet new classlib -n netcore-util
# add packages ( https://nuget.org )

cd ..
dotnet new xunit -n test
cd test
dotnet add reference ../util/netcore-util.csproj
cd ..

dotnet sln add src/util src/test examples/example01
dotnet build
dotnet test
```
