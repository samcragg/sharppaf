using System;
using System.Diagnostics;
using SharpPaf.Data;

namespace SqlServerExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const string PathToExampleData = @"D:\PAF\Fixed PAF";
            var mainfile = new Mainfile(PathToExampleData);
            var bulkInserter = new BulkInserter();

            Console.Write("Inserting records");
            var stopwatch = Stopwatch.StartNew();
            bulkInserter.Insert(mainfile).Wait();
            stopwatch.Stop();
            Console.WriteLine("finished ({0:f1} secs.)", stopwatch.Elapsed.TotalSeconds);
            Console.ReadKey();
        }
    }
}
