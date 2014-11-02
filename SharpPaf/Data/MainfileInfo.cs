namespace SharpPaf.Data
{
    using SharpPaf.Data.IO;

    /// <summary>
    /// Provides information about the file structure of a PAF file.
    /// </summary>
    public sealed class MainfileInfo
    {
        /// <summary>
        /// The size of the buffer to use that is big enough to contain any record.
        /// </summary>
        internal const int UnknownRecordBufferSize = 256;

        private readonly MainfileFooter footer;
        private readonly MainfileHeader header;
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the MainfileInfo class.
        /// </summary>
        /// <param name="path">
        /// The full path of the file to extract the information from.
        /// </param>
        public MainfileInfo(string path)
        {
            this.path = path;

            using (var stream = FileSystem.Instance.OpenRead(path))
            {
                this.header = new MainfileHeader(stream, path);
                this.footer = new MainfileFooter(stream);
            }
        }

        /// <summary>
        /// Gets the edition of the file.
        /// </summary>
        public string Edition
        {
            get { return this.header.Edition; }
        }

        /// <summary>
        /// Gets the type of the file this header belongs to.
        /// </summary>
        public MainfileType FileType
        {
            get { return this.header.FileType; }
        }

        /// <summary>
        /// Gets the path of the file.
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        /// <summary>
        /// Gets the number of records in the file, including the header and footer.
        /// </summary>
        public int RecordCount
        {
            get { return this.footer.RecordCount; }
        }

        /// <summary>
        /// Gets the length of a record, including the new line terminator.
        /// </summary>
        public int RecordLength
        {
            get { return this.header.RecordLength; }
        }
    }
}
