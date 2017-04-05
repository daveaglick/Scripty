## Intercept Directive Resolver
By default, the `SourceFileResolver` (derived from `SourceReferenceResolver`)  is called by `CSharpScript` when Creating, Running, or Evaluating scripts. 
Its job is to locate the targets in the `#r` and `#load` directives and provide those source files. 

An alternate implementaion of `SourceReferenceResolver` can be provided as part of the `ScriptOptions`. 

The `InterceptDirectiveResolver` is an alternate implementation that allows the script author to include a `.cs` class file as a `#load` directive.
For most uses, the script would look something like this...

``` c#
#r ".\..\..\..\packages\NUnit.3.6.1\lib\net45\nunit.framework.dll"
#load "..\TestCs\ReferencedClass.cs"
#load "ReferencedScript.csx"

//Write using supplied ScriptContext
Output.WriteLine("namespace TestNamespace{class TestClass{public void TestMethod(){}}}");

//Create instance from recompiled assembly
var rc1 = new ReferencedClass(Context);
Output.WriteLine($"// Emitting prop with backing field {rc1.PropertyWithBackingField}");
rc1.Owl($"// using the referenced class to output")

```


Mermaid is broken for my markdown editor (node text missing), if this shows up incorrectly for you, scroll to the image below
``` mermaid
graph TD

subgraph C# Engine
  CSEngine["<b>CSharpScript.Evaluate()<b/>"]--> mdr["<b>Metadata Resolution</b> - referenced <i>assemblies</i>are located and loaded."]
  mdr-->sdr["<b>Source Resolution</b> referenced <i>scripts</i> are located and loaded.<br/><br/><b>InterceptDirectiveResolver</b> is called to resolve each script reference."] 
 sdr-->beginint
 sdr-->cscrguts["CSharpScript completes script execution"]
end

subgraph Intercept Handling
beginint("for each script referenced")
beginint-->isCs{"is the reference a <b>.cs</b>"}
isCs-->|YES|CsRew["<b>CsRewriter</b> <i>compiles the <b>.cs</b> as <b>csx</b> (c# scripts) <br/>and collects any metadata needed for <br/>later injection or resolution </i>"]
isCs-->|NO|idr
 CsRew-->idr["<b>InterceptDirectiveResolver</b> returns content to the CSharpScript engine"]
 idr--> sdr
 end 

 cscrguts-->outputmade["Output is made"]

 jrs["Script Submission Created"]-->ScriptyEvaluate["<b>ScriptEngine.Evaluate()</b>"]

 subgraph Scripty ScriptEngine
  ScriptyEvaluate-->newIR["Create new <b>InterceptDirectiveResolver</b>"]
  newIR-->AddOptionsAndMetaData["Create <b>ScriptOptions</b> with metadata and new <b>InterceptDirectiveResolver</b>"]
  AddOptionsAndMetaData-->CSEngine
 end

classDef terminators fill:#a9f,stroke:#333,stroke-width:4px;
classDef orange fill:#f96,stroke:#333,stroke-width:4px;
class jrs terminators;
class next terminators;
class cont terminators;
class outputmade terminators;

```

![picture](images/intercept.png)

