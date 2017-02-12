namespace Scripty.Core.Tests.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using Core.Output;
    using Core.Resolvers;
    using NUnit.Framework;

    /// <remarks>
    ///     Best tested through the engine
    /// </remarks>
    public class InterceptDirectiveResolverTests : BaseFixture
    {
        public TestHelpers CsTestHelpers { get; set; } = new TestHelpers("Resolvers\\TestCs");
        public TestHelpers CsxTestHelpers { get; set; } = new TestHelpers("Resolvers\\TestCsx");
        

        private string _scriptWithoutAsmOrScriptOrClassRef;
        private string _scriptWithAsm_ButNoScriptOrClassRef;
        private string _scriptWithAsmAndScriptRef_ButNoClassRef;
        private string _scriptWithScriptAndClassRef_ButNoAsmRef;
        private string _scriptWithScriptRef_ButNoAsmOrClassRef;
        private string _scriptWithClassRef_ButNoAsmmOrScriptRef;
        private string _scriptWithAsmAndClassAndScriptRef;
        private string _referencedClassFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            _scriptWithoutAsmOrScriptOrClassRef = CsxTestHelpers.GetTestFilePath("WithoutAsmOrScriptOrClassRef.csx");
            _scriptWithAsm_ButNoScriptOrClassRef = CsxTestHelpers.GetTestFilePath("WithAsm_ButNoScriptOrClassRef.csx");
            _scriptWithAsmAndScriptRef_ButNoClassRef = CsxTestHelpers.GetTestFilePath("WithAsmAndScriptRef_ButNoClassRef.csx");
            _scriptWithScriptAndClassRef_ButNoAsmRef = CsxTestHelpers.GetTestFilePath("WithScriptAndClassRef_ButNoAsmRef.csx");
            _scriptWithScriptRef_ButNoAsmOrClassRef = CsxTestHelpers.GetTestFilePath("WithScriptRef_ButNoAsmOrClassRef.csx");
            _scriptWithClassRef_ButNoAsmmOrScriptRef = CsxTestHelpers.GetTestFilePath("WithClassRef_ButNoAsmOrScriptRef.csx");
            _scriptWithAsmAndClassAndScriptRef = CsxTestHelpers.GetTestFilePath("WithAsmAndClassAndScriptRef.csx");
            _referencedClassFilePath = CsTestHelpers.GetTestFilePath("ReferencedClass.csx");

            CsTestHelpers.RemoveFiles($"*.{CsRewriter.DEFAULT_REWRITE_TEMP_EXTENSION}");
            CsTestHelpers.RemoveFiles($"*.{CsRewriter.DEFAULT_REWRITE_EXTENSION}");
            CsxTestHelpers.RemoveFiles($"*.{OutputFileCollection.DEFAULT_TEMP_EXTENSION}");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            CsTestHelpers.RemoveFiles("*.rewrite.*");
        }

        [Test]
        public void ConstructionParamsTest()
        {
            var searchPaths = new List<string> { CsTestHelpers.ProjectFilePath, CsTestHelpers.GetTestFileSubFolder() };
            var basePath = AppContext.BaseDirectory;

            var idr = new InterceptDirectiveResolver(searchPaths.ToImmutableArray(), basePath);

            Assert.IsNotNull(idr);
        }

        [Test]
        public void ConstructionDefaultTest()
        {
            var idr = new InterceptDirectiveResolver();

            Assert.IsNotNull(idr);
        }

        [Test]
        public void EvaluateScript_WithoutAsmOrScriptOrClassRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithoutAsmOrScriptOrClassRef);
            
            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(1, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
        }
        
        [Test]
        public void EvaluateScript_WithAsm_ButNoScriptOrClassRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithAsm_ButNoScriptOrClassRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(2, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedAssemblyContent(actualResult[1]), actualResult[1]);
        }

        [Test]
        public void EvaluateScript_WithAsmAndScriptRef_ButNoClassRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithAsmAndScriptRef_ButNoClassRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(3, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedAssemblyContent(actualResult[1]), actualResult[1]);
            Assert.True(HasReferencedScriptTestContent(actualResult[2]), actualResult[2]);
        }

        [Test]
        public void EvaluateScript_WithScriptAndClassRef_ButNoAsmRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithScriptAndClassRef_ButNoAsmRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(4, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedScriptTestContent(actualResult[1]), actualResult[1]);
            Assert.True(HasReferencedClassTestContentWhole(actualResult[2]), actualResult[2]);
            Assert.True(HasReferencedClassTestContentStartingValue(actualResult[3]), actualResult[3]);
        }

        [Test]
        public void EvaluateScript_WithScriptRef_ButNoAsmOrClassRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithScriptRef_ButNoAsmOrClassRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(2, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedScriptTestContent(actualResult[1]), actualResult[1]);
            
        }

        [Test]
        public void EvaluateScript_WithClassRef_ButNoAsmOrScriptRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithClassRef_ButNoAsmmOrScriptRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(3, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedClassTestContentWhole(actualResult[1]), actualResult[1]);
            Assert.True(HasReferencedClassTestContentStartingValue(actualResult[2]), actualResult[2]);
        }

        [Test]
        public void EvaluateScript_WithAsmAndClassAndScriptRef()
        {
            var result = CsxTestHelpers.EvaluateScript(_scriptWithAsmAndClassAndScriptRef);

            AssertThereAreNoErrors(result);
            var actualResult = CsxTestHelpers.GetFileLines(result.OutputFiles.Single().TargetFilePath);
            Assert.AreEqual(5, actualResult.Length);
            Assert.True(HasScriptedTestContent(actualResult[0]), actualResult[0]);
            Assert.True(HasReferencedAssemblyContent(actualResult[1]), actualResult[1]);
            Assert.True(HasReferencedScriptTestContent(actualResult[2]), actualResult[2]);
            Assert.True(HasReferencedClassTestContentWhole(actualResult[3]), actualResult[3]);
            Assert.True(HasReferencedClassTestContentStartingValue(actualResult[4]), actualResult[4]);
        }



        //for reference
        public void RewriteReferencedClassFileAsAssembly()
        {
            var result = CsRewriter.CreateRewriteFileAsAssembly(_referencedClassFilePath);

            Assert.IsTrue(result.IsCompiled, "assembly was not compiled");

            Assert.IsTrue(FileUtilities.WriteAssembly(result.AssemblyFilePath, result.AssemblyBytes),
                "Failed to write assembly to disk");

            Assert.IsTrue(FileUtilities.WriteAssembly(result.PdbFilePath, result.PdbBytes),
                "Failed to write assembly pdb to disk");

            var asm = Assembly.LoadFile(result.AssemblyFilePath);
            var asmList = new List<Assembly>(result.FoundAssemblies) { asm };

            //dirty rewrite. Would be nice if had result.OriginalDirectivePath
            var callingScript = CsxTestHelpers.GetFileContent(_scriptWithAsmAndClassAndScriptRef);
            var rewrittenCallingScript = callingScript.Replace("#load \"..\\TestCs\\ReferencedClass.cs\"",
                $"#r \"{result.AssemblyFilePath}\"");
            var rewrittenScriptFileName = $"{_scriptWithAsmAndClassAndScriptRef}.rewrite.csx";
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

        [Test]
        public void ParseDirectives()
        {
            var scriptSource = BuildSimpleValidScriptSource();

            var result = InterceptDirectiveResolver.ParseDirectives(scriptSource.FilePath);

            Assert.IsNotNull(result);
        }

        private static void AssertThereAreNoErrors(ScriptResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Count == 0, string.Join(Environment.NewLine, result.Errors.Select(e => $"{e.Line} {e.Column} {e.Message}")));
        }

        private static bool HasScriptedTestContent(string line)
        {
            return "namespace TestNamespace{class TestClass{public void TestMethod(){}}}".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasReferencedAssemblyContent(string line)
        {
            return "// NUnit.Framework.Internal.PropertyBag".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasReferencedScriptTestContent(string line)
        {
            return "// we have multiplied 6".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasReferencedClassTestContentWhole(string line)
        {
            return "// Emitting prop with backing field 69".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasReferencedClassTestContentStartingValue(string line)
        {
            var exoectedValue = "// using the referenced class to output - Value_";
            return (exoectedValue.Equals(line, StringComparison.OrdinalIgnoreCase) == false
                && line.StartsWith(exoectedValue, StringComparison.OrdinalIgnoreCase));
        }

        private ScriptSource BuildSimpleValidScriptSource()
        {
            return new ScriptSource(_scriptWithAsmAndClassAndScriptRef, CsxTestHelpers.GetFileContent(_scriptWithAsmAndClassAndScriptRef));
        }
    }
}