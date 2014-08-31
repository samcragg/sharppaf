namespace UnitTests
{
    using NUnit.Framework;
    using SharpPaf;

    [TestFixture]
    public sealed class ExceptionRulesTests
    {
        private AddressFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            this.formatter = new AddressFormatter();
        }

        [Test]
        public void ShouldOutputTheBuildingNameWithoutSplittingTheNumberIfPrefexedByAKeyword()
        {
            // Table 24: Exception Rule indicator iv) Where the building name
            // has a numeric range or a numeric alpha suffix,and is prefixed by
            // the following keywords...
            var data = new PafData()
            {
                OrganisationName = "The Tambourine Warehouse",
                BuildingName = "Unit 1-3",
                DependentThoroughfareName = "Industrial Estate",
                ThoroughfareName = "Tame Road",
                PostTown = "LONDON",
                Postcode = "E6 7HS"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[1], Is.EqualTo("Unit 1-3").IgnoreCase);
            Assert.That(lines[2], Is.EqualTo("Industrial Estate").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheBuildingNameOnTheThoroughfareLine()
        {
            // Table 27a: Premises elements Rule 3: Building Name only
            var data = new PafData()
            {
                BuildingName = "1A",
                DependentThoroughfareName = "SEASTONE COURT",
                ThoroughfareName = "STATION ROAD",
                PostTown = "HOLT",
                Postcode = "NR25 7HG"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("1a Seastone Court").IgnoreCase);
        }

        [Test]
        public void ShouldExtractTheBuildingNumberFromTheBuildingName()
        {
            // Table 27c: Premises elements Rule 3: Building Name only
            var data = new PafData()
            {
                OrganisationName = "S D ALCOTT FLORISTS",
                BuildingName = "FLOWER HOUSE 189A",
                ThoroughfareName = "PYE GREEN ROAD",
                PostTown = "CANNOCK",
                Postcode = "WS11 5SB"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[1], Is.EqualTo("Flower House").IgnoreCase);
            Assert.That(lines[2], Is.EqualTo("189a Pye Green Road").IgnoreCase);
        }

        [Test]
        public void ShouldNotExtractWholeNumbersFromTheBuildingName()
        {
            // Table 27d: Premises elements Rule 3: Building Name only
            var data = new PafData()
            {
                OrganisationName = "JAMES VILLA HOLIDAYS",
                BuildingName = "CENTRE 30",
                ThoroughfareName = "ST. LAURENCE AVENUE",
                PostTown = "GRAFTON",
                Postcode = "ME16 0LP"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[1], Is.EqualTo("Centre 30").IgnoreCase);
        }

        [Test]
        public void ShouldOutputTheSubBuildingNameOnTheSameLineAsTheBuilingName()
        {
            // Table 30: Premises elements Rule 6:Sub Building Name and Building
            var data = new PafData()
            {
                SubBuildingName = "10B",
                BuildingName = "BARRY JACKSON TOWER",
                ThoroughfareName = "ESTONE WALK",
                PostTown = "BIRMINGHAM",
                Postcode = "B6 5BA"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("10B Barry Jackson Tower").IgnoreCase);
        }

        [Test]
        public void ShouldOutputSubBuildingBeforeBuildingNameCombinedWithThoroughfare()
        {
            // Table 31a: Building Name
            var data = new PafData()
            {
                SubBuildingName = "CARETAKERS FLAT",
                BuildingName = "110-114",
                ThoroughfareName = "HIGH STREET WEST",
                PostTown = "BRISTOL",
                Postcode = "BS1 2AW"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("Caretakers Flat").IgnoreCase);
            Assert.That(lines[1], Is.EqualTo("110-114 High Street West").IgnoreCase);
        }

        [Test]
        public void ShouldOutputSubBuildingWithBuildingNameBeforeBuildingNumber()
        {
            // Table 32a: Premises elements Rule 7:Sub Building Name, Building Name
            var data = new PafData()
            {
                SubBuildingName = "2B",
                BuildingName = "THE TOWER",
                BuildingNumber = "27",
                ThoroughfareName = "JOHN STREET",
                PostTown = "WINCHESTER",
                Postcode = "SO23 9AP"
            };

            string[] lines = this.formatter.Format(data);

            Assert.That(lines[0], Is.EqualTo("2B The Tower").IgnoreCase);
            Assert.That(lines[1], Is.EqualTo("27 John Street").IgnoreCase);
        }
    }
}
