namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class POBoxNumberTests
    {
        private AddressFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new AddressFormatter();
        }

        [Test]
        public void ShouldPrefixThePOBoxNumberWithTheCorrectText()
        {
            // Table 6: PO Box address without a name
            var data = new PafData
            {
                POBoxNumber = "22",
                PostTown = "FAREHAM",
                Postcode = "PO14 3XH"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("PO Box 22").IgnoreCase);
        }

        [Test]
        public void ShouldOutputThePOBoxAfterTheOrganisation()
        {
            // Table 6: PO Box address without a name
            var data = new PafData
            {
                OrganisationName = "ROBINSONS",
                POBoxNumber = "61",
                PostTown = "FAREHAM",
                Postcode = "PO14 1UX"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[1], Is.EqualTo("PO Box 61").IgnoreCase);
        }
    }
}
