using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Scripty.Core.Output;

namespace Scripty.Core.Tests.Output
{
    [TestFixture]
    [Parallelizable(ParallelScope.None | ParallelScope.None)]
    public class OutputFileCollectionFixture : BaseFixture
    {
        public class SetExtensionTests : OutputFileCollectionFixture
        {
            [TestCase("json", @"C:\Test\script.json")]
            [TestCase(".json", @"C:\Test\script.json")]
            [TestCase("test.json", @"C:\Test\script.test.json")]
            [TestCase(".test.json", @"C:\Test\script.test.json")]
            public void CorrectlySetsExtension(string extension, string expected)
            {
                // Given
                OutputFileCollection output = new OutputFileCollection(@"C:\Test\script.csx");

                // When
                output.SetExtension(extension);

                // Then
                Assert.AreEqual(expected, output.TargetFilePath);
            }

            [Test]
            public void ChangesExtensionOfScriptFilePath()
            {
                // Given
                OutputFileCollection output = new OutputFileCollection(@"C:\Test\script.csx");
                output.SetFilePath(@"C:\Test2\script2.csx");

                // When
                output.SetExtension(".json");

                // Then
                Assert.AreEqual(@"C:\Test\script.json", output.TargetFilePath);
            }
        }
    }
}
