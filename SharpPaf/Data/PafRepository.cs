namespace SharpPaf.Data
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Allows the storing of PAF information from the parsed files.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class PafRepository
    {
        /// <summary>
        /// When overridden in a derived class, adds the specified address
        /// information to the repository.
        /// </summary>
        /// <param name="record">Contains the address information.</param>
        public virtual void AddAddress(AddressRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified building
        /// name information to the repository.
        /// </summary>
        /// <param name="record">Contains the building name information.</param>
        public virtual void AddBuildingName(BuildingNameRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified locality
        /// information to the repository.
        /// </summary>
        /// <param name="record">Contains the locality information.</param>
        public virtual void AddLocality(LocalityRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified organisation
        /// information to the repository.
        /// </summary>
        /// <param name="record">Contains the organisation information.</param>
        public virtual void AddOrganisation(OrganisationRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified sub-building
        /// name information to the repository.
        /// </summary>
        /// <param name="record">Contains the sub-building name information.</param>
        public virtual void AddSubBuildingName(SubBuildingNameRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified thoroughfare
        /// information to the repository.
        /// </summary>
        /// <param name="record">Contains the thoroughfare information.</param>
        public virtual void AddThoroughfare(ThoroughfareRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified thoroughfare
        /// descriptor information to the repository.
        /// </summary>
        /// <param name="record">Contains the descriptor information.</param>
        public virtual void AddThoroughfareDescriptor(ThoroughfareDescriptorRecord record)
        {
        }

        /// <summary>
        /// When overridden in a derived class, adds the specified address
        /// information to the repository as a Welsh alternative address.
        /// </summary>
        /// <param name="record">Contains the address information.</param>
        public virtual void AddWelshAddress(AddressRecord record)
        {
        }
    }
}
