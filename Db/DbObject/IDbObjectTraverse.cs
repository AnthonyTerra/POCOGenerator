using System;
using System.Collections.Generic;

namespace Db.DbObject
{
    public interface IDbObjectTraverse : IDbObject
    {
        string Schema { get; }
        string Name { get; }
        IEnumerable<IColumn> Columns { get; }
        DbType DbType { get; }
        Exception Error { get; }
        Database Database { get; }
        string ClassName { get; set; }
    }
}
