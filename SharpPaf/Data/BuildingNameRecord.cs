namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about the building name of a premises, if any.
    /// </summary>
    public sealed class BuildingNameRecord
    {
        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the building.
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
