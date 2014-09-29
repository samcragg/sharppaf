namespace SharpPaf
{
    /// <summary>
    /// Represents the raw PAF data.
    /// </summary>
    public interface IPafData
    {
        string OrganisationName { get; }

        string DepartmentName { get; }

        string SubBuildingName { get; }

        string BuildingName { get; }

        string BuildingNumber { get; }

        string DependentThoroughfareName { get; }

        string DependentThoroughfareDescriptor { get; }

        string ThoroughfareName { get; }

        string ThoroughfareDescriptor { get; }

        string DoubleDependentLocality { get; }

        string DependentLocality { get; }

        string PostTown { get; }

        string Postcode { get; }

        string POBoxNumber { get; }

        /// <summary>
        /// Gets a value indicating whether the building number and the
        /// sub-building name should appear concatenated on the same line.
        /// </summary>
        bool ConcatenateBuildingNumber { get; }
    }
}
