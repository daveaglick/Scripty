namespace Scripty.Core.Tests.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.Resolvers;
    using NUnit.Framework;

    [SingleThreaded]
    public class CsFileRewriterTests : BaseFixture
    {
        public TestHelpers CsTestHelpers { get; set; } = new TestHelpers("Resolvers\\TestCs");
        public TestHelpers CsxTestHelpers { get; set; } = new TestHelpers("Resolvers\\TestCsx");

        private string _scriptFileToExecute;
        private string _referencedClassFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            _scriptFileToExecute = CsxTestHelpers.GetTestFilePath("ScriptToExecute.csx");
            _referencedClassFilePath = CsTestHelpers.GetTestFilePath("ReferencedClass.cs");
            CsTestHelpers.RemoveFiles("*.rewrite.*");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            CsTestHelpers.RemoveFiles("*.rewrite.*");
        }

        //[Test]
        public void RewriteReferencedClassFileAsAssembly()
        {
            var result = CsRewriter.CreateRewriteFileAsAssembly(_referencedClassFilePath);

            Assert.IsTrue(result.IsCompiled, "assembly was not compiled");

            Assert.IsTrue(FileUtilities.WriteAssembly(result.AssemblyFilePath, result.AssemblyBytes),
                "Failed to write assembly to disk");

            Assert.IsTrue(FileUtilities.WriteAssembly(result.PdbFilePath, result.PdbBytes),
                "Failed to write assembly pdb to disk");

            var asm = Assembly.LoadFile(result.AssemblyFilePath);
            var asmList = new List<Assembly>(result.FoundAssemblies) {asm};

            //dirty rewrite. Would be nice if had result.OriginalDirectivePath
            var callingScript = CsxTestHelpers.GetFileContent(_scriptFileToExecute);
            var rewrittenCallingScript = callingScript.Replace("#load \"..\\TestCs\\ReferencedClass.cs\"",
                $"#r \"{result.AssemblyFilePath}\"");
            var rewrittenScriptFileName = $"{_scriptFileToExecute}.rewrite.csx";
            CsxTestHelpers.WriteFileContent(rewrittenScriptFileName, rewrittenCallingScript);


            var runResult = CsTestHelpers.EvaluateScript(rewrittenScriptFileName, asmList, result.FoundNamespaces);
            Assert.IsNotNull(runResult);
            var expectedResult = new List<string>
            {
                "namespace TestNamespace{class TestClass{public void TestMethod(){}}}",
                "// Emitting prop with backing field 69",
                "// using the referenced class to output - Value_"
            };
            var actualResult = CsxTestHelpers.GetFileLines(runResult.OutputFiles.Single().TargetFilePath);
            StringAssert.AreEqualIgnoringCase(expectedResult[0], actualResult[0]);
            StringAssert.AreEqualIgnoringCase(expectedResult[1], actualResult[1]);
            StringAssert.StartsWith(expectedResult[2], actualResult[2]);
            StringAssert.AreNotEqualIgnoringCase(expectedResult[2], actualResult[2]);
        }
    }
}