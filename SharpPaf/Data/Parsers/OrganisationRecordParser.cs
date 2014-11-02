namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the parsing of organisation records.
    /// </summary>
    internal sealed class OrganisationRecordParser : RecordParser
    {
        private const int DepartmentLength = 60;
        private const int DepartmentStart = 69;
        private const int KeyLength = 8;
        private const int KeyStart = 0;
        private const int NameLength = 60;
        private const int NameStart = 9;
        private const int PostcodeTypeIndex = 8;

        /// <inheritdoc />
        internal override KeyValuePair<string, Type>[] GetColumns()
        {
            return new[]
            {
                new KeyValuePair<string, Type>("Key", typeof(int)),
                new KeyValuePair<string, Type>("PostcodeType", typeof(byte)),
                new KeyValuePair<string, Type>("Name", typeof(string)),
                new KeyValuePair<string, Type>("Department", typeof(string))
            };
        }

        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            var record = new OrganisationRecord();
            record.Key = GetInt32(iterator, KeyStart, KeyLength);
            record.PostcodeType = ParsePostcodeType(iterator.Buffer[iterator.Offset + PostcodeTypeIndex]);
            record.Name = GetString(iterator, NameStart, NameLength);
            record.Department = GetString(iterator, DepartmentStart, DepartmentLength);
            repository.AddOrganisation(record);
        }

        /// <inheritdoc />
        protected override object[] ParseLine(LineIterator iterator)
        {
            return new object[]
            {
                GetInt32(iterator, KeyStart, KeyLength),
                (byte)ParsePostcodeType(iterator.Buffer[iterator.Offset + PostcodeTypeIndex]),
                GetString(iterator, NameStart, NameLength),
                GetString(iterator, DepartmentStart, DepartmentLength),
            };
        }

        private static DeliveryPointType ParsePostcodeType(byte value)
        {
            switch (value)
            {
                case (byte)'L':
                    return DeliveryPointType.LargeUser;

                case (byte)'S':
                    return DeliveryPointType.SmallUser;

                default:
                    return DeliveryPointType.Unknown;
            }
        }
    }
}
