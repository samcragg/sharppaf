namespace UnitTests.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using NSubstitute;
    using SharpPaf.Data;
    using SharpPaf.Data.IO;

    /// <summary>
    /// Allows the creation of Mainfile under a test environment, mocking file
    /// system access.
    /// </summary>
    internal sealed class MainfileBuilder
    {
        private const string MainfilePath = "Mainfile";
        private List<string> filenames;
        private IFileSystem fileSystem;

        public MainfileBuilder()
        {
            this.filenames = new List<string>();
            this.fileSystem = Substitute.For<IFileSystem>();
            this.fileSystem.GetFiles(MainfilePath)
                           .Returns(this.filenames);

            FileSystem.Instance = this.fileSystem;
        }

        public void AddFile(string name, string contents)
        {
            this.filenames.Add(name);

            byte[] byteContents = Encoding.ASCII.GetBytes(contents);
            this.fileSystem.OpenRead(name)
                           .Returns(_ => new MemoryStream(byteContents, writable: false));
        }

        public Mainfile Create()
        {
            return new Mainfile(MainfilePath);
        }

        public void RemoveFile(string name)
        {
            this.filenames.Remove(name);
        }
    }
}
