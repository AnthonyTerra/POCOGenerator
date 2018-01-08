using System;

namespace Db.DbObject
{
    public class TVPColumn : IColumn
    {
        public string data_type { get; set; }
        public int object_id { get; set; }
        public string name { get; set; }
        public int column_id { get; set; }
        public byte system_type_id { get; set; }
        public int user_type_id { get; set; }
        public short max_length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public string collation_name { get; set; }
        public bool? is_nullable { get; set; }
        public bool is_ansi_padded { get; set; }
        public bool is_rowguidcol { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }
        public bool is_filestream { get; set; }
        public bool? is_replicated { get; set; }
        public bool? is_non_sql_subscribed { get; set; }
        public bool? is_merge_published { get; set; }
        public bool? is_dts_replicated { get; set; }
        public bool is_xml_document { get; set; }
        public int xml_collection_id { get; set; }
        public int default_object_id { get; set; }
        public int rule_object_id { get; set; }
        public bool? is_sparse { get; set; }
        public bool? is_column_set { get; set; }

        public TVP TVP { get; set; }

        public override string ToString()
        {
            return name + " (" + DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #region IColumn Members

        public string ColumnName { get { return name; } }
        public string DataTypeName { get { return data_type; } }
        public short? NumericPrecision { get { return precision; } }
        public int? NumericScale { get { return scale; } }
        public int? DateTimePrecision { get { return scale; } }
        public bool IsNullable { get { return (is_nullable.HasValue && is_nullable.Value); } }
        public int? ColumnOrdinal { get { return column_id; } }
        public bool? IsIdentity { get { return is_identity; } }
        public bool IsComputed { get { return is_computed; } }
        public bool IsPrimaryKey { get { return false; } }
        public bool HasUniqueKeys { get { return false; } }
        public bool HasForeignKeys { get { return false; } }
        public bool HasPrimaryForeignKeys { get { return false; } }
        public bool HasIndexColumns { get { return false; } }

        public int? StringPrecision
        {
            get
            {
                if (max_length > 0)
                    return (data_type.ToLower() == "nchar" || data_type.ToLower() == "nvarchar" ? max_length / 2 : max_length);
                else
                    return max_length;
            }
        }

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
                    if (max_length == -1)
                        precision = "(max)";
                    else if (max_length > 0)
                        precision = "(" + (data_type == "nchar" || data_type == "nvarchar" ? max_length / 2 : max_length) + ")";
                }
                else if (data_type == "decimal" || data_type == "numeric")
                {
                    precision = "(" + precision + "," + scale + ")";
                }
                else if (data_type == "datetime2" || data_type == "datetimeoffset" || data_type == "time")
                {
                    precision = "(" + scale + ")";
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
