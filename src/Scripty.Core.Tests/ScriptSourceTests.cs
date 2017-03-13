namespace Scripty.Core.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    public class ScriptSourceTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t \r \n")]
        public void EnsureArgExThrownFilePathIsNotProvided(string filePath)
        {
            var code = "var x = 1;";

            Assert.Throws<ArgumentException>(() => Construct(filePath, code));
        }

        [TestCase(".\\file.txt")]
        [TestCase("file.txt")]
        [TestCase("..File.txt")]
        public void EnsureArgExThrownFilePathIsNotRooted(string filePath)
        {
            var code = "var x = 1;";

            Assert.Throws<ArgumentException>(() => Construct(filePath, code));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t \r \n")]
        public void EnsureArgExThrownCodeIsNotProvided(string code)
        {
            var filePath = Assembly.GetExecutingAssembly().CodeBase;

            Assert.Throws<ArgumentException>(() => Construct(filePath, code));
        }


        private ScriptSource Construct(string filePath, string code)
        {
            return new ScriptSource(filePath, code);
        }

    }
}