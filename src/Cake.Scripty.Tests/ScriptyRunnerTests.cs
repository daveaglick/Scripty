using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Cake.Scripty.Tests
{
    [TestFixture]
    public class ScriptyRunnerTests
    {
        [Test]
        public void ShouldThrowOnNullProjectPath()
        {
            var fixture = new ScriptyFixture(r => r.Evaluate()) {ProjectFilePath = null};

            Assert.Throws<ArgumentNullException>(() => fixture.Run());
        }

        [Test]
        public void ShouldThrowWhenNoFilesProvided()
        {
            var fixture = new ScriptyFixture(r => r.Evaluate());

            Assert.Throws<ArgumentException>(() => fixture.Run());
        }

        [Test]
        public void ShouldAddProjectFile()
        {
            var fixture = new ScriptyFixture(r => r.Evaluate("file.csx"));
            var result = fixture.Run();
            var proj = fixture.GetProjectFilePath();
            Assert.True(result.Args.StartsWith($"\"{proj}\""));
        }

        [Test]
        public void ShouldAddSingleScriptFile()
        {
            var fixture = new ScriptyFixture(r => r.Evaluate("file.csx"));
            var result = fixture.Run();
            var proj = fixture.GetProjectFilePath();
            Assert.True(result.Args == $"\"{proj}\" \"file.csx\"");
        }

        [Test]
        public void ShouldAddMultipleFiles()
        {
            var fixture = new ScriptyFixture(r => r.Evaluate("file.csx", "script.csx"));
            var result = fixture.Run();
            var proj = fixture.GetProjectFilePath();
            Assert.True(result.Args == $"\"{proj}\" \"file.csx\" \"script.csx\"");
        }
    }
}
