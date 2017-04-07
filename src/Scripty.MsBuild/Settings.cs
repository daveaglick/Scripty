using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.MsBuild
{
    /// <summary>
    /// This gets serialized to JSON and passed to the Scripty console application.
    /// </summary>
    public class Settings
    {
        public string ProjectFilePath = null;
        public string SolutionFilePath = null;
        public IList<string> ScriptFilePaths = null;
        public IDictionary<string, string> Properties = null;
    }
}
