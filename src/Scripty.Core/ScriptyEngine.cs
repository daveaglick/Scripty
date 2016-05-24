using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Scripty.Core
{
    public class ScriptyEngine
    {
        public async Task<ScriptResult> Evaluate(ScriptSource source)
        {
            ScriptOptions options = ScriptOptions.Default
                .WithFilePath(source.FilePath);
            ScriptGlobals globals = new ScriptGlobals(source.FilePath);
            try
            {
                await CSharpScript.EvaluateAsync(source.Code, options, globals);
            }
            catch (CompilationErrorException compilationError)
            {
                return new ScriptResult(compilationError.Diagnostics.Select(x => x.ToString()).ToList());
            }
            finally
            {
                globals.Dispose();
            }
            return new ScriptResult();
        }
    }
}
