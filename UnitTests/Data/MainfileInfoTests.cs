namespace UnitTests.Data
{
    using System.IO;
    using System.Text;
    using NSubstitute;
    using NUnit.Framework;
    using SharpPaf.Data;
    using SharpPaf.Data.IO;

    [TestFixture]
    public sealed class MainfileInfoTests
    {
        [Test]
        public void ShouldParseBuildingNamesFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.BuildingNames);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.BuildingNames));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(58 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseBusinessMailFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.BuisinessMail);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.BusinessMail));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(10 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseLocalityNamesFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.Localities);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.Localities));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(151 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseMainAddressFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.MainAddress);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.Address));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(88 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseOrganisationsFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.Organisations);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.Organisations));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(153 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseSubBuildingNamesFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.SubBuildingName);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.SubBuildingNames));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(38 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseThoroughfareDescriptorsFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.ThoroughfareDescriptors);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.ThoroughfareDescriptors));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(30 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseThoroughfaresFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.Thoroughfares);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.Thoroughfares));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(68 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldParseWelshAddressFile()
        {
            MainfileInfo fileInfo = this.LoadFile(ExamplePafFileData.MainAddress, ExamplePafFileData.WelshFilename);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.WelshAddress));
            Assert.That(fileInfo.RecordLength, Is.EqualTo(88 + 2));
            CheckEditionAndRecords(fileInfo);
        }

        [Test]
        public void ShouldHandleInvalidInput()
        {
            MainfileInfo fileInfo = this.LoadFile(string.Empty);

            Assert.That(fileInfo.FileType, Is.EqualTo(MainfileType.Unknown));
        }

        private static void CheckEditionAndRecords(MainfileInfo fileInfo)
        {
            Assert.That(fileInfo.Edition, Is.EqualTo("Y14M05"));
            Assert.That(fileInfo.RecordCount, Is.EqualTo(3));
        }

        private MainfileInfo LoadFile(string contents, string filename = "example.c01")
        {
            FileSystem.Instance = Substitute.For<IFileSystem>();
            using (var stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(contents)))
            {
                FileSystem.Instance.OpenRead(filename)
                                   .Returns(stream);

                return new MainfileInfo(filename);
            }
        }
    }
}
