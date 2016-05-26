foreach(Document document in Project.Documents)
{
    Output.WriteLine($"// {document.FilePath}");
}