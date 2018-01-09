using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Db.DbObject
{
    public interface IResult : IDbObjectTraverse
    {
        string Name { get; }
        IEnumerable<IColumn> Columns { get; }
    }
}
