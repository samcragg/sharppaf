namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the parsing of thoroughfare descriptor records.
    /// </summary>
    internal sealed class ThoroughfareDescriptorRecordParser : RecordParser
    {
        private const int DescriptorLength = 20;
        private const int DescriptorStart = 4;
        private const int KeyLength = 4;
        private const int KeyStart = 0;

        /// <inheritdoc />
        internal override KeyValuePair<string, Type>[] GetColumns()
        {
            return new[]
            {
                new KeyValuePair<string, Type>("Key", typeof(int)),
                new KeyValuePair<string, Type>("Descriptor", typeof(string))
            };
        }

        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            var record = new ThoroughfareDescriptorRecord();
            record.Key = GetInt32(iterator, KeyStart, KeyLength);
            record.Descriptor = GetString(iterator, DescriptorStart, DescriptorLength);
            repository.AddThoroughfareDescriptor(record);
        }

        /// <inheritdoc />
        protected override object[] ParseLine(RecordParser.LineIterator iterator)
        {
            return new object[]
            {
                GetInt32(iterator, KeyStart, KeyLength),
                GetString(iterator, DescriptorStart, DescriptorLength)
            };
        }
    }
}
