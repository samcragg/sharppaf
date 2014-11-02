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
        /// Converts the specified ASCII character to its lowercase version.
        /// </summary>
        /// <param name="value">The character to convert.</param>
        /// <returns>The lowercased value.</returns>
        public static char ToLower(char value)
        {
            unchecked
            {
                ushort delta = (ushort)(value - 'A');
                if (delta <= (ushort)('Z' - 'A'))
                {
                    return (char)('a' + delta);
                }

                return value;
            }
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

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed
        /// integer equivalent.
        /// </summary>
        /// <param name="buffer">
        /// Contains the number to convert.
        /// </param>
        /// <param name="start">
        /// The index in the buffer where the number start.
        /// </param>
        /// <param name="length">
        /// The amount of digits to extract from the buffer.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit signed integer value
        /// equivalent to the number contained in buffer, if the conversion
        /// succeeded, or zero if the conversion failed.
        /// </param>
        /// <returns>
        /// true if buffer was converted successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method will not parse negative numbers.
        /// </remarks>
        public static bool TryParseInt32(byte[] buffer, int start, int length, out int result)
        {
            int converted = 0;
            int digits = 0;
            for (int i = start; i < buffer.Length; i++)
            {
                if (digits++ == length)
                {
                    break;
                }

                ushort digit = (ushort)(buffer[i] - '0');
                if (digit > '9')
                {
                    result = 0;
                    return false;
                }

                converted = (converted * 10) + digit;
            }

            result = converted;
            return true;
        }
    }
}
