namespace Example
{
    using System;
    using SharpPaf;
    using SharpPaf.Data;

    public static class Program
    {
        public static void Main(string[] args)
        {
            const string PathToExampleData = @"D:\PAF\Fixed PAF";
            var addressFormatter = new AddressFormatter();
            var mainfile = new Mainfile(PathToExampleData);
            var repository = new MemoryRepository(FormatOptions.Postcode | FormatOptions.TitleCase);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            mainfile.SaveAll(repository);
            sw.Stop();
            Console.WriteLine("Converted {0} addresses in {1}ms", repository.Addresses.Count, sw.ElapsedMilliseconds);

            Console.WriteLine(
                string.Join(
                    "\n",
                    addressFormatter.Format(repository.Addresses[1])));

            Console.ReadKey();
        }
    }
}
