using System;
using System.Collections.Generic;
using System.Linq;

namespace Db.DbObject
{
    public class Table : IDbObjectTraverse
    {
        public string table_schema { get; set; }
        public string table_name { get; set; }

        public Database Database { get; set; }
        public List<TableColumn> TableColumns { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public List<ExtendedProperty> ExtendedProperties { get; set; }
        public bool HasExtendedProperties { get { return ExtendedProperties != null && ExtendedProperties.Count > 0; } }
        
        public override string ToString()
        {
            return table_schema + "." + table_name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return table_schema; } }
        public string Name { get { return table_name; } }
        public IEnumerable<IColumn> Columns { get { return (TableColumns != null ? TableColumns.Cast<IColumn>() : null); } }
        public virtual DbType DbType { get { return DbType.Table; } }

        #endregion
    }
}
