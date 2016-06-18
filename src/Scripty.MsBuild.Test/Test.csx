using Microsoft.CodeAnalysis;

foreach(Document document in ProjectTree.Analysis.Documents)
{
    Output.WriteLine($"// {document.FilePath}");
}