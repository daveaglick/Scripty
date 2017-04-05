namespace Scripty.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    /// <remarks>
    ///     Single threaded so the file existence evaluations are clear. 
    /// </remarks>
    [SingleThreaded]
    public class ScriptEngineTests : BaseFixture
    {
        #region "common test members"

        public TestHelpers TestHelpers { get; set; } = new TestHelpers("TestCsx");

        private string _simpleSuccessScriptFilePath;
        private string _simpleSuccessOutputFilePath;
        private string _simpleScriptKeepScriptFilePath;
        private string _simpleScriptKeepOutputFilePath;
        private string _simpleScriptIgnoreScriptFilePath;
        private string _simpleScriptIgnoreOutputFilePath;
        private string _simpleCompileFailureScriptFilePath;
        private string _simpleCompileFailureOutputFilePath;
        private string _multipleScriptVaryingScriptFilePath;
        private string _multipleScriptVaryingTempFilePattern;
        private string _multipleScriptVaryingOutputFilePattern;

        [OneTimeSetUp]
        public void Setup()
        {
            _simpleSuccessScriptFilePath = TestHelpers.GetTestFilePath("SimpleSuccess.csx");
            _simpleSuccessOutputFilePath = TestHelpers.GetTestFilePath("SimpleSuccess.cs");
            _simpleScriptKeepScriptFilePath = TestHelpers.GetTestFilePath("SimpleScriptKeep.csx");
            _simpleScriptKeepOutputFilePath = TestHelpers.GetTestFilePath("SimpleScriptKeep.cs");
            _simpleScriptIgnoreScriptFilePath = TestHelpers.GetTestFilePath("SimpleScriptIgnore.csx");
            _simpleScriptIgnoreOutputFilePath = TestHelpers.GetTestFilePath("SimpleScriptIgnore.cs");
            _simpleCompileFailureScriptFilePath = TestHelpers.GetTestFilePath("SimpleCompileFailure.csx");
            _simpleCompileFailureOutputFilePath = TestHelpers.GetTestFilePath("SimpleCompileFailure.cs");
            _multipleScriptVaryingScriptFilePath = TestHelpers.GetTestFilePath("MultiFileScriptControl.csx");
            _multipleScriptVaryingTempFilePattern = "MultiFileScriptControl*.cs.scriptytmp";
            _multipleScriptVaryingOutputFilePattern = "MultiFileScriptControl*.cs";

            CleanupScriptOutputs();
        }

        private void CleanupScriptOutputs()
        {
            var files = new List<string>();
            files.Add(_simpleSuccessOutputFilePath);
            files.Add(_simpleCompileFailureOutputFilePath);
            files.Add(_simpleScriptKeepOutputFilePath);
            files.Add(_simpleScriptIgnoreOutputFilePath);
            TestHelpers.RemoveFiles(files);
            TestHelpers.RemoveFiles(_multipleScriptVaryingOutputFilePattern);
            TestHelpers.RemoveFiles(_multipleScriptVaryingTempFilePattern);
        }
        
        [OneTimeTearDown]
        public void Teardown()
        {
            CleanupScriptOutputs();
        }

        #endregion //#region "common test members"

        /// <summary>
        ///     Given the default output behavior is set and no errors occur
        ///     A file is placed in the output location, and its contents are overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_Success_WithDefaultOnScriptSaveBehavior()
        {
            var ep = new EngineParams
            {
                ScriptFile = _simpleSuccessScriptFilePath,
                OutputFile = _simpleSuccessOutputFilePath,
                GeneratedOutputFileCount = 1,
                ErrorCount = 0,
                OutContentMatchesTestContent = false
            };

            var result = EvaluateScriptAndGetResult(ep);

            Assert.IsNotNull(result);
            StringAssert.Contains("namespace TestNamespace{class TestClass{public void TestMethod(){}}}", result.DefaultOutputFileContents);
        }

        /// <summary>
        /// Given the default output behavior is set and compilation errors occur
        ///     A file is placed in the output location, and its contents are not overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_CompilationFailureWithDefaultOutputBehavior()
        {
            var ep = new EngineParams
            {
                ScriptFile = _simpleCompileFailureScriptFilePath,
                OutputFile = _simpleCompileFailureOutputFilePath,
                GeneratedOutputFileCount = 0,
                ErrorCount = 19,
                OutContentMatchesTestContent = true
            };

            var result = EvaluateScriptAndGetResult(ep);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Given the OutputBehavior is set to never generate output and no errors occur
        ///     A file is placed in the output location, and its contents are not overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_Success_WithNeverGenerateOutput()
        {
            var ep = new EngineParams
            {
                ScriptFile = _simpleSuccessScriptFilePath,
                OutputFile = _simpleSuccessOutputFilePath,
                GeneratedOutputFileCount = 0,
                ErrorCount = 0,
                OutContentMatchesTestContent = true,
                OutputBehavior = OutputBehavior.NeverGenerateOutput
            };

            var result = EvaluateScriptAndGetResult(ep);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Given the OutputBehavior is set to <see cref="OutputBehavior.ScriptControlsOutput"/>, the script is 
        ///     using <see cref="ScriptOutput.Keep"/> and no errors occur
        /// 
        ///     A file is placed in the output location, and its contents are overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_Success_WithScriptControlsOutputKeep()
        {
            var ep = new EngineParams
            {
                ScriptFile = _simpleScriptKeepScriptFilePath,
                OutputFile = _simpleScriptKeepOutputFilePath,
                GeneratedOutputFileCount = 1,
                ErrorCount = 0,
                OutContentMatchesTestContent = false,
                OutputBehavior = OutputBehavior.ScriptControlsOutput
            };

            var result = EvaluateScriptAndGetResult(ep);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Given the OutputBehavior is set to <see cref="OutputBehavior.ScriptControlsOutput"/>, the script is 
        ///     using <see cref="ScriptOutput.Ignore"/> and no errors occur
        /// 
        ///     A file is placed in the output location, and its contents are not overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_Success_WithScriptControlsOutputIgnore()
        {
            var ep = new EngineParams
            {
                ScriptFile = _simpleScriptIgnoreScriptFilePath,
                OutputFile = _simpleScriptIgnoreOutputFilePath,
                GeneratedOutputFileCount = 0,
                ErrorCount = 0,
                OutContentMatchesTestContent = true,
                OutputBehavior = OutputBehavior.ScriptControlsOutput
            };

            var result = EvaluateScriptAndGetResult(ep);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Given the OutputBehavior is set to <see cref="OutputBehavior.ScriptControlsOutput"/>, the script is 
        ///     using (<see><cref>Scripty.Core.OutputFile.KeepOutput</cref></see>== <see cref="bool.False"/>) on files that have (fileNumber % 3 == 0)
        ///     and each TestMethod() in the output returns the filename and no errors occur
        /// 
        ///     A file is placed in the output location, and its contents are not overwritten by a successfully processed script
        /// </summary>
        [Test]
        public void Evaluate_Success_WithScriptControlsOutput_ForMultipleFiles_WithVaryingOutputBehavior()
        {
            CleanupScriptOutputs();

            var scriptCode = TestHelpers.GetFileContent(_multipleScriptVaryingScriptFilePath);
            var scriptSource = new ScriptSource(_multipleScriptVaryingScriptFilePath, scriptCode);
            var se = TestHelpers.BuildScriptEngine();
            se.OutputBehavior = OutputBehavior.ScriptControlsOutput;

            var result = se.Evaluate(scriptSource).Result;

            Assert.IsNotNull(result);
            var generatedOutputFiles = result.OutputFiles.Where(w => w.OutputWasGenerated).ToList();
            Assert.AreEqual(10, generatedOutputFiles.Count());
            Assert.AreEqual(result.OutputFiles.Count, result.OutputFiles.Count(c => c.IsClosed));
            foreach (var of in generatedOutputFiles)
            {
                var fileName = new FileInfo(of.TargetFilePath).Name;
                var content = TestHelpers.GetFileContent(fileName);
                if (content.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    Assert.Fail($"fileName {fileName} did not contain the expected content");
                }
            }
        }

        /// <summary>
        ///     Runs the common test pattern
        /// </summary>
        /// <param name="ep">The ep.</param>
        /// <returns></returns>
        public EngineResults EvaluateScriptAndGetResult(EngineParams ep)
        {
            CleanupScriptOutputs();
            FileAssert.DoesNotExist(ep.OutputFile);
            TestHelpers.CreateFiles(new List<string> { ep.OutputFile });
            FileAssert.Exists(ep.OutputFile);

            var scriptCode = TestHelpers.GetFileContent(ep.ScriptFile);
            var scriptSource = new ScriptSource(ep.ScriptFile, scriptCode);
            var se = TestHelpers.BuildScriptEngine();
            if (ep.OutputBehavior.HasValue)
            {
                se.OutputBehavior = ep.OutputBehavior.Value;
            }

            var result = se.Evaluate(scriptSource).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Count == ep.ErrorCount, $"Unexpected Errors {result}");
            var generatedOutputCount = result.OutputFiles.Count(o => o.OutputWasGenerated == true);
            Assert.AreEqual(ep.GeneratedOutputFileCount, generatedOutputCount, $"Expected {ep.GeneratedOutputFileCount} file but got {generatedOutputCount}");
            FileAssert.Exists(ep.OutputFile);
            var content = TestHelpers.GetFileContent(ep.OutputFile);
            if (ep.OutContentMatchesTestContent == true)
            {
                StringAssert.AreEqualIgnoringCase(TestHelpers.TEST_FILE_CONTENT, content);
            }
            else
            {
                StringAssert.AreNotEqualIgnoringCase(TestHelpers.TEST_FILE_CONTENT, content);
            }

            return new EngineResults
            {
                DefaultOutputFileContents = content,
                ScriptResult = result
            };
        }



    }
}