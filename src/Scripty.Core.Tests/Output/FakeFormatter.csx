// Unformatted file

WriteFile(Context, "FakeFormatter.NoFormat.cs");

// Formatted file

Context.Output["FakeFormatter.Formatted.cs"].FormatterEnabled = true;
WriteFile(Context, "FakeFormatter.Formatted.cs");

// User-formatted file

Context.Output["FakeFormatter.UserFormatted.cs"].FormatterEnabled = true;
Context.Output["FakeFormatter.UserFormatted.cs"].FormatterOptions.NewLinesForBracesInTypes = false;
Context.Output["FakeFormatter.UserFormatted.cs"].FormatterOptions.NewLinesForBracesInMethods = false;
WriteFile(Context, "FakeFormatter.UserFormatted.cs");

public void WriteFile(ScriptContext context, string fileName)
{
    context.Output[fileName].WriteLine("namespace TestNamespace{class TestClass{public void TestMethod(){}}}");
}