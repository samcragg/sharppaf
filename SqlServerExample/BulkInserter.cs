namespace SqlServerExample
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using SharpPaf;
    using SharpPaf.Data;

    internal sealed class BulkInserter
    {
        private const string ConnectionString = @"Data Source=.\SQLExpress;Initial Catalog=SharpPaf;Trusted_Connection=True;";
        private readonly static int[,] DefaultMappings = new int[0, 2];
        private readonly TitleCaseConverter converter = new TitleCaseConverter();

        public async Task Insert(Mainfile data)
        {
            // We can prepare this table in memory in parallel with the others
            // but have to save it on its own due to foreign keys
            var addresses = new DataTable();

            await Task.WhenAll(
                this.SaveAddressesToTable(data, MainfileType.Address, addresses),
                this.SaveBuildingNames(data),
                this.SaveLocalities(data),
                this.SaveOrganisations(data),
                this.SaveSubBuildingNames(data),
                this.SaveThoroughfareDescriptors(data),
                this.SaveThoroughfares(data));

            // Now we can save the address table
            await this.SaveAddresses(addresses);
        }

        private static async Task BulkCopyTable(TableInformation info, DataTable table)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (SqlBulkCopy bulk = CreateBulkCopy(connection))
            {
                SetupBulkCopy(info, bulk);

                await connection.OpenAsync();
                await bulk.WriteToServerAsync(table);
            }
        }

        private static void CopyColumns(IDataReader reader, DataTable table)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
        }

        private static void CopyRows(IDataReader reader, DataTable table, Action<object[]> convertValues)
        {
            // In the sample data there are duplicate keys so handle those here.
            HashSet<int> keys = new HashSet<int>();
            int fields = reader.FieldCount;

            table.BeginLoadData();
            while (reader.Read())
            {
                object[] values = new object[fields];
                reader.GetValues(values);
                if (keys.Add((int)values[0]))
                {
                    convertValues(values);

                    DataRow row = table.NewRow();
                    row.ItemArray = values;
                    table.Rows.Add(row);
                }
            }
            table.EndLoadData();
        }

        private static SqlBulkCopy CreateBulkCopy(SqlConnection connection)
        {
            return new SqlBulkCopy(
                connection,
                SqlBulkCopyOptions.UseInternalTransaction,
                null);
        }

        private static void SetupBulkCopy(TableInformation info, SqlBulkCopy bulk)
        {
            bulk.BatchSize = 16384;
            bulk.DestinationTableName = info.Name;

            int length = info.ColumnMappings.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                bulk.ColumnMappings.Add(
                    info.ColumnMappings[i, 0],
                    info.ColumnMappings[i, 1]);
            }
        }

        private async Task BulkCopy(IDataReader reader, TableInformation info)
        {
            try
            {
                DataTable table = await Task.Run(() => this.CreateTable(reader, info.ColumnsToConvert));
                Console.Write('.');

                await BulkCopyTable(info, table);
                Console.Write('.');
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        private void ConvertRows(object[] values, int[] columnsToConvert)
        {
            for (int i = 0; i < columnsToConvert.Length; i++)
            {
                int index = columnsToConvert[i];
                if (values[index] != DBNull.Value)
                {
                    values[index] = this.converter.ToTitleCase((string)values[index]);
                }
            }
        }

        private DataTable CreateTable(IDataReader reader, int[] columnsToConvert)
        {
            var table = new DataTable();
            CopyColumns(reader, table);
            CopyRows(reader, table, values => this.ConvertRows(values, columnsToConvert));
            return table;
        }

        private Task SaveAddresses(DataTable table)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                Name = "Addresses"
            };
            return BulkCopyTable(info, table);
        }

        private Task SaveAddressesToTable(Mainfile data, MainfileType type, DataTable table)
        {
            return Task.Run(() =>
            {
                using (IDataReader reader = data.CreateReader(type))
                {
                    CopyColumns(reader, table);
                    CopyRows(
                        reader,
                        table,
                        values =>
                        {
                            string postcode = values[11] as string;
                            if ((postcode != null) && (postcode.Length > 3))
                            {
                                values[11] = postcode.Insert(postcode.Length - 3, " ");
                            }
                        });
                }
            });
        }

        private Task SaveBuildingNames(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1 },
                Name = "BuildingNames"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.BuildingNames), info);
        }

        private Task SaveLocalities(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1, 2 },
                Name = "Localities"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.Localities), info);
        }

        private Task SaveOrganisations(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1, 2 },
                Name = "Organisations"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.Organisations), info);
        }

        private Task SaveSubBuildingNames(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1 },
                Name = "SubBuildingNames"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.SubBuildingNames), info);
        }

        private Task SaveThoroughfareDescriptors(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1 },
                Name = "ThoroughfareDescriptors"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.ThoroughfareDescriptors), info);
        }

        private Task SaveThoroughfares(Mainfile data)
        {
            TableInformation info = new TableInformation
            {
                ColumnMappings = DefaultMappings,
                ColumnsToConvert = new[] { 1 },
                Name = "Thoroughfares"
            };
            return this.BulkCopy(data.CreateReader(MainfileType.Thoroughfares), info);
        }

        private struct TableInformation
        {
            public int[,] ColumnMappings { get; set; }

            public int[] ColumnsToConvert { get; set; }

            public string Name { get; set; }
        }
    }
}
