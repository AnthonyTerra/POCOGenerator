using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Db.DbObject
{
    public class TVP : IDbObjectTraverse
    {
        public string tvp_schema { get; set; }
        public string tvp_name { get; set; }
        public int type_table_object_id { get; set; }

        public Database Database { get; set; }
        public List<TVPColumn> TVPColumns { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public DataTable TVPDataTable { get; set; }

        public override string ToString()
        {
            return tvp_schema + "." + tvp_name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return tvp_schema; } }
        public string Name { get { return tvp_name; } }
        public IEnumerable<IColumn> Columns { get { return (TVPColumns != null ? TVPColumns.Cast<IColumn>() : null); } }
        public DbType DbType { get { return DbType.TVP; } }

        #endregion
    }
}
