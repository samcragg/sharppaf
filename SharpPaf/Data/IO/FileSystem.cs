namespace SharpPaf.Data.IO
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// Wraps the System.IO functions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class FileSystem : IFileSystem
    {
        private static IFileSystem instance = new FileSystem();

        private FileSystem()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the current IFileSystem.
        /// </summary>
        public static IFileSystem Instance
        {
            get { return instance; }
            internal set { instance = value; }
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        /// <inheritdoc />
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
    }
}
