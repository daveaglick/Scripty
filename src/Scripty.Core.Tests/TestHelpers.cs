namespace Scripty.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class TestHelpers
    {
        public const string TEST_FILE_CONTENT = "TESTCONTENT";
        public string ProjectFilePath { get; }
        private readonly string _testFileSubfolder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestHelpers"/> class.
        /// </summary>
        /// <param name="testFileSubfolder">The test file subfolder relative to the project root.</param>
        public TestHelpers(string testFileSubfolder = "")
        {
            ProjectFilePath = Path.Combine(GetProjectRootFolder(), "Scripty.Core.Tests.csproj");
            _testFileSubfolder = testFileSubfolder;
        }

        public ScriptEngine BuildScriptEngine()
        {
            var se = new ScriptEngine(ProjectFilePath);
            return se;
        }

        public ScriptResult EvaluateScript(string scriptFilePath, List<Assembly> additionalAssemblies = null,
            List<string> additionalNamespaces = null, ScriptEngine engine = null)
        {
            var eng = engine;
            if (eng == null)
            {
                eng = BuildScriptEngine();
            }
            var ss = new ScriptSource(scriptFilePath, GetFileContent(scriptFilePath));
            var result = eng.Evaluate(ss).Result; // additionalAssemblies, additionalNamespaces).Result;
            return result;
        }


        public static string GetProjectRootFolder()
        {
            return Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../");
        }

        public string GetTestFileSubFolder()
        {
            return Path.Combine(GetProjectRootFolder(), _testFileSubfolder);
        }

        public string GetTestFilePath(string fileName)
        {
            return Path.Combine(GetTestFileSubFolder(), fileName);
        }

        public string GetFileContent(string fileName)
        {
            return File.ReadAllText(GetTestFilePath(fileName));
        }

        public void WriteFileContent(string fileName, string fileContent)
        {
            File.WriteAllText(fileName, fileContent);
        }

        public void RemoveFiles(List<string> filesToRemoveIfPresent)
        {
            if (filesToRemoveIfPresent != null)
            {
                foreach (var file in filesToRemoveIfPresent)
                {
                    var filePath = GetTestFilePath(file);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }


        public void RemoveFiles(string filePattern)
        {
            foreach (var file in Directory.GetFiles(GetTestFileSubFolder(), filePattern))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed to delete file '{file}', err: {e}");
                }
            }
        }

        public void CreateFiles(List<string> filesToCreateIfNotPresent)
        {
            foreach (var file in filesToCreateIfNotPresent)
            {
                var filePath = GetTestFilePath(file);

                if (File.Exists(filePath) == false)
                {
                    File.WriteAllText(filePath, TEST_FILE_CONTENT);
                }
            }
        }

        public string[] GetFileLines(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to get file lines. {e}");
            }
            return null;
        }
    }
}