using System;
using System.Collections.Generic;

namespace Db.DbObject
{
    public class ForeignKey : IDbObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Is_One_To_One { get; set; }
        public bool Is_One_To_Many { get; set; }
        public bool Is_Many_To_Many { get; set; }
        public bool Is_Many_To_Many_Complete { get; set; }
        public bool Is_Cascade_Delete { get; set; }
        public bool Is_Cascade_Update { get; set; }
        public string Foreign_Schema_Id { get; set; }
        public string Foreign_Schema { get; set; }
        public string Foreign_Table_Id { get; set; }
        public string Foreign_Table { get; set; }
        public string Foreign_Column_Id { get; set; }
        public string Foreign_Column { get; set; }
        public bool Is_Foreign_PK { get; set; }
        public string Primary_Schema_Id { get; set; }
        public string Primary_Schema { get; set; }
        public string Primary_Table_Id { get; set; }
        public string Primary_Table { get; set; }
        public string Primary_Column_Id { get; set; }
        public string Primary_Column { get; set; }
        public bool Is_Primary_PK { get; set; }
        public int Ordinal { get; set; }

        public Database Database { get; set; }

        public Table FromTable { get; set; }
        public Table ToTable { get; set; }

        public NavigationProperty NavigationPropertyRefFrom { get; set; }
        public NavigationProperty NavigationPropertyRefTo { get; set; }
        public IEnumerable<NavigationProperty> NavigationPropertiesRefToManyToMany { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
