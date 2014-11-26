namespace Example
{
    using System.Collections.Generic;
    using SharpPaf;
    using SharpPaf.Data;

    /// <summary>
    /// Allows PAF data to be combined in memory.
    /// </summary>
    internal sealed class MemoryRepository : PafRepository
    {
        private readonly List<PafData> addresses = new List<PafData>();
        private readonly Dictionary<int, string> buildingNames = new Dictionary<int, string>();
        private readonly TitleCaseConverter caseConverter;
        private readonly Dictionary<int, string> descriptors = new Dictionary<int, string>();
        private readonly Dictionary<int, LocalityRecord> localities = new Dictionary<int, LocalityRecord>();
        private readonly Dictionary<int, OrganisationRecord> organisations = new Dictionary<int, OrganisationRecord>();
        private readonly PostcodeFormatter postcodeFormatter;
        private readonly Dictionary<int, string> subBuildingNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> thoroughfares = new Dictionary<int, string>();

        /// <summary>
        /// Initializes a new instance of the MemoryRepository class.
        /// </summary>
        public MemoryRepository()
            : this(FormatOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MemoryRepository class.
        /// </summary>
        /// <param name="options">
        /// Specifies any formatting options to perform on the data.
        /// </param>
        public MemoryRepository(FormatOptions options)
        {
            if (options.HasFlag(FormatOptions.TitleCase))
            {
                this.caseConverter = new TitleCaseConverter();
            }

            if (options.HasFlag(FormatOptions.Postcode))
            {
                this.postcodeFormatter = new PostcodeFormatter();
            }
        }

        /// <summary>
        /// Gets the collection of addresses added to this instance.
        /// </summary>
        public IReadOnlyList<IPafData> Addresses
        {
            get { return this.addresses; }
        }

        /// <inheritdoc />
        public override void AddAddress(AddressRecord record)
        {
            PafData paf = new PafData();
            paf.BuildingName = this.GetString(record.BuildingNameKey, this.buildingNames);
            paf.BuildingNumber = (record.BuildingNumber != null) ? record.BuildingNumber.ToString() : null;
            paf.ConcatenateBuildingNumber = record.IsBuildingNumberConcatenated;
            paf.DependentThoroughfareDescriptor = this.GetString(record.DependentThoroughfareDescriptorKey, this.descriptors);
            paf.DependentThoroughfareName = this.GetString(record.DependentThoroughfareKey, this.thoroughfares);
            paf.POBoxNumber = record.POBoxNumber;
            paf.SubBuildingName = this.GetString(record.SubBuildingNameKey, this.subBuildingNames);
            paf.ThoroughfareDescriptor = this.GetString(record.ThoroughfareDescriptorKey, this.descriptors);
            paf.ThoroughfareName = this.GetString(record.ThoroughfareKey, this.thoroughfares);

            paf.Postcode = (this.postcodeFormatter == null) ?
                record.Postcode :
                this.postcodeFormatter.Format(record.Postcode);

            LocalityRecord locality = GetRelated(record.LocalityKey, this.localities);
            if (locality != null)
            {
                paf.DependentLocality = this.GetString(locality.DependentLocality);
                paf.DoubleDependentLocality = this.GetString(locality.DoubleDependentLocality);
                paf.PostTown = locality.PostTown; // This should be uppercase
            }

            OrganisationRecord organisation = GetRelated(record.OrganisationKey, this.organisations);
            if (organisation != null)
            {
                paf.DepartmentName = this.GetString(organisation.Department);
                paf.OrganisationName = this.GetString(organisation.Name);
            }

            this.addresses.Add(paf);
        }

        /// <inheritdoc />
        public override void AddBuildingName(BuildingNameRecord record)
        {
            this.buildingNames[record.Key] = record.Name;
        }

        /// <inheritdoc />
        public override void AddLocality(LocalityRecord record)
        {
            this.localities[record.Key] = record;
        }

        /// <inheritdoc />
        public override void AddOrganisation(OrganisationRecord record)
        {
            this.organisations[record.Key] = record;
        }

        /// <inheritdoc />
        public override void AddSubBuildingName(SubBuildingNameRecord record)
        {
            this.subBuildingNames[record.Key] = record.Name;
        }

        /// <inheritdoc />
        public override void AddThoroughfare(ThoroughfareRecord record)
        {
            this.thoroughfares[record.Key] = record.Name;
        }

        /// <inheritdoc />
        public override void AddThoroughfareDescriptor(ThoroughfareDescriptorRecord record)
        {
            this.descriptors[record.Key] = record.Descriptor;
        }

        private static T GetRelated<T>(int? key, IDictionary<int, T> source)
        {
            return (key != null) ? source[key.Value] : default(T);
        }

        private string GetString(int? key, IDictionary<int, string> source)
        {
            return this.GetString(GetRelated(key, source));
        }

        private string GetString(string value)
        {
            if (this.caseConverter == null)
            {
                return value;
            }
            else
            {
                return this.caseConverter.ToTitleCase(value);
            }
        }
    }
}
