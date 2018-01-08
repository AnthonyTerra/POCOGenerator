using System;
using System.Collections.Generic;

namespace Db.DbObject
{
    public class TableColumn : IColumn
    {
        public string table_catalog { get; set; }
        public string table_schema { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
        public int? ordinal_position { get; set; }
        public string column_default { get; set; }
        public string is_nullable { get; set; }
        public string data_type { get; set; }
        public int? character_maximum_length { get; set; }
        public int? character_octet_length { get; set; }
        public byte? numeric_precision { get; set; }
        public short? numeric_precision_radix { get; set; }
        public int? numeric_scale { get; set; }
        public short? datetime_precision { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_schema { get; set; }
        public string character_set_name { get; set; }
        public string collation_catalog { get; set; }
        public bool? is_sparse { get; set; }
        public bool? is_column_set { get; set; }
        public bool? is_filestream { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }

        public Table Table { get; set; }

        public PrimaryKey PrimaryKey { get; set; }
        public bool IsPrimaryKey { get { return PrimaryKey != null; } }

        public List<UniqueKey> UniqueKeys { get; set; }
        public bool HasUniqueKeys { get { return UniqueKeys != null && UniqueKeys.Count > 0; } }

        public List<ForeignKey> ForeignKeys { get; set; }
        public bool HasForeignKeys { get { return ForeignKeys != null && ForeignKeys.Count > 0; } }

        public List<ForeignKey> PrimaryForeignKeys { get; set; }
        public bool HasPrimaryForeignKeys { get { return PrimaryForeignKeys != null && PrimaryForeignKeys.Count > 0; } }

        public List<IndexColumn> IndexColumns { get; set; }
        public bool HasIndexColumns { get { return IndexColumns != null && IndexColumns.Count > 0; } }

        public List<ExtendedProperty> ExtendedProperties { get; set; }
        public bool HasExtendedProperties { get { return ExtendedProperties != null && ExtendedProperties.Count > 0; } }

        public override string ToString()
        {
            return column_name + " (" + DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        public string ToStringFull()
        {
            return
                column_name + " (" +
                (IsPrimaryKey ? "PK, " : string.Empty) +
                (HasForeignKeys ? "FK, " : string.Empty) +
                (is_computed ? "Computed, " : string.Empty) +
                DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #region IColumn Members

        public string ColumnName { get { return column_name; } }
        public string DataTypeName { get { return data_type; } }
        public int? StringPrecision { get { return character_maximum_length; } }
        public short? NumericPrecision { get { return numeric_precision; } }
        public int? NumericScale { get { return numeric_scale; } }
        public int? DateTimePrecision { get { return datetime_precision; } }
        public bool IsNullable { get { return (is_nullable == "YES"); } }
        public int? ColumnOrdinal { get { return ordinal_position; } }
        public bool? IsIdentity { get { return is_identity; } }
        public bool IsComputed { get { return is_computed; } }

        public string DataTypeDisplay
        {
            get
            {
                if (data_type == "xml")
                    return "XML";
                return data_type;
            }
        }

        public string Precision
        {
            get
            {
                string precision = null;

                string data_type = this.data_type.ToLower();

                if (data_type == "binary" || data_type == "char" || data_type == "nchar" || data_type == "nvarchar" || data_type == "varbinary" || data_type == "varchar")
                {
                    if (character_maximum_length == -1)
                        precision = "(max)";
                    else if (character_maximum_length > 0)
                        precision = "(" + character_maximum_length + ")";
                }
                else if (data_type == "decimal" || data_type == "numeric")
                {
                    precision = "(" + numeric_precision + "," + numeric_scale + ")";
                }
                else if (data_type == "datetime2" || data_type == "datetimeoffset" || data_type == "time")
                {
                    precision = "(" + datetime_precision + ")";
                }
                else if (data_type == "xml")
                {
                    precision = "(.)";
                }

                return precision;
            }
        }

        #endregion
    }
}
