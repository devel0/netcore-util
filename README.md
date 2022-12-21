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

- [nuget package](https://www.nuget.org/packages/netcore-util/)

- [extension methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_ext.html)

```csharp
using SearchAThing;
```

- [toolkit methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_toolkit.html)

```csharp
using static SearchAThing.UtilToolkit;
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
dotnet add package Microsoft.CSharp --version 4.7.0
dotnet add package Microsoft.EntityFrameworkCore --version 5.0.1
dotnet add package Newtonsoft.Json --version 12.0.3
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 5.0.0
dotnet add package UnitsNet --version 4.74.0
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
