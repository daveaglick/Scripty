# Scripty

Tools to let you use Roslyn-powered C# scripts for code generation. You can think of it as a scripted alternative to T4 templates.

## Scripts

Scripty scripts are just [standard Roslyn C# scripts](https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples) with some special global properties to make them suitable for powering code generation. All the standard C# scripting conventions still apply such as using the `#r` preprocessor directive to load assemblies and the  `#load` directive to load external script files. They are also generally given `.csx` extensions just like normal C# scripts. This makes it easy to bootstrap evaluating them outside Scripty if you need to, providing whichever Scripty globals you use in the script yourself.

The following references are added to every script by default:
* `Microsoft.CodeAnalysis.Workspaces`

The following namespaces are imported to every script by default:
* `System`
* `Microsoft.CodeAnalysis`

### Globals

The following global properties are available when evaluating your script with Scripty:

* `ProjectFilePath`

Contains the full path to the current project file.

* `Project`

A Roslyn `Microsoft.CodeAnalysis.Project` that represents the current project.

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

This will output the following generated code:

```
class Foo
{
    int Bar => 42;
}
```

You can change the default output file by setting `Output.FilePath`. You can also do this partway through the generation to output multiple files.

```
// myscript.csx
Output.WriteLine("// Foo"); // Output to myscript.cs
Output.FilePath = "bar.txt";
Output.WriteLine("BAR!"); // Output to bar.txt 
```

Alternatively, can output more than one file by using the `Output` indexer.

```
Output["other.cs"].WriteLine("// Another file");
```

You can also control whether the generated file is included in the compilation with the `Compile` flag. By default, any output file that ends in `.cs` is compiled and all others are not.

```
Output.Compile = false;
Output["other.cs"].Compile = true;
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

### Scripty.CustomTool (Planned)

This library provides a single file generator (otherwise known as a custom tool) in Visual Studio that can be used to evaluate Scripty scripts whenever the underlying script file changes. Unlike `Scripty.MsBuild`, generated output from the scripts will get automatically included in the project. This library and `Scripty.MsBuild` can be used together to provide script evaluation on changes *and* on build.

### Scripty.Core

This library is the foundation of Scripty and can be used if you want bespoke Scripty evaluation. It lets you run the Scripty engine on C# scripts, supplying information that Scripty needs to set up the special global properties like the current project path.

### Scripty.Console (Planned)

A console application that can evaluate Scripty scripts, supplying information on the command line that Scripty needs to set up the special global properties like the current project path. This lets you evaluate Scripty scripts inside alternate build systems.

### Cake.Scripty (Planned)

A Cake addin for Scripty that allows you to evaluate Scripty scripts in Cake. Note that the recommended approach is to use the `Scripty.MsBuild` library so that you also get Scripty evaluation when building from Visual Studio, in which case this addin isn't needed if you're calling MsBuild from Cake (which most Cake scripts do). However, this addin lets you integrate Scripty evaluation into Cake in situations when you want to completely replace MsBuild.