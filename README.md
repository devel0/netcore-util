# netcore-util

[![NuGet Badge](https://buildstats.info/nuget/netcore-util)](https://www.nuget.org/packages/netcore-util/)

.NET core utilities

<hr/>

- [API Documentation](#api-documentation)
- [install](#install)
- [debugging unit tests](#debugging-unit-tests)
- [how this project was built](#how-this-project-was-built)

<hr/>

## API Documentation

- [Extensions](doc/api/UtilExt.md)
- [Path](doc/api/Util/Path.md)
- [Toolkit](doc/api/Util/Toolkit.md)
    - [RandomPasswordOptions](doc/api/Util/RandomPasswordOptions.md)
- [String Wrapper](doc/api/StringWrapper.md)
- [Error info](doc/api/ErrorInfo.md)
- Command line parser multi ( [**quickstart**](cmdline-parser.md) )
    - [CmdlineParser](doc/api/CmdlineParser.md)
    - [CmdlineParserItem](doc/api/CmdlineParseItem.md)
    - [CmdlineParserItemType](doc/api/CmdlineParseItemType.md)

## install

- [nuget package](https://www.nuget.org/packages/netcore-util/)

## debugging unit tests

- from vscode just run debug test from code lens balloon

## how this project was built

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
