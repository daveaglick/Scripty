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

[assembly: ProvideBindingRedirection(AssemblyName = "System.Collections.Immutable", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.2.1.0", NewVersion = "1.2.1.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Diagnostics.StackTrace", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.3.0", NewVersion = "4.0.3.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Security.Cryptography.Primitives", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.1.0", NewVersion = "4.0.1.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Xml.XPath.XDocument", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.2.0", NewVersion = "4.0.2.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Diagnostics.FileVersionInfo", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.1.0", NewVersion = "4.0.1.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Composition.AttributedModel", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.0.30.0", NewVersion = "1.0.30.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Composition.Runtime", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.0.30.0", NewVersion = "1.0.30.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Composition.TypedParts", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.0.30.0", NewVersion = "1.0.30.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Composition.Hosting", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.0.30.0", NewVersion = "1.0.30.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Threading.Thread", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.0.1.0", NewVersion = "4.0.1.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Reflection.Metadata", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "1.4.1.0", NewVersion = "1.4.1.0")]

[assembly: ProvideBindingRedirection(AssemblyName = "System.Xml.ReaderWriter", PublicKeyToken = "b03f5f7f11d50a3a",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = "4.1.0.0", NewVersion = "4.1.0.0")]