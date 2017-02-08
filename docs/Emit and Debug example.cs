//Found this example in my travels
//https://gist.github.com/toraora/a9d4eb8679383fe659da04d3be5c2d6e
// Two assemblies, src (the one that gets compiled), and repro (the one doing the compiling and debugging)

// This would ostensibly be a CSX and Scripty/Scripty.CustomTool
//  but preferably without emitting disk files (if possible, maybe config setting to govern this)
//  and using Process.Attach or the like.

// Microsoft provides CLR Managed Debugger Sample v4.0 at 
//  https://www.microsoft.com/en-us/download/details.aspx?id=2282&tduid=(f163e70979ee4cb489f5f143efc87e65)(256380)(2459594)(TnL5HPStwNw-WNDdfi6slvBGTiuOox0LaQ)()
//  which shows how to do lots of this stuff. There is also a nuget package "Microsoft.Samples.Debugging.CorApi"

namespace src
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Test
    {
        public static void Hello()
        {
            System.Diagnostics.Debugger.Launch();
            throw new NotImplementedException("meow");
        }
    }
}



namespace repro
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Emit;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var assembly = LoadIntoAppDomain("E:/repro/minrepro/src/hello.cs");
            assembly.GetTypes().First(t => t.FullName == "src.Test").GetMethod("Hello").Invoke(null, null);
        }

        public static Assembly LoadIntoAppDomain(string srcfile, bool debug = true)
        {
            string assemblyName = Path.GetRandomFileName();
            string assemblyDll = Path.Combine(Directory.GetCurrentDirectory(), assemblyName + ".dll");
            string assemblyPdb = Path.Combine(Directory.GetCurrentDirectory(), assemblyName + ".pdb");

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new List<SyntaxTree> { CSharpSyntaxTree.ParseText(File.ReadAllText(srcfile), path: srcfile, encoding: System.Text.Encoding.UTF8) },
                references: AppDomain.CurrentDomain.GetAssemblies().Select(a => MetadataReference.CreateFromFile(a.Location)),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = File.OpenWrite(assemblyDll))
            using (var pdb_ms = File.OpenWrite(assemblyPdb))
            {
                EmitResult result = compilation.Emit(ms, pdb_ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                    throw new Exception();
                }
            }
            return Assembly.Load(File.ReadAllBytes(assemblyDll), File.ReadAllBytes(assemblyPdb));
        }
    }
}
