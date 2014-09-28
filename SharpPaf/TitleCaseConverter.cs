namespace SharpPaf
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows the conversion from uppercase to title case, where each word
    /// starts with a capital letter.
    /// </summary>
    /// <remarks>
    /// This class has been tweaked to match the output of the CSV address
    /// format, e.g. <c>MCDONALD</c> would become <c>McDonald</c> and <c>UK</c>
    /// would remain unchanged.
    /// </remarks>
    public sealed class TitleCaseConverter
    {
        private readonly Dictionary<string, string> exceptionWords = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "ACCA", "ACCA" },
            { "AM", "AM" },
            { "BBC", "BBC" },
            { "BMW", "BMW" },
            { "BP", "BP" },
            { "CP", "CP" },
            { "DIY", "DIY" },
            { "GB", "GB" },
            { "GMBH", "GmbH" },
            { "LLP", "LLP" },
            { "MP", "MP" },
            { "NHS", "NHS" },
            { "PC", "PC" },
            { "PCT", "PCT" },
            { "PLC", "plc" },
            { "PM", "PM" },
            { "RAF", "RAF" },
            { "RC", "RC" },
            { "TSB", "TSB" },
            { "TV", "TV" },
            { "UK", "UK" },
            { "YMCA", "YMCA" }
        };

        private readonly HashSet<string> lowercaseWhenHyphenated = new HashSet<string>(StringComparer.Ordinal)
        {
            "AND",
            "AT",
            "BUT",
            "BY",
            "CUM",
            "FOR",
            "IN",
            "LE",
            "OF",
            "ON",
            "OR",
            "PLC",
            "SO",
            "THE",
            "TO",
            "UP",
            "Y",
            "YET",
        };

        private readonly RomanNumeralParser romanNumeral = new RomanNumeralParser();

        /// <summary>
        /// Converts the specified uppercased value into title case.
        /// </summary>
        /// <param name="value"
        /// >The value to convert, which is all in uppercase.
        /// </param>
        /// <returns>
        /// The specified value in title case, or null if <c>value</c> is null.
        /// </returns>
        /// <remarks>
        /// The input string is assumed to be in uppercase; if it contains any
        /// lowercase letters then these words may not be converted correctly
        /// (e.g. <c>NhS</c> might incorrectly be converted to <c>Nhs</c>).
        /// </remarks>
        public string ToTitleCase(string value)
        {
            if (value == null)
            {
                return null;
            }

            char[] converted = new char[value.Length];
            bool uppercaseNext = true;
            int start = 0;
            for (int i = 0; i < converted.Length; i++)
            {
                if (uppercaseNext)
                {
                    converted[i] = StringUtils.ToUpper(value[i]);
                }
                else
                {
                    converted[i] = StringUtils.ToLower(value[i]);
                }

                uppercaseNext = this.NextShouldBeUppercase(value, converted, i, ref start);
            }

            this.CheckForExceptionWords(value, converted, start, value.Length);
            EnsureStartsWithACapital(converted);
            return new string(converted);
        }

        private static void EnsureStartsWithACapital(char[] value)
        {
            if (value.Length > 1)
            {
                if (value[0] == '(')
                {
                    value[1] = StringUtils.ToUpper(value[1]);
                }
                else
                {
                    value[0] = StringUtils.ToUpper(value[0]);
                }
            }
        }

        private static bool IsEndOfWord(string value, int index)
        {
            if (index > value.Length)
            {
                return false;
            }
            else if (index == value.Length)
            {
                return true;
            }
            else
            {
                char c = value[index];
                return (c == ' ') | (c == '-') | (c == ')'); // Avoid branching by using bitwise-or
            }
        }

        private void CheckForExceptionWords(string value, char[] converted, int start, int end)
        {
            string word = value.Substring(start, end - start);
            string replacement;
            if (this.exceptionWords.TryGetValue(word, out replacement))
            {
                for (int i = 0; i < replacement.Length; i++)
                {
                    converted[start + i] = replacement[i];
                }
            }
            else if (this.romanNumeral.IsMatch(value, start, end))
            {
                for (int i = start; i < end; i++)
                {
                    converted[i] = StringUtils.ToUpper(converted[i]);
                }
            }
        }

        private void CheckForHyphenatedWords(string value, char[] converted, int start, int end)
        {
            // When this method is called we know that the end points to a
            // hyphen, however, we don't know if the word also starts with one.
            if ((start != 0) && (value[start - 1] == '-'))
            {
                string word = value.Substring(start, end - start);
                if (this.lowercaseWhenHyphenated.Contains(word))
                {
                    converted[start] = StringUtils.ToLower(converted[start]);
                    return;
                }
            }

            this.CheckForExceptionWords(value, converted, start, end);
        }

        private bool NextShouldBeUppercase(string value, char[] converted, int index, ref int start)
        {
            switch (value[index])
            {
                case 'C':
                    // Did we capitalize an M before this one (e.g. McDonald)?
                    return (index != 0) && (converted[index - 1] == 'M');

                case '-':
                    this.CheckForHyphenatedWords(value, converted, start, index);
                    start = index + 1; // Skip the hyphen
                    return true;

                case '(':
                case ')':
                case ' ':
                    this.CheckForExceptionWords(value, converted, start, index);
                    start = index + 1; // Skip the separator
                    return true;

                case '&':
                case '.':
                    // Don't start a new word just yet
                    return true;

                case '\'':
                    return !IsEndOfWord(value, index + 2);

                default:
                    return false;
            }
        }
    }
}
