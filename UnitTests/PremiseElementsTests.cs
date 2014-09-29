namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class PremiseElementsTests
    {
        private AddressFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new AddressFormatter();
        }

        [Test]
        public void ShouldOutputTheOrganisationNameFirst()
        {
            // Table 25: Premises elements Rule 1: Organisation Name
            var data = new PafData()
            {
                OrganisationName = "LEDA ENGINEERING LTD",
                DependentLocality = "APPLEFORD",
                PostTown = "ABINGDON",
                Postcode = "OX14 4PG"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Leda Engineering Ltd").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheDepartmentNameAfterTheOrganisation()
        {
            var data = new PafData()
            {
                OrganisationName = "SOUTH LANARKSHIRE COUNCIL",
                DepartmentName = "HEAD START",
                PostTown = "GLASGOW",
                Postcode = "G72 0UP"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[1], Is.EqualTo("Head Start").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheBuildingNumberWithTheFirstThoroughfare()
        {
            // Table 26: Premises elements Rule 2: Building Number only
            var data = new PafData()
            {
                BuildingNumber = "1",
                ThoroughfareName = "ACACIA AVENUE",
                PostTown = "ABINGDON",
                Postcode = "OX14 4PG"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("1 Acacia Avenue").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheBuildingNameBeforeThoroughfare()
        {
            // Table 27b: Premises elements Rule 3: Building Name only
            var data = new PafData()
            {
                BuildingName = "THE MANOR",
                ThoroughfareName = "UPPER HILL",
                PostTown = "HORLEY",
                Postcode = "RH6 OHP"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("The Manor").IgnoreCase);
        }

        [Test]
        public void ShouldOutputBuildingNameBeforeTheBuildingNumber()
        {
            // Table 28: Premises elements Rule 4: Building Name and Building
            var data = new PafData()
            {
                BuildingName = "VICTORIA HOUSE",
                BuildingNumber = "15",
                ThoroughfareName = "THE STREET",
                PostTown = "CHRISTCHURCH",
                Postcode = "BH23 6AA"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Victoria House").IgnoreCase);
            Assert.That(lines[1], Is.EqualTo("15 The Street").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheSubBuildingNameBeforeTheBuildingNumber()
        {
            // Table 29a: Premises elements Rule 5: Sub Building Name and Building
            var data = new PafData()
            {
                SubBuildingName = "FLAT 1",
                BuildingNumber = "12",
                ThoroughfareName = "LIME TREE AVENUE",
                PostTown = "BRISTOL",
                Postcode = "BS8 4AB"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Flat 1").IgnoreCase);
        }

        // TODO: Concatenation??

        [Test]
        public void ShouldOutputTheSubBuildingNameBeforeTheBuildingName()
        {
            // Table 31b: Building Name
            var data = new PafData()
            {
                SubBuildingName = "STABLES FLAT",
                BuildingName = "THE MANOR",
                ThoroughfareName = "UPPER HILL",
                PostTown = "HORLEY",
                Postcode = "RH6 OHP"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Stables Flat").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheSubBuildingAndBuildingNameBeforeTheBuildingNumber()
        {
            // Table 32b: Premises elements Rule 7: Sub Building Name, Building
            // Name and Building Number
            var data = new PafData()
            {
                SubBuildingName = "BASEMENT FLAT",
                BuildingName = "VICTORIA HOUSE",
                BuildingNumber = "15",
                ThoroughfareName = "THE STREET",
                PostTown = "CORYTON",
                Postcode = "BP23 6AA"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Basement Flat").IgnoreCase);
            Assert.That(lines[1], Is.EqualTo("Victoria House").IgnoreCase);
            Assert.That(lines[2], Is.EqualTo("15 The Street").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheBuildingNumberAndSubBuildingNameOnTheSameLineIfTheConcatenateFlagIsSet()
        {
            var data = new PafData()
            {
                SubBuildingName = "A",
                BuildingNumber = "12",
                ThoroughfareName = "SMITH STREET",
                PostTown = "CORYTON",
                Postcode = "BP23 6AA",
                ConcatenateBuildingNumber = true
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("12A Smith Street").IgnoreCase);
        }
    }
}
