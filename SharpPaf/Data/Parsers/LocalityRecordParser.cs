namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the parsing of locality records.
    /// </summary>
    internal sealed class LocalityRecordParser : RecordParser
    {
        private const int DependentLocalityLength = 35;
        private const int DependentLocalityStart = 81;
        private const int DoubleDependentLocalityLength = 35;
        private const int DoubleDependentLocalityStart = 116;
        private const int KeyLength = 6;
        private const int KeyStart = 0;
        private const int PostTownLength = 30;
        private const int PostTownStart = 51;

        /// <inheritdoc />
        internal override KeyValuePair<string, Type>[] GetColumns()
        {
            return new[]
            {
                new KeyValuePair<string, Type>("Key", typeof(int)),
                new KeyValuePair<string, Type>("DependentLocality", typeof(string)),
                new KeyValuePair<string, Type>("DoubleDependentLocality", typeof(string)),
                new KeyValuePair<string, Type>("PostTown", typeof(string))
            };
        }

        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            var record = new LocalityRecord();
            record.Key = GetInt32(iterator, KeyStart, KeyLength);
            record.PostTown = GetString(iterator, PostTownStart, PostTownLength);
            record.DependentLocality = GetString(iterator, DependentLocalityStart, DependentLocalityLength);
            record.DoubleDependentLocality = GetString(iterator, DoubleDependentLocalityStart, DoubleDependentLocalityLength);
            repository.AddLocality(record);
        }

        /// <inheritdoc />
        protected override object[] ParseLine(RecordParser.LineIterator iterator)
        {
            return new object[]
            {
                GetInt32(iterator, KeyStart, KeyLength),
                GetString(iterator, DependentLocalityStart, DependentLocalityLength),
                GetString(iterator, DoubleDependentLocalityStart, DoubleDependentLocalityLength),
                GetString(iterator, PostTownStart, PostTownLength)
            };
        }
    }
}
