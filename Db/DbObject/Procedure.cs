using System;
using System.Collections.Generic;
using System.Linq;

namespace Db.DbObject
{
    public class Procedure : IDbObjectTraverse
    {
        public string routine_schema { get; set; }
        public string routine_name { get; set; }

        public Database Database { get; set; }
        public List<ProcedureParameter> ProcedureParameters { get; set; }
        public List<ProcedureColumn> ProcedureColumns { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return routine_schema + "." + routine_name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return routine_schema; } }
        public string Name { get { return routine_name; } }
        public IEnumerable<IColumn> Columns { get { return (ProcedureColumns != null ? ProcedureColumns.Cast<IColumn>() : null); } }
        public virtual DbType DbType { get { return DbType.Procedure; } }

        #endregion
    }
}
