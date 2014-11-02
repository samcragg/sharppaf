namespace UnitTests.Data
{
    using System;
    using System.Data;
    using NUnit.Framework;
    using SharpPaf.Data;

    [TestFixture]
    public sealed class CreateReaderTests
    {
        private MainfileBuilder mainfileBuilder;

        [SetUp]
        public void SetUp()
        {
            this.mainfileBuilder = new MainfileBuilder();
        }

        [Test]
        public void ShouldBeAbleToReadAddressFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.MainAddress);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.Address))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(2572775));
                Assert.That(reader["Postcode"], Is.EqualTo("CF30AA"));
                Assert.That(reader["LocalityKey"], Is.EqualTo(24904));
                Assert.That(reader["ThoroughfareKey"], Is.EqualTo(21479));
                Assert.That(reader["ThoroughfareDescriptorKey"], Is.EqualTo(6));
                Assert.That(reader["DependentThoroughfareKey"], Is.EqualTo(DBNull.Value));
                Assert.That(reader["DependentThoroughfareDescriptorKey"], Is.EqualTo(DBNull.Value));
                Assert.That(reader["BuildingNumber"], Is.EqualTo(1));
                Assert.That(reader["BuildingNameKey"], Is.EqualTo(DBNull.Value));
                Assert.That(reader["SubBuildingNameKey"], Is.EqualTo(DBNull.Value));
                Assert.That(reader["NumberOfHouseholds"], Is.EqualTo(1));
                Assert.That(reader["OrganisationKey"], Is.EqualTo(DBNull.Value));
                Assert.That(reader["PostcodeType"], Is.EqualTo((int)DeliveryPointType.SmallUser));
                Assert.That(reader["IsBuildingNumberConcatenated"], Is.False);
                Assert.That(reader["DeliveryPointSuffix"], Is.EqualTo("1A"));
                Assert.That(reader["IsSmallUserOrganisation"], Is.False);
                Assert.That(reader.IsDBNull(reader.GetOrdinal("POBoxNumber")), Is.True);
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadBuildingNameFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.BuildingNames);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.BuildingNames))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(1));
                Assert.That(reader["Name"], Is.EqualTo("23A"));
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadLocalityFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.Localities);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.Localities))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(3658));
                Assert.That(reader["PostTown"], Is.EqualTo("BODMIN"));
                Assert.That(reader["DependentLocality"], Is.EqualTo("CARDINHAM"));
                Assert.That(reader["DoubleDependentLocality"], Is.EqualTo("LITTLE DOWNS"));
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadOrganisationFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.Organisations);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.Organisations))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(122173));
                Assert.That(reader["PostcodeType"], Is.EqualTo((int)DeliveryPointType.SmallUser));
                Assert.That(reader["Name"], Is.EqualTo("H M COASTGUARD"));
                Assert.That(reader["Department"], Is.EqualTo("M R S C HUMBER"));
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadSubBuildingNameFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.SubBuildingName);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.SubBuildingNames))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(2));
                Assert.That(reader["Name"], Is.EqualTo("1"));
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadThoroughfareFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.Thoroughfares);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.Thoroughfares))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(1));
                Assert.That(reader["Name"], Is.EqualTo("NORTH"));
                Assert.That(reader.Read(), Is.False);
            }
        }

        [Test]
        public void ShouldBeAbleToReadThoroughfareDescriptorFiles()
        {
            this.mainfileBuilder.AddFile(string.Empty, ExamplePafFileData.ThoroughfareDescriptors);
            Mainfile mainfile = this.mainfileBuilder.Create();

            using (IDataReader reader = mainfile.CreateReader(MainfileType.ThoroughfareDescriptors))
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader["Key"], Is.EqualTo(1));
                Assert.That(reader["Descriptor"], Is.EqualTo("ROAD"));
                Assert.That(reader.Read(), Is.False);
            }
        }
    }
}
