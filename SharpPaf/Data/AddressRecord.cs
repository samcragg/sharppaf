namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about a delivery point address.
    /// </summary>
    public sealed class AddressRecord
    {
        /// <summary>
        /// Gets or sets the key of the building name record, if any.
        /// </summary>
        public int? BuildingNameKey { get; set; }

        /// <summary>
        /// Gets or sets the building number, if the address has one.
        /// </summary>
        public short? BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the unique Royal Mail two character code for the
        /// delivery point.
        /// </summary>
        /// <remarks>
        /// The delivery point suffix is a unique Royal Mail two character code
        /// (the first numeric and the second alphabetical – e.g. 2B), which,
        /// when added to the Postcode, enables each live delivery point to be
        /// uniquely identified.
        /// </remarks>
        [MaxLength(2)]
        public string DeliveryPointSuffix { get; set; }

        /// <summary>
        /// Gets or sets the key of the dependent thoroughfare descriptor record, if any.
        /// </summary>
        public int? DependentThoroughfareDescriptorKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the dependent thoroughfare record, if any.
        /// </summary>
        public int? DependentThoroughfareKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to concatenate the building
        /// number and sub-building name on the same address line.
        /// </summary>
        public bool IsBuildingNumberConcatenated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the a small user
        /// organisation is present at this address.
        /// </summary>
        public bool IsSmallUserOrganisation { get; set; }

        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the key of the locality record.
        /// </summary>
        public int LocalityKey { get; set; }

        /// <summary>
        /// Gets or sets the multi-occupancy information.
        /// </summary>
        /// <remarks>
        /// When equal to one it indicates that there is one household at the
        /// address, when greater than one the field contains the number of
        /// households present.
        /// </remarks>
        public short NumberOfHouseholds { get; set; }

        /// <summary>
        /// Gets or sets the key of the organisation record, if any.
        /// </summary>
        public int? OrganisationKey { get; set; }

        /// <summary>
        /// Gets or sets the PO Box number, if any.
        /// </summary>
        /// <remarks>
        /// The PO Box details can occasionally consist of a combination of
        /// numbers and letters.
        /// </remarks>
        [MaxLength(6)]
        public string POBoxNumber { get; set; }

        /// <summary>
        /// Gets or sets the Postcode of the address.
        /// </summary>
        [MaxLength(7)]
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the type of delivery point for the address.
        /// </summary>
        public DeliveryPointType PostcodeType { get; set; }

        /// <summary>
        /// Gets or sets the key of the sub-building name record, if any.
        /// </summary>
        public int? SubBuildingNameKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the thoroughfare descriptor record, if any.
        /// </summary>
        public int? ThoroughfareDescriptorKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the thoroughfare record, if any.
        /// </summary>
        public int? ThoroughfareKey { get; set; }
    }
}
