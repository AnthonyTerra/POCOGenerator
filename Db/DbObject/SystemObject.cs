using System;

namespace Db.DbObject
{
    public class SystemObject : IDbObject
    {
        public string type { get; set; }
        public string object_schema { get; set; }
        public string object_name { get; set; }

        public Database Database { get; set; }
        public Exception Error { get; set; }

        public override string ToString()
        {
            return object_schema + "." + object_name;
        }
    }
}
