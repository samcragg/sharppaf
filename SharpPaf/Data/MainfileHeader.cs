namespace SharpPaf.Data
{
    using System.IO;

    /// <summary>
    /// Provides information about the file header structure of a PAF file.
    /// </summary>
    internal sealed class MainfileHeader
    {
        /// <summary>
        /// The length of records inside the Business Mail file.
        /// </summary>
        internal const int BusinessMailHeaderLength = 12;

        private readonly string edition;
        private readonly MainfileType fileType;
        private readonly int recordLength;

        /// <summary>
        /// Initializes a new instance of the MainfileHeader class.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="path">
        /// The full path of the file to extract the information from.
        /// </param>
        public MainfileHeader(Stream stream, string path)
        {
            byte[] buffer = new byte[MainfileInfo.UnknownRecordBufferSize];
            stream.Read(buffer, 0, buffer.Length);

            this.recordLength = GetRecordLength(buffer);

            int index = SkipLowBytes(buffer, 0, this.recordLength);
            index = SkipRecordIdentifier(buffer, index, this.recordLength);

            string fileRecord = GetString(8, buffer, index, this.recordLength);
            MainfileType mainfileType = GetMainfileType(fileRecord, this.recordLength);
            this.fileType = GetMainfileTypeCheckingForWelshAddresses(mainfileType, path);

            if (mainfileType != MainfileType.BusinessMail)
            {
                index += fileRecord.Length;
            }
            this.edition = GetString(6, buffer, index, this.recordLength);
        }

        /// <summary>
        /// Gets the edition of the file.
        /// </summary>
        public string Edition
        {
            get { return this.edition; }
        }

        /// <summary>
        /// Gets the type of the file this header belongs to.
        /// </summary>
        public MainfileType FileType
        {
            get { return this.fileType; }
        }

        /// <summary>
        /// Gets the length of a record, including the new line terminator.
        /// </summary>
        public int RecordLength
        {
            get { return this.recordLength; }
        }

        private static MainfileType GetMainfileType(string fileRecord, int recordLength)
        {
            switch (fileRecord)
            {
                case "LOCALITY":
                    return MainfileType.Localities;

                case "THOROUGH":
                    return MainfileType.Thoroughfares;

                case "THDESCRI":
                    return MainfileType.ThoroughfareDescriptors;

                case "BUILDING":
                    return MainfileType.BuildingNames;

                case "SUBBUILD":
                    return MainfileType.SubBuildingNames;

                case "ORGANISA":
                    return MainfileType.Organisations;

                case "ADDRESS ":
                    return MainfileType.Address;

                default:
                    if (recordLength == BusinessMailHeaderLength)
                    {
                        return MainfileType.BusinessMail;
                    }

                    return MainfileType.Unknown;
            }
        }

        private static MainfileType GetMainfileTypeCheckingForWelshAddresses(MainfileType mainfileType, string path)
        {
            if (mainfileType == MainfileType.Address)
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                if ("WFMAINFL".Equals(filename, System.StringComparison.OrdinalIgnoreCase))
                {
                    return MainfileType.WelshAddress;
                }
            }

            return mainfileType;
        }

        private static int GetRecordLength(byte[] buffer)
        {
            const byte CarriageReturn = (byte)'\r';
            const byte LineFeed = (byte)'\n';

            for (int i = 0; i < buffer.Length - 1; i++)
            {
                if ((buffer[i] == CarriageReturn) & (buffer[i + 1] == LineFeed))
                {
                    return i + 2; // +2 to include \r\n
                }
            }

            return -1;
        }

        private static string GetString(int length, byte[] buffer, int start, int end)
        {
            if (end - start < length)
            {
                return string.Empty;
            }

            char[] charBuffer = new char[length];
            int index = start;
            for (int i = 0; i < length; i++)
            {
                charBuffer[i] = (char)buffer[index++];
            }

            return new string(charBuffer);
        }

        private static int SkipLowBytes(byte[] buffer, int start, int end)
        {
            const byte Low = 0;
            const byte Space = (byte)' ';

            int index = start;
            for (; index < end; index++)
            {
                byte value = buffer[index];
                if ((value != Low) & (value != Space))
                {
                    break;
                }
            }

            return index;
        }

        private static int SkipRecordIdentifier(byte[] buffer, int start, int end)
        {
            const byte Zero = (byte)'0';

            int index = start;
            for (; index < end; index++)
            {
                if (buffer[index] != Zero)
                {
                    break;
                }
            }

            return index;
        }
    }
}
