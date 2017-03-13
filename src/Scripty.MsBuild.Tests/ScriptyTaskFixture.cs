using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Scripty.MsBuild.Tests
{
    [TestFixture]
    public class ScriptyTaskFixture
    {
        static readonly string SolutionFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/Sample.sln");
        static readonly string ProjectFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/Proj/Proj.csproj");
        static readonly string ScriptyAssembly = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/Scripty.MsBuild.dll");

        string _msbuild;
        string _output;

        [OneTimeSetUp]
        public void InitFixture()
        {
            _msbuild = FindMsBuild();
            _output = Path.Combine(Path.GetDirectoryName(ProjectFilePath), "test.cs");
        }

        private static string FindMsBuild()
        {
            foreach (var v in new[] { "15.0", "14.0", "12.0", "4.0" })
            {
                string exe = null;

                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey($@"SOFTWARE\Microsoft\MSBuild\ToolsVersions\{v}"))
                {
                    if (key != null)
                    {
                        exe = (string) key.GetValue("MSBuildToolsPath");
                    }
                }

                if (exe != null)
                {
                    return exe + "msbuild.exe";
                }
            }

            throw new ApplicationException("Could not find the location of MSBuild.");
        }

        [SetUp]
        public void InitTest()
        {
            File.Delete(_output);
        }

        [Test]
        public void UsesSolutionAndProperties()
        {
            var args = $"\"{SolutionFilePath}\" /p:ScriptyAssembly=\"{ScriptyAssembly}\";Include1=true;Include3=true";

            var info = new ProcessStartInfo(_msbuild, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process p = new Process();
            p.StartInfo = info;
            p.OutputDataReceived += (s, e) => TestContext.Out.WriteLine(e.Data);
            p.ErrorDataReceived += (s, e) => TestContext.Out.WriteLine(e.Data);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            Assert.AreEqual(0, p.ExitCode);
            Assert.That(File.Exists(_output));
            Assert.AreEqual($@"//Class1.cs;Class3.cs;ClassSolution.cs", File.ReadAllText(_output));
        }

    }
}
