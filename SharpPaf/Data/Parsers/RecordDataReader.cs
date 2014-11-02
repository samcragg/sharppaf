namespace SharpPaf.Data.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Adapts a RecordParser to an IDataReader.
    /// </summary>
    internal sealed class RecordDataReader : IDataReader
    {
#if DEBUG
        private readonly MainfileType fileType;
#endif
        private readonly string[] columnNames;
        private readonly Type[] columnTypes;
        private readonly RecordParser parser;
        private object[] currentLine;
        private IEnumerator<MainfileInfo> fileEnumerator;

        /// <summary>
        /// Initializes a new instance of the RecordDataReader class.
        /// </summary>
        /// <param name="files">The files to iterate over.</param>
        public RecordDataReader(IEnumerable<MainfileInfo> files)
        {
            Check.IsNotNull(files, "files");

            this.fileEnumerator = files.GetEnumerator();
            if (this.fileEnumerator.MoveNext())
            {
                MainfileInfo file = this.fileEnumerator.Current;
#if DEBUG
                this.fileType = file.FileType;
#endif
                this.parser = RecordParserFactory.Create(file.FileType);
                if (parser != null)
                {
                    KeyValuePair<string, Type>[] columns = this.parser.GetColumns();
                    this.columnNames = Array.ConvertAll(columns, kvp => kvp.Key);
                    this.columnTypes = Array.ConvertAll(columns, kvp => kvp.Value);

                    this.parser.LoadFile(file);
                }
            }
        }

        /// <inheritdoc />
        public int FieldCount
        {
            get { return this.columnNames == null ? 0 : this.columnNames.Length; }
        }

        /// <inheritdoc />
        public bool IsClosed
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public object this[string name]
        {
            get { return this[this.GetOrdinal(name)]; }
        }

        /// <inheritdoc />
        public object this[int i]
        {
            get { return this.GetValue(i); }
        }

        /// <inheritdoc />
        public void Close()
        {
            this.Dispose();
            this.IsClosed = true;
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.fileEnumerator.Dispose();
            if (this.parser != null)
            {
                this.parser.Dispose();
            }
        }

        /// <inheritdoc />
        public Type GetFieldType(int i)
        {
            // If we don't have any types then we don't have any columns, so any
            // passed in value will be out of range!
            if (this.columnTypes == null)
            {
                throw new IndexOutOfRangeException();
            }

            return this.columnTypes[i];
        }

        /// <inheritdoc />
        public int GetOrdinal(string name)
        {
            if (this.columnNames == null)
            {
                return -1;
            }

            return Array.IndexOf(this.columnNames, name);
        }

        /// <inheritdoc />
        public object GetValue(int i)
        {
            this.AssertValidCurrentLine();
            return this.currentLine[i] ?? DBNull.Value;
        }

        /// <inheritdoc />
        public int GetValues(object[] values)
        {
            Check.IsNotNull(values, "values");
            this.AssertValidCurrentLine();

            int max = Math.Max(values.Length, this.currentLine.Length);
            for (int i = 0; i < max; i++)
            {
                values[i] = this.GetValue(i);
            }

            return max;
        }

        /// <inheritdoc />
        public bool IsDBNull(int i)
        {
            this.AssertValidCurrentLine();
            return this.currentLine[i] == null;
        }

        /// <inheritdoc />
        public bool Read()
        {
            if (this.parser == null)
            {
                return false;
            }

            this.currentLine = this.parser.Read();
            while (this.currentLine == null)
            {
                if (!this.fileEnumerator.MoveNext())
                {
                    return false;
                }

#if DEBUG
                System.Diagnostics.Debug.Assert(this.fileEnumerator.Current.FileType == this.fileType);
#endif
                this.parser.LoadFile(this.fileEnumerator.Current);
                this.currentLine = this.parser.Read();
            }

            return true;
        }

        private void AssertValidCurrentLine()
        {
            if (this.currentLine == null)
            {
                throw new InvalidOperationException();
            }
        }

        #region Not Implemented Members

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] // Hide from the debugger when debugging
        [ExcludeFromCodeCoverage]
        int IDataReader.Depth
        {
            get { throw new NotImplementedException(); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] // Hide from the debugger when debugging
        [ExcludeFromCodeCoverage]
        int IDataReader.RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        [ExcludeFromCodeCoverage]
        DataTable IDataReader.GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        bool IDataReader.NextResult()
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        bool IDataRecord.GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        byte IDataRecord.GetByte(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        char IDataRecord.GetChar(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        IDataReader IDataRecord.GetData(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        string IDataRecord.GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        DateTime IDataRecord.GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        decimal IDataRecord.GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        double IDataRecord.GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        float IDataRecord.GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        Guid IDataRecord.GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        short IDataRecord.GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        int IDataRecord.GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        long IDataRecord.GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        string IDataRecord.GetName(int i)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        string IDataRecord.GetString(int i)
        {
            throw new NotImplementedException();
        }

        #endregion Not Implemented Members
    }
}
