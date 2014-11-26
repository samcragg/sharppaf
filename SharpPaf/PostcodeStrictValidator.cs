namespace SharpPaf
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Performs more rigorous validation of a postcode.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly",
            Justification = "This is the only derived class of PostcodeValidator")]
    internal sealed class PostcodeStrictValidator : PostcodeValidator
    {
        /// <summary>
        /// Initializes a new instance of the PostcodeStrictValidator class.
        /// </summary>
        /// <param name="options">The options to use while validating.</param>
        public PostcodeStrictValidator(PostcodeOptions options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public override int CheckInwardCode(string postcode, int index)
        {
            var iterator = new ReverseIterator(this.SkipInvalid, postcode, index);

            if (IsValidInwardAlpha(iterator.GetNext()) &&
                IsValidInwardAlpha(iterator.GetNext()) &&
                IsNumber(iterator.GetNext()))
            {
                return iterator.Index;
            }

            return InvalidPostcode;
        }

        /// <inheritdoc />
        public override int CheckOutwardCode(string postcode, int index)
        {
            // Three basic scenarios:
            // + Letter Number Letter (Letter) - AA1A/A1A
            // + Number Number Letter (Letter) - AA11/A11
            // + Number Letter (Letter) - AA1/A1
            var iterator = new ReverseIterator(this.SkipInvalid, postcode, index);
            var stack = new CharStack(3);
            iterator = CheckOutwardCodeEnd(iterator, ref stack);

            char current = iterator.GetNext();
            if (IsLetter(current))
            {
                stack.Push(current);

                int start;
                ReverseIterator lookAhead = iterator;
                current = lookAhead.GetNext();
                if (IsLetter(current))
                {
                    stack.Push(current);
                    start = lookAhead.Index;
                }
                else
                {
                    start = iterator.Index;
                }

                if (AreValidOutwardLetters(stack))
                {
                    return start;
                }
            }

            return InvalidPostcode;
        }

        private static bool AreValidOutwardLetters(CharStack stack)
        {
            // All post codes start with a letter so there will always be at
            // least one letter in the stack for this method to get called, so
            // no need to check if the stack is empty here.
            if (!IsValidFirstAlpha(stack.Pop()))
            {
                return false;
            }

            if (!stack.Empty && !IsValidSecondAlpha(stack.Pop()))
            {
                return false;
            }

            return stack.Empty || IsValidThirdAlpha(stack.Pop());
        }

        private static ReverseIterator CheckOutwardCodeEnd(ReverseIterator iterator, ref CharStack stack)
        {
            // This checks for outward parts ending in 1A, 11 or 1
            char endChar = iterator.GetNext();
            if (IsLetter(endChar))
            {
                if (!IsNumber(iterator.GetNext()))
                {
                    return ReverseIterator.Invalid;
                }

                stack.Push(endChar);
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

        private static bool IsValidFirstAlpha(char value)
        {
            switch (StringUtils.ToUpper(value))
            {
                case 'Q':
                case 'V':
                case 'X':
                    return false;

                default:
                    return true;
            }
        }

        private static bool IsValidInwardAlpha(char value)
        {
            // Unlike the other IsValid helpers in this class, we're not sure
            // we've even been passed a letter in this method so we need to
            // check that first before the switch on the disallowed letters.
            value = StringUtils.ToUpper(value);
            if ((value < 'A') | (value > 'Z'))
            {
                return false;
            }

            switch (value)
            {
                case 'C':
                case 'I':
                case 'K':
                case 'M':
                case 'V':
                    return false;

                default:
                    return true;
            }
        }

        private static bool IsValidSecondAlpha(char value)
        {
            switch (StringUtils.ToUpper(value))
            {
                case 'I':
                case 'Z':
                    return false;

                default:
                    return true;
            }
        }

        private static bool IsValidThirdAlpha(char value)
        {
            switch (StringUtils.ToUpper(value))
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'J':
                case 'K':
                case 'P':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                    return true;

                default:
                    return false;
            }
        }

        private struct CharStack
        {
            private readonly char[] buffer;
            private int length;

            public CharStack(int capacity)
            {
                this.buffer = new char[capacity];
                this.length = 0;
            }

            public bool Empty
            {
                get { return this.length == 0; }
            }

            public char Pop()
            {
                this.length--;
                return this.buffer[this.length];
            }

            public void Push(char value)
            {
                this.buffer[this.length] = value;
                this.length++;
            }
        }
    }
}
