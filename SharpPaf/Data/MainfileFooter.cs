namespace SharpPaf.Data
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides information about the record count of a PAF file.
    /// </summary>
    internal sealed class MainfileFooter
    {
        private readonly int recordCount;

        /// <summary>
        /// Initializes a new instance of the MainfileFooter class.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public MainfileFooter(Stream stream)
        {
            // Move to the end of the file
            long start = stream.Length - MainfileInfo.UnknownRecordBufferSize;
            stream.Position = Math.Max(start, 0);

            byte[] buffer = new byte[MainfileInfo.UnknownRecordBufferSize];
            int read = stream.Read(buffer, 0, buffer.Length);

            int recordEnd = FindLineEnd(buffer, read - 1);
            int recordStart = FindLineEnd(buffer, recordEnd - 2); // Search before the \n\r
            this.recordCount = GetRecordCount(buffer, recordStart, recordEnd);
        }

        /// <summary>
        /// Gets the number of records in the file, including the header and footer.
        /// </summary>
        public int RecordCount
        {
            get { return this.recordCount; }
        }

        private static int FindLineEnd(byte[] buffer, int end)
        {
            const byte CarriageReturn = (byte)'\r';
            const byte LineFeed = (byte)'\n';

            for (int i = end; i > 0; i--)
            {
                if ((buffer[i] == LineFeed) & (buffer[i - 1] == CarriageReturn))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private static int SkipHighBytes(byte[] buffer, int start, int end)
        {
            const byte High = 0xff;
            const byte Space = (byte)' ';

            int index = start;
            for (; index < end; index++)
            {
                byte value = buffer[index];
                if ((value != High) & (value != Space))
                {
                    break;
                }
            }

            return index;
        }

        private static int SkipRecordIdentifier(byte[] buffer, int start, int end)
        {
            const byte Nine = (byte)'9';

            int index = start;
            for (; index < end; index++)
            {
                if (buffer[index] != Nine)
                {
                    break;
                }
            }

            return index;
        }

        private int GetRecordCount(byte[] buffer, int start, int end)
        {
            int length = (end - start) == MainfileHeader.BusinessMailHeaderLength ? 6 : 8;

            int index = start;
            index = SkipHighBytes(buffer, start, end);
            index = SkipRecordIdentifier(buffer, index, end);

            int value;
            StringUtils.TryParseInt32(buffer, index, length, out value);
            return value;
        }
    }
}
