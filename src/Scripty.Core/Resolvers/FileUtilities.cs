namespace Scripty.Core.Resolvers
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    ///     A collection of non-exception throwing filesystem utils. 
    /// </summary>
    /// <remarks>
    ///     Exceptions caught are output to Trace.TraceError
    /// </remarks>
    public static class FileUtilities
    {
        /// <summary>
        ///     Removes the file if present.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void RemoveIfPresent(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed to remove file '{fileName}', {e}");
                }
            }
        }

        /// <summary>
        ///     Gets the content of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetFileContent(string fileName)
        {
            try
            {
                return File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to get content for file '{fileName}', {e}");
            }
            return string.Empty;
        }

        /// <summary>
        ///     Writes the assembly.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static bool WriteAssembly(string targetPath, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(targetPath, bytes);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to write assembly for '{targetPath}', {e}");
            }
            return false;
        }
    }
}