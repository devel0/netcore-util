# netcore-util

[![devel0 MyGet Build Status](https://www.myget.org/BuildSource/Badge/devel0?identifier=bf42235b-95d6-4b7e-8e2c-4ed4a9075c15)](https://www.myget.org/)

.NET core utilities

- [Date](src/Date.cs)
- [Dynamic](src/Dynamic.cs)
- [Number](src/Number.cs)
- [Password](src/Password.cs)
- [String](src/String.cs)

## install and usage

browse [myget istructions](https://www.myget.org/feed/devel0/package/nuget/netcore-util)

## debugging unit tests

- from vscode just run debug test from code lens balloon

## how this project was built

```sh
mkdir -p netcore-util/{src,test}
cd netcore-util
dotnet new sln
cd src
dotnet new classlib
dotnet add package Newtonsoft.Json --version 11.0.2
dotnet add package Microsoft.CSharp --version 4.5.0
cd ../test
dotnet new xunit
dotnet add reference ../src/src.csproj
cd ..
dotnet sln netcore-util.sln add src/src.csproj
dotnet sln netcore-util.sln add test/test.csproj 
dotnet restore
dotnet build
dotnet test test/test.csproj
```
