using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Scripty.Core.Tests.Output
{
    [TestFixture]
    public class EngineFixture
    {
        static readonly string SolutionFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/Sample.sln");
        static readonly string ProjectFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/ProjA/ProjA.csproj");

        [Test]
        public void TestProjectLoading()
        {
            var engine = new ScriptEngine(ProjectFilePath);

            Assert.IsNull(engine.ProjectRoot.Analysis.Solution.FilePath);
            Assert.AreEqual(engine.ProjectRoot.Analysis.FilePath, ProjectFilePath);
        }

        [Test]
        public void TestSolutionLoading()
        {
            var engine = new ScriptEngine(ProjectFilePath, SolutionFilePath);

            Assert.AreEqual(engine.ProjectRoot.Analysis.Solution.FilePath, SolutionFilePath);
            Assert.AreEqual(engine.ProjectRoot.Analysis.FilePath, ProjectFilePath);
        }
    }
}
