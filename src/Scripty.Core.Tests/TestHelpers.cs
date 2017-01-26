namespace Scripty.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class TestHelpers
    {
        public const string TEST_FILE_CONTENT = "TESTCONTENT";
        private readonly string _ProjectFilePath;
        private string _TestFileSubfolder;

        public TestHelpers(string testFileSubfolder = "")
        {
            _ProjectFilePath = Path.Combine(GetProjectRootFolder(), "Scripty.Core.Tests.csproj");
            _TestFileSubfolder = testFileSubfolder;
        }

        public ScriptEngine BuildScriptEngine()
        {
            var se = new ScriptEngine(_ProjectFilePath);
            return se;
        }

        public static string GetProjectRootFolder()
        {
            return Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../");
        }

        public string GetTestFileSubFolder()
        {
            return Path.Combine(GetProjectRootFolder(), _TestFileSubfolder);
        }

        public string GetTestFilePath(string fileName)
        {
            return Path.Combine(GetTestFileSubFolder(), fileName);
        }

        public string GetFileContent(string fileName)
        {
            return File.ReadAllText(GetTestFilePath(fileName));
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
                File.Delete(file);
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

    }
}