// Unformatted file

WriteFile(Context, "FakeFormatter.NoFormat.cs");

// Formatted file

Context.Output["FakeFormatter.Formatted.cs"].FormatterEnabled = true;
WriteFile(Context, "FakeFormatter.Formatted.cs");


public void WriteFile(ScriptContext context, string fileName)
{
    context.Output[fileName].WriteLine("namespace TestNamespace");
    context.Output[fileName].WriteLine("{");

    context.Output[fileName].WriteLine("class TestClass");
    context.Output[fileName].WriteLine("{");

    context.Output[fileName].WriteLine("public void TestMethod()");
    context.Output[fileName].WriteLine("{");
    context.Output[fileName].WriteLine("}");

    context.Output[fileName].WriteLine("}");

    context.Output[fileName].WriteLine("}");
}