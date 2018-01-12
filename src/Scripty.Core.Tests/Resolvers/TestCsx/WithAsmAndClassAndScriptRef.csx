#r ".\..\..\..\packages\NUnit.3.9.0\lib\net45\nunit.framework.dll"
#load "..\TestCs\ReferencedClass.cs"
#load "ReferencedScript.csx"

//Write using supplied ScriptContext
Output.WriteLine("namespace TestNamespace{class TestClass{public void TestMethod(){}}}");

//with the referenced assembly
var bag = new NUnit.Framework.Internal.PropertyBag();
Output.WriteLine($"// {bag}");

//With referenced script
Output.WriteLine($"// we have multiplied {Go4th(2, 3)}");

//Create instance from intercepted class
var rc1 = new ReferencedClass(Context);
Output.WriteLine($"// Emitting prop with backing field {rc1.PropertyWithBackingField}");
rc1.Owl($"// using the referenced class to output")
