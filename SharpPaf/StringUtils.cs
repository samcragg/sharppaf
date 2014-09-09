namespace SharpPaf
{
    /// <summary>
    /// Provides utilities for working with ASCII strings.
    /// </summary>
    internal static class StringUtils
    {
        /// <summary>
        /// Determines whether the specified character represents whitespace.
        /// </summary>
        /// <param name="value">The character to test.</param>
        /// <returns>true if it is whitespace; otherwise, false.</returns>
        public static bool IsWhitespace(char value)
        {
            // These are the values used by System.Globalization.NumberStyles:
            //     U+0009, U+000A, U+000B, U+000C, U+000D, and U+0020.
            unchecked
            {
                uint delta = (uint)(value - '\x09');
                return (delta < 5) || (delta == (0x20 - 0x09));
            }
        }

        /// <summary>
        /// Find the index of the first non-whitespace character from the start
        /// of the string.
        /// </summary>
        /// <param name="value">The string to check, must not be null.</param>
        /// <returns>The index of the first non-whitespace character.</returns>
        public static int SkipLeadingWhite(string value)
        {
            int index = 0;
            for (; index < value.Length; index++)
            {
                if (!IsWhitespace(value[index]))
                {
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Find the index of the last non-whitespace character from the start
        /// of the string.
        /// </summary>
        /// <param name="value">The string to check, must not be null.</param>
        /// <returns>The index of the last non-whitespace character.</returns>
        public static int SkipTrailingWhite(string value)
        {
            return SkipWhitespaceBackwards(value, value.Length);
        }

        /// <summary>
        /// Find the index of the first non-whitespace character before the
        /// specified index.
        /// </summary>
        /// <param name="value">The string to check, must not be null.</param>
        /// <param name="end">The index to search backwards from.</param>
        /// <returns>The index of the first non-whitespace character.</returns>
        public static int SkipWhitespaceBackwards(string value, int end)
        {
            int index = end - 1;
            for (; index > 0; index--)
            {
                if (!IsWhitespace(value[index]))
                {
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Converts an ASCII value to its uppercase equivalent.
        /// </summary>
        /// <param name="value">The character to convert.</param>
        /// <returns>An uppercase version of <c>value</c>.</returns>
        public static char ToUpper(char value)
        {
            unchecked
            {
                uint delta = (uint)(value - 'a');
                if (delta <= ('z' - 'a'))
                {
                    return (char)('A' + delta);
                }

                return value;
            }
        }
    }
}
