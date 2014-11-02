namespace SharpPaf.Data.Parsers
{
    /// <summary>
    /// Allows the parsing of Welsh address records.
    /// </summary>
    internal sealed class WelshAddressRecordParser : AddressRecordParser
    {
        /// <inheritdoc />
        protected override void ParseLine(PafRepository repository, LineIterator iterator)
        {
            repository.AddWelshAddress(this.ParseRecord(iterator));
        }
    }
}
