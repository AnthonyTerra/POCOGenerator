using System;

namespace Db.DbObject
{
    public interface IComplexType : IColumn
    {
    }

    public class ComplexType : IComplexType
    {
        public string ComplexTypeName { get; set; }

        public ComplexType(string complexTypeName, TableColumn tableColumn)
        {
            ComplexTypeName = complexTypeName;
            ColumnOrdinal = tableColumn.ColumnOrdinal;
        }

        public override string ToString()
        {
            return ComplexTypeName;
        }

        #region IColumn Members

        public string ColumnName { get { return ComplexTypeName + "_CT"; } }
        public int? ColumnOrdinal { get; set; }
        public string DataTypeName { get { return ComplexTypeName; } }
        public string DataTypeDisplay { get { return ComplexTypeName; } }
        public string Precision { get { return null; } }
        public int? StringPrecision { get { return null; } }
        public short? NumericPrecision { get { return null; } }
        public int? NumericScale { get { return null; } }
        public int? DateTimePrecision { get { return null; } }
        public bool IsNullable { get { return true; } }
        public bool? IsIdentity { get { return false; } }
        public bool IsComputed { get { return false; } }
        public bool IsPrimaryKey { get { return false; } }
        public bool HasUniqueKeys { get { return false; } }
        public bool HasForeignKeys { get { return false; } }
        public bool HasPrimaryForeignKeys { get { return false; } }
        public bool HasIndexColumns { get { return false; } }

        #endregion
    }
}
