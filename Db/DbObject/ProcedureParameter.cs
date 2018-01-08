using System;

namespace Db.DbObject
{
    public class ProcedureParameter : IDbObject
    {
        public string specific_catalog { get; set; }
        public string specific_schema { get; set; }
        public string specific_name { get; set; }
        public int? ordinal_position { get; set; }
        public string parameter_mode { get; set; }
        public string is_result { get; set; }
        public string as_locator { get; set; }
        public string parameter_name { get; set; }
        public string data_type { get; set; }
        public int? character_maximum_length { get; set; }
        public int? character_octet_length { get; set; }
        public string collation_catalog { get; set; }
        public string collation_schema { get; set; }
        public string collation_name { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_schema { get; set; }
        public string character_set_name { get; set; }
        public byte? numeric_precision { get; set; }
        public short? numeric_precision_radix { get; set; }
        public int? numeric_scale { get; set; }
        public short? datetime_precision { get; set; }
        public string interval_type { get; set; }
        public short? interval_precision { get; set; }

        public Procedure Procedure { get; set; }

        public string DataTypeDisplay
        {
            get
            {
                if (data_type == "xml")
                    return "XML";
                return data_type;
            }
        }

        public string PrecisionDisplay
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

        public string Direction
        {
            get
            {
                if (parameter_mode == "IN")
                    return "Input";
                else if (parameter_mode == "INOUT")
                    return "Input/Output";
                else if (parameter_mode == "OUT")
                    return "Output";
                return null;
            }
        }

        public bool IsResult
        {
            get { return (is_result == "YES"); }
        }

        public override string ToString()
        {
            if (IsResult)
                return "Returns " + DataTypeDisplay + PrecisionDisplay;
            return parameter_name + " (" + DataTypeDisplay + PrecisionDisplay + ", " + Direction + ")";
        }
    }
}
