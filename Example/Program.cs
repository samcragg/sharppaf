namespace Example
{
    using System;
    using SharpPaf;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var formatter = new AddressFormatter();
            var data = new PafData()
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

            PerformanceTest(500, () => formatter.Format(data).Length);
            PerformanceTest(100000, () => formatter.Format(data).Length);
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
