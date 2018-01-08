using System;

namespace Db.DbObject
{
    public class ExtendedProperty : IDbObject
    {
        // database: Class = 0, Schema_Id = 0, Table_Id = 0, Id
        // schema: Class = 3, Schema_Id, Table_Id = 0, Id = 0
        // table: Class = 1, Schema_Id, Table_Id, Id = 0
        // table column: Class = 1, Schema_Id, Table_Id, Id
        // index: Class = 7, Schema_Id, Table_Id, Id

        public byte Class { get; set; }
        public string Class_Desc { get; set; }
        public int Schema_Id { get; set; }
        public int Table_Id { get; set; }
        public int Id { get; set; }
        public string Schema_Name { get; set; }
        public string Table_Name { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Database Database { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
