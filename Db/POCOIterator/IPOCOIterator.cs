using System;

namespace Db.POCOIterator
{
    public interface IPOCOIterator
    {
        void Iterate();
        void Clear();

        bool IsProperties { get; set; }
        bool IsVirtualProperties { get; set; }
        bool IsOverrideProperties { get; set; }
        bool IsPartialClass { get; set; }
        bool IsAllStructNullable { get; set; }
        bool IsComments { get; set; }
        bool IsCommentsWithoutNull { get; set; }
        bool IsUsing { get; set; }
        string Namespace { get; set; }
        string Inherit { get; set; }
        string Tab { get; set; }
        bool IsColumnDefaults { get; set; }
        bool IsNewLineBetweenMembers { get; set; }
        bool IsNavigationProperties { get; set; }
        bool IsNavigationPropertiesVirtual { get; set; }
        bool IsNavigationPropertiesOverride { get; set; }
        bool IsNavigationPropertiesShowJoinTable { get; set; }
        bool IsNavigationPropertiesComments { get; set; }
        bool IsNavigationPropertiesList { get; set; }
        bool IsNavigationPropertiesICollection { get; set; }
        bool IsNavigationPropertiesIEnumerable { get; set; }
        bool IsSingular { get; set; }
        bool IsIncludeDB { get; set; }
        string DBSeparator { get; set; }
        bool IsIncludeSchema { get; set; }
        bool IsIgnoreDboSchema { get; set; }
        string SchemaSeparator { get; set; }
        string WordsSeparator { get; set; }
        bool IsCamelCase { get; set; }
        bool IsUpperCase { get; set; }
        bool IsLowerCase { get; set; }
        string Search { get; set; }
        string Replace { get; set; }
        bool IsSearchIgnoreCase { get; set; }
        string FixedClassName { get; set; }
        string Prefix { get; set; }
        string Suffix { get; set; }
    }
}
