Tools to let you use Roslyn-powered C# scripts for code generation. You can think of it as a scripted alternative to T4 templates.

**Note that this document corresponds to the currently active source code branch, which may be in development. To view documentation for the latest *release* of Scripty make sure you are viewing the `master` branch.**

# Quick Start

There are two different ways to use Scripty, and they can be used together or separately depending on your use case.

The first is to use the "custom tool" functionality contained in the [Scripty Visual Studio extension](https://visualstudiogallery.msdn.microsoft.com/52c02bb2-1d79-476e-82fb-5abfbfe6b3e4). Just add that extension to Visual Studio and it's ready to go. Once the extension is installed, you have to "turn it on" for specific files. You do this by manually entering `ScriptyGenerator` as the "Custom Tool" for a file or files in the project (presumably .csx files, though it'll work for any extension). The "Custom Tool" setting can be found in the file properties in Visual Studio. Once that's set, just right-click on the file and select "Run Custom Tool". Note that the scripts *will not* be evaluated on every build automatically using this approach.

The other way to use Scripty is with the MSBuild task. In this case, add the [Scripty.MsBuild NuGet package](https://www.nuget.org/packages/Scripty.MsBuild/) to your project. It will automatically pick up any .csx files in your project at build time, evaluate them, and add the results to your compilation. *You won't actually see the results in your project*. If you want the output of the scripts to show up in the project after using the MSBuild task, you'll need to manually add them after the first build.

I'd recommend looking at the [Scripty Visual Studio extension](https://visualstudiogallery.msdn.microsoft.com/52c02bb2-1d79-476e-82fb-5abfbfe6b3e4) first since it's more obvious when the generation happens and you can immediately see the results. Then once you're comfortable, maybe experiment with the build task. You can also use both together to run code generation on-demand with the custom tool and also on every build with the MsBuild task. Also note that you shouldn't need to install the Roslyn SDK or manually add any of the Roslyn packages or projects to the solution. Everything is self contained in the Scripty extensions and packages.

# Scripts

Scripty scripts are just [standard Roslyn C# scripts](https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples) with some special global properties to make them suitable for powering code generation. All the standard C# scripting conventions still apply such as using the `#r` preprocessor directive to load assemblies and the  `#load` directive to load external script files. They are also generally given `.csx` extensions just like normal C# scripts. This makes it easy to bootstrap evaluating them outside Scripty if you need to, providing whichever Scripty globals you use in the script yourself.

The following references are added to every script by default:
* `Microsoft.CodeAnalysis.Workspaces`
* `Microsoft.Build`
* `Scripty.Core`

The following namespaces are imported to every script by default:
* `System`
* `Scripty.Core`
* `Scripty.Core.Output`
* `Scripty.Core.ProjectTree`

## Script File Extension

By default, Scripty looks for files with a `.csx` extension. This is the same extension used by standard C# scripts and there are a couple reasons why an alternate extension wasn't chosen:

* `.csx` is already recognized by Visual Studio and other tooling as a Roslyn/scriptcs script and so syntax highlighting, Intellisense, etc. will automatically light up. While this could be handled in Visual Studio for a different extension, there are lots of other tools like Visual Studio Code that also have this recognition built-in.
* While all Scripty scripts are not *necessarily* generic C# scripts because of the global properties (see below), those that don't make use of the global properties will also work with a normal C# script runner such as Roslyn or scriptcs out of the box. In addition, all normal C# scripts are also Scripty scripts. This provides some nice interoperability when you want to design a script that is both usable from Scripty and from a C# script runner. Of course, you could specify a different extension for a script when sending it to the Roslyn or scriptcs script runner, but why make it more complicated?

If you don't like this behavior, it's easy to change the extension Scripty uses. For the MSBuild task you can change the extension or only process a subset of files by adding an `ItemGroup` item named `ScriptyFile` and including whatever files you want the task to process. The custom tool is opt-in to begin with since you have to set the custom tool property. You can have the custom tool process any file, `.csx` extension or not, by setting the Custom Tool property.

## Global Properties

The following global properties are available when evaluating your script with Scripty:

* `Context`

  This is a reference to the global `ScriptContext` class that holds each of the properties described below. You can use it to pass into a method that needs access to the full context. To put it another way, this property holds a class that contains all of the global properties in one object. This can be useful because classes and methods in the script are "lifted" and don't have access to the same global properties unscoped code does.

  ```csharp
  WritePaths(Context);

  public void WritePaths(ScriptContext context)
  {
    context.Output.WriteLine($"Script File: {context.ScriptFilePath}");
    context.Output.WriteLine($"Project File: {context.ProjectFilePath}");
  }
  ```

* `ScriptFilePath`

  The full path to the currently evaluating script.

* `ProjectFilePath`

  The full path to the current project file.

* `Project`

  A leaky abstraction over both the MSBuild API and the Roslyn Workspaces API. This object allows you to traverse a hierarchical model of the project, providing access to the MSBuild `ProjectItem` and Roslyn `Document` for any item. This property is a `ProjectRoot` that exposes other `ProjectNode` objects by implementing `IReadOnlyDictionary<string, ProjectNode>` (or you can also access child nodes via the `Children` property).
 
  * `Analysis`

    A Roslyn [`Microsoft.CodeAnalysis.Project`](http://source.roslyn.io/#Microsoft.CodeAnalysis.Workspaces/Workspace/Solution/Project.cs) that represents the current project. You can use this to access the files in the project as well as other information, including getting the compilation for the project. For example, this script will output comments with the path of each source file in the project:

    ```csharp
    using Microsoft.CodeAnalysis;

    foreach(Document document in Project.Analysis.Documents)
    {
        Output.WriteLine($"// {document.FilePath}");
    }
    ```

    The documentation on Roslyn and specifically the Workspace API isn't great right now. The [Roslyn source code browser](http://source.roslyn.io) is a good place to start.

  * `Build`

    A `Microsoft.Build.Evaluation.Project`. This API and the Roslyn Workspaces API exposed in the `Analysis` property have overlapping functionality, but are also complementary. While the Roslyn Workspaces API is more focused on the compilation, the MSBuild API is more focused on the makeup of the project file. Similar to the example for the Roslyn API, this script will output comments with the path of each source file in the project using the MSBuild API:

    ```csharp
    using Microsoft.Build.Evaluation;

    foreach(ProjectItem item in Project.Build.Items.Where(x => x.ItemType == "Compile"))
    {
      Output.WriteLine($"// {item.EvaluatedInclude}");
    }
    ```

  * `Children`

    A `IReadOnlyDictionary<string, ProjectNode>` collection that contains the children of the current node.

  * `Parent`

    The parent `ProjectNode`.

  * `Name`

    The name of this node (folder or file name). This is `string.Empty` for the root node.

  * `ProjectItem`

    The MSBuild `Microsoft.Build.Evaluation.ProjectItem`. This is `null` for the root node.

  * `Document`

    The Roslyn `Microsoft.CodeAnalysis.Document` if there is one. Not all project items have a Roslyn document since Roslyn generally only supports source code files at this time. This is `null` for the root node.

* `Output`

  A thin wrapper around `TextWriter` that should be used to output generated content. Using this object instead of direct file writing mechanisms ensures that Scripty can keep track of which files were generated and pass that information back to the build process as needed. By default, a file with the same name as the script but with a `.cs` extension is output. A handy pattern is to use script interpolation along with verbatim strings to output large chunks of code at once: 

  ```csharp
  string propertyName = "Bar";
  Output.WriteLine($@"
  class Foo 
  {{ 
    int {propertyName} => 42; 
  }}");
  ```

  Given the above script named `script.csx`, the following generated code will be output to `script.cs`:

  ```csharp
  class Foo
  {
    int Bar => 42;
  }
  ```

  You can output more than one file by using the `Output` indexer.

  ```csharp
  Output["other.cs"].WriteLine("// Another file");
  ```

  * `FilePath`

    Gets the currently set file path for the output file.

  * `SetFilePath(string filePath)`

    Changes the file path for the main output file.

  * `SetExtension(string extension)`

    Sets the file path for the main output file to the same path as the script with the specified extension. Note that if an alternate file path is specified using `SetFilePath()`, this method will ignore that and will base the new file path on the script file path.

  * `BuildAction`

    You can also control the build action for the generated file using the `BuildAction` property. By default, any output file that ends in `.cs` is compiled and all others are included in the project but not compiled (I.e., a build action of "None"). The `BuildAction` property takes the following enum:

    ```csharp
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

    ```csharp
    Output.BuildAction = BuildAction.None;
    Output["embedded.xml"].BuildAction = BuildAction.EmbeddedResource;
    ```

# Libraries

Scripty support is provided via a variety of libraries (and corresponding NuGet packages) for different scenarios.

## Scripty.MsBuild

This installs an MsBuild task into your project that will evaluate script files on each build.

By default, all files in the project with the `.csx` extension are evaluated. You can customize this with the `ScriptyFiles` `ItemGroup` (for example, if you have `.csx` files that aren't part of code generation or that you intend to load with `#load` and thus shouldn't be evaluated directly):

```xml
<ItemGroup>
    <ScriptyFiles Include="codegen.csx" />
</ItemGroup>
```

Files that get generated using the MsBuild task are included during compilation (as long as their `Compile` flag is set in the script), but not in the project (unless your script modifies the project to include them). If you'd like to have them in the project as well (for example, to enable Intellisense) just include them manually after the first generation. You may want to also commit an empty placeholder file to any shared repository if you do include generated files in the project so that people obtaining the project won't get a missing file prior to their first build.

By default, the MSBuild task will cause the build to fail if there are any problems evaluating scripts. You can override this behavior and continue with the build on errors by placing the following in your project file:

```xml
<PropertyGroup>
  <ScriptyContinueOnError>true</ScriptyContinueOnError>
</PropertyGroup>
```

## Scripty.CustomTool

This library provides a single file generator (otherwise known as a custom tool) in Visual Studio that can be used to evaluate Scripty scripts whenever the underlying script file changes. Unlike `Scripty.MsBuild`, generated output from the scripts will get automatically included in the project. This library and `Scripty.MsBuild` can be used together to provide script evaluation on changes *and* on build.

The generator is provided as a Visual Studio extension and you can install it from the gallery (just search for "Scripty"). To use it, set the "Custom Tool" property for any `.csx` file to "ScriptyGenerator". After setting the custom tool. Scripty will automatically run whenever the underlying script is saved or when you right-click the script and select "Run Custom Tool."

## Scripty

A console application that can evaluate Scripty scripts. This can be used to integrate Scripty into alternate build systems. It's also used by Scripty.MsBuild to evaluate scripts outside the process space of the currently running build.

```
>Scripty.exe --help
usage:  <ProjectFilePath> <ScriptFilePaths>...

    <ProjectFilePath>       The full path of the project file.
    <ScriptFilePaths>...    The path(s) of script files to evaluate (can
                            be absolute or relative to the project).
```

## Scripty.Core

This library is the foundation of Scripty and can be used if you want to embed Scripty evaluation. It lets you run the Scripty engine directly, supplying any data needed to set up the special global properties like the current project path.

## Cake.Scripty

A Cake addin for Scripty that allows you to evaluate Scripty scripts in Cake. The recommended approach is to use the `Scripty.MsBuild` library so that you also get Scripty evaluation when building from Visual Studio, If you are calling MsBuild from Cake (which most Cake scripts do) this addin is not needed. However, this addin lets you integrate Scripty evaluation into Cake in situations when you want to completely replace MsBuild.

When using the addin it is important to include both the addin and the Scripty tool. To use the addin call Scripty constructor and then chain the Evaluate method off it. 

The constructor takes an string for the project file and an optional ScriptySettings which inherits from ToolSettings with no extra settings added. The Evaluate method takes a param list of script files to process. A simple Cake file would look like:

```csharp
#addin nuget:?package=Cake.Scripty
#tool nuget:?package=Scripty

var target = Argument("target", "Default");

Task("Default")
.Does(() => {
    Scripty("Project.csproj", new ScriptySettings()
        {
            WorkingDirectory = "./OptionalWorkingDirectory"
        })
        .Evaluate("Script1.csx", "Script2.csx", "Script3.csx");
});

RunTarget(target);
```

# Help!

If you need help, have a feature request, or notice a bug, just submit a GitHub issue.
