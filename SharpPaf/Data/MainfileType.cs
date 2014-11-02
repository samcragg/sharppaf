namespace SharpPaf.Data
{
    /// <summary>
    /// Represents the type of information contained in a file.
    /// </summary>
    public enum MainfileType
    {
        /// <summary>
        /// The file contains unknown information.
        /// </summary>
        Unknown,

        /// <summary>
        /// The file contains one record for each Locality held on PAF.
        /// </summary>
        Localities,

        /// <summary>
        /// The file contains one record for each Thoroughfare on PAF.
        /// </summary>
        Thoroughfares,

        /// <summary>
        /// The file contains one record for each Thoroughfare Descriptor held
        /// on PAF.
        /// </summary>
        ThoroughfareDescriptors,

        /// <summary>
        /// The file contains one record for each Building Name held on PAF.
        /// </summary>
        BuildingNames,

        /// <summary>
        /// The file contains one record for each Sub Building Name held on PAF.
        /// </summary>
        SubBuildingNames,

        /// <summary>
        /// The file contains one record for each occurrence of an Organisation
        /// held on PAF.
        /// </summary>
        Organisations,

        /// <summary>
        /// The file contains one record for each Delivery Point held on PAF.
        /// </summary>
        Address,

        /// <summary>
        /// The file contains Welsh alternative addresses.
        /// </summary>
        WelshAddress,

        /// <summary>
        /// The file contains one record for each Postcode sector.
        /// </summary>
        /// <remarks>
        /// This file is optional. Business Mail data is used by customers who
        /// presort their mail to achieve discounts.
        /// </remarks>
        BusinessMail
    }
}
