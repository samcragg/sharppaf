namespace LuceneExample
{
    using System;
    using System.Collections.Generic;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;

    /// <summary>
    /// Allows the searching of an address in the index created by Indexer.
    /// </summary>
    internal sealed class AddressFinder : IDisposable
    {
        private const int MaximumResults = 10;

        private static readonly char[] QuerySplitChars = new[]
        {
            '\n',
            '\r',
            ' ',
            '\t',
            ','
        };

        private static readonly Dictionary<string, string> Replacements =
            new Dictionary<string, string>()
            {
                { "AV", "AVENUE" },
                { "AVE", "AVENUE" },
                { "CL", "CLOSE" },
                { "CRES", "CRESCENT" },
                { "CT", "COURT" },
                { "DR", "DRIVE" },
                { "EST", "ESTATE" },
                { "GDNS", "GARDENS" },
                { "GR", "GROVE" },
                { "LA", "LANE" },
                { "PDE", "PARADE" },
                { "PK", "PARK" },
                { "PL", "PLACE" },
                { "RD", "ROAD" },
                { "SQ", "SQUARE" },
                { "ST", "STREET" },
                { "TER", "TERRACE" }
            };

        private readonly Directory indexDirectory;
        private readonly IndexSearcher indexSearcher;

        /// <summary>
        /// Initializes a new instance of the AddressFinder class.
        /// </summary>
        public AddressFinder()
        {
            this.indexDirectory = FSDirectory.Open(Indexer.IndexDirectory);
            this.indexSearcher = new IndexSearcher(this.indexDirectory, readOnly: true);
        }

        /// <summary>
        /// Releases resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.indexSearcher.Dispose();
            this.indexDirectory.Dispose();
        }

        /// <summary>
        /// Finds addresses matching the query text, returning their keys.
        /// </summary>
        /// <param name="queryText">The text to search for.</param>
        /// <returns>
        /// A list of keys in best match order.
        /// </returns>
        public int[] FindAddresses(string queryText)
        {
            var collector = TopScoreDocCollector.Create(MaximumResults, docsScoredInOrder: true);
            var query = BuildQuery(queryText);
            this.indexSearcher.Search(query, collector);

            return Array.ConvertAll(
                collector.TopDocs().ScoreDocs,
                this.GetAddressKeyFromScoreDoc);
        }

        private static Query BuildQuery(string text)
        {
            var query = new BooleanQuery();
            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] terms = text.Split(QuerySplitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (string term in terms)
                {
                    query.Add(
                        new FuzzyQuery(
                            new Term(Indexer.AddressFieldName, ExpandTerm(term.ToUpperInvariant()))),
                            Occur.MUST);
                }
            }

            return query;
        }

        private static string ExpandTerm(string term)
        {
            string replacement;
            if (Replacements.TryGetValue(term, out replacement))
            {
                return replacement;
            }
            else
            {
                return term;
            }
        }

        private int GetAddressKeyFromScoreDoc(ScoreDoc doc)
        {
            Document document = this.indexSearcher.Doc(doc.Doc);
            byte[] value = document.GetBinaryValue(Indexer.KeyFieldName);
            if ((value == null) || (value.Length != 4))
            {
                return 0;
            }

            return BitConverter.ToInt32(value, 0);
        }
    }
}
