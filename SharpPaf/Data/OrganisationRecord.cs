namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about the organisation of an address, if any.
    /// </summary>
    public sealed class OrganisationRecord
    {
        /// <summary>
        /// Gets or sets the department in the organisation, if any.
        /// </summary>
        [MaxLength(60)]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the organisation.
        /// </summary>
        [MaxLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of delivery point for the organisation.
        /// </summary>
        public DeliveryPointType PostcodeType { get; set; }
    }
}
