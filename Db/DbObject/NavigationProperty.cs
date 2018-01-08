using System;

namespace Db.DbObject
{
    public class NavigationProperty : IDbObject
    {
        public ForeignKey ForeignKey { get; set; }
        public bool IsRefFrom { get; set; }
        public bool IsSingle { get; set; }
        public string PropertyName { get; set; }

        public NavigationProperty InverseProperty { get; set; }

        public string RenamedPropertyName { get; set; }
        public string ClassName { get; set; }
        public bool HasMultipleRelationships { get; set; }

        public override string ToString()
        {
            return (string.IsNullOrEmpty(RenamedPropertyName) ? PropertyName : RenamedPropertyName);
        }
    }
}
