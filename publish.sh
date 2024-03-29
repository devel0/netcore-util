#!/bin/bash

exdir=$(dirname `readlink -f "$0"`)

cd "$exdir"/src/util
rm -fr bin obj
dotnet pack -c Release
dotnet nuget push bin/Release/*.nupkg -k $(cat ~/security/nuget-api.key) -s https://api.nuget.org/v3/index.json

cd "$exdir"
