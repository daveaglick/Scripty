using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

[assembly: AssemblyTitle("Scripty.CustomTool")]
[assembly: AssemblyDescription("")]
[assembly: Guid("dd26cc89-e1e8-4a94-8e8c-ad5215616c6c")]
[assembly: ComVisible(true)]

[assembly: ProvideBindingRedirection(AssemblyName = "System.IO.FileSystem", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.2.0", NewVersion = "4.0.2.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.IO.FileSystem.Primitives", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.2.0", NewVersion = "4.0.2.0")]