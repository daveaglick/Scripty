using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Scripty.Core
{
    public class ScriptEngine
    {
        public async Task<ScriptResult> Evaluate(ScriptSource source)
        {
            ScriptOptions options = ScriptOptions.Default
                .WithFilePath(source.FilePath);
            using (ScriptGlobals globals = new ScriptGlobals(source.FilePath))
            {
                try
                {
                    await CSharpScript.EvaluateAsync(source.Code, options, globals);
                }
                catch (CompilationErrorException compilationError)
                {
                    return new ScriptResult(globals.Output.OutputFiles, compilationError.Diagnostics.Select(x => x.ToString()).ToList());
                }
                return new ScriptResult(globals.Output.OutputFiles);
            }
        }
    }
}
