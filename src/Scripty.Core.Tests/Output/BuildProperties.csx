using System.Linq;

foreach (var file in Context.Project.Analysis.Documents.OrderBy(x => x.FilePath))
{
    var name = System.IO.Path.GetFileName(file.FilePath);
    
    if (name.StartsWith("Class"))
    {
        Context.Output.WriteLine(System.IO.Path.GetFileName(file.FilePath));
    }
}
