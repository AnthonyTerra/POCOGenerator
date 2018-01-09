using System;
using System.Collections.Generic;

namespace Db.DbObject
{
    public interface IDbObjectTraverse : IDbObject
    {
        string Schema { get; }
        string Name { get; }
        DbType DbType { get; }
        Exception Error { get; }
        Database Database { get; }
        string ClassName { get; set; }
    }

    public interface IDbColumnTraverse : IDbObjectTraverse
    {
        IEnumerable<IColumn> Columns { get; }
    }
    public interface IDbResultTraverse : IDbObjectTraverse
    {
        IEnumerable<IResult> Results { get; }
    }
}
