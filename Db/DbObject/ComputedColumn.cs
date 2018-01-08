using System;

namespace Db.DbObject
{
    public class ComputedColumn : IDbObject
    {
        public string Schema_Name { get; set; }
        public string Table_Name { get; set; }
        public string Column_Name { get; set; }

        public Database Database { get; set; }

        public override string ToString()
        {
            return Column_Name;
        }
    }
}
