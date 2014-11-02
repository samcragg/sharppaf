namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using SharpPaf.Data.IO;

    /// <summary>
    /// Parses a file and adds the parsed records to the repository.
    /// </summary>
    internal abstract partial class RecordParser : IDisposable
    {
        private LineIterator readerIterator;
        private Stream readerStream;

        /// <summary>
        /// Releases the unmanaged resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.DisposeReaderStream();
        }

        /// <summary>
        /// Parses the specified file, adding parsed records to the specified
        /// repository.
        /// </summary>
        /// <param name="info">Contains the information about the file.</param>
        /// <param name="repository">The repository to add records to.</param>
        public void Parse(MainfileInfo info, PafRepository repository)
        {
            Check.IsNotNull(info, "info");
            Check.IsNotNull(repository, "repository");

            using (Stream stream = FileSystem.Instance.OpenRead(info.Path))
            {
                var iterator = new LineIterator(stream, info.RecordLength, info.RecordCount);
                while (iterator.MoveNext())
                {
                    this.ParseLine(repository, iterator);
                }
            }
        }

        /// <summary>
        /// Gets the column names and types that will be parsed.
        /// </summary>
        /// <returns>An array of column names and type.</returns>
        internal abstract KeyValuePair<string, Type>[] GetColumns();

        /// <summary>
        /// Loads the specified file for calls to Read.
        /// </summary>
        /// <param name="fileInfo">The file to load.</param>
        internal void LoadFile(MainfileInfo fileInfo)
        {
            Check.IsNotNull(fileInfo, "fileInfo");

            this.DisposeReaderStream();
            this.readerStream = FileSystem.Instance.OpenRead(fileInfo.Path);
            this.readerIterator = new LineIterator(this.readerStream, fileInfo.RecordLength, fileInfo.RecordCount);
        }

        /// <summary>
        /// Tries to read the next record from the file.
        /// </summary>
        /// <returns>
        /// The objects read for the next record, or null if there are no more
        /// records.
        /// </returns>
        internal object[] Read()
        {
            if (!this.readerIterator.MoveNext())
            {
                return null;
            }

            return this.ParseLine(this.readerIterator);
        }

        /// <summary>
        /// Extracts a boolean value for a Yes/No value from the current line.
        /// </summary>
        /// <param name="iterator">The current line information.</param>
        /// <param name="index">
        /// The index of the Yes/No value relative to the start of the line.
        /// </param>
        /// <returns>A boolean representation of the character.</returns>
        protected static bool GetBoolean(LineIterator iterator, int index)
        {
            return iterator.Buffer[iterator.Offset + index] == (byte)'Y';
        }

        /// <summary>
        /// Extracts an integer from the current line at the specified location.
        /// </summary>
        /// <param name="iterator">The current line information.</param>
        /// <param name="start">
        /// The start index of the integer relative to the start of the line.
        /// </param>
        /// <param name="length">The length of the integer.</param>
        /// <returns>An integer representation of the string.</returns>
        protected static int GetInt32(LineIterator iterator, int start, int length)
        {
            int value;
            StringUtils.TryParseInt32(iterator.Buffer, iterator.Offset + start, length, out value);
            return value;
        }

        /// <summary>
        /// Extracts a string from the current line at the specified location.
        /// </summary>
        /// <param name="iterator">The current line information.</param>
        /// <param name="start">
        /// The start index of the string relative to the start of the line.
        /// </param>
        /// <param name="length">The length of the string.</param>
        /// <returns>The extracted trimmed string.</returns>
        protected static string GetString(LineIterator iterator, int start, int length)
        {
            const byte Space = (byte)' ';

            byte[] buffer = iterator.Buffer;
            int first = iterator.Offset + start;
            int last = first + length - 1;

            for (; first <= last; first++)
            {
                if (buffer[first] != Space)
                {
                    break;
                }
            }

            for (; last >= first; last--)
            {
                if (buffer[last] != Space)
                {
                    break;
                }
            }

            if (first > last)
            {
                return null;
            }

            char[] converted = new char[last - first + 1];
            for (int i = 0; i < converted.Length; i++)
            {
                converted[i] = (char)buffer[first];
                first++;
            }
            return new string(converted);
        }

        /// <summary>
        /// Parses a record from the specified line.
        /// </summary>
        /// <param name="repository">The repository to add the record to.</param>
        /// <param name="iterator">Contains the current line information.</param>
        protected abstract void ParseLine(PafRepository repository, LineIterator iterator);

        /// <summary>
        /// Parses a record from the specified line.
        /// </summary>
        /// <param name="iterator">Contains the current line information.</param>
        /// <returns>The values prased from the current line.</returns>
        protected abstract object[] ParseLine(LineIterator iterator);

        private void DisposeReaderStream()
        {
            if (this.readerStream != null)
            {
                this.readerStream.Dispose();
                this.readerStream = null;
            }
        }

        protected class LineIterator
        {
            private const int DesiredBufferSize = 4096;
            private readonly byte[] buffer;
            private readonly int lineSize;
            private readonly int maxLines;
            private readonly Stream stream;
            private int length;
            private int linesRead;
            private int offset;

            public LineIterator(Stream stream, int lineSize, int lines)
            {
                this.stream = stream;
                this.lineSize = lineSize;
                this.maxLines = lines - 1; // Skip footer

                int bufferSize = (DesiredBufferSize / lineSize) * lineSize;
                this.buffer = new byte[bufferSize];
                this.ReadNextBlock();
            }

            public byte[] Buffer
            {
                get { return this.buffer; }
            }

            public int Offset
            {
                get { return this.offset; }
            }

            public bool MoveNext()
            {
                this.offset += this.lineSize;
                if (this.offset >= this.length)
                {
                    this.ReadNextBlock();
                }

                this.linesRead++;
                return (this.offset < this.length) && (this.linesRead < this.maxLines);
            }

            private void ReadNextBlock()
            {
                this.offset = 0;
                int toRead = this.buffer.Length;
                this.length = stream.Read(this.buffer, 0, this.buffer.Length);
            }
        }
    }
}
