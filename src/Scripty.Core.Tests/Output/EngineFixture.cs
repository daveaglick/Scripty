using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Scripty.Core.Tests.Output
{
    [TestFixture]
    public class EngineFixture
    {
        static readonly string SolutionFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/Sample.sln");
        static readonly string ProjectFilePath = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../SampleSolution/ProjA/ProjA.csproj");
        static readonly string BinDirectory = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/Output");

        const string BuildPropertiesScript = "BuildProperties.csx";
        const string BuildPropertiesScriptOutput = "BuildProperties.cs";
        const string ExpectedExcludeOptionalOutput = "BuildProperties.ExcludeOptional.cs.expected";
        const string ExpectedIncludeOptionalOutput = "BuildProperties.IncludeOptional.cs.expected";

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
            var engine = new ScriptEngine(ProjectFilePath, SolutionFilePath, null);

            Assert.AreEqual(engine.ProjectRoot.Analysis.Solution.FilePath, SolutionFilePath);
            Assert.AreEqual(engine.ProjectRoot.Analysis.FilePath, ProjectFilePath);
        }

        [Test]
        public void TestProjectProperties()
        {
            ScriptEngine engine;
            MSBuildWorkspace workspace;
            Dictionary<string, string> props;
            ScriptResult result;

            props = new Dictionary<string, string>();
            props.Add("IncludeOptional", "false");
            engine = new ScriptEngine(ProjectFilePath, SolutionFilePath, props);

            // Check that the workspace has the properties.
            workspace = (MSBuildWorkspace)engine.ProjectRoot.Analysis.Solution.Workspace;
            Assert.True(workspace.Properties.ContainsKey("IncludeOptional"), "Workspace does not contain custom properties.");
            Assert.AreEqual(workspace.Properties["IncludeOptional"], "false");

            // Check that the properties flow through.
            result = engine.Evaluate(new ScriptSource(GetFilePath(BuildPropertiesScript), GetFileContent(BuildPropertiesScript))).Result;
            Assert.That(File.Exists(GetFilePath(BuildPropertiesScriptOutput)), "Output files does not exist.");
            Assert.AreEqual(GetFileContent(ExpectedExcludeOptionalOutput), GetFileContent(BuildPropertiesScriptOutput), "Output is different when excluding optional files.");

            // And try with the opposite property value.
            props = new Dictionary<string, string>();
            props.Add("IncludeOptional", "true");
            engine = new ScriptEngine(ProjectFilePath, SolutionFilePath, props);

            workspace = (MSBuildWorkspace)engine.ProjectRoot.Analysis.Solution.Workspace;
            Assert.AreEqual(workspace.Properties["IncludeOptional"], "true");

            result = engine.Evaluate(new ScriptSource(GetFilePath(BuildPropertiesScript), GetFileContent(BuildPropertiesScript))).Result;
            Assert.AreEqual(GetFileContent(ExpectedIncludeOptionalOutput), GetFileContent(BuildPropertiesScriptOutput), "Output is different when including optional files.");
        }

        string GetFilePath(string fileName)
        {
            return Path.Combine(BinDirectory, fileName);
        }

        string GetFileContent(string fileName)
        {
            return File.ReadAllText(GetFilePath(fileName));
        }
    }
}
