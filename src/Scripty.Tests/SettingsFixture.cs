using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Scripty.Tests
{
    [TestFixture]
    public class SettingsFixture
    {

        [Test]
        public void CanParseProjectAndScriptFiles()
        {
            bool errors;
            Settings settings;
            string[] args;

            settings = new Settings();

            args = new[] {
                @"C:\project.proj",
                @"C:\script1.csx", @"C:\script2.csx"
            };

            Assert.That(settings.ParseArgs(args, out errors));
            Assert.IsFalse(errors);
            Assert.AreEqual(@"C:\project.proj", settings.ProjectFilePath);
            Assert.AreEqual(2, settings.ScriptFilePaths.Count);
            Assert.AreEqual(@"C:\script1.csx", settings.ScriptFilePaths[0]);
            Assert.AreEqual(@"C:\script2.csx", settings.ScriptFilePaths[1]);
        }

        [Test]
        public void CanReceiveSolution()
        {
            bool errors;
            Settings settings;
            string[] args;

            settings = new Settings();

            args = new[] {
                "--solution", @"C:\solution.sln",
                @"C:\project.proj",
                @"C:\script1.csx"
            };

            Assert.That(settings.ParseArgs(args, out errors));
            Assert.IsFalse(errors);
            Assert.AreEqual(@"C:\project.proj", settings.ProjectFilePath);
            Assert.AreEqual(@"C:\solution.sln", settings.SolutionFilePath);
        }

        [Test]
        public void CanReceiveProperties()
        {
            bool errors;
            Settings settings;
            string[] args;

            settings = new Settings();

            args = new[] {
                "-p", @"key=value",
                @"C:\project.proj",
                @"C:\script1.csx"
            };

            Assert.That(settings.ParseArgs(args, out errors));
            Assert.IsFalse(errors);
            Assert.IsNotNull(settings.Properties);
            Assert.AreEqual(1, settings.Properties.Count);
            Assert.That(settings.Properties.ContainsKey("key"));
            Assert.AreEqual("value", settings.Properties["key"]);
        }

        [Test]
        public void CanReceiveMultipleProperties()
        {
            bool errors;
            Settings settings;
            string[] args;

            settings = new Settings();

            args = new[] {
                "-p", "key=value",
                "-p", "foo=bar",
                @"C:\project.proj",
                @"C:\script1.csx"
            };

            Assert.That(settings.ParseArgs(args, out errors));
            Assert.IsFalse(errors);
            Assert.IsNotNull(settings.Properties);
            Assert.AreEqual(2, settings.Properties.Count);
            Assert.That(settings.Properties.ContainsKey("key"));
            Assert.AreEqual("value", settings.Properties["key"]);
            Assert.That(settings.Properties.ContainsKey("foo"));
            Assert.AreEqual("bar", settings.Properties["foo"]);
        }

        [Test]
        public void CanReceiveAllSettings()
        {
            bool errors;
            Settings settings;
            string[] args;

            settings = new Settings();

            args = new[] {
                "--attach",
                "--solution", @"C:\solution.sln",
                "-p", "key=value",
                "-p", "foo=bar",
                @"C:\project.proj",
                @"C:\script1.csx", @"C:\script2.csx"
            };

            Assert.That(settings.ParseArgs(args, out errors));
            Assert.IsFalse(errors);

            Assert.That(settings.Attach);

            Assert.AreEqual(@"C:\solution.sln", settings.SolutionFilePath);

            Assert.IsNotNull(settings.Properties);
            Assert.AreEqual(2, settings.Properties.Count);
            Assert.That(settings.Properties.ContainsKey("key"));
            Assert.AreEqual("value", settings.Properties["key"]);
            Assert.That(settings.Properties.ContainsKey("foo"));
            Assert.AreEqual("bar", settings.Properties["foo"]);

            Assert.AreEqual(@"C:\project.proj", settings.ProjectFilePath);

            Assert.AreEqual(2, settings.ScriptFilePaths.Count);
            Assert.AreEqual(@"C:\script1.csx", settings.ScriptFilePaths[0]);
            Assert.AreEqual(@"C:\script2.csx", settings.ScriptFilePaths[1]);
        }

    }
}
