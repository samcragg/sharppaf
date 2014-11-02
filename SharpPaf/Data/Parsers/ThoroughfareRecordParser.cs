﻿namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the parsing of thoroughfare records.
    /// </summary>
    internal sealed class ThoroughfareRecordParser : RecordParser
    {
        private const int KeyLength = 8;
        private const int KeyStart = 0;
        private const int NameLength = 60;
        private const int NameStart = 8;

        /// <inheritdoc />
        internal override KeyValuePair<string, Type>[] GetColumns()
        {
            return new[]
            {
                new KeyValuePair<string, Type>("Key", typeof(int)),
                new KeyValuePair<string, Type>("Name", typeof(string))
            };
        }

        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            var record = new ThoroughfareRecord();
            record.Key = GetInt32(iterator, KeyStart, KeyLength);
            record.Name = GetString(iterator, NameStart, NameLength);
            repository.AddThoroughfare(record);
        }

        /// <inheritdoc />
        protected override object[] ParseLine(LineIterator iterator)
        {
            return new object[]
            {
                GetInt32(iterator, KeyStart, KeyLength),
                GetString(iterator, NameStart, NameLength)
            };
        }
    }
}
