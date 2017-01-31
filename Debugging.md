Because you are curious, here is 

## How to debug or modify Scripty

There are at least two general paths take depending on how you need to be scripty. 

#### _If you are using the Visual Studio extension_

Set the start project to `Scripty.CustomTool` and open the project 
properties. On the **Debug** tab, the **Start External Program** option 
should be selected and the path pointed to your `devenv.exe` (you will have to change this
if you are not using the default installation path path)

The default installation path is for VS 2015 is
> C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe

The command line arguments should be set to `/rootsuffix Exp /log` 

At that point you can start the solution. 



#### _If you are using the console app_

Set the start project to `Scripty` and open the project properties. On the **Debug** tab, 
in the **Start Options -> Command Line Arguments** box, enter values to match your needs. 

As an example, this will load the ProjectFile.csproj and execute the Script.csx.
> "c:\ProjectFile.csproj" "c:\Script.csx" 

To debug that interaction, Start the solution.



##### Interesting breakpoints

`Scripty.CustomTool`  `ScriptyGenerator.cs` `GenerateCode(string inputFileContent)` is
where the the script input is executed and the output generated. Look for these lines to set a 
breakpoint. 

``` 
  ScriptEngine engine = new ScriptEngine(project.FullName);
  ScriptResult result = engine.Evaluate(source).Result;
```


`Scripty.exe` `Program.cs` `Run(string[] args)` is the equivalent location for the console
app.

```
Console.WriteLine($"Adding task to evaluate {x}");
tasks.Add(engine.Evaluate(new ScriptSource(x, File.ReadAllText(x))));
```