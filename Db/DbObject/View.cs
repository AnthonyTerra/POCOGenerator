using System;

namespace Db.DbObject
{
    public class View : Table
    {
        public override DbType DbType { get { return DbType.View; } }
    }
}
