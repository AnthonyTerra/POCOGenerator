using System;

namespace Db.DbObject
{
    public class PrimaryKey : IDbObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Schema_Id { get; set; }
        public string Schema_Name { get; set; }
        public int Table_Id { get; set; }
        public string Table_Name { get; set; }
        public byte Ordinal { get; set; }
        public int Column_Id { get; set; }
        public string Column_Name { get; set; }
        public bool? Is_Descending { get; set; }
        public bool Is_Identity { get; set; }
        public bool Is_Computed { get; set; }

        public Database Database { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
