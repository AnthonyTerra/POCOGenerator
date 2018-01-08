using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CommandLine;
using Db.POCOIterator;

namespace POCOGenerator.CommandLine
{
    [Serializable]
    public class Options
    {
        #region Connection String

        [Option("ConnectionString", "Connection String")]
        [Switch("CS")]
        public string ConnectionString { get; set; }

        #endregion

        #region POCO

        [Option("Properties", "Properties", Default: true, MutuallyExclusiveSet: "Set1")]
        [Switch('P')]
        public bool IsProperties { get; set; }

        [Option("DataMembers", "Data Members", MutuallyExclusiveSet: "Set1")]
        [Switch("DM")]
        public bool IsDataMembers { get; set; }

        [Option("VirtualProperties", "Virtual Properties")]
        [Switch("VP")]
        public bool IsVirtualProperties { get; set; }

        [Option("OverrideProperties", "Override Properties")]
        [Switch("OP")]
        public bool IsOverrideProperties { get; set; }

        [Option("PartialClass", "Partial Class")]
        [Switch("PC")]
        public bool IsPartialClass { get; set; }

        [Option("StructNullable", "Struct Types Nullable")]
        [Switch("SN")]
        public bool IsAllStructNullable { get; set; }

        [Option("Comments", "Comments")]
        [Switch('C')]
        public bool IsComments { get; set; }

        [Option("CommentsWithoutNull", "Comments Without Null")]
        [Switch("CWN")]
        public bool IsCommentsWithoutNull { get; set; }

        [Option("Using", "using")]
        [Switch('U')]
        public bool IsUsing { get; set; }

        [Option("Namespace", "Namespace")]
        [Switch('N')]
        public string Namespace { get; set; }

        [Option("Inherit", "Inherit")]
        [Switch('I')]
        public string Inherit { get; set; }

        [Option("ColumnDefaults", "Column Defaults")]
        [Switch("CD")]
        public bool IsColumnDefaults { get; set; }

        [Option("NewLine", "New Line Between Members")]
        [Switch("NL")]
        public bool IsNewLineBetweenMembers { get; set; }

        [Option("Tab", "Tab", Default: DbIterator.TAB)]
        [Switch('T')]
        public string Tab { get; set; }

        #endregion

        #region Navigation Properties

        [Option("NP", "Navigation Properties")]
        public bool IsNavigationProperties { get; set; }

        [Option("NPVirtual", "Virtual Navigation Properties")]
        [Switch("NPV")]
        public bool IsNavigationPropertiesVirtual { get; set; }

        [Option("NPOverride", "Override Navigation Properties")]
        [Switch("NPO")]
        public bool IsNavigationPropertiesOverride { get; set; }

        [Option("NPJoinTable", "Show Many-to-Many Join Table")]
        [Switch("NPJT")]
        public bool IsNavigationPropertiesShowJoinTable { get; set; }

        [Option("NPComments", "Navigation Properties Comments")]
        [Switch("NPC")]
        public bool IsNavigationPropertiesComments { get; set; }

        [Option("NPList", "Navigation Properties List", Default: true, MutuallyExclusiveSet: "Set2")]
        [Switch("NPL")]
        public bool IsNavigationPropertiesList { get; set; }

        [Option("NPICollection", "Navigation Properties ICollection", MutuallyExclusiveSet: "Set2")]
        [Switch("NPIC")]
        public bool IsNavigationPropertiesICollection { get; set; }

        [Option("NPIEnumerable", "Navigation Properties IEnumerable", MutuallyExclusiveSet: "Set2")]
        [Switch("NPIE")]
        public bool IsNavigationPropertiesIEnumerable { get; set; }

        #endregion

        #region Class Name

        [Option("Singular", "Singular (Tables, Views, TVPs)")]
        [Switch('S')]
        public bool IsSingular { get; set; }

        [Option("IncludeDB", "Include DB")]
        [Switch("DB")]
        public bool IsIncludeDB { get; set; }

        [Option("DBSeparator", "DB Separator")]
        [Switch("DBS")]
        public string DBSeparator { get; set; }

        [Option("IncludeSchema", "Include Schema")]
        [Switch("SC")]
        public bool IsIncludeSchema { get; set; }

        [Option("IgnoreDboSchema", "Ignore dbo Schema")]
        [Switch("SCD")]
        public bool IsIgnoreDboSchema { get; set; }

        [Option("SchemaSeparator", "Schema Separator")]
        [Switch("SCS")]
        public string SchemaSeparator { get; set; }

        [Option("WordsSeparator", "Separator between _ and words in CamelCase")]
        [Switch("WS")]
        public string WordsSeparator { get; set; }

        [Option("CamelCase", "CamelCase")]
        [Switch("CC")]
        public bool IsCamelCase { get; set; }

        [Option("UpperCase", "UPPER CASE")]
        [Switch("UC")]
        public bool IsUpperCase { get; set; }

        [Option("LowerCase", "lower case")]
        [Switch("LC")]
        public bool IsLowerCase { get; set; }

        [Option("Search", "Search")]
        [Switch("TS")]
        public string Search { get; set; }

        [Option("Replace", "Replace with")]
        [Switch("TR")]
        public string Replace { get; set; }

        [Option("SearchIgnoreCase", "Search Ignore Case")]
        [Switch("TIC")]
        public bool IsSearchIgnoreCase { get; set; }

        [Option("FixedClassName", "Fixed Class Name")]
        [Switch("FCN")]
        public string FixedClassName { get; set; }

        [Option("Prefix", "Prefix")]
        [Switch("PF")]
        public string Prefix { get; set; }

        [Option("Suffix", "Suffix")]
        [Switch("SF")]
        public string Suffix { get; set; }

        #endregion

        #region EF Code-First Annotations

        [Option("EF", "Entity Framework")]
        public bool IsEF { get; set; }

        [Option("EFColumn", "EF Column")]
        [Switch("EFC")]
        public bool IsEFColumn { get; set; }

        [Option("EFRequired", "EF Required")]
        [Switch("EFR")]
        public bool IsEFRequired { get; set; }

        [Option("EFRequiredWithErrorMessage", "EF Required with ErrorMessage")]
        [Switch("EFREM")]
        public bool IsEFRequiredWithErrorMessage { get; set; }

        [Option("EFConcurrencyCheck", "EF ConcurrencyCheck")]
        [Switch("EFCC")]
        public bool IsEFConcurrencyCheck { get; set; }

        [Option("EFStringLength", "EF StringLength")]
        [Switch("EFSL")]
        public bool IsEFStringLength { get; set; }

        [Option("EFDisplay", "EF Display")]
        [Switch("EFD")]
        public bool IsEFDisplay { get; set; }

        [Option("EFDescription", "EF Description")]
        [Switch("EFDS")]
        public bool IsEFDescription { get; set; }

        [Option("EFComplexType", "EF ComplexType")]
        [Switch("EFCT")]
        public bool IsEFComplexType { get; set; }

        [Option("EFIndex", "EF Index (EF6)")]
        [Switch("EFI")]
        public bool IsEFIndex { get; set; }

        [Option("EFForeignKey", "EF ForeignKey & InverseProperty")]
        [Switch("EFFK")]
        public bool IsEFForeignKey { get; set; }

        #endregion

        #region Export to Files

        [Option("Folder", "Export Folder")]
        [Switch('F')]
        public string Folder { get; set; }

        [Option("AppendFile", "Append to File")]
        [Switch("AF")]
        public bool IsSingleFile { get; set; }

        [Option("FileName", "Export File Name")]
        [Switch("FN")]
        public string FileName { get; set; }

        #endregion

        #region All

        [Option("IncludeAll", "Include All Objects")]
        [Switch("IA")]
        [XmlIgnore]
        public bool IsIncludeAll { get; set; }

        #endregion

        #region Tables

        [Option("IncludeAllTables", "Include All Tables")]
        [Switch("IAT")]
        [XmlIgnore]
        public bool IsIncludeAllTables { get; set; }

        [Option("ExcludeAllTables", "Exclude All Tables")]
        [Switch("EAT")]
        [XmlIgnore]
        public bool IsExcludeAllTables { get; set; }

        [Option("IncludeTables", "Include Tables")]
        [Switch("IT")]
        [XmlIgnore]
        public List<string> IncludeTables { get; set; }

        [Option("ExcludeTables", "Exclude Tables")]
        [Switch("ET")]
        [XmlIgnore]
        public List<string> ExcludeTables { get; set; }

        #endregion

        #region Views

        [Option("IncludeAllViews", "Include All Views")]
        [Switch("IAV")]
        [XmlIgnore]
        public bool IsIncludeAllViews { get; set; }

        [Option("ExcludeAllViews", "Exclude All Views")]
        [Switch("EAV")]
        [XmlIgnore]
        public bool IsExcludeAllViews { get; set; }

        [Option("IncludeViews", "Include Views")]
        [Switch("IV")]
        [XmlIgnore]
        public List<string> IncludeViews { get; set; }

        [Option("ExcludeViews", "Exclude Views")]
        [Switch("EV")]
        [XmlIgnore]
        public List<string> ExcludeViews { get; set; }

        #endregion

        #region Stored Procedures

        [Option("IncludeAllStoredProcedures", "Include All Stored Procedures")]
        [Switch("IASP")]
        [XmlIgnore]
        public bool IsIncludeAllStoredProcedures { get; set; }

        [Option("ExcludeAllStoredProcedures", "Exclude All Stored Procedures")]
        [Switch("EASP")]
        [XmlIgnore]
        public bool IsExcludeAllStoredProcedures { get; set; }

        [Option("IncludeStoredProcedures", "Include Stored Procedures")]
        [Switch("ISP")]
        [XmlIgnore]
        public List<string> IncludeStoredProcedures { get; set; }

        [Option("ExcludeStoredProcedures", "Exclude Stored Procedures")]
        [Switch("ESP")]
        [XmlIgnore]
        public List<string> ExcludeStoredProcedures { get; set; }

        #endregion

        #region Functions

        [Option("IncludeAllFunctions", "Include All Functions")]
        [Switch("IAFN")]
        [XmlIgnore]
        public bool IsIncludeAllFunctions { get; set; }

        [Option("ExcludeAllFunctions", "Exclude All Functions")]
        [Switch("EAFN")]
        [XmlIgnore]
        public bool IsExcludeAllFunctions { get; set; }

        [Option("IncludeFunctions", "Include Functions")]
        [Switch("IFN")]
        [XmlIgnore]
        public List<string> IncludeFunctions { get; set; }

        [Option("ExcludeFunctions", "Exclude Functions")]
        [Switch("EFN")]
        [XmlIgnore]
        public List<string> ExcludeFunctions { get; set; }

        #endregion

        #region TVPs

        [Option("IncludeAllTVPs", "Include All TVPs")]
        [Switch("IATVP")]
        [XmlIgnore]
        public bool IsIncludeAllTVPs { get; set; }

        [Option("ExcludeAllTVPs", "Exclude All TVPs")]
        [Switch("EATVP")]
        [XmlIgnore]
        public bool IsExcludeAllTVPs { get; set; }

        [Option("IncludeTVPs", "Include TVPs")]
        [Switch("ITVP")]
        [XmlIgnore]
        public List<string> IncludeTVPs { get; set; }

        [Option("ExcludeTVPs", "Exclude TVPs")]
        [Switch("ETVP")]
        [XmlIgnore]
        public List<string> ExcludeTVPs { get; set; }

        #endregion
    }
}
