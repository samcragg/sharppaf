namespace Example
{
    using SharpPaf;

    internal sealed class PafData : IPafData
    {
        public string OrganisationName { get; set; }

        public string DepartmentName { get; set; }

        public string SubBuildingName { get; set; }

        public string BuildingName { get; set; }

        public string BuildingNumber { get; set; }

        public string DependentThoroughfareName { get; set; }

        public string DependentThoroughfareDescriptor { get; set; }

        public string ThoroughfareName { get; set; }

        public string ThoroughfareDescriptor { get; set; }

        public string DoubleDependentLocality { get; set; }

        public string DependentLocality { get; set; }

        public string PostTown { get; set; }

        public string Postcode { get; set; }

        public string POBoxNumber { get; set; }
    }
}
