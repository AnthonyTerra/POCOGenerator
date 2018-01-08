using System;
using System.Collections.Generic;

namespace Db.DbObject
{
    public class IndexColumn : IDbObject
    {
        public string Name { get; set; }
        public string Schema_Name { get; set; }
        public string Table_Name { get; set; }
        public bool? Is_Unique { get; set; }
        public bool? Is_Clustered { get; set; }
        public byte Ordinal { get; set; }
        public string Column_Name { get; set; }
        public bool? Is_Descending { get; set; }

        public Database Database { get; set; }

        public List<ExtendedProperty> ExtendedProperties { get; set; }
        public bool HasExtendedProperties { get { return ExtendedProperties != null && ExtendedProperties.Count > 0; } }

        public override string ToString()
        {
            return Name;
        }

        public string ToStringFull()
        {
            return
                Name + " (" +
                (Is_Unique == true ? "unique" : "not unique") + ", " +
                (Is_Clustered == true ? "clustered" : "not clustered") + ")";
        }
    }
}
