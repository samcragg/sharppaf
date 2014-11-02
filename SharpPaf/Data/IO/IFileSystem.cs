namespace SharpPaf.Data.IO
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Allows the abstraction of the file system during unit tests.
    /// </summary>
    internal interface IFileSystem
    {
         /// <summary>
        /// Returns a collection of file names in the specified path.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// A collection of the full names (including paths) for the files in
        /// the directory specified by path.
        /// </returns>
        IEnumerable<string> GetFiles(string path);

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A read-only Stream of the specified file.</returns>
        Stream OpenRead(string path);
    }
}
