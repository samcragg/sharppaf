namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class TitleCaseCoverterTests
    {
        private TitleCaseConverter converter;

        [SetUp]
        public void SetUp()
        {
            this.converter = new TitleCaseConverter();
        }

        [Test]
        public void ShouldReturnNullIfPassedNull()
        {
            string result = this.converter.ToTitleCase(null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ShouldCapitaliseTheFirstLetterOfEachWord()
        {
            string result = this.converter.ToTitleCase("ONE TWO THREE");

            Assert.That(result, Is.EqualTo("One Two Three"));
        }

        [Test]
        public void ShouldHandleThePrefixMc()
        {
            string result = this.converter.ToTitleCase("MCDONALD");

            Assert.That(result, Is.EqualTo("McDonald"));
        }

        [Test]
        public void ShouldCapitaliseTheWordAfterAFullStop()
        {
            string result = this.converter.ToTitleCase("ONE.TWO");

            Assert.That(result, Is.EqualTo("One.Two"));
        }

        [TestCase("PLC", Result = "Plc")]
        [TestCase("(PLC)", Result = "(Plc)")]
        public string ShouldCapitaliseTheFirstLetterOfTheValue(string input)
        {
            return this.converter.ToTitleCase(input);
        }

        [TestCase("LXVI", Result = "LXVI")]
        [TestCase("XIX", Result = "XIX")]
        [TestCase("CLX", Result = "Clx")]
        public string ShouldCapitaliseRomanNumeralsUpTo100(string input)
        {
            return this.converter.ToTitleCase(input);
        }

        [TestCase("ONE-TWO-THREE", Result = "One-Two-Three")]
        [TestCase("ONE-IN-THREE", Result = "One-in-Three")]
        [TestCase("ONE-UK-THREE", Result = "One-UK-Three")]
        public string ShouldCorrectlyCapitaliseHyphenatedWords(string input)
        {
            return this.converter.ToTitleCase(input);
        }

        [TestCase("ONE'TWO", Result = "One'Two")]
        [TestCase("ONE'S TWO", Result = "One's Two")]
        [TestCase("ONE TWO'S", Result = "One Two's")]
        [TestCase("ONE TWO'", Result = "One Two'")]
        public string ShouldCorrectlyCapitaliseApostrophies(string input)
        {
            return this.converter.ToTitleCase(input);
        }
    }
}
