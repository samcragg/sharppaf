namespace UnitTests.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NSubstitute;
    using NUnit.Framework;
    using SharpPaf.Data;

    [TestFixture]
    public sealed class MainfileTests
    {
        private MainfileBuilder mainfileBuilder;

        [SetUp]
        public void SetUp()
        {
            this.mainfileBuilder = new MainfileBuilder();
        }

        [Test]
        public void SaveAllShouldCheckForNullArguments()
        {
            Mainfile mainfile = this.mainfileBuilder.Create();

            Assert.That(() => mainfile.SaveAll(null),
                        Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SaveAllShouldSaveAddressFilesLast()
        {
            foreach (FieldInfo field in typeof(ExamplePafFileData).GetFields())
            {
                this.mainfileBuilder.AddFile(field.Name, (string)field.GetValue(null));
            }

            Mainfile mainfile = this.mainfileBuilder.Create();
            var repository = Substitute.For<PafRepository>();

            mainfile.SaveAll(repository);

            MethodInfo lastCalled = repository.ReceivedCalls().Select(c => c.GetMethodInfo()).Last();
            Assert.That(lastCalled.Name, Is.EqualTo("AddAddress"));
        }

        [Test]
        public void SaveAllShouldSaveWelshAddressFilesLast()
        {
            foreach (FieldInfo field in typeof(ExamplePafFileData).GetFields())
            {
                this.mainfileBuilder.AddFile(field.Name, (string)field.GetValue(null));
            }
            this.mainfileBuilder.RemoveFile("MainAddress");
            this.mainfileBuilder.AddFile(ExamplePafFileData.WelshFilename, ExamplePafFileData.MainAddress);

            Mainfile mainfile = this.mainfileBuilder.Create();
            var repository = Substitute.For<PafRepository>();

            mainfile.SaveAll(repository);

            MethodInfo lastCalled = repository.ReceivedCalls().Select(c => c.GetMethodInfo()).Last();
            Assert.That(lastCalled.Name, Is.EqualTo("AddWelshAddress"));
        }

        [Test]
        public void SaveToShouldCheckForNullArguments()
        {
            Mainfile mainfile = this.mainfileBuilder.Create();

            Assert.That(() => mainfile.SaveTo(null, MainfileType.Address),
                        Throws.InstanceOf<ArgumentNullException>());
        }

        [TestCase(ExamplePafFileData.BuildingNames, MainfileType.BuildingNames, Result = "AddBuildingName", TestName = "SaveToBuildingNames")]
        [TestCase(ExamplePafFileData.Localities, MainfileType.Localities, Result = "AddLocality", TestName = "SaveToLocalities")]
        [TestCase(ExamplePafFileData.MainAddress, MainfileType.Address, Result = "AddAddress", TestName = "SaveToAddresses")]
        [TestCase(ExamplePafFileData.Organisations, MainfileType.Organisations, Result = "AddOrganisation", TestName = "SaveToOrganisations")]
        [TestCase(ExamplePafFileData.SubBuildingName, MainfileType.SubBuildingNames, Result = "AddSubBuildingName", TestName = "SaveToSubBuildingNames")]
        [TestCase(ExamplePafFileData.ThoroughfareDescriptors, MainfileType.ThoroughfareDescriptors, Result = "AddThoroughfareDescriptor", TestName = "SaveToThoroughfareDescriptors")]
        [TestCase(ExamplePafFileData.Thoroughfares, MainfileType.Thoroughfares, Result = "AddThoroughfare", TestName = "SaveToThoroughfare")]
        public string SaveToShouldSaveTheSpecifiedType(string content, MainfileType type)
        {
            PafRepository repository = Substitute.For<PafRepository>();
            this.mainfileBuilder.AddFile(type.ToString(), content);
            Mainfile mainfile = this.mainfileBuilder.Create();

            mainfile.SaveTo(repository, type);

            return repository.ReceivedCalls().Single().GetMethodInfo().Name;
        }

        [Test]
        public void SaveToShouldReadFromMultipleFilesOfTheSameType()
        {
            this.mainfileBuilder.AddFile("Locality1", ExamplePafFileData.Localities);
            this.mainfileBuilder.AddFile("Locality2", ExamplePafFileData.Localities);
            this.mainfileBuilder.AddFile("MainAddress1", ExamplePafFileData.MainAddress);

            Mainfile mainfile = this.mainfileBuilder.Create();

            PafRepository repository = Substitute.For<PafRepository>();
            mainfile.SaveTo(repository, MainfileType.Localities);

            repository.ReceivedWithAnyArgs(2).AddLocality(null);
        }
    }
}
