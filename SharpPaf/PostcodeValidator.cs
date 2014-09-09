namespace SharpPaf
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Performs basic validation on a postcode.
    /// </summary>
    internal class PostcodeValidator
    {
        /// <summary>Protected because PostcodeStrictValidator needs access.</summary>
        protected const int InvalidPostcode = -1;

        /// <summary>Protected because PostcodeStrictValidator needs access.</summary>
        protected readonly bool SkipInvalid;

        /// <summary>
        /// Initializes a new instance of the PostcodeValidator class.
        /// </summary>
        /// <param name="options">The options to use while validating.</param>
        public PostcodeValidator(PostcodeOptions options)
        {
            this.SkipInvalid = options.HasFlag(PostcodeOptions.SkipInvalidCharacters);
        }

        /// <summary>
        /// Gets the index of the start of the inward code.
        /// </summary>
        /// <param name="postcode">The value to check.</param>
        /// <param name="index">The index to the end of the postcode.</param>
        /// <returns>
        /// The start of the inward part of the postcode, or -1 the part is invalid.
        /// </returns>
        public virtual int CheckInwardCode(string postcode, int index)
        {
            var iterator = new ReverseIterator(this.SkipInvalid, postcode, index);

            if (IsLetter(iterator.GetNext()) &&
                IsLetter(iterator.GetNext()) &&
                IsNumber(iterator.GetNext()))
            {
                return iterator.Index;
            }

            return InvalidPostcode;
        }

        /// <summary>
        /// Gets the index of the start of the outward code.
        /// </summary>
        /// <param name="postcode">The value to check.</param>
        /// <param name="index">
        /// The index to the start of the inward part or of the whitespace
        /// between the parts.
        /// </param>
        /// <returns>
        /// The start of the outward part of the postcode, or -1 the part is invalid.
        /// </returns>
        public virtual int CheckOutwardCode(string postcode, int index)
        {
            // Three basic scenarios:
            // + Letter Number Letter (Letter) - AA1A/A1A
            // + Number Number Letter (Letter) - AA11/A11
            // + Number Letter (Letter) - AA1/A1
            var iterator = new ReverseIterator(this.SkipInvalid, postcode, index);
            iterator = CheckOutwardCodeEnd(iterator);
            if (IsLetter(iterator.GetNext()))
            {
                ReverseIterator lookAhead = iterator;
                return IsLetter(lookAhead.GetNext()) ? lookAhead.Index : iterator.Index;
            }

            return InvalidPostcode;
        }

        /// <summary>
        /// Determines whether the specified value is a valid postcode character.
        /// </summary>
        /// <param name="value">The character to test.</param>
        /// <returns>
        /// true if the character could appear in a postcode; otherwise, false.
        /// </returns>
        internal static bool IsValidCharacter(char value)
        {
            return IsNumber(value) || IsLetter(value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "This method is only visible to PostcodeStrictValidator")]
        protected static bool IsLetter(char value)
        {
            return ((value >= 'A') & (value <= 'Z')) ||
                   ((value >= 'a') & (value <= 'z'));
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "This method is only visible to PostcodeStrictValidator")]
        protected static bool IsNumber(char value)
        {
            return (value >= '0') & (value <= '9');
        }

        private static ReverseIterator CheckOutwardCodeEnd(ReverseIterator iterator)
        {
            // This checks for outward parts ending in 1A, 11 or 1
            char endChar = iterator.GetNext();
            if (IsLetter(endChar))
            {
                if (!IsNumber(iterator.GetNext()))
                {
                    return ReverseIterator.Invalid;
                }
            }
            else if (IsNumber(endChar))
            {
                ReverseIterator lookAhead = iterator;
                if (IsNumber(lookAhead.GetNext()))
                {
                    return lookAhead;
                }
            }
            else
            {
                return ReverseIterator.Invalid;
            }

            return iterator;
        }

        // Note: This must be a struct for easy copying when looking ahead for
        // a value.
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "This structure is only visible to PostcodeStrictValidator")]
        protected struct ReverseIterator
        {
            public static readonly ReverseIterator Invalid = new ReverseIterator(false, null, -1);

            private readonly string postcode;
            private readonly bool skipInvalid;
            private int index;

            public ReverseIterator(bool skipInvalid, string postcode, int index)
            {
                this.postcode = postcode;
                this.skipInvalid = skipInvalid;
                this.index = index + 1; // We always subtract one from the index when calling GetNext
            }

            public int Index
            {
                get { return this.index; }
            }

            public char GetNext()
            {
                while (this.index > 0)
                {
                    this.index--;
                    char c = this.postcode[this.index];
                    if ((this.skipInvalid && !IsValidCharacter(c)) ||
                        StringUtils.IsWhitespace(c))
                    {
                        continue;
                    }

                    return c;
                }

                this.index = -1;
                return '\0';
            }
        }
    }
}
