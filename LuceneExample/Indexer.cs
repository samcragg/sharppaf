namespace LuceneExample
{
    using System;
    using System.Globalization;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Store;

    /// <summary>
    /// Creates the Lucene.NET index for searching addresses.
    /// </summary>
    internal sealed class Indexer : IDisposable
    {
        /// <summary>
        /// The name of the field that stores the full address text.
        /// </summary>
        internal const string AddressFieldName = "Address";

        /// <summary>
        /// The where the index will be stored.
        /// </summary>
        internal const string IndexDirectory = @"D:\PAF\Lucene";

        /// <summary>
        /// The name of the field that stores the PAF address key.
        /// </summary>
        internal const string KeyFieldName = "Key";

        private readonly Directory indexDirectory;
        private readonly IndexWriter indexWriter;

        /// <summary>
        /// Initializes a new instance of the Indexer class.
        /// </summary>
        public Indexer()
        {
            this.indexDirectory = FSDirectory.Open(IndexDirectory);

            this.indexWriter = new IndexWriter(
                this.indexDirectory,
                new AddressAnalyzer(),
                IndexWriter.MaxFieldLength.LIMITED);

            this.indexWriter.SetMaxFieldLength(512);
        }

        /// <summary>
        /// Adds the address to the index, stored against the specified key.
        /// </summary>
        /// <param name="key">The key of the address.</param>
        /// <param name="address">The address text.</param>
        public void Add(int key, string address)
        {
            var document = new Document();

            document.Add(
                new Field(
                    KeyFieldName,
                    BitConverter.GetBytes(key),
                    Field.Store.YES));

            document.Add(
                new Field(
                    AddressFieldName,
                    address,
                    Field.Store.NO,
                    Field.Index.ANALYZED));

            this.indexWriter.AddDocument(document);
        }

        /// <summary>
        /// Releases resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.indexWriter.Dispose();
            this.indexDirectory.Dispose();
        }

        /// <summary>
        /// Called when all the addresses have been added to optimize the index.
        /// </summary>
        public void Optimize()
        {
            this.indexWriter.Optimize();
        }
    }
}
