using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Db.DbObject;
using Db.Helpers;

namespace Db.POCOIterator
{
    public class EFIterator : DbIterator, IEFIterator
    {
        #region Constructor

        public EFIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
            : base(dbObjects, pocoWriter)
        {
        }

        #endregion

        #region EF Properties

        public virtual bool IsEF { get; set; }
        public virtual bool IsEFColumn { get; set; }
        public virtual bool IsEFRequired { get; set; }
        public virtual bool IsEFRequiredWithErrorMessage { get; set; }
        public virtual bool IsEFConcurrencyCheck { get; set; }
        public virtual bool IsEFStringLength { get; set; }
        public virtual bool IsEFDisplay { get; set; }
        public virtual bool IsEFDescription { get; set; }
        public virtual bool IsEFComplexType { get; set; }
        public virtual bool IsEFIndex { get; set; }
        public virtual bool IsEFForeignKey { get; set; }

        #endregion

        #region Using

        protected override void WriteUsing()
        {
            if (IsUsing)
            {
                WriteUsingClause();

                if (IsEF)
                {
                    if (dbObjects != null && dbObjects.Count() > 0)
                    {
                        if (dbObjects.Any(o => o.DbType == DbType.Table))
                        {
                            if (IsEFDescription)
                            {
                                pocoWriter.WriteKeyword("using");
                                pocoWriter.WriteLine(" System.ComponentModel;");
                            }

                            pocoWriter.WriteKeyword("using");
                            pocoWriter.WriteLine(" System.ComponentModel.DataAnnotations;");

                            pocoWriter.WriteKeyword("using");
                            pocoWriter.WriteLine(" System.ComponentModel.DataAnnotations.Schema;");
                        }
                    }
                }

                pocoWriter.WriteLine();
            }
        }

        #endregion

        #region Class Attributes

        protected override void WriteClassAttributes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsEF && dbObject.DbType == DbType.Table)
            {
                WriteEFTable(dbObject, namespaceOffset);

                if (IsEFDescription)
                {
                    Table table = (Table)dbObject;
                    if (table.HasExtendedProperties)
                    {
                        foreach (ExtendedProperty extendedProperty in table.ExtendedProperties)
                            WriteEFDescription(extendedProperty.Description, false, namespaceOffset);
                    }
                }
            }
        }

        protected virtual void WriteEFTable(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Table");
            pocoWriter.Write("(");
            pocoWriter.WriteString("\"");
            if (dbObject.Schema != "dbo")
            {
                pocoWriter.WriteString(dbObject.Schema);
                pocoWriter.WriteString(".");
            }
            pocoWriter.WriteString(dbObject.Name);
            pocoWriter.WriteString("\"");
            pocoWriter.WriteLine(")]");
        }

        #endregion

        #region Column Attributes

        protected override void WriteColumnAttributes(IColumn column, string cleanColumnName, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsEF && dbObject.DbType == DbType.Table)
            {
                // Primary Key
                bool isCompositePrimaryKey = IsCompositePrimaryKey(dbObject);
                if (column.IsPrimaryKey)
                {
                    if (isCompositePrimaryKey)
                        WriteEFCompositePrimaryKey(column.ColumnName, column.DataTypeName, ((TableColumn)column).PrimaryKey.Ordinal, namespaceOffset);
                    else
                        WriteEFPrimaryKey(namespaceOffset);
                }

                // Index
                if (IsEFIndex && column.HasIndexColumns)
                {
                    TableColumn tableColumn = (TableColumn)column;
                    foreach (IndexColumn indexColumn in tableColumn.IndexColumns.OrderBy(ic => ic.Name))
                    {
                        bool isCompositeIndex = tableColumn.Table.TableColumns.Exists(tc => tc != tableColumn && tc.HasIndexColumns && tc.IndexColumns.Exists(ic => ic.Name == indexColumn.Name));
                        if (isCompositeIndex)
                            WriteEFCompositeIndex(indexColumn.Name, indexColumn.Is_Unique, indexColumn.Is_Clustered, indexColumn.Is_Descending, indexColumn.Ordinal, namespaceOffset);
                        else
                            WriteEFIndex(indexColumn.Name, indexColumn.Is_Unique, indexColumn.Is_Clustered, indexColumn.Is_Descending, namespaceOffset);
                    }
                }

                // Column
                if ((IsEFColumn && (column.IsPrimaryKey == false || isCompositePrimaryKey == false)) || (column.ColumnName != cleanColumnName))
                    if (column is IComplexType == false)
                        WriteEFColumn(column.ColumnName, column.DataTypeName, namespaceOffset);

                // MaxLength
                if (column.DataTypeName == "binary" || column.DataTypeName == "char" || column.DataTypeName == "nchar" || column.DataTypeName == "nvarchar" || column.DataTypeName == "varbinary" || column.DataTypeName == "varchar")
                    WriteEFMaxLength(column.StringPrecision, namespaceOffset);

                // StringLength
                if (IsEFStringLength)
                {
                    if (column.DataTypeName == "binary" || column.DataTypeName == "char" || column.DataTypeName == "nchar" || column.DataTypeName == "nvarchar" || column.DataTypeName == "varbinary" || column.DataTypeName == "varchar")
                    {
                        if (column.StringPrecision > 0)
                            WriteEFStringLength(column.StringPrecision.Value, namespaceOffset);
                    }
                }

                // Timestamp
                if (column.DataTypeName == "timestamp")
                    WriteEFTimestamp(namespaceOffset);

                // ConcurrencyCheck
                if (IsEFConcurrencyCheck)
                {
                    if (column.DataTypeName == "timestamp" || column.DataTypeName == "rowversion")
                        WriteEFConcurrencyCheck(namespaceOffset);
                }

                // DatabaseGenerated Identity
                if (column.IsIdentity == true)
                    WriteEFDatabaseGeneratedIdentity(namespaceOffset);

                // DatabaseGenerated Computed
                if (column.IsComputed)
                    WriteEFDatabaseGeneratedComputed(namespaceOffset);

                string display = null;
                if (IsEFRequiredWithErrorMessage || IsEFDisplay)
                    display = GetEFDisplay(column.ColumnName);

                // Required
                if (IsEFRequired || IsEFRequiredWithErrorMessage)
                {
                    if (column.IsNullable == false)
                        WriteEFRequired(display, namespaceOffset);
                }

                // Display
                if (IsEFDisplay)
                    WriteEFDisplay(display, namespaceOffset);

                // Description
                if (IsEFDescription)
                {
                    TableColumn tableColumn = (TableColumn)column;

                    if (tableColumn.HasExtendedProperties)
                    {
                        foreach (ExtendedProperty extendedProperty in tableColumn.ExtendedProperties)
                            WriteEFDescription(extendedProperty.Description, true, namespaceOffset);
                    }

                    /*if (IsEFIndex && tableColumn.HasIndexColumns)
                    {
                        foreach (IndexColumn indexColumn in tableColumn.IndexColumns.OrderBy(ic => ic.Name))
                        {
                            if (indexColumn.HasExtendedProperties)
                            {
                                foreach (ExtendedProperty extendedProperty in indexColumn.ExtendedProperties)
                                    WriteEFDescription(extendedProperty.Description, true, namespaceOffset);
                            }
                        }
                    }*/
                }
            }
        }

        protected virtual bool IsCompositePrimaryKey(IDbObjectTraverse dbObject)
        {
            if (dbObject.Columns != null && dbObject.Columns.Count() > 0)
            {
                var primaryKeys = dbObject.Columns.Where(c => c.IsPrimaryKey);
                return (primaryKeys.Count() > 1);
            }
            return false;
        }

        protected virtual void WriteEFPrimaryKey(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Key");
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFCompositePrimaryKey(string columnName, string dataTypeName, byte ordinal, string namespaceOffset)
        {
            WriteEFPrimaryKey(namespaceOffset);

            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Column");
            pocoWriter.Write("(");

            if (IsEFColumn)
            {
                pocoWriter.Write("Name = ");
                pocoWriter.WriteString("\"");
                pocoWriter.WriteString(columnName);
                pocoWriter.WriteString("\"");
                pocoWriter.Write(", TypeName = ");
                pocoWriter.WriteString("\"");
                pocoWriter.WriteString(dataTypeName);
                pocoWriter.WriteString("\"");
                pocoWriter.Write(", ");
            }

            pocoWriter.Write("Order = ");
            pocoWriter.Write(ordinal.ToString());
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteEFIndex(string indexName, bool? isUnique, bool? isClustered, bool? isDescending, string namespaceOffset)
        {
            WriteEFIndexSortOrderError(indexName, isDescending, namespaceOffset);
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Index");
            pocoWriter.Write("(");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(indexName);
            pocoWriter.WriteString("\"");
            if (isUnique == true)
            {
                pocoWriter.Write(", IsUnique = ");
                pocoWriter.WriteKeyword("true");
            }
            if (isClustered == true)
            {
                pocoWriter.Write(", IsClustered = ");
                pocoWriter.WriteKeyword("true");
            }
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteEFCompositeIndex(string indexName, bool? isUnique, bool? isClustered, bool? isDescending, byte ordinal, string namespaceOffset)
        {
            WriteEFIndexSortOrderError(indexName, isDescending, namespaceOffset);
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Index");
            pocoWriter.Write("(");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(indexName);
            pocoWriter.WriteString("\"");
            pocoWriter.Write(", ");
            pocoWriter.Write(ordinal.ToString());
            if (isUnique == true)
            {
                pocoWriter.Write(", IsUnique = ");
                pocoWriter.WriteKeyword("true");
            }
            if (isClustered == true)
            {
                pocoWriter.Write(", IsClustered = ");
                pocoWriter.WriteKeyword("true");
            }
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteEFIndexSortOrderError(string indexName, bool? isDescending, string namespaceOffset)
        {
            if (isDescending == true)
            {
                pocoWriter.Write(namespaceOffset);
                pocoWriter.Write(Tab);
                pocoWriter.WriteError("/* ");
                pocoWriter.WriteError(indexName);
                pocoWriter.WriteLineError(". Sort order is Descending. Index doesn't support sort order. */");
            }
        }

        protected virtual void WriteEFColumn(string columnName, string dataTypeName, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Column");
            pocoWriter.Write("(Name = ");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(columnName);
            pocoWriter.WriteString("\"");
            pocoWriter.Write(", TypeName = ");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(dataTypeName);
            pocoWriter.WriteString("\"");
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteEFMaxLength(int? stringPrecision, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("MaxLength");
            if (stringPrecision > 0)
            {
                pocoWriter.Write("(");
                pocoWriter.Write(stringPrecision.ToString());
                pocoWriter.Write(")");
            }
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFStringLength(int stringPrecision, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("StringLength");
            pocoWriter.Write("(");
            pocoWriter.Write(stringPrecision.ToString());
            pocoWriter.Write(")");
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFTimestamp(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Timestamp");
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFConcurrencyCheck(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("ConcurrencyCheck");
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFDatabaseGeneratedIdentity(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("DatabaseGenerated");
            pocoWriter.Write("(");
            pocoWriter.WriteUserType("DatabaseGeneratedOption");
            pocoWriter.WriteLine(".Identity)]");
        }

        protected virtual void WriteEFDatabaseGeneratedComputed(string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("DatabaseGenerated");
            pocoWriter.Write("(");
            pocoWriter.WriteUserType("DatabaseGeneratedOption");
            pocoWriter.WriteLine(".Computed)]");
        }

        protected static readonly Regex regexDisplay1 = new Regex("[^0-9a-zA-Z]", RegexOptions.Compiled);
        protected static readonly Regex regexDisplay2 = new Regex("([^A-Z]|^)(([A-Z\\s]*)($|[A-Z]))", RegexOptions.Compiled);
        protected static readonly Regex regexDisplay3 = new Regex("\\s{2,}", RegexOptions.Compiled);

        protected virtual string GetEFDisplay(string columnName)
        {
            string display = columnName;
            display = regexDisplay1.Replace(display, " ");
            display = regexDisplay2.Replace(display, "$1 $3 $4");
            display = display.Trim();
            display = regexDisplay3.Replace(display, " ");
            return display;
        }

        protected virtual void WriteEFRequired(string display, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Required");
            if (IsEFRequiredWithErrorMessage)
                WriteEFRequiredErrorMessage(display);
            pocoWriter.WriteLine("]");
        }

        protected virtual void WriteEFRequiredErrorMessage(string display)
        {
            pocoWriter.Write("(ErrorMessage = ");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(display);
            pocoWriter.WriteString(" is required");
            pocoWriter.WriteString("\"");
            pocoWriter.Write(")");
        }

        protected virtual void WriteEFDisplay(string display, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("Display");
            pocoWriter.Write("(Name = ");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(display);
            pocoWriter.WriteString("\"");
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteEFDescription(string description, bool writeTab, string namespaceOffset)
        {
            if (string.IsNullOrEmpty(description) == false)
            {
                pocoWriter.Write(namespaceOffset);
                if (writeTab)
                    pocoWriter.Write(Tab);
                pocoWriter.Write("[");
                pocoWriter.WriteUserType("Description");
                pocoWriter.Write("(");
                pocoWriter.WriteString("\"");
                pocoWriter.WriteString(description);
                pocoWriter.WriteString("\"");
                pocoWriter.WriteLine(")]");
            }
        }

        #endregion

        #region Column

        private List<string> complexTypeNames;
        private List<ComplexTypeColumn> complexTypeColumns;

        protected override void WriteColumn(IColumn column, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsEF && dbObject.DbType == DbType.Table && IsEFComplexType)
            {
                string columnName = column.ColumnName.Trim();
                int index = columnName.IndexOf('_');
                if (index != -1 && index != 0 && index != columnName.Length - 1)
                {
                    string complexTypeName = NameHelper.CleanName(columnName.Substring(0, index));
                    string complexTypeColumnName = columnName.Substring(index + 1);

                    if (complexTypeNames == null)
                        complexTypeNames = new List<string>();
                    if (complexTypeNames.Contains(complexTypeName) == false)
                    {
                        ComplexType complexType = new ComplexType(complexTypeName, (TableColumn)column);
                        base.WriteColumn(complexType, isLastColumn, dbObject, namespaceOffset);
                        complexTypeNames.Add(complexTypeName);
                    }

                    ComplexTypeColumn complexTypeColumn = new ComplexTypeColumn(complexTypeName, complexTypeColumnName, (TableColumn)column);
                    if (complexTypeColumns == null)
                        complexTypeColumns = new List<ComplexTypeColumn>();
                    complexTypeColumns.Add(complexTypeColumn);
                }
                else
                {
                    base.WriteColumn(column, isLastColumn, dbObject, namespaceOffset);
                }
            }
            else
            {
                base.WriteColumn(column, isLastColumn, dbObject, namespaceOffset);
            }
        }

        protected override void WriteColumnDataType(IColumn column)
        {
            if (IsEF && IsEFComplexType && column is IComplexType)
                pocoWriter.WriteUserType(column.DataTypeDisplay);
            else
                base.WriteColumnDataType(column);
        }

        #endregion

        #region Navigation Properties

        protected override List<NavigationProperty> GetNavigationProperties(IDbObjectTraverse dbObject/*, IEnumerable<Table> tables*/)
        {
            List<NavigationProperty> navigationProperties = base.GetNavigationProperties(dbObject/*, tables*/);
            GetNavigationPropertiesMultipleRelationships(navigationProperties);
            return navigationProperties;
        }

        protected virtual void GetNavigationPropertiesMultipleRelationships(List<NavigationProperty> navigationProperties)
        {
            if (navigationProperties != null && navigationProperties.Count > 0)
            {
                var multipleRels = navigationProperties
                    .GroupBy(np => new
                    {
                        np.ForeignKey.Foreign_Schema_Id,
                        np.ForeignKey.Foreign_Table_Id,
                        np.ForeignKey.Primary_Schema_Id,
                        np.ForeignKey.Primary_Table_Id
                    })
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g);

                foreach (var np in multipleRels)
                    np.HasMultipleRelationships = true;
            }
        }

        protected override void WriteNavigationPropertyAttributes(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsEF && IsEFForeignKey)
            {
                if (IsNavigableObject(dbObject))
                {
                    if (navigationProperty.IsRefFrom)
                        WriteNavigationPropertyForeignKeyAttribute(navigationProperty, dbObject, namespaceOffset);

                    if (navigationProperty.IsRefFrom == false && navigationProperty.HasMultipleRelationships)
                        WriteNavigationPropertyInversePropertyAttribute(navigationProperty, dbObject, namespaceOffset);
                }
            }
        }

        protected virtual void WriteNavigationPropertyForeignKeyAttribute(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("ForeignKey");
            pocoWriter.Write("(");
            pocoWriter.WriteString("\"");
            if (navigationProperty.HasMultipleRelationships)
                pocoWriter.WriteString(navigationProperty.ForeignKey.Foreign_Column);
            else
                pocoWriter.WriteString(navigationProperty.ForeignKey.Primary_Column);
            pocoWriter.WriteString("\"");
            pocoWriter.WriteLine(")]");
        }

        protected virtual void WriteNavigationPropertyInversePropertyAttribute(NavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("InverseProperty");
            pocoWriter.Write("(");
            pocoWriter.WriteString("\"");
            pocoWriter.WriteString(navigationProperty.InverseProperty.ToString());
            pocoWriter.WriteString("\"");
            pocoWriter.WriteLine(")]");
        }

        #endregion

        #region Class End

        protected override void WriteClassEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsEF && dbObject.DbType == DbType.Table && IsEFComplexType)
            {
                if (complexTypeColumns != null && complexTypeColumns.Count > 0)
                    WriteComplexTypes(dbObject, namespaceOffset);
            }

            base.WriteClassEnd(dbObject, namespaceOffset);
        }

        protected virtual void WriteComplexTypes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            var complexTypes = complexTypeColumns.GroupBy(x => x.ComplexTypeName);
            foreach (var complexType in complexTypes)
                WriteComplexType(complexType.Key, complexType, dbObject, namespaceOffset);
        }

        protected virtual void WriteComplexType(string complexTypeName, IEnumerable<ComplexTypeColumn> complexTypeColumns, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            pocoWriter.WriteLine();

            // Class Attribute
            pocoWriter.Write(namespaceOffset);
            pocoWriter.Write(Tab);
            pocoWriter.Write("[");
            pocoWriter.WriteUserType("ComplexType");
            pocoWriter.WriteLine("]");

            namespaceOffset += Tab;

            // Class Start
            base.WriteClassStart(complexTypeName, dbObject, namespaceOffset);

            // Columns
            var columns = complexTypeColumns.OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
            var lastColumn = columns.Last();
            foreach (IColumn column in columns)
                base.WriteColumn(column, column == lastColumn, dbObject, namespaceOffset);

            // Class End
            base.WriteClassEnd(dbObject, namespaceOffset);
        }

        #endregion
    }
}
