using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Db.DbObject
{
    public class ProcedureCollection : IDbObject
    {
        public List<Procedure> Procedures { get; set; }
    }
}
