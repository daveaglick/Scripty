# 0.5.0

- [Feature] CLI now accepts relative paths to project files
- [Feature] Adds `ScriptFiles` to the list of available build actions in Visual Studio (#45, thanks @ap0llo)
- [Feature] CLI now scans for `.csx` files in project if none are explicitly specified (#5)
- [Feature] Automatic indentation support (#15)
- [Feature] Support for the Roslyn formatter to automatically format output (#47, thanks @thebigb)
- [Feature] Implements a Cake addin for out-of-band Scripty evaluation during Cake builds (#31, thanks @agc93).

# 0.4.0

- [Feature][Breaking Change] New project tree abstraction (#13). If you were using the `Project` property previous to this release, you can now get to the Roslyn project object using the `Project.Analysis` property.
- [Feature] Added `SetExtension()` and `SetFilePath()` methods to `OutputFileCollection` allowing you to change the extension or file path of the default output file (#19).
- [Feature] The `OutputFile` class now exposes a fluent API for chaining calls like `WriteLine()` (#26, thanks @Tydude4Christ).

# 0.3.0

- [Feature] All output file objects now derive from a common `OutputFile` base class.
- [Feature] Added the `Scripty.Core` namespace and assembly to the script.
- [Feature] Moved the globals into a `ScriptContext` class and exposed it as a `Context` property.
- [Refactoring] Renamed the MSBuild item group item to `ScriptyFile` (#6).

# 0.2.0

- [Feature] Added a Visual Studio single file generator (a.k.a "custom tool").

# 0.1.0

- Initial public release!