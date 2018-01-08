using System;
using System.Collections.Generic;
using Db.DbObject;

namespace Db.POCOIterator
{
    public abstract class POCOIteratorFactory
    {
        public abstract IPOCOIterator CreateIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter);
    }

    public class DbIteratorFactory : POCOIteratorFactory
    {
        public override IPOCOIterator CreateIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
        {
            return new DbIterator(dbObjects, pocoWriter);
        }
    }

    public class EFIteratorFactory : POCOIteratorFactory
    {
        public override IPOCOIterator CreateIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
        {
            return new EFIterator(dbObjects, pocoWriter);
        }
    }
}
