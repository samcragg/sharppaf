namespace SharpPaf
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Allows the formatting of PAF data for printing.
    /// </summary>
    public sealed class AddressFormatter
    {
        /// <summary>
        /// Formats the specified data into printable lines.
        /// </summary>
        /// <param name="data">The raw PAF data.</param>
        /// <returns>
        /// An array of lines representing the specified data.
        /// </returns>
        /// <exception cref="ArgumentNullException">data is null.</exception>
        public string[] Format(IPafData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var lines = new List<string>(9);
            AddUpperCase(lines, data.Postcode);
            AddUpperCase(lines, data.PostTown);
            AddTitleCase(lines, data.DependentLocality);
            AddTitleCase(lines, data.DoubleDependentLocality);
            AddTitleCase(lines, FormatThoroughfare(data.ThoroughfareName, data.ThoroughfareDescriptor));
            AddTitleCase(lines, FormatThoroughfare(data.DependentThoroughfareName, data.DependentThoroughfareDescriptor));

            if (!string.IsNullOrWhiteSpace(data.POBoxNumber))
            {
                AddPOBox(lines, data.POBoxNumber);
            }
            else
            {
                AddPremisesElements(lines, data);
            }

            AddTitleCase(lines, data.DepartmentName);
            AddTitleCase(lines, data.OrganisationName);

            return CopyToReversedArray(lines);
        }

        private static void AddPOBox(List<string> lines, string number)
        {
            lines.Add("PO Box " + number.Trim().ToUpperInvariant());
        }

        private static void AddPremisesElements(List<string> lines, IPafData data)
        {
            BuildingInfo info = ProcessExceptionRules(data);
            PrependIfSpecified(lines, info.BuildingNumber);
            AddTitleCase(lines, info.BuildingName);
            PrependIfSpecified(lines, info.SubBuildingNumber);
            AddTitleCase(lines, info.SubBuildingName);
        }

        private static void AddTitleCase(List<string> lines, string value)
        {
            if (value != null)
            {
                value = TrimAndFormatToTitleCase(value);
                if (value.Length > 0)
                {
                    lines.Add(value);
                }
            }
        }

        private static void AddUpperCase(List<string> lines, string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Length > 0)
                {
                    lines.Add(value.ToUpperInvariant());
                }
            }
        }

        private static bool BuildingNameBelongsWithNumber(string name)
        {
            // Table 24
            switch (name.ToUpperInvariant())
            {
                case "BACK OF":
                case "BLOCK":
                case "BLOCKS":
                case "BUILDING":
                case "MAISONETTE":
                case "MAISONETTES":
                case "REAR OF":
                case "SHOP":
                case "SHOPS":
                case "STALL":
                case "STALLS":
                case "SUITE":
                case "SUITES":
                case "UNIT":
                case "UNITS":
                    return true;

                default:
                    return false;
            }
        }

        private static string[] CopyToReversedArray(IList<string> source)
        {
            string[] output = new string[source.Count];

            int index = source.Count - 1;
            for (int i = 0; i < source.Count; i++)
            {
                output[index] = source[i];
                index--;
            }

            return output;
        }

        private static string FormatThoroughfare(string name, string descriptor)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                if (!string.IsNullOrWhiteSpace(descriptor))
                {
                    return name + " " + descriptor.Trim();
                }
            }

            return name;
        }

        private static bool HasNumericAlphaSuffix(string value, int startIndex)
        {
            // First and penultimate characters are numeric, last character is alphabetic (eg '12A')
            if ((value.Length - startIndex) < 2)
            {
                return false;
            }

            return IsNumeric(value[startIndex]) && IsNumeric(value[value.Length - 2]);
        }

        private static bool HasNumericRangeSuffix(string value, int startIndex)
        {
            // First and last characters of the Building Name are numeric (eg '1to1' or '100:1')
            return IsNumeric(value[startIndex]) && IsNumeric(value[value.Length - 1]);
        }

        private static bool IsNumeric(char value)
        {
            return (value >= '0') && (value <= '9');
        }

        private static bool IsNumericSuffix(string value, int startIndex)
        {
            for (int i = startIndex; i < value.Length; i++)
            {
                if (!IsNumeric(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static void PrependIfSpecified(List<string> lines, string value)
        {
            if ((value != null) && (lines.Count > 0))
            {
                value = value.Trim();
                if (value.Length > 0)
                {
                    lines[lines.Count - 1] = value + " " + lines[lines.Count - 1];
                }
            }
        }

        private static BuildingInfo ProcessExceptionRules(IPafData data)
        {
            var info = new BuildingInfo();
            info.BuildingNumber = data.BuildingNumber;
            ProcessExceptions(data.BuildingName, ref info.BuildingName, ref info.BuildingNumber);
            ProcessExceptions(data.SubBuildingName, ref info.SubBuildingName, ref info.SubBuildingNumber);

            if (info.BuildingName != null)
            {
                SplitBuildingNumberFromBuildingName(info);
            }

            return info;
        }

        private static void ProcessExceptions(string originalName, ref string name, ref string number)
        {
            if (originalName == null)
            {
                return;
            }

            originalName = originalName.Trim();
            if (originalName.Length == 0)
            {
                return;
            }

            if (HasNumericRangeSuffix(originalName, 0) ||
                HasNumericAlphaSuffix(originalName, 0))
            {
                number = originalName;
            }
            else if (originalName.Length == 1)
            {
                // Building Name has only one character (eg 'A')
                number = originalName + ",";
            }
            else
            {
                name = originalName;
            }
        }

        private static bool ShouldSplitName(string name, int index)
        {
            if (IsNumericSuffix(name, index))
            {
                // The Building Name is NOT split if the numeric part is simply
                // a number between 1 and 9999 [...] because the building number
                // field would have been populated if it were a true building
                // number, so it is assumed that thisis an integral part of the
                // building name.
                return (name.Length - index) > 4;
            }

            return HasNumericRangeSuffix(name, index) ||
                   HasNumericAlphaSuffix(name, index);
        }

        private static void SplitBuildingNumberFromBuildingName(BuildingInfo info)
        {
            string name = info.BuildingName;
            int space = name.LastIndexOf(' ') + 1;
            if (space == 0)
            {
                return;
            }

            // Trim should have already been called on the name before passing
            // in to this method so space should never point to the end.
            System.Diagnostics.Debug.Assert(space != name.Length);

            if (ShouldSplitName(name, space))
            {
                string namePart = name.Substring(0, space - 1).Trim(); // -1 because we added one to space
                if (!BuildingNameBelongsWithNumber(namePart))
                {
                    info.BuildingName = namePart;
                    info.BuildingNumber = name.Substring(space);
                }
            }
        }

        private static char ToLower(char c)
        {
            // Since PAF only contains ASCII text, this is quicker than
            // char.ToLowerInvariant
            if ((c >= 'A') && (c <= 'Z'))
            {
                return (char)('a' + (c - 'A'));
            }

            return c;
        }

        private static char ToUpper(char c)
        {
            // As with ToLower, this is quicker than char.ToUpperInvariant
            if ((c >= 'a') && (c <= 'z'))
            {
                return (char)('A' + (c - 'a'));
            }

            return c;
        }

        private static string TrimAndFormatToTitleCase(string value)
        {
            int start = -1;
            do
            {
                if (++start == value.Length)
                {
                    return string.Empty;
                }
            } while (char.IsWhiteSpace(value[start]));

            int end;
            for (end = value.Length - 1; end >= start; end--)
            {
                if (!char.IsWhiteSpace(value[end]))
                {
                    break;
                }
            }

            var sb = new StringBuilder(value.Length);
            sb.Append(ToUpper(value[start]));

            for (int i = start + 1; i <= end; i++)
            {
                char previous = value[i - 1];
                char current = value[i];

                sb.Append(char.IsWhiteSpace(previous) ?
                    ToUpper(current) :
                    ToLower(current));
            }

            return sb.ToString();
        }

        private class BuildingInfo
        {
            public string BuildingName;
            public string BuildingNumber;
            public string SubBuildingName;
            public string SubBuildingNumber;
        }
    }
}
