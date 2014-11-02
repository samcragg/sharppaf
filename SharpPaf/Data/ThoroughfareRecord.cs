namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about the thoroughfare of an address, if any.
    /// </summary>
    /// <remarks>
    /// For some addresses there may be no thoroughfare information present at
    /// all. This usually occurs in rural areas, when the locality information
    /// identifies the location of the address.
    /// </remarks>
    public sealed class ThoroughfareRecord
    {
        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the thoroughfare.
        /// </summary>
        /// <remarks>
        /// This is usually the first part of the text and can be followed by a
        /// thoroughfare descriptor.
        /// </remarks>
        [MaxLength(60)]
        public string Name { get; set; }
    }
}
