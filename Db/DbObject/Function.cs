using System;

namespace Db.DbObject
{
    public class Function : Procedure
    {
        public override DbType DbType { get { return DbType.Function; } }
    }
}
