namespace SharpPaf
{
    using System;
    using System.Collections.Generic;

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
            AddIfNotEmpty(lines, data.Postcode);
            AddIfNotEmpty(lines, data.PostTown);
            AddIfNotEmpty(lines, data.DependentLocality);
            AddIfNotEmpty(lines, data.DoubleDependentLocality);
            AddIfNotEmpty(lines, FormatThoroughfare(data.ThoroughfareName, data.ThoroughfareDescriptor));
            AddIfNotEmpty(lines, FormatThoroughfare(data.DependentThoroughfareName, data.DependentThoroughfareDescriptor));

            if (!string.IsNullOrWhiteSpace(data.POBoxNumber))
            {
                AddPOBox(lines, data.POBoxNumber);
            }
            else
            {
                AddPremisesElements(lines, data);
            }

            AddIfNotEmpty(lines, data.DepartmentName);
            AddIfNotEmpty(lines, data.OrganisationName);

            return CopyToReversedArray(lines);
        }

        private static void AddIfNotEmpty(List<string> lines, string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Length > 0)
                {
                    lines.Add(value);
                }
            }
        }

        private static void AddPOBox(List<string> lines, string number)
        {
            lines.Add("PO Box " + number.Trim().ToUpperInvariant());
        }

        private static void AddPremisesElements(List<string> lines, IPafData data)
        {
            BuildingInfo info = ProcessExceptionRules(data);
            PrependIfSpecified(lines, info.BuildingNumber);
            AddIfNotEmpty(lines, info.BuildingName);
            PrependIfSpecified(lines, info.SubBuildingNumber);
            AddIfNotEmpty(lines, info.SubBuildingName);
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

        private static void ProcessBuildingNumberAndSubBuildingName(IPafData data, BuildingInfo info)
        {
            if (data.ConcatenateBuildingNumber)
            {
                info.BuildingNumber += data.SubBuildingName;
                info.SubBuildingName = null;
                info.SubBuildingNumber = null;
            }
        }

        private static BuildingInfo ProcessExceptionRules(IPafData data)
        {
            var info = new BuildingInfo();
            info.BuildingNumber = data.BuildingNumber;
            ProcessExceptions(data.BuildingName, ref info.BuildingName, ref info.BuildingNumber);
            ProcessExceptions(data.SubBuildingName, ref info.SubBuildingName, ref info.SubBuildingNumber);
            ProcessBuildingNumberAndSubBuildingName(data, info);

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

        private class BuildingInfo
        {
            public string BuildingName;
            public string BuildingNumber;
            public string SubBuildingName;
            public string SubBuildingNumber;
        }
    }
}
