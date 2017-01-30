using System.Collections.Generic;
using System.Linq;

List<string> files = new List<string>();

foreach (var file in Context.Project.Analysis.Documents.OrderBy(x => x.FilePath))
{
    var name = System.IO.Path.GetFileName(file.FilePath);
    
    if (name.StartsWith("Class"))
    {
        files.Add(name);
    }
}

Context.Output.Write("//" + String.Join(";", files));
