using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Db.Helpers;

namespace Db.DbObject
{
    public class Procedure : IDbResultTraverse
    {
        public string routine_schema { get; set; }
        public string routine_name { get; set; }

        public Database Database { get; set; }
        public List<ProcedureParameter> ProcedureParameters { get; set; }
        public List<ProcedureResult> ProcedureResults { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return routine_schema + "." + routine_name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return routine_schema; } }
        public string Name { get { return routine_name; } }
        public IEnumerable<IResult> Results => this.ProcedureResults;

        public virtual DbType DbType { get { return DbType.Procedure; } }

        #endregion
    }

    public class ProcedureResult : IDbColumnTraverse, IResult
    {
        public Procedure ParentProcedure { get; set; }
        public List<ProcedureColumn> ProcedureColumns { get; set; }
        public int ItemNumber { get; set; }
        #region IDbObjectTraverse Members
        public string Schema => ParentProcedure.Schema;
        public string Name
        {
            get
            {
                if(ParentProcedure.ProcedureResults.Count>1)
                    return ParentProcedure.Name + this.ItemNumber.GetOrdinalIdentifier();

                return ParentProcedure.Name;
            }
        }
        
        public IEnumerable<IColumn> Columns => ProcedureColumns?.Cast<IColumn>();
        public Exception Error { get; }
        public Database Database => ParentProcedure.Database;
        public string ClassName { get; set; }
        #endregion
        public virtual DbType DbType => DbType.ProcedureResultSet;
        public override string ToString()
        {
            return ParentProcedure.Schema + "." + this.Name;
        }

        
    }
}
