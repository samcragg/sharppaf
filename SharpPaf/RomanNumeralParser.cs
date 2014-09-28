namespace SharpPaf
{
    /// <summary>
    /// Allows matching of Roman numerals up to 99.
    /// </summary>
    internal sealed class RomanNumeralParser
    {
        private readonly IMatcher matcher;

        /// <summary>
        /// Initializes a new instance of the RomanNumeralParser class.
        /// </summary>
        public RomanNumeralParser()
        {
            // This is the regex we're matching (from http://stackoverflow.com/a/267405):
            //     (XC|XL|L?X{0,3})(IX|IV|V?I{0,3})
            //
            // Changing the optionals to a range and grouping things gives:
            //     (XC | XL | (L{0,1} X{0,3})) (IX | IV | (V{0,1} I{0,3})))
            IMatcher tens = new MatchAny(new MatchTwoChars("XC"), new MatchTwoChars("XL"), new MatchBoth(new MatchZeroOrMore('L', 1), new MatchZeroOrMore('X', 3)));
            IMatcher ones = new MatchAny(new MatchTwoChars("IX"), new MatchTwoChars("IV"), new MatchBoth(new MatchZeroOrMore('V', 1), new MatchZeroOrMore('I', 3)));
            this.matcher = new MatchBoth(tens, ones);
        }

        private interface IMatcher
        {
            int Match(string input, int index);
        }

        /// <summary>
        /// Determines whether the specified string contains a Roman numeral in
        /// the specified range.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="start">The index of the first character to check.</param>
        /// <param name="end">The index of the last character to check.</param>
        /// <returns>
        /// true if the specified range is a valid Roman numeral; otherwise, false.
        /// </returns>
        /// <remarks>
        /// The string must be uppercase for it to match.
        /// </remarks>
        public bool IsMatch(string value, int start, int end)
        {
            return this.matcher.Match(value, start) == end;
        }

        private class MatchAny : IMatcher
        {
            private readonly IMatcher[] matchers;

            public MatchAny(params IMatcher[] matchers)
            {
                this.matchers = matchers;
            }

            public int Match(string input, int index)
            {
                for (int i = 0; i < this.matchers.Length; i++)
                {
                    int newIndex = this.matchers[i].Match(input, index);
                    if (newIndex != index)
                    {
                        return newIndex;
                    }
                }

                return index;
            }
        }

        private class MatchBoth : IMatcher
        {
            private readonly IMatcher first;
            private readonly IMatcher second;

            public MatchBoth(IMatcher first, IMatcher second)
            {
                this.first = first;
                this.second = second;
            }

            public int Match(string input, int index)
            {
                index = this.first.Match(input, index);
                return this.second.Match(input, index);
            }
        }

        private class MatchTwoChars : IMatcher
        {
            private readonly char first;
            private readonly char second;

            public MatchTwoChars(string value)
            {
                this.first = value[0];
                this.second = value[1];
            }

            public int Match(string input, int index)
            {
                if (index > input.Length - 2)
                {
                    return index;
                }

                if ((input[index] != this.first) | (input[index + 1] != this.second))
                {
                    return index;
                }

                return index + 2;
            }
        }

        private class MatchZeroOrMore : IMatcher
        {
            private readonly char match;
            private readonly int max;

            public MatchZeroOrMore(char value, int maximum)
            {
                this.match = value;
                this.max = maximum;
            }

            public int Match(string input, int index)
            {
                for (int i = 0; i < this.max; i++)
                {
                    if ((index >= input.Length) || (input[index] != this.match))
                    {
                        break;
                    }

                    index++;
                }

                return index;
            }
        }
    }
}
