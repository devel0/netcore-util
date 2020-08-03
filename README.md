# netcore-util

[![NuGet Badge](https://buildstats.info/nuget/netcore-util)](https://www.nuget.org/packages/netcore-util/)

.NET core utilities

<hr/>

- [API Documentation](https://devel0.github.io/netcore-util/api/SearchAThing.UtilExt.html)
- [Recent changes](#recent-changes)
- [Quickstart](#quickstart)
- [Unit tests](#unit-tests)
- [How this project was built](#how-this-project-was-built)

<hr/>

## Recent changes

- **1.6.0** : move some Object extension method to UtilToolkit as static methods ; added ToRad, ToDeg, Clamp float/double methods
- **1.4.0** : refactored api namespace to have partial static UtilToolkit class in SearchAThing namespace ; moved object extension methods to UtilToolkit to avoid intellisense on object type

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
