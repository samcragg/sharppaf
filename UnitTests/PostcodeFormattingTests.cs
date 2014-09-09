namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class PostcodeFormattingTests
    {
        private PostcodeFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            formatter = new PostcodeFormatter();
        }

        [Test]
        public void ShouldReturnNullForNullParameters()
        {
            string result = this.formatter.Format(null);
            Assert.That(result, Is.Null);
        }

        [TestCase("AB1C2DE", Result = "AB1C 2DE")]
        [TestCase("AB123CD", Result = "AB12 3CD")]
        [TestCase("AB12CD", Result = "AB1 2CD")]
        [TestCase("A1B2CD", Result = "A1B 2CD")]
        [TestCase("A123BC", Result = "A12 3BC")]
        [TestCase("A12BC", Result = "A1 2BC")]
        public string ShouldInsertASpaceInTheCorrectPosition(string input)
        {
            return this.formatter.Format(input);
        }

        [Test]
        public void ShouldChangeTheCaseByDefault()
        {
            string result = this.formatter.Format("ab1c 2yz");
            Assert.That(result, Is.EqualTo("AB1C 2YZ"));
        }

        [Test]
        public void ShouldSkipLeadingWhitespace()
        {
            string result = this.formatter.Format("\r\n \tA1 2BC");
            Assert.That(result, Is.EqualTo("A1 2BC"));
        }

        [Test]
        public void ShouldSkipTrailingWhitespace()
        {
            string result = this.formatter.Format("A1 2BC \t\r\n");
            Assert.That(result, Is.EqualTo("A1 2BC"));
        }

        [Test]
        public void ShouldSkipNonLettersOrNumbersCharacters()
        {
            string result = this.formatter.Format("A1_-2BC");
            Assert.That(result, Is.EqualTo("A1 2BC"));
        }

        [Test]
        public void ShouldTruncateExtraCharacters()
        {
            string result = this.formatter.Format("AB123CDEFG");
            Assert.That(result, Is.EqualTo("AB12 3CD"));
        }

        [Test]
        public void ShouldNotChangeCaseIfOptionIsNotSet()
        {
            string result = this.formatter.Format("a12bC", PostcodeOptions.None);
            Assert.That(result, Is.EqualTo("a1 2bC"));
        }

        [Test]
        public void ShouldNotSkipNonLettersOrNumbersIfOptionIsNotSet()
        {
            string result = this.formatter.Format("A1_2BC", PostcodeOptions.None);
            Assert.That(result, Is.EqualTo("A1_ 2BC"));
        }
    }
}
