using Microsoft.CodeAnalysis;

foreach(Document document in Project.Analysis.Documents)
{
    Output.WriteLine($"// {document.FilePath}");
}