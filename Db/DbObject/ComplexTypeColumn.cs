using System;

namespace Db.DbObject
{
    public class ComplexTypeColumn : TableColumn
    {
        public string ComplexTypeName { get; set; }
        public string ComplexTypeColumnName { get; set; }

        public ComplexTypeColumn(string complexTypeName, string complexTypeColumnName, TableColumn tableColumn)
            : base()
        {
            ComplexTypeName = complexTypeName;
            ComplexTypeColumnName = complexTypeColumnName;
            column_name = complexTypeColumnName;

            table_catalog = tableColumn.table_catalog;
            table_schema = tableColumn.table_schema;
            table_name = tableColumn.table_name;
            ordinal_position = tableColumn.ordinal_position;
            column_default = tableColumn.column_default;
            is_nullable = tableColumn.is_nullable;
            data_type = tableColumn.data_type;
            character_maximum_length = tableColumn.character_maximum_length;
            character_octet_length = tableColumn.character_octet_length;
            numeric_precision = tableColumn.numeric_precision;
            numeric_precision_radix = tableColumn.numeric_precision_radix;
            numeric_scale = tableColumn.numeric_scale;
            datetime_precision = tableColumn.datetime_precision;
            character_set_catalog = tableColumn.character_set_catalog;
            character_set_schema = tableColumn.character_set_schema;
            character_set_name = tableColumn.character_set_name;
            collation_catalog = tableColumn.collation_catalog;
            is_sparse = tableColumn.is_sparse;
            is_column_set = tableColumn.is_column_set;
            is_filestream = tableColumn.is_filestream;
            is_identity = tableColumn.is_identity;
            is_computed = tableColumn.is_computed;
            Table = tableColumn.Table;
            PrimaryKey = tableColumn.PrimaryKey;
            ForeignKeys = tableColumn.ForeignKeys;
            IndexColumns = tableColumn.IndexColumns;
        }
    }
}
