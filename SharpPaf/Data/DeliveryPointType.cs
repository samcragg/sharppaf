namespace SharpPaf.Data
{
    /// <summary>
    /// Represents the type of delivery point for an organisation.
    /// </summary>
    public enum DeliveryPointType : byte
    {
        /// <summary>
        /// The delivery point is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// A number of small user organisations and/or residential addresses
        /// can share a single Postcode.
        /// </summary>
        /// <remarks>
        /// The minimum number of delivery points on a Postcode is one, the
        /// average is fifteen, and the maximum is ninety-nine.
        /// </remarks>
        SmallUser,

        /// <summary>
        /// A large user organisation which has their own Postcode.
        /// </summary>
        /// <remarks>
        /// Large user organisation receiving a minimum of a thousand or more
        /// items of mail a day.
        /// </remarks>
        LargeUser
    }
}
