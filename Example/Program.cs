namespace Example
{
    using System;
    using SharpPaf;

    public static class Program
    {
        private static TitleCaseConverter converter = new TitleCaseConverter();

        public static void Main(string[] args)
        {
            var formatter = new AddressFormatter();
            var data = CreatePafData();

            PerformanceTest(500, () => formatter.Format(data).Length);
            PerformanceTest(500, () => ConvertCase(CreatePafData()));
            PerformanceTest(100000, () => formatter.Format(data).Length);
            PerformanceTest(100000, () => CreatePafData().BuildingName.Length);
            PerformanceTest(100000, () => ConvertCase(CreatePafData()));
        }

        private static int ConvertCase(PafData data)
        {
            data.OrganisationName = converter.ToTitleCase(data.OrganisationName);
            data.DepartmentName = converter.ToTitleCase(data.DepartmentName);
            data.SubBuildingName = converter.ToTitleCase(data.SubBuildingName);
            data.BuildingName = converter.ToTitleCase(data.BuildingName);
            data.ThoroughfareName = converter.ToTitleCase(data.ThoroughfareName);
            data.ThoroughfareDescriptor = converter.ToTitleCase(data.ThoroughfareDescriptor);
            data.DependentThoroughfareName = converter.ToTitleCase(data.DependentThoroughfareName);
            data.DependentThoroughfareDescriptor = converter.ToTitleCase(data.DependentThoroughfareDescriptor);
            data.DoubleDependentLocality = converter.ToTitleCase(data.DoubleDependentLocality);
            data.DependentLocality = converter.ToTitleCase(data.DependentLocality);
            return 1;
        }

        private static PafData CreatePafData()
        {
            return new PafData()
            {
                OrganisationName = "SOUTH LANARKSHIRE COUNCIL",
                DepartmentName = "HEAD START",
                SubBuildingName = "UNIT 1",
                BuildingName = "BLOCK 3",
                ThoroughfareName = "THIRD",
                ThoroughfareDescriptor = "ROAD",
                DoubleDependentLocality = "BLANTYRE INDUSTRIAL ESTATE",
                DependentLocality = "BLANTYRE",
                PostTown = "GLASGOW",
                Postcode = "G72 0UP"
            };
        }

        private static void PerformanceTest(int count, Func<int> action)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            int counter = 0;
            for (int i = 0; i < count; i++)
            {
                counter += action();
            }
            sw.Stop();

            int perSecond = (int)((((double)count) / sw.ElapsedMilliseconds) * 1000);
            Console.WriteLine("{0} in {1}ms ({2}/sec)", counter, sw.ElapsedMilliseconds, perSecond);
        }
    }
}
