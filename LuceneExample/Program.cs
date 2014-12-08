namespace LuceneExample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using SharpPaf.Data;

    public static class Program
    {
        public static void Main(string[] args)
        {
            const string PathToExampleData = @"D:\PAF\Fixed PAF";
            var mainfile = new Mainfile(PathToExampleData);
            var repository = new MemoryRepository();

            // Load the PAF data (this could be changed to use the view in the
            // SqlServerExample).
            Console.Write("Creating repository...");
            var stopwatch = Stopwatch.StartNew();
            mainfile.SaveAll(repository);
            stopwatch.Stop();
            Console.WriteLine("finished ({0} inserted in {1:f1} secs.)", repository.Addresses.Count, stopwatch.Elapsed.TotalSeconds);

            // Create the Lucene.NET index
            Console.Write("Creating index...");
            DeleteExistingIndex();
            stopwatch.Restart();
            using (var indexer = new Indexer())
            {
                foreach (KeyValuePair<int, string> address in repository.Addresses)
                {
                    indexer.Add(address.Key, address.Value);
                }
            
                indexer.Optimize();
            }
            stopwatch.Stop();
            Console.WriteLine("finished ({0:f1} secs.)", stopwatch.Elapsed.TotalSeconds);

            // An example search
            using (var finder = new AddressFinder())
            {
                stopwatch.Restart();
                int[] addresses = finder.FindAddresses("2 Chuch St"); // Notice we've misspelled 'Church' and used an abbreviation for Street
                stopwatch.Stop();
                Console.WriteLine("{0} matches found in {1}ms.", addresses.Length, stopwatch.ElapsedMilliseconds);

                // This could be replaced with a call to SQL to get all the
                // addresses + postcodes.
                foreach (int addressKey in addresses)
                {
                    Console.WriteLine("{0}: '{1}'", addressKey, repository.FindAddress(addressKey));
                }
            }

            Console.ReadKey();
        }

        private static void DeleteExistingIndex()
        {
            // We could call Exists but then there's nothing stopping the directory
            // being deleted between Exists and Delete so no choice but to catch
            // the exception :(
            try
            {
                Directory.Delete(Indexer.IndexDirectory, recursive: true);
            }
            catch
            {
            }
        }
    }
}
