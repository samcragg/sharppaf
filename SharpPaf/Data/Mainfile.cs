namespace SharpPaf.Data
{
    using System.Data;
    using System.Linq;
    using SharpPaf.Data.IO;
    using SharpPaf.Data.Parsers;

    /// <summary>
    /// Contains information about the PAF mainfile.
    /// </summary>
    public sealed class Mainfile
    {
        private readonly MainfileInfo[] fileInfos;

        /// <summary>
        /// Initializes a new instance of the MainFile class.
        /// </summary>
        /// <param name="folder">
        /// The full path to the folder that contains the main files.
        /// </param>
        public Mainfile(string folder)
        {
            this.fileInfos = FileSystem.Instance.GetFiles(folder)
                                       .Select(file => new MainfileInfo(file))
                                       .Where(mi => mi.FileType != MainfileType.Unknown)
                                       .ToArray();
        }

        /// <summary>
        /// Creates an IDataReader that can read records of the specified type.
        /// </summary>
        /// <param name="type">The type of records to return.</param>
        /// <returns>
        /// An IDataReader that reads the specified type of records.
        /// </returns>
        public IDataReader CreateReader(MainfileType type)
        {
            return new RecordDataReader(this.fileInfos.Where(fi => fi.FileType == type));
        }

        /// <summary>
        /// Saves all the records to the specified repository.
        /// </summary>
        /// <param name="repository">The repository to save to.</param>
        public void SaveAll(PafRepository repository)
        {
            this.SaveTo(repository, MainfileType.BuildingNames);
            this.SaveTo(repository, MainfileType.BusinessMail);
            this.SaveTo(repository, MainfileType.Localities);
            this.SaveTo(repository, MainfileType.Organisations);
            this.SaveTo(repository, MainfileType.SubBuildingNames);
            this.SaveTo(repository, MainfileType.ThoroughfareDescriptors);
            this.SaveTo(repository, MainfileType.Thoroughfares);

            // These must be saved after the others, as they have foreign keys
            // referencing the above.
            this.SaveTo(repository, MainfileType.Address);
            this.SaveTo(repository, MainfileType.WelshAddress);
        }

        /// <summary>
        /// Saves the specified type of records to the specified repository.
        /// </summary>
        /// <param name="repository">The repository to save to.</param>
        /// <param name="type">The types of records to save.</param>
        public void SaveTo(PafRepository repository, MainfileType type)
        {
            Check.IsNotNull(repository, "repository");

            RecordParser parser = RecordParserFactory.Create(type);
            if (parser != null)
            {
                foreach (MainfileInfo info in this.fileInfos.Where(fi => fi.FileType == type))
                {
                    parser.Parse(info, repository);
                }
            }
        }
    }
}
