namespace SharpPaf
{
    /// <summary>
    /// Represents the raw PAF data.
    /// </summary>
    public interface IPafData
    {
        /// <summary>
        /// Gets the name of the organisation.
        /// </summary>
        string OrganisationName { get; }

        /// <summary>
        /// Gets the department in the organisation.
        /// </summary>
        string DepartmentName { get; }

        /// <summary>
        /// Gets the name of the sub-building.
        /// </summary>
        string SubBuildingName { get; }

        /// <summary>
        /// Gets the name of the building.
        /// </summary>
        string BuildingName { get; }

        /// <summary>
        /// Gets the number of the building.
        /// </summary>
        string BuildingNumber { get; }

        /// <summary>
        /// Gets the name part of the dependent thoroughfare.
        /// </summary>
        string DependentThoroughfareName { get; }

        /// <summary>
        /// Gets the descriptor part of the dependent thoroughfare.
        /// </summary>
        string DependentThoroughfareDescriptor { get; }

        /// <summary>
        /// Gets the name part of the thoroughfare.
        /// </summary>
        string ThoroughfareName { get; }

        /// <summary>
        /// Gets the descriptor part of the thoroughfare.
        /// </summary>
        string ThoroughfareDescriptor { get; }

        /// <summary>
        /// Gets the dependent locality information.
        /// </summary>
        string DoubleDependentLocality { get; }

        /// <summary>
        /// Gets the double dependent locality information.
        /// </summary>
        string DependentLocality { get; }

        /// <summary>
        /// Gets the Post Town information.
        /// </summary>
        string PostTown { get; }

        /// <summary>
        /// Gets the postcode information.
        /// </summary>
        string Postcode { get; }

        /// <summary>
        /// Gets the P.O. Box information.
        /// </summary>
        string POBoxNumber { get; }

        /// <summary>
        /// Gets a value indicating whether the building number and the
        /// sub-building name should appear concatenated on the same line.
        /// </summary>
        bool ConcatenateBuildingNumber { get; }
    }
}
