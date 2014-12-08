namespace LuceneExample
{
    using System.IO;
    using Lucene.Net.Analysis;

    /// <summary>
    /// A simple analyzer that extracts tokens from PAF data that separates
    /// tokens by spaces.
    /// </summary>
    internal sealed class AddressAnalyzer : Analyzer
    {
        /// <inheritdoc />
        public override TokenStream ReusableTokenStream(string fieldName, TextReader reader)
        {
            var tokenizer = (Tokenizer)this.PreviousTokenStream;
            if (tokenizer == null)
            {
                tokenizer = new AddressTokenizer(reader);
                this.PreviousTokenStream = tokenizer;
            }
            else
            {
                tokenizer.Reset(reader);
            }
            return tokenizer;
        }

        /// <inheritdoc />
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            return new AddressTokenizer(reader);
        }

        private sealed class AddressTokenizer : CharTokenizer
        {
            public AddressTokenizer(TextReader reader)
                : base(reader)
            {
            }

            protected override bool IsTokenChar(char c)
            {
                return c != ' ';
            }
        }
    }
}
