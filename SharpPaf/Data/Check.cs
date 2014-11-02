namespace SharpPaf.Data
{
    using System;

    /// <summary>
    /// Provides methods to help validate arguments.
    /// </summary>
    internal static class Check
    {
        /// <summary>
        /// Verifies the specified argument is not null.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="name">The name of the argument.</param>
        public static void IsNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
