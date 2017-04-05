

var fileNames = new System.Collections.Generic.Dictionary<int,string>();
for (int i = 1; i <= 15; i++)
{
    var fileName = $"MultiFileScriptControl{i}.cs";
    fileNames.Add(i, fileName);
    if (i % 3 == 0)
    {
        Output[fileName].KeepOutput = false;
    }
}

foreach (var fileName in fileNames)
{
    var content = $"namespace TestNamespace {{ class TestClass {{ public void TestMethod() {{ return \"{fileName.Value}\";}} }} }}";
    Output[fileName.Value].WriteLine(content);
}

