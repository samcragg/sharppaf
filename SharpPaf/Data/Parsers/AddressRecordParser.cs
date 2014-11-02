namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the parsing of address records.
    /// </summary>
    internal class AddressRecordParser : RecordParser
    {
        private const int BuildingNameLength = 8;
        private const int BuildingNameStart = 49;
        private const int BuildingNumberLength = 4;
        private const int BuildingNumberStart = 45;
        private const int ConcatenatedIndex = 78;
        private const int DeliveryPointSuffixLength = 2;
        private const int DeliveryPointSuffixStart = 79;
        private const int DependentThoroughfareDescriptorLength = 4;
        private const int DependentThoroughfareDescriptorStart = 41;
        private const int DependentThoroughfareLength = 8;
        private const int DependentThoroughfareStart = 33;
        private const int KeyLength = 8;
        private const int KeyStart = 7;
        private const int LocalityLength = 6;
        private const int LocalityStart = 15;
        private const int NumberOfHouseholdsLength = 4;
        private const int NumberOfHouseholdsStart = 65;
        private const int OrganisationLength = 8;
        private const int OrganisationStart = 69;
        private const int POBoxNumberLength = 6;
        private const int POBoxNumberStart = 82;
        private const int PostcodeTypeIndex = 77;
        private const int SmallUserOrganisationIndex = 81;
        private const int SubBuildingNameLength = 8;
        private const int SubBuildingNameStart = 57;
        private const int ThoroughfareDescriptorLength = 4;
        private const int ThoroughfareDescriptorStart = 29;
        private const int ThoroughfareLength = 8;
        private const int ThoroughfareStart = 21;

        /// <inheritdoc />
        internal override KeyValuePair<string, Type>[] GetColumns()
        {
            return new[]
            {
                new KeyValuePair<string, Type>("Key", typeof(int)),
                new KeyValuePair<string, Type>("Postcode", typeof(string)),
                new KeyValuePair<string, Type>("LocalityKey", typeof(int)),
                new KeyValuePair<string, Type>("ThoroughfareKey", typeof(int)),
                new KeyValuePair<string, Type>("ThoroughfareDescriptorKey", typeof(int)),
                new KeyValuePair<string, Type>("DependentThoroughfareKey", typeof(int)),
                new KeyValuePair<string, Type>("DependentThoroughfareDescriptorKey", typeof(int)),
                new KeyValuePair<string, Type>("BuildingNumber", typeof(short)),
                new KeyValuePair<string, Type>("BuildingNameKey", typeof(int)),
                new KeyValuePair<string, Type>("SubBuildingNameKey", typeof(int)),
                new KeyValuePair<string, Type>("NumberOfHouseholds", typeof(short)),
                new KeyValuePair<string, Type>("OrganisationKey", typeof(int)),
                new KeyValuePair<string, Type>("PostcodeType", typeof(byte)),
                new KeyValuePair<string, Type>("IsBuildingNumberConcatenated", typeof(bool)),
                new KeyValuePair<string, Type>("DeliveryPointSuffix", typeof(string)),
                new KeyValuePair<string, Type>("IsSmallUserOrganisation", typeof(bool)),
                new KeyValuePair<string, Type>("POBoxNumber", typeof(string)),
            };
        }

        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            repository.AddAddress(this.ParseRecord(iterator));
        }

        /// <inheritdoc />
        protected override object[] ParseLine(RecordParser.LineIterator iterator)
        {
            return new object[]
            {
                 GetInt32(iterator, KeyStart, KeyLength),
                 GetPostcode(iterator),
                 GetInt32(iterator, LocalityStart, LocalityLength),
                 GetOptionalInt32(iterator, ThoroughfareStart, ThoroughfareLength),
                 GetOptionalInt32(iterator, ThoroughfareDescriptorStart, ThoroughfareDescriptorLength),
                 GetOptionalInt32(iterator, DependentThoroughfareStart, DependentThoroughfareLength),
                 GetOptionalInt32(iterator, DependentThoroughfareDescriptorStart, DependentThoroughfareDescriptorLength),
                 (short?)GetOptionalInt32(iterator, BuildingNumberStart, BuildingNumberLength),
                 GetOptionalInt32(iterator, BuildingNameStart, BuildingNameLength),
                 GetOptionalInt32(iterator, SubBuildingNameStart, SubBuildingNameLength),
                 (short)GetInt32(iterator, NumberOfHouseholdsStart, NumberOfHouseholdsLength),
                 GetOptionalInt32(iterator, OrganisationStart, OrganisationLength),
                 (byte)ParsePostcodeType(iterator.Buffer[iterator.Offset + PostcodeTypeIndex]),
                 GetBoolean(iterator, ConcatenatedIndex),
                 GetString(iterator, DeliveryPointSuffixStart, DeliveryPointSuffixLength),
                 GetBoolean(iterator, SmallUserOrganisationIndex),
                 GetString(iterator, POBoxNumberStart, POBoxNumberLength)
            };
        }

        /// <summary>
        /// Extracts an address record from the specified line.
        /// </summary>
        /// <param name="iterator">Contains information about the line.</param>
        /// <returns>An address record.</returns>
        protected AddressRecord ParseRecord(LineIterator iterator)
        {
            var record = new AddressRecord();

            record.Key = GetInt32(iterator, KeyStart, KeyLength);
            record.Postcode = GetPostcode(iterator);
            record.LocalityKey = GetInt32(iterator, LocalityStart, LocalityLength);
            record.ThoroughfareKey = GetOptionalInt32(iterator, ThoroughfareStart, ThoroughfareLength);
            record.ThoroughfareDescriptorKey = GetOptionalInt32(iterator, ThoroughfareDescriptorStart, ThoroughfareDescriptorLength);
            record.DependentThoroughfareKey = GetOptionalInt32(iterator, DependentThoroughfareStart, DependentThoroughfareLength);
            record.DependentThoroughfareDescriptorKey = GetOptionalInt32(iterator, DependentThoroughfareDescriptorStart, DependentThoroughfareDescriptorLength);
            record.BuildingNumber = (short?)GetOptionalInt32(iterator, BuildingNumberStart, BuildingNumberLength);
            record.BuildingNameKey = GetOptionalInt32(iterator, BuildingNameStart, BuildingNameLength);
            record.SubBuildingNameKey = GetOptionalInt32(iterator, SubBuildingNameStart, SubBuildingNameLength);
            record.NumberOfHouseholds = (short)GetInt32(iterator, NumberOfHouseholdsStart, NumberOfHouseholdsLength);
            record.OrganisationKey = GetOptionalInt32(iterator, OrganisationStart, OrganisationLength);
            record.PostcodeType = ParsePostcodeType(iterator.Buffer[iterator.Offset + PostcodeTypeIndex]);
            record.IsBuildingNumberConcatenated = GetBoolean(iterator, ConcatenatedIndex);
            record.DeliveryPointSuffix = GetString(iterator, DeliveryPointSuffixStart, DeliveryPointSuffixLength);
            record.IsSmallUserOrganisation = GetBoolean(iterator, SmallUserOrganisationIndex);
            record.POBoxNumber = GetString(iterator, POBoxNumberStart, POBoxNumberLength);

            return record;
        }

        private static int? GetOptionalInt32(LineIterator iterator, int start, int length)
        {
            int result = GetInt32(iterator, start, length);
            return result == 0 ? (int?)null : result;
        }

        private static string GetPostcode(LineIterator iterator)
        {
            // Parse the Postcode in two parts to remove the space in the middle
            // if the outward code is only three characters.
            return GetString(iterator, 0, 4) + GetString(iterator, 4, 3);
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
