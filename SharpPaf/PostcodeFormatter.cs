namespace SharpPaf
{
    using System.Text;

    /// <summary>
    /// Allows the formatting and validation of a UK postcode.
    /// </summary>
    public sealed class PostcodeFormatter
    {
        private const int MaximumPostcodeLength = 8; // AA11 1AA

        /// <summary>
        /// Formats the postcode with the default options.
        /// </summary>
        /// <param name="postcode">The string to format.</param>
        /// <returns>
        /// A formatted postcode, or null if <c>postcode</c> is null.
        /// </returns>
        public string Format(string postcode)
        {
            return this.Format(postcode, PostcodeOptions.Default);
        }

        /// <summary>
        /// Formats the postcode with the specified options.
        /// </summary>
        /// <param name="postcode">The string to format.</param>
        /// <param name="options">The options to use when formatting.</param>
        /// <returns>
        /// A formatted postcode, or null if <c>postcode</c> is null.
        /// </returns>
        public string Format(string postcode, PostcodeOptions options)
        {
            if (postcode == null)
            {
                return null;
            }

            var sb = new StringBuilder(MaximumPostcodeLength);
            int start = StringUtils.SkipLeadingWhite(postcode);
            int end = StringUtils.SkipTrailingWhite(postcode);
            for (int i = start; i <= end; i++)
            {
                // -1 because we're skipping the space
                if (sb.Length == MaximumPostcodeLength - 1)
                {
                    break;
                }

                AppendChar(sb, postcode[i], options);
            }

            if (sb.Length > 3)
            {
                sb.Insert(sb.Length - 3, " ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified postcode is in the correct format
        /// using the default options.
        /// </summary>
        /// <param name="postcode">The string to validate.</param>
        /// <returns>
        /// true if <c>postcode</c> is in the correct format for a valid
        /// postcode; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method cannot determine whether a postcode is a known valid
        /// one, but can help to detect values which adhere to common rules
        /// that all valid postcodes follow.
        /// </remarks>
        public bool IsValid(string postcode)
        {
            return this.IsValid(postcode, PostcodeOptions.Default);
        }

        /// <summary>
        /// Determines whether the specified postcode is in the correct format
        /// using the specified options.
        /// </summary>
        /// <param name="postcode">The string to validate.</param>
        /// <param name="options">The options to use when validating.</param>
        /// <returns>
        /// true if <c>postcode</c> is in the correct format for a valid
        /// postcode; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method cannot determine whether a postcode is a known valid
        /// one, but can help to detect values which adhere to common rules
        /// that all valid postcodes follow.
        /// </remarks>
        public bool IsValid(string postcode, PostcodeOptions options)
        {
            if (postcode == null)
            {
                return false;
            }

            PostcodeValidator validator = options.HasFlag(PostcodeOptions.Strict) ?
                new PostcodeStrictValidator(options) :
                new PostcodeValidator(options);

            int index = StringUtils.SkipTrailingWhite(postcode);
            index = validator.CheckInwardCode(postcode, index);

            index = StringUtils.SkipWhitespaceBackwards(postcode, index);
            index = validator.CheckOutwardCode(postcode, index);

            return index == StringUtils.SkipLeadingWhite(postcode);
        }

        private static void AppendChar(StringBuilder sb, char value, PostcodeOptions options)
        {
            if (options.HasFlag(PostcodeOptions.SkipInvalidCharacters) &&
                !PostcodeValidator.IsValidCharacter(value))
            {
                return;
            }

            if (options.HasFlag(PostcodeOptions.ChangeCase))
            {
                value = StringUtils.ToUpper(value);
            }

            if (!StringUtils.IsWhitespace(value))
            {
                sb.Append(value);
            }
        }
    }
}
