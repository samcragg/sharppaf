namespace LuceneExample
{
    using System.Collections.Generic;
    using System.Text;
    using SharpPaf.Data;

    /// <summary>
    /// Copied from the MemoryExample project.
    /// </summary>
    internal sealed class MemoryRepository : PafRepository
    {
        private readonly Dictionary<int, string> addresses = new Dictionary<int, string>();
        private readonly Dictionary<int, string> buildingNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> descriptors = new Dictionary<int, string>();
        private readonly Dictionary<int, LocalityRecord> localities = new Dictionary<int, LocalityRecord>();
        private readonly Dictionary<int, OrganisationRecord> organisations = new Dictionary<int, OrganisationRecord>();
        private readonly Dictionary<int, string> subBuildingNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> thoroughfares = new Dictionary<int, string>();

        /// <summary>
        /// Gets the collection of addresses added to this instance.
        /// </summary>
        public IReadOnlyCollection<KeyValuePair<int, string>> Addresses
        {
            get { return this.addresses; }
        }

        /// <inheritdoc />
        public override void AddAddress(AddressRecord record)
        {
            var builder = new StringBuilder(512); // The CSV format of the PAF has a 490 maximum

            OrganisationRecord organisation = GetRelated(record.OrganisationKey, this.organisations);
            if (organisation != null)
            {
                builder.Append(organisation.Name)
                       .Append(' ')
                       .Append(organisation.Department)
                       .Append(' ');
            }

            if (record.BuildingNumber != null)
            {
                builder.Append(record.BuildingNumber)
                       .Append(' ');
            }

            builder.Append(GetRelated(record.BuildingNameKey, this.buildingNames))
                   .Append(' ')
                   .Append(GetRelated(record.SubBuildingNameKey, this.subBuildingNames))
                   .Append(' ');

            builder.Append(GetRelated(record.DependentThoroughfareKey, this.thoroughfares))
                   .Append(' ')
                   .Append(GetRelated(record.DependentThoroughfareDescriptorKey, this.descriptors))
                   .Append(' ')
                   .Append(GetRelated(record.ThoroughfareKey, this.thoroughfares))
                   .Append(' ')
                   .Append(GetRelated(record.ThoroughfareDescriptorKey, this.descriptors))
                   .Append(' ')
                   .Append(record.POBoxNumber)
                   .Append(' ');

            LocalityRecord locality = GetRelated(record.LocalityKey, this.localities);
            if (locality != null)
            {
                builder.Append(locality.DoubleDependentLocality)
                       .Append(' ')
                       .Append(locality.DependentLocality)
                       .Append(' ')
                       .Append(locality.PostTown);
            }

            this.addresses[record.Key] = builder.ToString();
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

        /// <summary>
        /// Finds the full address text for the specified key.
        /// </summary>
        /// <param name="key">The key of the address to find.</param>
        /// <returns>The full address text, or null if key wasn't found.</returns>
        public string FindAddress(int key)
        {
            string address;
            this.addresses.TryGetValue(key, out address);
            return address;
        }

        private static T GetRelated<T>(int? key, IDictionary<int, T> source)
        {
            return (key != null) ? source[key.Value] : default(T);
        }
    }
}
