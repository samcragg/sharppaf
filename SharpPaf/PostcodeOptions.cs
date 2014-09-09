namespace SharpPaf
{
    using System;

    /// <summary>
    /// Determines the formatting/validating options of postcodes.
    /// </summary>
    [Flags]
    public enum PostcodeOptions
    {
        /// <summary>
        /// Indicates that no option elements, such as changing case, are used
        /// during formatting or validating.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the string should be converted to uppercase when
        /// formatting the postcode.
        /// </summary>
        ChangeCase = 1,

        /// <summary>
        /// Indicates that any character non-alphanumeric ASCII character will
        /// be skipped during formatting/validation.
        /// </summary>
        SkipInvalidCharacters = 2,

        /// <summary>
        /// Indicates that additional checks are performed during validation to
        /// detect postcodes that have invalid letters (such as starting with a
        /// Q, V or X).
        /// </summary>
        Strict = 4,

        /// <summary>
        /// Indicates the default options for formatting/validating the string.
        /// </summary>
        Default = ChangeCase | SkipInvalidCharacters,
    }
}
