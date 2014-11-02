namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about a geographical area used for delivery of mail.
    /// </summary>
    /// <remarks>
    /// Since there can be only one locality per Postcode, all addresses in a
    /// Postcode have the same locality.
    /// </remarks>
    public sealed class LocalityRecord
    {
        /// <summary>
        /// Gets or sets the dependent locality information.
        /// </summary>
        [MaxLength(35)]
        public string DependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the double dependent locality information.
        /// </summary>
        /// <remarks>
        /// This can only be present when a dependent locality is present.
        /// </remarks>
        [MaxLength(35)]
        public string DoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the Post Town information.
        /// </summary>
        /// <remarks>
        /// This is required for delivery of mail to a delivery point and may
        /// not necessarily be the nearest town geographically.
        /// </remarks>
        [MaxLength(30)]
        public string PostTown { get; set; }
    }
}
