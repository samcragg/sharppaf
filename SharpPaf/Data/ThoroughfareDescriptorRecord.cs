namespace SharpPaf.Data
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides information about the descriptor of a thoroughfare.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The descriptor part of the thoroughfare applies to the last word in the
    /// text (such as Avenue, Street or Centre), but doesn't include North,
    /// South, East or West.
    /// </para>
    /// <para>
    /// The descriptor won't be split from the text if this would result in the
    /// single word 'The' being held as the thoroughfare.
    /// </para>
    /// <para>
    /// Although mentioned in the mainfile description, the approved
    /// abbreviation is always space filled and has therefore been excluded
    /// from the record.
    /// </para>
    /// </remarks>
    public sealed class ThoroughfareDescriptorRecord
    {
        /// <summary>
        /// Gets or sets the descriptor text.
        /// </summary>
        [MaxLength(20)]
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        [Key]
        public int Key { get; set; }
    }
}
