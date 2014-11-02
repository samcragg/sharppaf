namespace SharpPaf.Data.Parsers
{
    /// <summary>
    /// Creates a RecordParser that is capable of parsing a file.
    /// </summary>
    internal static class RecordParserFactory
    {
        /// <summary>
        /// Creates a RecordParser that is capable of parsing the specified file.
        /// </summary>
        /// <param name="fileType">The type of file to parse.</param>
        /// <returns>
        /// An object that can parse the file, or null if none were found.
        /// </returns>
        public static RecordParser Create(MainfileType fileType)
        {
            switch (fileType)
            {
                case MainfileType.Address:
                    return new AddressRecordParser();

                case MainfileType.BuildingNames:
                    return new BuildingNameRecordParser();

                case MainfileType.Localities:
                    return new LocalityRecordParser();

                case MainfileType.Organisations:
                    return new OrganisationRecordParser();

                case MainfileType.SubBuildingNames:
                    return new SubBuildingNameRecordParser();

                case MainfileType.ThoroughfareDescriptors:
                    return new ThoroughfareDescriptorRecordParser();

                case MainfileType.Thoroughfares:
                    return new ThoroughfareRecordParser();

                case MainfileType.WelshAddress:
                    return new WelshAddressRecordParser();

                default:
                    return null;
            }
        }
    }
}
