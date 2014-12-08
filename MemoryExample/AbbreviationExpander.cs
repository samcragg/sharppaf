namespace SqlServerExample
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Expands common abbreviations in thoroughfare names (such as Rd to Road).
    /// </summary>
    internal sealed class AbbreviationExpander
    {
        private static readonly Dictionary<string, string> Replacements =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Av", "Avenue" },
                { "Ave", "Avenue" },
                { "Cl", "Close" },
                { "Cres", "Crescent" },
                { "Ct", "Court" },
                { "Dr", "Drive" },
                { "Est", "Estate" },
                { "Gdns", "Gardens" },
                { "Gr", "Grove" },
                { "La", "Lane" },
                { "Pde", "Parade" },
                { "Pk", "Park" },
                { "Pl", "Place" },
                { "Rd", "Road" },
                { "Sq", "Square" },
                { "St", "Street" },
                { "Ter", "Terrace" }
            };

        private static readonly Regex WordsRegex = new Regex(@"\b[A-Za-z]+\b");

        public string Expand(string input)
        {
            var builder = new StringBuilder(input);
            MatchCollection words = WordsRegex.Matches(input);
            for (int i = words.Count; i > 0; i--)
            {
                Match word = words[i - 1];
                string replacement;
                if (Replacements.TryGetValue(word.Value, out replacement))
                {
                    builder.Remove(word.Index, word.Length);
                    builder.Insert(word.Index, replacement);
                }
            }

            return builder.ToString();
        }
    }
}
