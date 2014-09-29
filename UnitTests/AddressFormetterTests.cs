namespace UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class AddressFormetterTests
    {
        private AddressFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new AddressFormatter();
        }

        [Test]
        public void ShouldThrowArgumentNullException()
        {
            Assert.That(() => this.formatter.Format(null),
                        Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ShouldConcatenateDescriptorWithThoroughfare()
        {
            var data = new PafData()
            {
                ThoroughfareName = "ACACIA",
                ThoroughfareDescriptor = "AVENUE",
                PostTown = "ABINGDON",
                Postcode = "OX14 4PG"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Acacia Avenue").IgnoreCase);
        }

        [Test]
        public void ShouldConcatenateDescriptorWithDependentThoroughfare()
        {
            var data = new PafData()
            {
                DependentThoroughfareName = "ACACIA",
                DependentThoroughfareDescriptor = "AVENUE",
                PostTown = "ABINGDON",
                Postcode = "OX14 4PG"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Acacia Avenue").IgnoreCase);
        }
    }
}
