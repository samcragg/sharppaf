namespace Example
{
    using System;

    /// <summary>
    /// Determines how data will be formatted by the MemoryRepository.
    /// </summary>
    [Flags]
    internal enum FormatOptions
    {
        /// <summary>
        /// No formatting is done.
        /// </summary>
        None = 0,

        /// <summary>
        /// Address information will be converted to Title Case.
        /// </summary>
        TitleCase = 0x1,

        /// <summary>
        /// The postcode will be formatted to include a space.
        /// </summary>
        Postcode = 0x2
    }
}
