using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Db.DbObject;
using Db.Helpers;

namespace Db.POCOIterator
{
    public class DbIterator : IPOCOIterator
    {
        #region Constructor

        protected IEnumerable<IDbObjectTraverse> dbObjects;
        protected IPOCOWriter pocoWriter;

        public const string TAB = "    ";
        public virtual string Tab { get; set; }

        public DbIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
        {
            this.dbObjects = dbObjects;
            this.pocoWriter = pocoWriter;
            this.Tab = TAB;
        }

        #endregion

        #region Iterate

        public void Iterate()
        {
            Clear();

            if (dbObjects == null || dbObjects.Count() == 0)
                return;

            bool isExistDbObject = (dbObjects.Any(o => o.Error == null));

            string namespaceOffset = string.Empty;
            if (isExistDbObject)
            {
                // Using
                WriteUsing();

                // Namespace Start
                namespaceOffset = WriteNamespaceStart();
            }

            /*IEnumerable<Table> tables = null;
            if (IsNavigationProperties)
            {
                tables = dbObjects.Where(t => t.DbType == DbType.Table).Cast<Table>();
            }*/

            IDbObjectTraverse lastDbObject = dbObjects.Last();
            foreach (IDbObjectTraverse dbObject in dbObjects)
            {
                // Class Name
                string className = GetClassName(dbObject.Database.ToString(), dbObject.Schema, dbObject.Name, dbObject.DbType);
                dbObject.ClassName = className;

                if (dbObject.Error != null)
                {
                    // Error
                    WriteError(dbObject, namespaceOffset);
                }
                else
                {
                    // Navigation Properties
                    List<NavigationProperty> navigationProperties = GetNavigationProperties(dbObject/*, tables*/);

                    if (IsWriteObject(navigationProperties, dbObject))
                    {
                        // Class Attributes
                        WriteClassAttributes(dbObject, namespaceOffset);

                        // Class Start
                        WriteClassStart(className, dbObject, namespaceOffset);

                        // Constructor
                        WriteConstructor(className, navigationProperties, dbObject, namespaceOffset);

                        // Columns
                        if (dbObject.Columns != null && dbObject.Columns.Any())
                        {
                            var columns = dbObject.Columns.OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
                            var lastColumn = columns.Last();
                            foreach (IColumn column in columns)
                                WriteColumn(column, column == lastColumn, dbObject, namespaceOffset);
                        }

                        // Navigation Properties
                        WriteNavigationProperties(navigationProperties, dbObject, namespaceOffset);

                        // Class End
                        WriteClassEnd(dbObject, namespaceOffset);
                    }
                }

                if (dbObject != lastDbObject)
                    pocoWriter.WriteLine();
            }

            if (isExistDbObject)
            {
                // Namespace End
                WriteNamespaceEnd();
            }
        }

        #endregion

        #region Clear

        public void Clear()
        {
            pocoWriter.Clear();
        }

        #endregion

        #region Using

        public virtual bool IsUsing { get; set; }

        protected virtual void WriteUsing()
        {
            if (IsUsing)
            {
                WriteUsingClause();
                pocoWriter.WriteLine();
            }
        }

        protected virtual void WriteUsingClause()
        {
            pocoWriter.WriteKeyword("using");
            pocoWriter.WriteLine(" System;");

            if (IsNavigationProperties)
            {
                pocoWriter.WriteKeyword("using");
                pocoWriter.WriteLine(" System.Collections.Generic;");
            }

            if (IsSpecialSQLTypes())
            {
                pocoWriter.WriteKeyword("using");
                pocoWriter.WriteLine(" Microsoft.SqlServer.Types;");
            }
        }

        protected virtual bool IsSpecialSQLTypes()
        {
            if (dbObjects == null || dbObjects.Count() == 0)
                return false;

            foreach (var dbObject in dbObjects)
            {
                if (dbObject.Columns != null && dbObject.Columns.Any())
                {
                    foreach (IColumn column in dbObject.Columns)
                    {
                        string data_type = (column.DataTypeName ?? string.Empty).ToLower();
                        if (data_type.Contains("geography") || data_type.Contains("geometry") || data_type.Contains("hierarchyid"))
                            return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Namespace Start

        public virtual string Namespace { get; set; }

        protected virtual string WriteNamespaceStart()
        {
            string namespaceOffset = string.Empty;

            if (string.IsNullOrEmpty(Namespace) == false)
            {
                WriteNamespaceStartClause();
                namespaceOffset = Tab;
            }

            return namespaceOffset;
        }

        protected virtual void WriteNamespaceStartClause()
        {
            pocoWriter.WriteKeyword("namespace");
            pocoWriter.Write(" ");
            pocoWriter.WriteLine(Namespace);
            pocoWriter.WriteLine("{");
        }

        #endregion

        #region Error

        protected virtual void WriteError(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteLineError("/*");

            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteLineError(string.Format("{0}.{1}", dbObject.Database.ToString(), dbObject.ToString()));

            Exception currentError = dbObject.Error;
            while (currentError != null)
            {
                pocoWriter.Write(namespaceOffset);
                pocoWriter.WriteLineError(currentError.Message);
                currentError = currentError.InnerException;
            }

            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteLineError("*/");
        }

        #endregion

        #region Is Write Object

        protected virtual bool IsWriteObject(List<NavigationProperty> navigationProperties, IDbObjectTraverse dbObject)
        {
            if (dbObject.DbType == DbType.Table)
            {
                if (IsNavigationPropertiesShowJoinTable == false)
                {
                    if (navigationProperties != null && navigationProperties.Count > 0)
                    {
                        // hide many-to-many join table.
                        // join table is complete. all the columns are part of the pk. there are no columns other than the pk.
                        return navigationProperties.All(p => p.IsRefFrom && p.ForeignKey.Is_Many_To_Many && p.ForeignKey.Is_Many_To_Many_Complete) == false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Class Attributes

        protected virtual void WriteClassAttributes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
        }

        #endregion

        #region Class Name

        public virtual string Prefix { get; set; }
        public virtual string FixedClassName { get; set; }
        public virtual bool IsIncludeDB { get; set; }
        public virtual bool IsCamelCase { get; set; }
        public virtual string WordsSeparator { get; set; }
        public virtual bool IsUpperCase { get; set; }
        public virtual bool IsLowerCase { get; set; }
        public virtual string DBSeparator { get; set; }
        public virtual bool IsIncludeSchema { get; set; }
        public virtual bool IsIgnoreDboSchema { get; set; }
        public virtual string SchemaSeparator { get; set; }
        public virtual bool IsSingular { get; set; }
        public virtual string Search { get; set; }
        public virtual string Replace { get; set; }
        public virtual bool IsSearchIgnoreCase { get; set; }
        public virtual string Suffix { get; set; }

        protected virtual string GetClassName(string database, string schema, string name, DbType dbType)
        {
            string className = null;

            // prefix
            if (string.IsNullOrEmpty(Prefix) == false)
                className += Prefix;

            if (string.IsNullOrEmpty(FixedClassName))
            {
                if (IsIncludeDB)
                {
                    // database
                    if (IsCamelCase || string.IsNullOrEmpty(WordsSeparator) == false)
                        className += NameHelper.TransformName(database, WordsSeparator, IsCamelCase, IsUpperCase, IsLowerCase);
                    else if (IsUpperCase)
                        className += database.ToUpper();
                    else if (IsLowerCase)
                        className += database.ToLower();
                    else
                        className += database;

                    // db separator
                    if (string.IsNullOrEmpty(DBSeparator) == false)
                        className += DBSeparator;
                }

                if (IsIncludeSchema)
                {
                    if (IsIgnoreDboSchema == false || schema != "dbo")
                    {
                        // schema
                        if (IsCamelCase || string.IsNullOrEmpty(WordsSeparator) == false)
                            className += NameHelper.TransformName(schema, WordsSeparator, IsCamelCase, IsUpperCase, IsLowerCase);
                        else if (IsUpperCase)
                            className += schema.ToUpper();
                        else if (IsLowerCase)
                            className += schema.ToLower();
                        else
                            className += schema;

                        // schema separator
                        if (string.IsNullOrEmpty(SchemaSeparator) == false)
                            className += SchemaSeparator;
                    }
                }

                // name
                if (IsSingular)
                {
                    if (dbType == DbType.Table || dbType == DbType.View || dbType == DbType.TVP)
                        name = NameHelper.GetSingularName(name);
                }

                if (IsCamelCase || string.IsNullOrEmpty(WordsSeparator) == false)
                    className += NameHelper.TransformName(name, WordsSeparator, IsCamelCase, IsUpperCase, IsLowerCase);
                else if (IsUpperCase)
                    className += name.ToUpper();
                else if (IsLowerCase)
                    className += name.ToLower();
                else
                    className += name;

                if (string.IsNullOrEmpty(Search) == false)
                {
                    if (IsSearchIgnoreCase)
                        className = Regex.Replace(className, Search, Replace ?? string.Empty, RegexOptions.IgnoreCase);
                    else
                        className = className.Replace(Search, Replace ?? string.Empty);
                }
            }
            else
            {
                // fixed name
                className += FixedClassName;
            }

            // postfix
            if (string.IsNullOrEmpty(Suffix) == false)
                className += Suffix;

            return className;
        }

        #endregion

        #region Class Start

        public virtual bool IsPartialClass { get; set; }
        public virtual string Inherit { get; set; }

        protected virtual void WriteClassStart(string className, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteKeyword("public");
            pocoWriter.Write(" ");
            if (IsPartialClass)
            {
                pocoWriter.WriteKeyword("partial");
                pocoWriter.Write(" ");
            }
            pocoWriter.WriteKeyword("class");
            pocoWriter.Write(" ");
            pocoWriter.WriteUserType(className);
            if (string.IsNullOrEmpty(Inherit) == false)
            {
                pocoWriter.Write(" : ");
                string[] inherit = Inherit.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                pocoWriter.WriteUserType(inherit[0]);
                for (int i = 1; i < inherit.Length; i++)
                {
                    pocoWriter.Write(", ");
                    pocoWriter.WriteUserType(inherit[i]);
                }
            }
            pocoWriter.WriteLine();

            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteLine("{");
        }

        #endregion

        #region Class Constructor

        protected virtual void WriteConstructor(string className, List<NavigationProperty> navigationProperties, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            bool hasColumnDefaults = HasColumnDefaults(dbObject);
            bool hasNavigationProperties = HasNavigationProperties(dbObject, navigationProperties);

            if (hasColumnDefaults || hasNavigationProperties)
            {
                WriteConstructorStart(className, dbObject, namespaceOffset);

                if (hasColumnDefaults)
                {
                    Table table = (Table)dbObject;
                    foreach (var column in table.TableColumns.Where(c => c.column_default != null).OrderBy(c => c.ordinal_position ?? 0))
                        WriteColumnDefaultConstructorInitialization(column, namespaceOffset);
                }

                if (hasColumnDefaults && hasNavigationProperties)
                    pocoWriter.WriteLine();

                if (hasNavigationProperties)
                {
                    foreach (var np in navigationProperties.Where(p => p.IsSingle == false))
                        WriteNavigationPropertyConstructorInitialization(np, namespaceOffset);
                }

                WriteConstructorEnd(dbObject, namespaceOffset);
                pocoWriter.WriteLine();
            }
        }

        protected virtual void WriteConstructorStart(string className, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.WriteKeyword("public");
            pocoWriter.Write(" ");
            pocoWriter.Write(className);
            pocoWriter.WriteLine("()");

            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.WriteLine("{");
        }

        protected virtual void WriteConstructorEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.WriteLine("}");
        }

        #region Class Constructor - Navigation Properties

        protected virtual bool HasNavigationProperties(IDbObjectTraverse dbObject, List<NavigationProperty> navigationProperties)
        {
            return
                IsNavigableObject(dbObject) &&
                navigationProperties != null &&
                navigationProperties.Count > 0 &&
                navigationProperties.Any(p => p.IsSingle == false);
        }

        protected virtual void WriteNavigationPropertyConstructorInitialization(NavigationProperty navigationProperty, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write(Tab);
            pocoWriter.WriteKeyword("this");
            pocoWriter.Write(".");
            pocoWriter.Write(navigationProperty.ToString());
            pocoWriter.Write(" = ");
            pocoWriter.WriteKeyword("new");
            pocoWriter.Write(" ");
            pocoWriter.WriteUserType(IsNavigationPropertiesICollection ? "HashSet" : "List");
            pocoWriter.Write("<");
            pocoWriter.WriteUserType(navigationProperty.ClassName);
            pocoWriter.WriteLine(">();");
        }

        #endregion

        #region Class Constructor - Column Defaults

        public virtual bool IsColumnDefaults { get; set; }

        protected virtual bool HasColumnDefaults(IDbObjectTraverse dbObject)
        {
            if (IsColumnDefaults && dbObject.DbType == DbType.Table)
            {
                Table table = (Table)dbObject;
                if (table.TableColumns != null && table.TableColumns.Count > 0)
                    return table.TableColumns.Any(c => c.column_default != null);
            }

            return false;
        }

        protected virtual void WriteColumnDefaultConstructorInitialization(TableColumn column, string namespaceOffset)
        {
            string columnDefault = column.column_default;
            columnDefault = CleanColumnDefault(columnDefault);

            if (column.DataTypeName == "uniqueidentifier")
            {
                if (columnDefault.IndexOf("newid", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    WriteColumnDefaultConstructorInitializationGuid(column, namespaceOffset);
                    return;
                }
            }
            else if (column.DataTypeName == "int" ||
                column.DataTypeName == "smallint" ||
                column.DataTypeName == "bigint" ||
                column.DataTypeName == "tinyint" ||
                column.DataTypeName == "float" ||
                column.DataTypeName == "decimal" ||
                column.DataTypeName == "numeric" ||
                column.DataTypeName == "money" ||
                column.DataTypeName == "smallmoney" ||
                column.DataTypeName == "real")
            {
                WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
                return;
            }
            else if (column.DataTypeName == "bit")
            {
                if (columnDefault.IndexOf("1", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    WriteColumnDefaultConstructorInitializationBool(true, column, namespaceOffset);
                    return;
                }
                else if (columnDefault.IndexOf("0", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    WriteColumnDefaultConstructorInitializationBool(false, column, namespaceOffset);
                    return;
                }
            }
            else if (column.DataTypeName == "date" ||
                column.DataTypeName == "datetime" ||
                column.DataTypeName == "datetime2" ||
                column.DataTypeName == "smalldatetime" ||
                column.DataTypeName == "time" ||
                column.DataTypeName == "datetimeoffset")
            {
                if (columnDefault.IndexOf("getutcdate", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    columnDefault.IndexOf("sysutcdatetime", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    WriteColumnDefaultConstructorInitializationDateTimeUtcNow(column, namespaceOffset);
                    return;
                }
                else if (columnDefault.IndexOf("getdate", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    columnDefault.IndexOf("sysdatetime", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    columnDefault.IndexOf("sysdatetimeoffset", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    columnDefault.IndexOf("current_timestamp", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    WriteColumnDefaultConstructorInitializationDateTimeNow(column, namespaceOffset);
                    return;
                }
                else
                {
                    DateTime date = DateTime.MinValue;
                    if (DateTime.TryParse(columnDefault, out date))
                    {
                        WriteColumnDefaultConstructorInitializationDateTime(date, column, namespaceOffset);
                        return;
                    }
                }
            }
            else if (column.DataTypeName == "char" ||
                column.DataTypeName == "nchar" ||
                column.DataTypeName == "ntext" ||
                column.DataTypeName == "nvarchar" ||
                column.DataTypeName == "text" ||
                column.DataTypeName == "varchar" ||
                column.DataTypeName == "xml")
            {
                WriteColumnDefaultConstructorInitializationString(columnDefault, column, namespaceOffset);
                return;
            }
            else if (column.DataTypeName == "binary" ||
                column.DataTypeName == "varbinary" ||
                column.DataTypeName == "filestream" ||
                column.DataTypeName == "image")
            {
                WriteColumnDefaultConstructorInitializationBinary(columnDefault, column, namespaceOffset);
                return;
            }

            WriteColumnDefaultConstructorInitializationDefault(column.column_default, column, namespaceOffset);
        }

        protected virtual string CleanColumnDefault(string columnDefault)
        {
            if (columnDefault.StartsWith("('") && columnDefault.EndsWith("')"))
                columnDefault = columnDefault.Substring(2, columnDefault.Length - 4);
            else if (columnDefault.StartsWith("(N'") && columnDefault.EndsWith("')"))
                columnDefault = columnDefault.Substring(3, columnDefault.Length - 5);
            else if (columnDefault.StartsWith("((") && columnDefault.EndsWith("))"))
                columnDefault = columnDefault.Substring(2, columnDefault.Length - 4);
            return columnDefault;
        }

        protected virtual void WriteColumnDefaultConstructorInitializationGuid(TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteUserType("Guid");
            pocoWriter.Write(".NewGuid()");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationNumber(string columnDefault, TableColumn column, string namespaceOffset)
        {
            columnDefault = columnDefault.Replace("(", string.Empty).Replace(")", string.Empty);
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.Write(columnDefault);
            if (column.DataTypeName == "decimal" || column.DataTypeName == "numeric" || column.DataTypeName == "money" || column.DataTypeName == "smallmoney")
                pocoWriter.Write("M");
            else if (column.DataTypeName == "real")
                pocoWriter.Write("F");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationBool(bool value, TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteKeyword(value.ToString().ToLower());
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeUtcNow(TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteUserType(column.DataTypeName == "datetimeoffset" ? "DateTimeOffset" : "DateTime");
            pocoWriter.Write(".UtcNow");
            if (column.DataTypeName == "time")
                pocoWriter.Write(".TimeOfDay");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeNow(TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteUserType(column.DataTypeName == "datetimeoffset" ? "DateTimeOffset" : "DateTime");
            pocoWriter.Write(".Now");
            if (column.DataTypeName == "time")
                pocoWriter.Write(".TimeOfDay");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime(DateTime date, TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);

            if (column.DataTypeName == "datetimeoffset")
            {
                pocoWriter.WriteKeyword("new ");
                pocoWriter.WriteUserType("DateTimeOffset");
                pocoWriter.Write("(");
            }

            pocoWriter.WriteKeyword("new ");
            pocoWriter.WriteUserType("DateTime");
            pocoWriter.Write("(");
            pocoWriter.Write(date.Year.ToString());
            pocoWriter.Write(", ");
            pocoWriter.Write(date.Month.ToString());
            pocoWriter.Write(", ");
            pocoWriter.Write(date.Day.ToString());
            if (date.Hour != 0 || date.Minute != 0 || date.Second != 0 || date.Millisecond != 0)
            {
                pocoWriter.Write(", ");
                pocoWriter.Write(date.Hour.ToString());
                pocoWriter.Write(", ");
                pocoWriter.Write(date.Minute.ToString());
                pocoWriter.Write(", ");
                pocoWriter.Write(date.Second.ToString());
                if (date.Millisecond != 0)
                {
                    pocoWriter.Write(", ");
                    pocoWriter.Write(date.Millisecond.ToString());
                }
            }
            pocoWriter.Write(")");

            if (column.DataTypeName == "time")
                pocoWriter.Write(".TimeOfDay");

            if (column.DataTypeName == "datetimeoffset")
                pocoWriter.Write(")");

            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationString(string columnDefault, TableColumn column, string namespaceOffset)
        {
            columnDefault = columnDefault.Replace("\\", "\\\\").Replace("\"", "\\\"");
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(columnDefault);
            pocoWriter.WriteString("\"");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationBinary(string columnDefault, TableColumn column, string namespaceOffset)
        {
            columnDefault = columnDefault.Replace("(", string.Empty).Replace(")", string.Empty);

            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            pocoWriter.WriteUserType("BitConverter");
            pocoWriter.Write(".GetBytes(");
            if (columnDefault.StartsWith("0x"))
            {
                pocoWriter.WriteUserType("Convert");
                pocoWriter.Write(".ToInt32(");
                pocoWriter.WriteString("\"");
                pocoWriter.WriteString(columnDefault);
                pocoWriter.WriteString("\"");
                pocoWriter.Write(", 16)");
            }
            else
            {
                pocoWriter.Write(columnDefault);
            }
            pocoWriter.Write(")");
            WriteColumnDefaultConstructorInitializationEnd();

            string cleanColumnName = NameHelper.CleanName(column.ColumnName);
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write(Tab);
            pocoWriter.WriteKeyword("if");
            pocoWriter.Write(" (");
            pocoWriter.WriteUserType("BitConverter");
            pocoWriter.WriteLine(".IsLittleEndian)");
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write(Tab);
            pocoWriter.Write(Tab);
            pocoWriter.WriteUserType("Array");
            pocoWriter.Write(".Reverse(");
            pocoWriter.WriteKeyword("this");
            pocoWriter.Write(".");
            pocoWriter.Write(cleanColumnName);
            pocoWriter.WriteLine(");");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDefault(string columnDefault, TableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset, true);
            pocoWriter.WriteComment(columnDefault);
            WriteColumnDefaultConstructorInitializationEnd(true);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationStart(TableColumn column, string namespaceOffset, bool isComment = false)
        {
            string cleanColumnName = NameHelper.CleanName(column.ColumnName);
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write(Tab);
            if (isComment)
            {
                pocoWriter.WriteComment("/* this.");
                pocoWriter.WriteComment(cleanColumnName);
                pocoWriter.WriteComment(" = ");
            }
            else
            {
                pocoWriter.WriteKeyword("this");
                pocoWriter.Write(".");
                pocoWriter.Write(cleanColumnName);
                pocoWriter.Write(" = ");
            }
        }

        protected virtual void WriteColumnDefaultConstructorInitializationEnd(bool isComment = false)
        {
            if (isComment)
                pocoWriter.WriteLineComment("; */");
            else
                pocoWriter.WriteLine(";");
        }

        #endregion

        #endregion

        #region Column Attributes

        protected virtual void WriteColumnAttributes(IColumn column, string cleanColumnName, IDbObjectTraverse dbObject, string namespaceOffset)
        {
        }

        #endregion

        #region Column

        public virtual bool IsProperties { get; set; }
        public virtual bool IsVirtualProperties { get; set; }
        public virtual bool IsOverrideProperties { get; set; }
        public virtual bool IsAllStructNullable { get; set; }
        public virtual bool IsComments { get; set; }
        public virtual bool IsCommentsWithoutNull { get; set; }
        public virtual bool IsNewLineBetweenMembers { get; set; }

        protected virtual void WriteColumn(IColumn column, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            string cleanColumnName = NameHelper.CleanName(column.ColumnName);

            WriteColumnAttributes(column, cleanColumnName, dbObject, namespaceOffset);

            WriteColumnStart(namespaceOffset);

            WriteColumnDataType(column);

            WriteColumnName(cleanColumnName);

            WriteColumnEnd();

            WriteColumnComments(column);

            pocoWriter.WriteLine();

            if (IsNewLineBetweenMembers && isLastColumn == false)
                pocoWriter.WriteLine();
        }

        protected virtual void WriteColumnStart(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.WriteKeyword("public");
            pocoWriter.Write(" ");

            if (IsProperties && IsVirtualProperties)
            {
                pocoWriter.WriteKeyword("virtual");
                pocoWriter.Write(" ");
            }
            else if (IsProperties && IsOverrideProperties)
            {
                pocoWriter.WriteKeyword("override");
                pocoWriter.Write(" ");
            }
        }

        protected virtual void WriteColumnDataType(IColumn column)
        {
            switch ((column.DataTypeDisplay ?? string.Empty).ToLower())
            {
                case "bigint": WriteColumnBigInt(column.IsNullable); break;
                case "binary": WriteColumnBinary(); break;
                case "bit": WriteColumnBit(column.IsNullable); break;
                case "char": WriteColumnChar(); break;
                case "date": WriteColumnDate(column.IsNullable); break;
                case "datetime": WriteColumnDateTime(column.IsNullable); break;
                case "datetime2": WriteColumnDateTime2(column.IsNullable); break;
                case "datetimeoffset": WriteColumnDateTimeOffset(column.IsNullable); break;
                case "decimal": WriteColumnDecimal(column.IsNullable); break;
                case "filestream": WriteColumnFileStream(); break;
                case "float": WriteColumnFloat(column.IsNullable); break;
                case "geography": WriteColumnGeography(); break;
                case "geometry": WriteColumnGeometry(); break;
                case "hierarchyid": WriteColumnHierarchyId(); break;
                case "image": WriteColumnImage(); break;
                case "int": WriteColumnInt(column.IsNullable); break;
                case "money": WriteColumnMoney(column.IsNullable); break;
                case "nchar": WriteColumnNChar(); break;
                case "ntext": WriteColumnNText(); break;
                case "numeric": WriteColumnNumeric(column.IsNullable); break;
                case "nvarchar": WriteColumnNVarChar(); break;
                case "real": WriteColumnReal(column.IsNullable); break;
                case "rowversion": WriteColumnRowVersion(); break;
                case "smalldatetime": WriteColumnSmallDateTime(column.IsNullable); break;
                case "smallint": WriteColumnSmallInt(column.IsNullable); break;
                case "smallmoney": WriteColumnSmallMoney(column.IsNullable); break;
                case "sql_variant": WriteColumnSqlVariant(); break;
                case "text": WriteColumnText(); break;
                case "time": WriteColumnTime(column.IsNullable); break;
                case "timestamp": WriteColumnTimeStamp(); break;
                case "tinyint": WriteColumnTinyInt(column.IsNullable); break;
                case "uniqueidentifier": WriteColumnUniqueIdentifier(column.IsNullable); break;
                case "varbinary": WriteColumnVarBinary(); break;
                case "varchar": WriteColumnVarChar(); break;
                case "xml": WriteColumnXml(); break;
                default: WriteColumnObject(); break;
            }
        }

        protected virtual void WriteColumnName(string columnName)
        {
            pocoWriter.Write(" ");
            pocoWriter.Write(columnName);
        }

        protected virtual void WriteColumnEnd()
        {
            if (IsProperties)
            {
                pocoWriter.Write(" { ");
                pocoWriter.WriteKeyword("get");
                pocoWriter.Write("; ");
                pocoWriter.WriteKeyword("set");
                pocoWriter.Write("; }");
            }
            else
            {
                pocoWriter.Write(";");
            }
        }

        protected virtual void WriteColumnComments(IColumn column)
        {
            if (IsComments)
            {
                pocoWriter.Write(" ");
                pocoWriter.WriteComment("//");
                pocoWriter.WriteComment(" ");
                pocoWriter.WriteComment(column.DataTypeDisplay);
                pocoWriter.WriteComment(column.Precision ?? string.Empty);

                if (IsCommentsWithoutNull == false)
                {
                    pocoWriter.WriteComment(",");
                    pocoWriter.WriteComment(" ");
                    pocoWriter.WriteComment((column.IsNullable ? "null" : "not null"));
                }
            }
        }

        #region Column Data Types

        protected virtual void WriteColumnBigInt(bool isNullable)
        {
            pocoWriter.WriteKeyword("long");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnBinary()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnBit(bool isNullable)
        {
            pocoWriter.WriteKeyword("bool");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnChar()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnDate(bool isNullable)
        {
            pocoWriter.WriteUserType("DateTime");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnDateTime(bool isNullable)
        {
            pocoWriter.WriteUserType("DateTime");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnDateTime2(bool isNullable)
        {
            pocoWriter.WriteUserType("DateTime");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnDateTimeOffset(bool isNullable)
        {
            pocoWriter.WriteUserType("DateTimeOffset");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnDecimal(bool isNullable)
        {
            pocoWriter.WriteKeyword("decimal");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnFileStream()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnFloat(bool isNullable)
        {
            pocoWriter.WriteKeyword("double");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnGeography()
        {
            if (IsUsing == false)
                pocoWriter.Write("Microsoft.SqlServer.Types.");
            pocoWriter.WriteUserType("SqlGeography");
        }

        protected virtual void WriteColumnGeometry()
        {
            if (IsUsing == false)
                pocoWriter.Write("Microsoft.SqlServer.Types.");
            pocoWriter.WriteUserType("SqlGeometry");
        }

        protected virtual void WriteColumnHierarchyId()
        {
            if (IsUsing == false)
                pocoWriter.Write("Microsoft.SqlServer.Types.");
            pocoWriter.WriteUserType("SqlHierarchyId");
        }

        protected virtual void WriteColumnImage()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnInt(bool isNullable)
        {
            pocoWriter.WriteKeyword("int");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnMoney(bool isNullable)
        {
            pocoWriter.WriteKeyword("decimal");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnNChar()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnNText()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnNumeric(bool isNullable)
        {
            pocoWriter.WriteKeyword("decimal");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnNVarChar()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnReal(bool isNullable)
        {
            pocoWriter.WriteUserType("Single");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnRowVersion()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnSmallDateTime(bool isNullable)
        {
            pocoWriter.WriteUserType("DateTime");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnSmallInt(bool isNullable)
        {
            pocoWriter.WriteKeyword("short");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnSmallMoney(bool isNullable)
        {
            pocoWriter.WriteKeyword("decimal");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnSqlVariant()
        {
            pocoWriter.WriteKeyword("object");
        }

        protected virtual void WriteColumnText()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnTime(bool isNullable)
        {
            pocoWriter.WriteUserType("TimeSpan");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnTimeStamp()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnTinyInt(bool isNullable)
        {
            pocoWriter.WriteKeyword("byte");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnUniqueIdentifier(bool isNullable)
        {
            pocoWriter.WriteUserType("Guid");
            if (isNullable || IsAllStructNullable)
                pocoWriter.Write("?");
        }

        protected virtual void WriteColumnVarBinary()
        {
            pocoWriter.WriteKeyword("byte");
            pocoWriter.Write("[]");
        }

        protected virtual void WriteColumnVarChar()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnXml()
        {
            pocoWriter.WriteKeyword("string");
        }

        protected virtual void WriteColumnObject()
        {
            pocoWriter.WriteKeyword("object");
        }

        #endregion

        #endregion

        #region Navigation Properties

        public virtual bool IsNavigationProperties { get; set; }
        public virtual bool IsNavigationPropertiesVirtual { get; set; }
        public virtual bool IsNavigationPropertiesOverride { get; set; }
        public virtual bool IsNavigationPropertiesShowJoinTable { get; set; }
        public virtual bool IsNavigationPropertiesComments { get; set; }
        public virtual bool IsNavigationPropertiesList { get; set; }
        public virtual bool IsNavigationPropertiesICollection { get; set; }
        public virtual bool IsNavigationPropertiesIEnumerable { get; set; }

        protected virtual bool IsNavigableObject(IDbObjectTraverse dbObject)
        {
            return (IsNavigationProperties && dbObject.DbType == DbType.Table);
        }

        #region Get Navigation Properties

        protected virtual List<NavigationProperty> GetNavigationProperties(IDbObjectTraverse dbObject/*, IEnumerable<Table> tables*/)
        {
            List<NavigationProperty> navigationProperties = null;

            if (IsNavigableObject(dbObject))
            {
                if (dbObject.Columns != null && dbObject.Columns.Any())
                {
                    // columns are referencing (IsForeignKey)
                    var columnsFrom = dbObject.Columns.Where(c => c.HasForeignKeys).OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
                    if (columnsFrom.Any())
                    {
                        if (navigationProperties == null)
                            navigationProperties = new List<NavigationProperty>();

                        foreach (var column in columnsFrom.Cast<TableColumn>())
                        {
                            foreach (var fk in column.ForeignKeys)
                            {
                                string className = GetClassName(dbObject.Database.ToString(), fk.Primary_Schema, fk.Primary_Table, dbObject.DbType);
                                fk.NavigationPropertyRefFrom.ClassName = className;
                                navigationProperties.Add(fk.NavigationPropertyRefFrom);
                            }
                        }
                    }

                    // columns are referenced (IsPrimaryForeignKey)
                    var columnsTo = dbObject.Columns.Where(c => c.HasPrimaryForeignKeys).OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
                    if (columnsTo.Any())
                    {
                        if (navigationProperties == null)
                            navigationProperties = new List<NavigationProperty>();

                        foreach (var column in columnsTo.Cast<TableColumn>())
                        {
                            foreach (var fk in column.PrimaryForeignKeys)
                            {
                                string className = GetClassName(dbObject.Database.ToString(), fk.Foreign_Schema, fk.Foreign_Table, dbObject.DbType);

                                if (IsNavigationPropertiesShowJoinTable == false && fk.NavigationPropertiesRefToManyToMany != null)
                                {
                                    foreach (var np in fk.NavigationPropertiesRefToManyToMany)
                                    {
                                        np.ClassName = className;
                                        navigationProperties.Add(np);
                                    }
                                }
                                else
                                {
                                    fk.NavigationPropertyRefTo.ClassName = className;
                                    navigationProperties.Add(fk.NavigationPropertyRefTo);
                                }
                            }
                        }
                    }

                    // remove tables that don't participate
                    /*if (navigationProperties != null && navigationProperties.Count > 0)
                    {
                        if (tables == null || tables.Count() == 0)
                        {
                            navigationProperties.Clear();
                        }
                        else
                        {
                            navigationProperties.RemoveAll(np => (
                                (np.IsRefFrom && tables.Contains(np.ForeignKey.ToTable)) ||
                                (np.IsRefFrom == false && tables.Contains(np.ForeignKey.FromTable))
                            ) == false);
                        }
                    }*/

                    // rename duplicates
                    RenameDuplicateNavigationProperties(navigationProperties, dbObject);
                }
            }

            return navigationProperties;
        }

        protected static readonly Regex regexEndNumber = new Regex("(\\d+)$", RegexOptions.Compiled);

        protected virtual void RenameDuplicateNavigationProperties(List<NavigationProperty> navigationProperties, IDbObjectTraverse dbObject)
        {
            if (navigationProperties != null && navigationProperties.Count > 0)
            {
                // groups of navigation properties with the same name
                var groups1 = navigationProperties.GroupBy(p => p.ToString()).Where(g => g.Count() > 1);

                // if the original column name ended with a number, then assign that number to the property name
                foreach (var group in groups1)
                {
                    foreach (var np in group)
                    {
                        string columnName = (np.IsRefFrom ? np.ForeignKey.Primary_Column : np.ForeignKey.Foreign_Column);
                        var match = regexEndNumber.Match(columnName);
                        if (match.Success)
                            np.RenamedPropertyName = np.ToString() + match.Value;
                    }
                }

                // if there are still duplicate property names, then rename them with a running number suffix
                var groups2 = navigationProperties.GroupBy(p => p.ToString()).Where(g => g.Count() > 1);
                foreach (var group in groups2)
                {
                    int suffix = 1;
                    foreach (var np in group.Skip(1))
                        np.RenamedPropertyName = np.ToString() + (suffix++);
                }
            }
        }

        #endregion

        #region Write Navigation Properties

        protected virtual void WriteNavigationProperties(List<NavigationProperty> navigationProperties, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsNavigableObject(dbObject))
            {
                if (navigationProperties != null && navigationProperties.Count > 0)
                {
                    if (IsNewLineBetweenMembers == false)
                        pocoWriter.WriteLine();

                    foreach (var np in navigationProperties)
                        WriteNavigationProperty(np, dbObject, namespaceOffset);
                }
            }
        }

        protected virtual void WriteNavigationProperty(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsNewLineBetweenMembers)
                pocoWriter.WriteLine();

            WriteNavigationPropertyComments(navigationProperty, dbObject, namespaceOffset);

            WriteNavigationPropertyAttributes(navigationProperty, dbObject, namespaceOffset);

            if (navigationProperty.IsSingle)
                WriteNavigationPropertySingle(navigationProperty, dbObject, namespaceOffset);
            else
                WriteNavigationPropertyMultiple(navigationProperty, dbObject, namespaceOffset);
        }

        protected virtual void WriteNavigationPropertyComments(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsNavigationPropertiesComments)
            {
                pocoWriter.Write(namespaceOffset);
                pocoWriter.Write(Tab);
                pocoWriter.WriteComment("// ");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Foreign_Schema);
                pocoWriter.WriteComment(".");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Foreign_Table);
                pocoWriter.WriteComment(".");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Foreign_Column);
                pocoWriter.WriteComment(" -> ");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Primary_Schema);
                pocoWriter.WriteComment(".");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Primary_Table);
                pocoWriter.WriteComment(".");
                pocoWriter.WriteComment(navigationProperty.ForeignKey.Primary_Column);
                if (string.IsNullOrEmpty(navigationProperty.ForeignKey.Name) == false)
                {
                    pocoWriter.WriteComment(" (");
                    pocoWriter.WriteComment(navigationProperty.ForeignKey.Name);
                    pocoWriter.WriteComment(")");
                }
                pocoWriter.WriteLine();
            }
        }

        protected virtual void WriteNavigationPropertyAttributes(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
        }

        protected virtual void WriteNavigationPropertySingle(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteNavigationPropertyStart(namespaceOffset);
            pocoWriter.WriteUserType(navigationProperty.ClassName);
            pocoWriter.Write(" ");
            pocoWriter.Write(navigationProperty.ToString());
            WriteNavigationPropertyEnd();
            pocoWriter.WriteLine();
        }

        protected virtual void WriteNavigationPropertyMultiple(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteNavigationPropertyStart(namespaceOffset);
            if (IsNavigationPropertiesList)
                pocoWriter.WriteUserType("List");
            else if (IsNavigationPropertiesICollection)
                pocoWriter.WriteUserType("ICollection");
            else if (IsNavigationPropertiesIEnumerable)
                pocoWriter.WriteUserType("IEnumerable");
            pocoWriter.Write("<");
            pocoWriter.WriteUserType(navigationProperty.ClassName);
            pocoWriter.Write("> ");
            pocoWriter.Write(navigationProperty.ToString());
            WriteNavigationPropertyEnd();
            pocoWriter.WriteLine();
        }

        protected virtual void WriteNavigationPropertyStart(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.WriteKeyword("public");
            pocoWriter.Write(" ");

            if (IsProperties && IsNavigationPropertiesVirtual)
            {
                pocoWriter.WriteKeyword("virtual");
                pocoWriter.Write(" ");
            }
            else if (IsProperties && IsNavigationPropertiesOverride)
            {
                pocoWriter.WriteKeyword("override");
                pocoWriter.Write(" ");
            }
        }

        protected virtual void WriteNavigationPropertyEnd()
        {
            if (IsProperties)
            {
                pocoWriter.Write(" { ");
                pocoWriter.WriteKeyword("get");
                pocoWriter.Write("; ");
                pocoWriter.WriteKeyword("set");
                pocoWriter.Write("; }");
            }
            else
            {
                pocoWriter.Write(";");
            }
        }

        #endregion

        #endregion

        #region Class End

        protected virtual void WriteClassEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.WriteLine("}");
        }

        #endregion

        #region Namespace End

        protected virtual void WriteNamespaceEnd()
        {
            if (string.IsNullOrEmpty(Namespace) == false)
                pocoWriter.WriteLine("}");
        }

        #endregion
    }
}
