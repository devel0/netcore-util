# Embedded resources

## add resources as embedded

``` xml
<Project Sdk="Microsoft.NET.Sdk">    
    <ItemGroup>
        <EmbeddedResource Include="lab/vertexShader.glsl" />        
        <EmbeddedResource Include="lab/fragmentShader.glsl" />
    </ItemGroup>
</Project>
```

## list embedded resources

[sample](/api/SearchAThing.UtilExt.html#SearchAThing_UtilExt_CopyFromExclude__1___0___0_System_String___)

``` csharp
var q = SearchAThing.Util.Toolkit.GetEmbeddedResourceNames();
```

results is an array of strings `assemblyname.folder.subfld.filename.ext` like the follow

``` csharp
"MyApp.shaders.vertexShader.glsl"
```

## get embedded resource content

note: the template type can be any existing in the target assembly from which extract the resource

``` csharp
var fileContent = "MyApp.shaders.vertexShader.glsl".GetEmbeddedFileContent<MyApp>()
```
