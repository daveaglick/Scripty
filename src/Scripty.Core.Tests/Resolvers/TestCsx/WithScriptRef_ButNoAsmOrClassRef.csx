#load "ReferencedScript.csx"

//Write using supplied ScriptContext
Output.WriteLine("namespace TestNamespace{class TestClass{public void TestMethod(){}}}");


//With referenced script
Output.WriteLine($"// we have multiplied {Go4th(2, 3)}");
