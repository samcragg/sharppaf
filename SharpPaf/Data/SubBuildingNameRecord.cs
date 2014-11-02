namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about the sub-building name of a premises, if any.
    /// </summary>
    public sealed class SubBuildingNameRecord
    {
        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub-building.
        /// </summary>
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
