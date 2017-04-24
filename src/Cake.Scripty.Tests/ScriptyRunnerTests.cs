using Cake.Core.IO;
using Cake.Testing.Fixtures;
using NUnit.Framework;
using System;

namespace Cake.Scripty.Tests
{
    [TestFixture]
    public class ScriptyRunnerTests
    {
        [Test]
        public void ShouldThrowOnNullProjectPath()
        {
            // Given
            ScriptyFixture fixture = new ScriptyFixture(r => r.Evaluate()) {ProjectFilePath = null};

            // When, Then
            Assert.Throws<ArgumentNullException>(() => fixture.Run());
        }

        [Test]
        public void ShouldThrowWhenNoFilesProvided()
        {
            // Given
            ScriptyFixture fixture = new ScriptyFixture(r => r.Evaluate());

            // When, Then
            Assert.Throws<ArgumentException>(() => fixture.Run());
        }

        [Test]
        public void ShouldAddProjectFile()
        {
            // Given
            ScriptyFixture fixture = new ScriptyFixture(r => r.Evaluate("file.csx"));

            // When
            ToolFixtureResult result = fixture.Run();

            // Then
            FilePath proj = fixture.GetProjectFilePath();
            Assert.True(result.Args.StartsWith($"\"{proj}\""));
        }

        [Test]
        public void ShouldAddSingleScriptFile()
        {
            // Given
            ScriptyFixture fixture = new ScriptyFixture(r => r.Evaluate("file.csx"));

            // When
            ToolFixtureResult result = fixture.Run();

            // Then
            FilePath proj = fixture.GetProjectFilePath();
            Assert.True(result.Args == $"\"{proj}\" \"file.csx\"");
        }

        [Test]
        public void ShouldAddMultipleFiles()
        {
            // Given
            ScriptyFixture fixture = new ScriptyFixture(r => r.Evaluate("file.csx", "script.csx"));

            // When
            ToolFixtureResult result = fixture.Run();

            // Then
            FilePath proj = fixture.GetProjectFilePath();
            Assert.True(result.Args == $"\"{proj}\" \"file.csx\" \"script.csx\"");
        }
    }
}
