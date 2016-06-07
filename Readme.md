# Scripty

Tools to let you use Roslyn-powered C# scripts for code generation. You can think of it as a scripted alternative to T4 templates.

## Quick Start

The easiest way to get up and running is to install the `Scripty.MsBuild` NuGet package into an existing project. Then just create a script file with a `.csx` extension, add it to your project, and watch the magic happen on your next build. Alternativly, you can install the Scripty Visual Studio extension to add custom tool support for code generation outside the build process.

## Scripts

Scripty scripts are just [standard Roslyn C# scripts](https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples) with some special global properties to make them suitable for powering code generation. All the standard C# scripting conventions still apply such as using the `#r` preprocessor directive to load assemblies and the  `#load` directive to load external script files. They are also generally given `.csx` extensions just like normal C# scripts. This makes it easy to bootstrap evaluating them outside Scripty if you need to, providing whichever Scripty globals you use in the script yourself.

The following references are added to every script by default:
* `Microsoft.CodeAnalysis.Workspaces`

The following namespaces are imported to every script by default:
* `System`
* `Microsoft.CodeAnalysis`

### Globals

The following global properties are available when evaluating your script with Scripty:

* `ScriptFilePath`

The full path to the currently evaluating script.

* `ProjectFilePath`

The full path to the current project file.

* `Project`

A Roslyn `Microsoft.CodeAnalysis.Project` that represents the current project. You can use this to access the files in the project as well as other information, including getting the Roslyn compilation for the project. For example, this script will output comments with the file name of each file in the project:

```
foreach(Document document in Project.Documents)
{
    Output.WriteLine($"// {document.FilePath}");
}
```

* `Output`

A thin wrapper around `TextWriter` that should be used to output generated content. Using this object instead of direct file writing mechanisms ensures that Scripty can keep track of which files were generated and pass that information back to the build process as needed. By default, a file with the same name as the script but with a `.cs` extension is output. A handy pattern is to use script interpolation along with verbatim strings to output large chunks of code at once: 

```
int propertyName = "Bar";
Output.WriteLine($@"
class Foo 
{{ 
    int {propertyName} => 42; 
}}");
```

Given the above script named `script.csx`, the following generated code will be output to `script.cs`:

```
class Foo
{
    int Bar => 42;
}
```

You can output more than one file by using the `Output` indexer.

```
Output["other.cs"].WriteLine("// Another file");
```

* `BuildAction`

You can also control the build action for the generated file using the `BuildAction` property. By default, any output file that ends in `.cs` is compiled and all others are included in the project but not compiled (I.e., a build action of "None"). The `BuildAction` property takes the following enum:

```
public enum BuildAction
{
    /// <summary>
    /// Only generates the file but does not add it to the project.
    /// </summary>
    GenerateOnly,

    /// <summary>
    /// Adds the file to the project and does not set a build action.
    /// </summary>
    None,

    /// <summary>
    /// Adds the file to the project and sets the compile build action.
    /// </summary>
    Compile,

    /// <summary>
    /// Adds the file to the project and sets the content build action.
    /// </summary>
    Content,

    /// <summary>
    /// Adds the file to the project and sets the embedded resource build action.
    /// </summary>
    EmbeddedResource
}
```

For example, to set the build action for the default generated file and an alternate generated file you would write:

```
Output.BuildAction = BuildAction.None;
Output["embedded.xml"].BuildAction = BuildAction.EmbeddedResource;
```

## Libraries

Scripty support is provided via a variety of libraries (and corresponding NuGet packages) for different scenarios.

### Scripty.MsBuild

This installs an MsBuild task into your project that will evaluate script files on each build.

By default, all files in the project with the `.csx` extension are evaluated. You can customize this with the `ScriptFiles` `ItemGroup` (for example, if you have `.csx` files that aren't part of code generation or that you intend to load with `#load` and thus shouldn't be evaluated directly):

```
<ItemGroup>
    <ScriptFiles Include="codegen.csx" />
</ItemGroup>
```

Files that get generated using the MsBuild task are included during compilation (as long as their `Compile` flag is set in the script), but not in the project (unless your script modifies the project to include them). If you'd like to have them in the project as well (for example, to enable Intellisense) just include them manually after the first generation. You may want to also commit an empty placeholder file to any shared repository if you do include generated files in the project so that people obtaining the project won't get a missing file prior to their first build.

### Scripty.CustomTool

This library provides a single file generator (otherwise known as a custom tool) in Visual Studio that can be used to evaluate Scripty scripts whenever the underlying script file changes. Unlike `Scripty.MsBuild`, generated output from the scripts will get automatically included in the project. This library and `Scripty.MsBuild` can be used together to provide script evaluation on changes *and* on build.

The generator is provided as a Visual Studio extension and you can install it from the gallery (just search for "Scripty"). To use it, set the "Custom Tool" property for any `.csx` file to "ScriptyGenerator". After setting the custom tool. Scripty will automatically run whenever the underlying script is saved or when you right-click the script and select "Run Custom Tool."

### Scripty

A console application that can evaluate Scripty scripts. This can be used to integrate Scripty into alternate build systems. It's also used by Scripty.MsBuild to evaluate scripts outside the process space of the currently running build.

```
>Scripty.exe --help
usage:  <ProjectFilePath> <ScriptFilePaths>...

    <ProjectFilePath>       The full path of the project file.
    <ScriptFilePaths>...    The path(s) of script files to evaluate (can
                            be absolute or relative to the project).
```

### Scripty.Core

This library is the foundation of Scripty and can be used if you want to embed Scripty evaluation. It lets you run the Scripty engine directly, supplying any data needed to set up the special global properties like the current project path.

### Cake.Scripty (Planned)

A Cake addin for Scripty that allows you to evaluate Scripty scripts in Cake. Note that the recommended approach is to use the `Scripty.MsBuild` library so that you also get Scripty evaluation when building from Visual Studio, in which case this addin isn't needed if you're calling MsBuild from Cake (which most Cake scripts do). However, this addin lets you integrate Scripty evaluation into Cake in situations when you want to completely replace MsBuild.

## Help!

If you need help, have a feature request, or notice a bug, just submit a GitHub issue.