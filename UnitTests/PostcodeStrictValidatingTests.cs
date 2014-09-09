namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class PostcodeStrictValidatingTests
    {
        private const PostcodeOptions DefaultStrict = PostcodeOptions.Default | PostcodeOptions.Strict;
        private PostcodeFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new PostcodeFormatter();
        }

        [TestCase("AA1A 1AA", Result = true)]
        [TestCase("AA11 1AA", Result = true)]
        [TestCase("AA1 1AA", Result = true)]
        [TestCase("A1A 1AA", Result = true)]
        [TestCase("A11 1AA", Result = true)]
        [TestCase("A1 1AA", Result = true)]
        public bool ShouldReturnTrueForValidPostcodes(string input)
        {
            return this.formatter.IsValid(input, DefaultStrict);
        }

        [TestCase("AA1A1AA", Result = true)]
        [TestCase("AA111AA", Result = true)]
        [TestCase("AA11AA", Result = true)]
        [TestCase("A1A1AA", Result = true)]
        [TestCase("A111AA", Result = true)]
        [TestCase("A11AA", Result = true)]
        public bool ShouldReturnTrueForValidPostcodesWithoutSpaces(string input)
        {
            return this.formatter.IsValid(input, DefaultStrict);
        }

        [TestCase("AB1 23C", Result = false)]
        [TestCase("ABC 1DE", Result = false)]
        [TestCase("AB CDE", Result = false)]
        [TestCase("12 345", Result = false)]
        [TestCase("123 4DE", Result = false)]
        public bool ShouldReturnFalseForInvalidPostcodes(string input)
        {
            return this.formatter.IsValid(input, DefaultStrict);
        }

        [TestCase("QA1 1AA", Result = false)]
        [TestCase("AI1 1AA", Result = false)]
        [TestCase("A1I 1AA", Result = false)]
        [TestCase("AA1I 1AA", Result = false)]
        [TestCase("AA1 1IA", Result = false)]
        [TestCase("AA1 1AI", Result = false)]
        public bool ShouldReturnFalseForPostcodesWithInvalidLetters(string input)
        {
            return this.formatter.IsValid(input, DefaultStrict);
        }

        [Test]
        public void ShouldIgnoreTheCaseByDefault()
        {
            bool result = this.formatter.IsValid("aa1a 1aa", DefaultStrict);
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipLeadingWhitespace()
        {
            bool result = this.formatter.IsValid("\r\n \tA1 1AA", DefaultStrict);
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipTrailingWhitespace()
        {
            bool result = this.formatter.IsValid("A1 1AA \t\r\n", DefaultStrict);
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipNonLettersOrNumbersCharacters()
        {
            bool result = this.formatter.IsValid("A1_-1AA", DefaultStrict);
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldReturnFalseIfTooManyCharacters()
        {
            bool result = this.formatter.IsValid("AAAA11 1AA", DefaultStrict);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldReturnFalseIfSkipInvalidCharactersOptionIsNotSet()
        {
            bool result = this.formatter.IsValid("A1_1AA", PostcodeOptions.Strict | PostcodeOptions.None);
            Assert.That(result, Is.False);
        }
    }
}
