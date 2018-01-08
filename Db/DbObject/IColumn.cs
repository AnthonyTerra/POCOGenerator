using System;

namespace Db.DbObject
{
    public interface IColumn : IDbObject
    {
        string ColumnName { get; }
        int? ColumnOrdinal { get; }
        string DataTypeName { get; }
        string DataTypeDisplay { get; }
        string Precision { get; }
        int? StringPrecision { get; }
        short? NumericPrecision { get; }
        int? NumericScale { get; }
        int? DateTimePrecision { get; }
        bool IsNullable { get; }
        bool? IsIdentity { get; }
        bool IsComputed { get; }
        bool IsPrimaryKey { get; }
        bool HasUniqueKeys { get; }
        bool HasForeignKeys { get; }
        bool HasPrimaryForeignKeys { get; }
        bool HasIndexColumns { get; }
    }
}
