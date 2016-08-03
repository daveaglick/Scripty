using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Scripty.Core.Output;

namespace Scripty.Core.Tests.Output
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self | ParallelScope.Children)]
    public class OutputFileWriterFixture : BaseFixture
    {
        public class IndentTests : OutputFileWriterFixture
        {
            [Test]
            public void IncrementsIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentLevel = 3;
                output.Indent();

                // Then
                Assert.AreEqual(4, output.IndentLevel);
            }

            [Test]
            public void ReturnsPreviousIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentLevel = 3;
                int previousIndentLevel = output.Indent();

                // Then
                Assert.AreEqual(3, previousIndentLevel);
            }

            [Test]
            public void SetsToSpecifiedIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentLevel = 3;
                output.Indent(2);

                // Then
                Assert.AreEqual(2, output.IndentLevel);
            }

            [Test]
            public void ReturnsPreviousIndentLevelWhenSettingToSpecifiedIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentLevel = 3;
                int previousIndentLevel = output.Indent(2);

                // Then
                Assert.AreEqual(3, previousIndentLevel);
            }
        }

        public class IndentLevelTests : OutputFileWriterFixture
        {
            [Test]
            public void SetsToSpecifiedIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentLevel = 3;

                // Then
                Assert.AreEqual(3, output.IndentLevel);
            }
        }

        public class WithIndentTests : OutputFileWriterFixture
        {
            [Test]
            public void IndentsUntilDisposal()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"A
    B
C
";

                // When
                output.WriteLine("A");
                using (output.WithIndent())
                {
                    output.WriteLine("B");
                }
                output.WriteLine("C");

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }

            [Test]
            public void RestoresPreviousIndentLevelOnDisposal()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"A
        B
    C
D
";

                // When
                output.WriteLine("A");
                using (output.WithIndent())
                {
                    using (output.WithIndent())
                    {
                        output.WriteLine("B");
                    }
                    output.WriteLine("C");
                }
                output.WriteLine("D");

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }

            [Test]
            public void IndentsSpecifiedAmount()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"A
        B
C
";

                // When
                output.WriteLine("A");
                using (output.WithIndent(2))
                {
                    output.WriteLine("B");
                }
                output.WriteLine("C");

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }

            [Test]
            public void RestoresPreviousIndentLevelOnDisposalWithSpecifiedAmount()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"A
    B
C
    D
E
";

                // When
                output.WriteLine("A");
                using (output.WithIndent())
                {
                    output.WriteLine("B");
                    using (output.WithIndent(0))
                    {
                        output.WriteLine("C");
                    }
                    output.WriteLine("D");
                }
                output.WriteLine("E");

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }
        }

        public class WriteIndentTests
        {
            [Test]
            public void WritesTheCurrentIndentString()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"..";

                // When
                output.IndentString = "..";
                output.IndentLevel = 1;
                output.WriteIndent();

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }

            [Test]
            public void DoesNotWriteIfNoIndentLevel()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));

                // When
                output.IndentString = "..";
                output.IndentLevel = 0;
                output.WriteIndent();

                // Then
                Assert.AreEqual(string.Empty, builder.ToString());
            }

            [Test]
            public void WritesTheCurrentIndentStringMultipleTimes()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"....";

                // When
                output.IndentString = "..";
                output.IndentLevel = 2;
                output.WriteIndent();

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }

            [Test]
            public void DoesNotResetIndentWriteFlag()
            {
                // Given
                StringBuilder builder = new StringBuilder();
                OutputFile output = new OutputFileWriter(new StringWriter(builder));
                string expected = @"A
....B
C
";

                // When
                output.IndentString = "..";
                output.WriteLine("A");
                using (output.WithIndent())
                {
                    output.WriteIndent();
                    output.WriteLine("B");
                }
                output.WriteLine("C");

                // Then
                Assert.AreEqual(expected, builder.ToString());
            }
        }
    }
}
