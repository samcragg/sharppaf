namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class PostcodeBasicValidatingTests
    {
        private PostcodeFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new PostcodeFormatter();
        }

        [Test]
        public void ShouldReturnFalseForNullParameters()
        {
            bool result = this.formatter.IsValid(null);
            Assert.That(result, Is.False);
        }

        [TestCase("AB1C 2DE", Result = true)]
        [TestCase("AB12 3CD", Result = true)]
        [TestCase("AB1 2CD", Result = true)]
        [TestCase("A1B 2CD", Result = true)]
        [TestCase("A12 3BC", Result = true)]
        [TestCase("A1 2BC", Result = true)]
        public bool ShouldReturnTrueForValidPostcodes(string input)
        {
            return this.formatter.IsValid(input);
        }

        [TestCase("AB1C2DE", Result = true)]
        [TestCase("AB123CD", Result = true)]
        [TestCase("AB12CD", Result = true)]
        [TestCase("A1B2CD", Result = true)]
        [TestCase("A123BC", Result = true)]
        [TestCase("A12BC", Result = true)]
        public bool ShouldReturnTrueForValidPostcodesWithoutSpaces(string input)
        {
            return this.formatter.IsValid(input);
        }

        [TestCase("AB1 23C", Result = false)]
        [TestCase("ABC 1DE", Result = false)]
        [TestCase("AB CDE", Result = false)]
        [TestCase("12 345", Result = false)]
        [TestCase("123 4DE", Result = false)]
        public bool ShouldReturnFalseForInvalidPostcodes(string input)
        {
            return this.formatter.IsValid(input);
        }

        [Test]
        public void ShouldAllowPostcodesWithInvalidLetters()
        {
            // Postcodes cannot start with Q and cannot have O as a letter in
            // the inward part.
            bool result = this.formatter.IsValid("Q1 2OO");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldIgnoreTheCaseByDefault()
        {
            bool result = this.formatter.IsValid("ab1c 2yz");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipLeadingWhitespace()
        {
            bool result = this.formatter.IsValid("\r\n \tA1 2BC");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipTrailingWhitespace()
        {
            bool result = this.formatter.IsValid("A1 2BC \t\r\n");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldSkipNonLettersOrNumbersCharacters()
        {
            bool result = this.formatter.IsValid("A1_-2BC");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldReturnFalseIfTooManyCharacters()
        {
            bool result = this.formatter.IsValid("XXAB12 3CD");
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldReturnFalseIfSkipInvalidCharactersOptionIsNotSet()
        {
            bool result = this.formatter.IsValid("A1_2BC", PostcodeOptions.None);
            Assert.That(result, Is.False);
        }
    }
}
