using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using CommandLine;
using Db;
using Db.DbObject;
using Db.Helpers;
using Db.POCOIterator;
using Microsoft.Data.ConnectionUI;
using POCOGenerator.CommandLine;
using POCOGenerator.Extensions;
using POCOGenerator.Helpers;
using POCOGenerator.POCOWriter;

namespace POCOGenerator
{
    public partial class POCOGeneratorForm : Form
    {
        #region Form

        public POCOGeneratorForm()
        {
            InitializeComponent();
        }

        private void POCOGeneratorForm_Load(object sender, EventArgs e)
        {
            IsProperties = true;
            IsNavigationPropertiesList = true;
            LoadOptions();
        }

        private void POCOGeneratorForm_Shown(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString(DbHelper.ConnectionString);
            if (string.IsNullOrEmpty(connectionString) == false)
            {
                SetConnectionString(connectionString);
                BuildServerTree();
            }
            else
            {
                this.Close();
            }
        }

        private void POCOGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Connection String

        private string GetConnectionString(string connectionString = null)
        {
            using (DataConnectionDialog dcd = new DataConnectionDialog())
            {
                dcd.DataSources.Add(DataSource.SqlDataSource);
                dcd.DataSources.Add(DataSource.SqlFileDataSource);
                dcd.SelectedDataSource = DataSource.SqlDataSource;
                dcd.SelectedDataProvider = DataProvider.SqlDataProvider;

                try
                {
                    if (string.IsNullOrEmpty(connectionString) == false)
                        dcd.ConnectionString = connectionString;
                }
                catch
                {
                }

                if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
                    return dcd.ConnectionString;

                return null;
            }
        }

        private void SetConnectionString(string connectionString)
        {
            DbHelper.ConnectionString = connectionString;

            int index = connectionString.IndexOf("Data Source=");
            if (index != -1)
            {
                string server = connectionString.Substring(index + "Data Source=".Length);
                index = server.IndexOf(';');
                if (index != -1)
                    server = server.Substring(0, index);
                string instanceName = null;
                index = server.LastIndexOf("\\");
                if (index != -1)
                {
                    instanceName = server.Substring(index + 1);
                    server = server.Substring(0, index);
                }

                Server = new Server()
                {
                    ServerName = server,
                    InstanceName = instanceName
                };

                index = connectionString.IndexOf("User ID=");
                if (index != -1)
                {
                    string userId = connectionString.Substring(index + "User ID=".Length);
                    index = userId.IndexOf(';');
                    if (index != -1)
                        userId = userId.Substring(0, index);
                    Server.UserId = userId;
                }
                else
                {
                    index = connectionString.IndexOf("Integrated Security=True");
                    if (index != -1)
                        Server.UserId = WindowsIdentity.GetCurrent().Name;
                }

                Server.Version = DbHelper.GetServerVersion();
            }

            index = connectionString.IndexOf("Initial Catalog=");
            if (index != -1)
            {
                string initialCatalog = connectionString.Substring(index + "Initial Catalog=".Length);
                index = initialCatalog.IndexOf(';');
                if (index != -1)
                    initialCatalog = initialCatalog.Substring(0, index);
                InitialCatalog = initialCatalog;
            }
        }

        #endregion

        #region Server Tree

        private Server Server;
        private string InitialCatalog;

        private class NodeTag
        {
            public DbType NodeType { get; set; }
            public IDbObject DbObject { get; set; }
        }

        private enum ImageType
        {
            Server,
            Database,
            Folder,
            Table,
            View,
            Procedure,
            Function,
            Column,
            PrimaryKey,
            ForeignKey,
            UniqueKey,
            Index
        }

        private void BuildServerTree()
        {
            try
            {
                DisableServerTree();

                TreeNode serverNode = BuildServerNode();
                trvServer.Nodes.Add(serverNode);
                Application.DoEvents();

                Database databaseCurrent = null;
                TreeNode databaseNodeCurrent = null;
                TreeNode tablesNode = null;
                TreeNode viewsNode = null;
                TreeNode proceduresNode = null;
                TreeNode functionsNode = null;
                TreeNode tvpsNode = null;

                Action<IDbObject> buildingDbObject = (IDbObject dbObject) =>
                {
                    if (dbObject is Database)
                    {
                        Database database = dbObject as Database;
                        TreeNode databaseNode = AddDatabaseNode(serverNode, database);

                        databaseCurrent = database;
                        databaseNodeCurrent = databaseNode;
                        tablesNode = null;
                        viewsNode = null;
                        proceduresNode = null;
                        functionsNode = null;
                        tvpsNode = null;
                    }

                    ShowBuildingStatus(dbObject);
                };

                Action<IDbObject> builtDbObject = (IDbObject dbObject) =>
                {
                    if (dbObject is Database)
                    {
                        Database database = dbObject as Database;
                        if (database.Errors.Count > 0)
                            databaseNodeCurrent.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = string.Empty;
                        Application.DoEvents();
                    }
                    else if (dbObject is Table && (dbObject is Db.DbObject.View) == false)
                    {
                        Table table = dbObject as Table;
                        tablesNode = AddTablesNode(tablesNode, databaseCurrent, databaseNodeCurrent);
                        AddTableNode(tablesNode, table);
                    }
                    else if (dbObject is Db.DbObject.View)
                    {
                        Db.DbObject.View view = dbObject as Db.DbObject.View;
                        viewsNode = AddViewsNode(viewsNode, databaseCurrent, databaseNodeCurrent);
                        AddViewNode(viewsNode, view);
                    }
                    else if (dbObject is Procedure && (dbObject is Function) == false)
                    {
                        Procedure procedure = dbObject as Procedure;
                        proceduresNode = AddProceduresNode(proceduresNode, databaseCurrent, databaseNodeCurrent);
                        AddProcedureNode(proceduresNode, procedure);
                    }
                    else if (dbObject is Function)
                    {
                        Function function = dbObject as Function;
                        functionsNode = AddFunctionsNode(functionsNode, databaseCurrent, databaseNodeCurrent);
                        AddFunctionNode(functionsNode, function);
                    }
                    else if (dbObject is TVP)
                    {
                        TVP tvp = dbObject as TVP;
                        tvpsNode = AddTVPsNode(tvpsNode, databaseCurrent, databaseNodeCurrent);
                        AddTVPNode(tvpsNode, tvp);
                    }
                };

                DbHelper.BuildServerSchema(Server, InitialCatalog, buildingDbObject, builtDbObject);

                trvServer.SelectedNode = serverNode;

                EnableServerTree();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = (ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : string.Empty)).Replace(Environment.NewLine, " ");
                toolStripStatusLabel.ForeColor = Color.Red;
            }
        }

        private void DisableServerTree()
        {
            trvServer.BeforeCollapse += trvServer_DisableEvent;
            trvServer.BeforeExpand += trvServer_DisableEvent;
            trvServer.AfterCheck += trvServer_AfterCheck;
            trvServer.MouseUp += trvServer_MouseUp;
        }

        private void EnableServerTree()
        {
            trvServer.BeforeCollapse -= trvServer_DisableEvent;
            trvServer.BeforeExpand -= trvServer_DisableEvent;
            trvServer.AfterCheck -= trvServer_AfterCheck;
            trvServer.MouseUp -= trvServer_MouseUp;
        }

        private void trvServer_DisableEvent(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ShowBuildingStatus(IDbObject dbObject)
        {
            if (dbObject is Table)
            {
                Table table = dbObject as Table;
                toolStripStatusLabel.Text = string.Format("{0}.{1}", table.Database.ToString(), dbObject.ToString());
            }
            else if (dbObject is Procedure)
            {
                Procedure procedure = dbObject as Procedure;
                toolStripStatusLabel.Text = string.Format("{0}.{1}", procedure.Database.ToString(), dbObject.ToString());
            }
            else if (dbObject is TVP)
            {
                TVP tvp = dbObject as TVP;
                toolStripStatusLabel.Text = string.Format("{0}.{1}", tvp.Database.ToString(), dbObject.ToString());
            }
            else
            {
                toolStripStatusLabel.Text = string.Format("{0}", dbObject.ToString());
            }
            toolStripStatusLabel.ForeColor = Color.Black;
            Application.DoEvents();
        }

        private TreeNode AddDatabaseNode(TreeNode serverNode, Database database)
        {
            TreeNode databaseNode = BuildDatabaseNode(database);
            serverNode.Nodes.AddSorted(databaseNode);
            EnableServerTree();
            serverNode.Expand();
            Application.DoEvents();
            DisableServerTree();
            return databaseNode;
        }

        private TreeNode AddTablesNode(TreeNode tablesNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (tablesNode == null)
            {
                tablesNode = BuildTablesNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Insert(0, tablesNode);
                Application.DoEvents();
            }
            return tablesNode;
        }

        private void AddTableNode(TreeNode tablesNode, Table table)
        {
            TreeNode tableNode = BuildTableNode(table);
            tablesNode.Nodes.AddSorted(tableNode);
            Application.DoEvents();
        }

        private TreeNode AddViewsNode(TreeNode viewsNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (viewsNode == null)
            {
                viewsNode = BuildViewsNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Insert(1, viewsNode);
                Application.DoEvents();
            }
            return viewsNode;
        }

        private void AddViewNode(TreeNode viewsNode, Db.DbObject.View view)
        {
            TreeNode viewNode = BuildViewNode(view);
            viewsNode.Nodes.AddSorted(viewNode);
            Application.DoEvents();
        }

        private TreeNode AddProceduresNode(TreeNode proceduresNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (proceduresNode == null)
            {
                proceduresNode = BuildProceduresNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Insert(2, proceduresNode);
                Application.DoEvents();
            }
            return proceduresNode;
        }

        private void AddProcedureNode(TreeNode proceduresNode, Procedure procedure)
        {
            TreeNode procedureNode = BuildProcedureNode(procedure);
            proceduresNode.Nodes.AddSorted(procedureNode);
            Application.DoEvents();
        }

        private TreeNode AddFunctionsNode(TreeNode functionsNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (functionsNode == null)
            {
                functionsNode = BuildFunctionsNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Insert(3, functionsNode);
                Application.DoEvents();
            }
            return functionsNode;
        }

        private void AddFunctionNode(TreeNode functionsNode, Function function)
        {
            TreeNode functionNode = BuildFunctionNode(function);
            functionsNode.Nodes.AddSorted(functionNode);
            Application.DoEvents();
        }

        private TreeNode AddTVPsNode(TreeNode tvpsNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (tvpsNode == null)
            {
                tvpsNode = BuildTVPsNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Add(tvpsNode); // first one to be inserted
                Application.DoEvents();
            }
            return tvpsNode;
        }

        private void AddTVPNode(TreeNode tvpsNode, TVP tvp)
        {
            TreeNode tvpNode = BuildTVPNode(tvp);
            tvpsNode.Nodes.AddSorted(tvpNode);
            Application.DoEvents();
        }

        private TreeNode BuildServerNode()
        {
            string serverName = Server.ToString();
            if (string.IsNullOrEmpty(Server.Version) == false)
            {
                serverName += string.Format(" (SQL Server {0}", Server.Version.Substring(0, Server.Version.LastIndexOf('.')));
                if (string.IsNullOrEmpty(Server.UserId) == false)
                    serverName += " - " + Server.UserId;
                serverName += ")";
            }
            TreeNode serverNode = new TreeNode(serverName);
            serverNode.Tag = new NodeTag() { NodeType = DbType.Server, DbObject = Server };
            serverNode.ImageIndex = (int)ImageType.Server;
            serverNode.SelectedImageIndex = (int)ImageType.Server;
            return serverNode;
        }

        private TreeNode BuildDatabaseNode(Database database)
        {
            TreeNode databaseNode = new TreeNode(database.ToString());
            databaseNode.Tag = new NodeTag() { NodeType = DbType.Database, DbObject = database };
            databaseNode.ImageIndex = (int)ImageType.Database;
            databaseNode.SelectedImageIndex = (int)ImageType.Database;
            return databaseNode;
        }

        private TreeNode BuildTablesNode(Database database)
        {
            TreeNode tablesNode = new TreeNode("Tables");
            tablesNode.Tag = new NodeTag() { NodeType = DbType.Tables, DbObject = database };
            tablesNode.ImageIndex = (int)ImageType.Folder;
            tablesNode.SelectedImageIndex = (int)ImageType.Folder;
            return tablesNode;
        }

        private TreeNode BuildTableNode(Table table)
        {
            TreeNode tableNode = new TreeNode(table.ToString());
            tableNode.Tag = new NodeTag() { NodeType = DbType.Table, DbObject = table };
            tableNode.ImageIndex = (int)ImageType.Table;
            tableNode.SelectedImageIndex = (int)ImageType.Table;

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;
            tableNode.Nodes.Add(columnsNode);

            if (table.TableColumns != null)
            {
                foreach (TableColumn column in table.TableColumns.OrderBy(c => c.ordinal_position ?? 0))
                    columnsNode.Nodes.Add(BuildTableColumn(column));

                if (table.TableColumns.Exists(c => c.IsPrimaryKey))
                    tableNode.Nodes.Add(BuildPrimaryKeysNode(table));

                if (table.TableColumns.Exists(c => c.HasUniqueKeys))
                    tableNode.Nodes.Add(BuildUniqueKeysNode(table));

                if (table.TableColumns.Exists(c => c.HasForeignKeys))
                    tableNode.Nodes.Add(BuildForeignKeysNode(table));

                if (table.TableColumns.Exists(c => c.HasIndexColumns))
                    tableNode.Nodes.Add(BuildIndexesNode(table));
            }
            else if (table.Error != null)
            {
                tableNode.ForeColor = Color.Red;
            }

            return tableNode;
        }

        private TreeNode BuildPrimaryKeysNode(Table table)
        {
            TreeNode primaryKeysNode = new TreeNode("Primary Keys");
            primaryKeysNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
            primaryKeysNode.ImageIndex = (int)ImageType.Folder;
            primaryKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var primaryKeys = table.TableColumns.Where(c => c.IsPrimaryKey).Select(c => c.PrimaryKey.Name).Distinct().OrderBy(n => n);
            foreach (string primaryKey in primaryKeys)
            {
                TreeNode columnNode = new TreeNode(primaryKey);
                columnNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.PrimaryKey;
                columnNode.SelectedImageIndex = (int)ImageType.PrimaryKey;
                primaryKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.IsPrimaryKey && c.PrimaryKey.Name == primaryKey).OrderBy(c => c.PrimaryKey.Ordinal))
                    columnNode.Nodes.Add(BuildTableColumn(column));
            }

            return primaryKeysNode;
        }

        private TreeNode BuildUniqueKeysNode(Table table)
        {
            TreeNode uniqueKeysNode = new TreeNode("Unique Keys");
            uniqueKeysNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
            uniqueKeysNode.ImageIndex = (int)ImageType.Folder;
            uniqueKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var uniqueKeys = table.TableColumns.Where(c => c.HasUniqueKeys).SelectMany(c => c.UniqueKeys).Select(uk => uk.Name).Distinct().OrderBy(n => n);
            foreach (string uniqueKey in uniqueKeys)
            {
                TreeNode columnNode = new TreeNode(uniqueKey);
                columnNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.UniqueKey;
                columnNode.SelectedImageIndex = (int)ImageType.UniqueKey;
                uniqueKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.HasUniqueKeys && c.UniqueKeys.Exists(uk => uk.Name == uniqueKey)).OrderBy(c => c.UniqueKeys.First(uk => uk.Name == uniqueKey).Ordinal))
                    columnNode.Nodes.Add(BuildTableColumn(column));
            }

            return uniqueKeysNode;
        }

        private TreeNode BuildForeignKeysNode(Table table)
        {
            TreeNode foreignKeysNode = new TreeNode("Foreign Keys");
            foreignKeysNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
            foreignKeysNode.ImageIndex = (int)ImageType.Folder;
            foreignKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var foreignKeys = table.TableColumns.Where(c => c.HasForeignKeys).SelectMany(c => c.ForeignKeys).Select(fk => fk.Name).Distinct().OrderBy(n => n);
            foreach (string foreignKey in foreignKeys)
            {
                TreeNode columnNode = new TreeNode(foreignKey);
                columnNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.ForeignKey;
                columnNode.SelectedImageIndex = (int)ImageType.ForeignKey;
                foreignKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.HasForeignKeys && c.ForeignKeys.Exists(fk => fk.Name == foreignKey)).OrderBy(c => c.ForeignKeys.First(fk => fk.Name == foreignKey).Ordinal))
                {
                    ForeignKey fk = column.ForeignKeys.First(fk1 => fk1.Name == foreignKey);
                    string primary = string.Format(" -> {0}.{1}.{2}", fk.Primary_Schema, fk.Primary_Table, fk.Primary_Column);
                    columnNode.Nodes.Add(BuildTableColumn(column, primary));
                }
            }

            return foreignKeysNode;
        }

        private TreeNode BuildIndexesNode(Table table)
        {
            TreeNode indexesNode = new TreeNode("Indexes");
            indexesNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
            indexesNode.ImageIndex = (int)ImageType.Folder;
            indexesNode.SelectedImageIndex = (int)ImageType.Folder;

            var indexColumns = table.TableColumns.Where(c => c.HasIndexColumns).SelectMany(c => c.IndexColumns).Select(ic => ic.ToStringFull()).Distinct().OrderBy(n => n);
            foreach (string indexColumn in indexColumns)
            {
                TreeNode columnNode = new TreeNode(indexColumn);
                columnNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.Index;
                columnNode.SelectedImageIndex = (int)ImageType.Index;
                indexesNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.HasIndexColumns && c.IndexColumns.Exists(ic => ic.ToStringFull() == indexColumn)).OrderBy(c => c.IndexColumns.First(ic => ic.ToStringFull() == indexColumn).Ordinal))
                    columnNode.Nodes.Add(BuildTableColumn(column));
            }

            return indexesNode;
        }

        private TreeNode BuildTableColumn(TableColumn column, string postfix = null)
        {
            TreeNode tableColumnNode = new TreeNode(column.ToStringFull() + postfix);
            tableColumnNode.Tag = new NodeTag() { NodeType = DbType.Column, DbObject = column };
            tableColumnNode.ImageIndex = (int)(column.IsPrimaryKey ? ImageType.PrimaryKey : (column.HasForeignKeys ? ImageType.ForeignKey : ImageType.Column));
            tableColumnNode.SelectedImageIndex = (int)(column.IsPrimaryKey ? ImageType.PrimaryKey : (column.HasForeignKeys ? ImageType.ForeignKey : ImageType.Column));
            return tableColumnNode;
        }

        private TreeNode BuildViewsNode(Database database)
        {
            TreeNode viewsNode = new TreeNode("Views");
            viewsNode.Tag = new NodeTag() { NodeType = DbType.Views, DbObject = database };
            viewsNode.ImageIndex = (int)ImageType.Folder;
            viewsNode.SelectedImageIndex = (int)ImageType.Folder;
            return viewsNode;
        }

        private TreeNode BuildViewNode(Db.DbObject.View view)
        {
            TreeNode viewNode = new TreeNode(view.ToString());
            viewNode.Tag = new NodeTag() { NodeType = DbType.View, DbObject = view };
            viewNode.ImageIndex = (int)ImageType.View;
            viewNode.SelectedImageIndex = (int)ImageType.View;

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = DbType.Columns, DbObject = view };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;
            viewNode.Nodes.Add(columnsNode);

            if (view.TableColumns != null)
            {
                foreach (TableColumn column in view.TableColumns.OrderBy(c => c.ordinal_position ?? 0))
                {
                    TreeNode columnNode = new TreeNode(column.ToString());
                    columnNode.Tag = new NodeTag() { NodeType = DbType.Column, DbObject = column };
                    columnNode.ImageIndex = (int)ImageType.Column;
                    columnNode.SelectedImageIndex = (int)ImageType.Column;
                    columnsNode.Nodes.Add(columnNode);
                }
            }
            else if (view.Error != null)
            {
                viewNode.ForeColor = Color.Red;
            }

            return viewNode;
        }

        private TreeNode BuildProceduresNode(Database database)
        {
            TreeNode proceduresNode = new TreeNode("Stored Procedures");
            proceduresNode.Tag = new NodeTag() { NodeType = DbType.Procedures, DbObject = database };
            proceduresNode.ImageIndex = (int)ImageType.Folder;
            proceduresNode.SelectedImageIndex = (int)ImageType.Folder;
            return proceduresNode;
        }

        private TreeNode BuildProcedureNode(Procedure procedure)
        {
            TreeNode procedureNode = new TreeNode(procedure.ToString());
            procedureNode.Tag = new NodeTag() { NodeType = DbType.Procedure, DbObject = procedure };
            procedureNode.ImageIndex = (int)ImageType.Procedure;
            procedureNode.SelectedImageIndex = (int)ImageType.Procedure;

            TreeNode parametersNode = new TreeNode("Parameters");
            parametersNode.Tag = new NodeTag() { NodeType = DbType.ProcedureParameters, DbObject = procedure };
            parametersNode.ImageIndex = (int)ImageType.Folder;
            parametersNode.SelectedImageIndex = (int)ImageType.Folder;
            procedureNode.Nodes.Add(parametersNode);

            if (procedure.ProcedureParameters != null && procedure.ProcedureParameters.Count > 0)
            {
                foreach (ProcedureParameter parameter in procedure.ProcedureParameters.OrderBy<ProcedureParameter, int>(c => c.ordinal_position ?? 0))
                {
                    TreeNode parameterNode = new TreeNode(parameter.ToString());
                    parameterNode.Tag = new NodeTag() { NodeType = DbType.ProcedureParameter, DbObject = parameter };
                    parameterNode.ImageIndex = (int)ImageType.Column;
                    parameterNode.SelectedImageIndex = (int)ImageType.Column;
                    parametersNode.Nodes.Add(parameterNode);
                }
            }

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = DbType.ProcedureColumns, DbObject = procedure };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;

            if (procedure.ProcedureColumns != null && procedure.ProcedureColumns.Count > 0)
            {
                procedureNode.Nodes.Add(columnsNode);

                foreach (ProcedureColumn column in procedure.ProcedureColumns.OrderBy<ProcedureColumn, int>(c => c.ColumnOrdinal ?? 0))
                {
                    TreeNode columnNode = new TreeNode(column.ToString());
                    columnNode.Tag = new NodeTag() { NodeType = DbType.ProcedureColumn, DbObject = column };
                    columnNode.ImageIndex = (int)ImageType.Column;
                    columnNode.SelectedImageIndex = (int)ImageType.Column;
                    columnsNode.Nodes.Add(columnNode);
                }
            }
            else if (procedure.Error != null)
            {
                procedureNode.ForeColor = Color.Red;
            }

            return procedureNode;
        }

        private TreeNode BuildFunctionsNode(Database database)
        {
            TreeNode functionsNode = new TreeNode("Table-valued Functions");
            functionsNode.Tag = new NodeTag() { NodeType = DbType.Functions, DbObject = database };
            functionsNode.ImageIndex = (int)ImageType.Folder;
            functionsNode.SelectedImageIndex = (int)ImageType.Folder;
            return functionsNode;
        }

        private TreeNode BuildFunctionNode(Function function)
        {
            TreeNode functionNode = new TreeNode(function.ToString());
            functionNode.Tag = new NodeTag() { NodeType = DbType.Function, DbObject = function };
            functionNode.ImageIndex = (int)ImageType.Function;
            functionNode.SelectedImageIndex = (int)ImageType.Function;

            TreeNode parametersNode = new TreeNode("Parameters");
            parametersNode.Tag = new NodeTag() { NodeType = DbType.ProcedureParameters, DbObject = function };
            parametersNode.ImageIndex = (int)ImageType.Folder;
            parametersNode.SelectedImageIndex = (int)ImageType.Folder;
            functionNode.Nodes.Add(parametersNode);

            if (function.ProcedureParameters != null && function.ProcedureParameters.Count > 0)
            {
                foreach (ProcedureParameter parameter in function.ProcedureParameters.OrderBy<ProcedureParameter, int>(c => c.ordinal_position ?? 0))
                {
                    TreeNode parameterNode = new TreeNode(parameter.ToString());
                    parameterNode.Tag = new NodeTag() { NodeType = DbType.ProcedureParameter, DbObject = parameter };
                    parameterNode.ImageIndex = (int)ImageType.Column;
                    parameterNode.SelectedImageIndex = (int)ImageType.Column;
                    parametersNode.Nodes.Add(parameterNode);
                }
            }

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = DbType.ProcedureColumns, DbObject = function };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;

            if (function.ProcedureColumns != null && function.ProcedureColumns.Count > 0)
            {
                functionNode.Nodes.Add(columnsNode);

                foreach (ProcedureColumn column in function.ProcedureColumns.OrderBy<ProcedureColumn, int>(c => c.ColumnOrdinal ?? 0))
                {
                    TreeNode columnNode = new TreeNode(column.ToString());
                    columnNode.Tag = new NodeTag() { NodeType = DbType.ProcedureColumn, DbObject = column };
                    columnNode.ImageIndex = (int)ImageType.Column;
                    columnNode.SelectedImageIndex = (int)ImageType.Column;
                    columnsNode.Nodes.Add(columnNode);
                }
            }
            else if (function.Error != null)
            {
                functionNode.ForeColor = Color.Red;
            }

            return functionNode;
        }

        private TreeNode BuildTVPsNode(Database database)
        {
            TreeNode tvpsNode = new TreeNode("User-Defined Table Types");
            tvpsNode.Tag = new NodeTag() { NodeType = DbType.TVPs, DbObject = database };
            tvpsNode.ImageIndex = (int)ImageType.Folder;
            tvpsNode.SelectedImageIndex = (int)ImageType.Folder;
            return tvpsNode;
        }

        private TreeNode BuildTVPNode(TVP tvp)
        {
            TreeNode tvpNode = new TreeNode(tvp.ToString());
            tvpNode.Tag = new NodeTag() { NodeType = DbType.TVP, DbObject = tvp };
            tvpNode.ImageIndex = (int)ImageType.Table;
            tvpNode.SelectedImageIndex = (int)ImageType.Table;

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = DbType.TVPColumns, DbObject = tvp };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;
            tvpNode.Nodes.Add(columnsNode);

            if (tvp.TVPColumns != null)
            {
                foreach (TVPColumn column in tvp.TVPColumns.OrderBy<TVPColumn, int>(c => c.column_id))
                {
                    TreeNode columnNode = new TreeNode(column.ToString());
                    columnNode.Tag = new NodeTag() { NodeType = DbType.TVPColumn, DbObject = column };
                    columnNode.ImageIndex = (int)ImageType.Column;
                    columnNode.SelectedImageIndex = (int)ImageType.Column;
                    columnsNode.Nodes.Add(columnNode);
                }
            }
            else if (tvp.Error != null)
            {
                tvpNode.ForeColor = Color.Red;
            }

            return tvpNode;
        }

        #endregion

        #region Server Tree CheckBoxes

        private void trvServer_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            DbType nodeType = ((NodeTag)e.Node.Tag).NodeType;

            bool isDrawCheckBox =
                nodeType == DbType.Database ||
                nodeType == DbType.Tables ||
                nodeType == DbType.Views ||
                nodeType == DbType.Procedures ||
                nodeType == DbType.Functions ||
                nodeType == DbType.TVPs ||
                nodeType == DbType.Table ||
                nodeType == DbType.View ||
                nodeType == DbType.Procedure ||
                nodeType == DbType.Function ||
                nodeType == DbType.TVP;

            if (isDrawCheckBox == false)
                trvServer.HideCheckBox(e.Node);
            e.DrawDefault = true;
        }

        private void trvServer_AfterCheck(object sender, TreeViewEventArgs e)
        {
            trvServer.AfterCheck -= trvServer_AfterCheck;

            SetChildrenCheckBoxes(e.Node);

            TreeNode root = e.Node;
            while (root != null)
            {
                root.Checked = IsAllChildrenChecked(root);
                root = root.Parent;
            }

            WritePocoToEditor(false);

            trvServer.AfterCheck += trvServer_AfterCheck;
        }

        private void SetChildrenCheckBoxes(TreeNode root)
        {
            if (root != null)
            {
                bool isChecked = root.Checked;
                foreach (TreeNode node in root.Nodes)
                {
                    node.Checked = isChecked;
                    SetChildrenCheckBoxes(node);
                }
            }
        }

        private bool IsAllChildrenChecked(TreeNode root)
        {
            if (root != null)
            {
                foreach (TreeNode node in root.Nodes)
                {
                    if (node.Checked == false)
                        return false;
                    if (IsAllChildrenChecked(node) == false)
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region Server Tree Context Menu

        private static FilterSettingsForm filterSettingsForm = new FilterSettingsForm();
        private static Dictionary<TreeNode, FilterSettings> filters = new Dictionary<TreeNode, FilterSettings>();
        private const string filteredPostfix = " (filtered)";

        private void trvServer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = new Point(e.X, e.Y);

                TreeNode node = trvServer.GetNodeAt(point);
                if (node != null)
                {
                    trvServer.SelectedNode = node;

                    DbType nodeType = ((NodeTag)node.Tag).NodeType;

                    bool isShowContextMenu = false;
                    removeFilterToolStripMenuItem.Visible = false;
                    filterSettingsToolStripMenuItem.Visible = false;
                    clearCheckBoxesToolStripMenuItem.Visible = false;
                    checkTablesConnectedFromToolStripMenuItem.Visible = false;
                    checkTablesConnectedToToolStripMenuItem.Visible = false;
                    checkTablesConnectedToolStripMenuItem.Visible = false;
                    checkRecursivelyTablesConnectedFromToolStripMenuItem.Visible = false;
                    checkRecursivelyTablesConnectedToToolStripMenuItem.Visible = false;
                    checkRecursivelyTablesConnectedToolStripMenuItem.Visible = false;
                    refreshToolStripMenuItem.Visible = false;
                    refreshTableToolStripMenuItem.Visible = false;

                    if (nodeType == DbType.Database)
                    {
                        isShowContextMenu = true;
                        clearCheckBoxesToolStripMenuItem.Visible = true;
                    }
                    else if (
                        nodeType == DbType.Tables ||
                        nodeType == DbType.Views ||
                        nodeType == DbType.Procedures ||
                        nodeType == DbType.Functions ||
                        nodeType == DbType.TVPs)
                    {
                        isShowContextMenu = true;
                        removeFilterToolStripMenuItem.Visible = true;
                        removeFilterToolStripMenuItem.Enabled = filters.ContainsKey(node);
                        filterSettingsToolStripMenuItem.Visible = true;
                        clearCheckBoxesToolStripMenuItem.Visible = true;
                    }
                    else if (
                        nodeType == DbType.Table ||
                        nodeType == DbType.View ||
                        nodeType == DbType.Procedure ||
                        nodeType == DbType.Function ||
                        nodeType == DbType.TVP)
                    {
                        isShowContextMenu = true;
                        if (nodeType == DbType.Table)
                        {
                            checkTablesConnectedFromToolStripMenuItem.Visible = true;
                            checkTablesConnectedToToolStripMenuItem.Visible = true;
                            checkTablesConnectedToolStripMenuItem.Visible = true;
                            checkRecursivelyTablesConnectedFromToolStripMenuItem.Visible = true;
                            checkRecursivelyTablesConnectedToToolStripMenuItem.Visible = true;
                            checkRecursivelyTablesConnectedToolStripMenuItem.Visible = true;
                            refreshTableToolStripMenuItem.Visible = true;
                        }
                        else
                        {
                            refreshToolStripMenuItem.Visible = true;
                        }
                    }

                    if (isShowContextMenu)
                    {
                        if (nodeType == DbType.Table)
                            contextMenuTable.Show(trvServer, point);
                        else
                            contextMenu.Show(trvServer, point);
                    }
                    else
                    {
                        contextMenu.Hide();
                        contextMenuTable.Hide();
                    }
                }
                else
                {
                    contextMenu.Hide();
                    contextMenuTable.Hide();
                }
            }
        }

        private void removeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            if (filters.ContainsKey(node))
            {
                FilterSettings filterSettings = filters[node];
                foreach (TreeNode child in filterSettings.Nodes)
                    node.Nodes.AddSorted(child);
                filters.Remove(node);
                node.Text = node.Text.Replace(filteredPostfix, string.Empty);
            }
        }

        private void filterSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            bool isContains = filters.ContainsKey(node);
            if (isContains)
                filterSettingsForm.SetFilter(filters[node]);
            else
                filterSettingsForm.ClearFilter();

            DialogResult dialogResult = filterSettingsForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                FilterSettings filterSettings = filterSettingsForm.GetFilter();
                if (isContains)
                {
                    filterSettings.Nodes = filters[node].Nodes;
                    filters.Remove(node);
                }
                filters.Add(node, filterSettings);
                if (isContains == false)
                    node.Text += filteredPostfix;

                DbType nodeType = ((NodeTag)node.Tag).NodeType;

                List<TreeNode> outList = new List<TreeNode>();
                List<TreeNode> inList = new List<TreeNode>();

                if (nodeType == DbType.Tables || nodeType == DbType.Views)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        Table table = (Table)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, table.table_name, table.table_schema);
                        if (isMatchFilter == false)
                            outList.Add(child);
                    }

                    foreach (TreeNode child in filterSettings.Nodes)
                    {
                        Table table = (Table)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, table.table_name, table.table_schema);
                        if (isMatchFilter)
                            inList.Add(child);
                    }
                }
                else if (nodeType == DbType.Procedures || nodeType == DbType.Functions)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        Procedure procedure = (Procedure)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, procedure.routine_name, procedure.routine_schema);
                        if (isMatchFilter == false)
                            outList.Add(child);
                    }

                    foreach (TreeNode child in filterSettings.Nodes)
                    {
                        Procedure procedure = (Procedure)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, procedure.routine_name, procedure.routine_schema);
                        if (isMatchFilter)
                            inList.Add(child);
                    }
                }
                else if (nodeType == DbType.TVPs)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        TVP tvp = (TVP)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, tvp.tvp_name, tvp.tvp_schema);
                        if (isMatchFilter == false)
                            outList.Add(child);
                    }

                    foreach (TreeNode child in filterSettings.Nodes)
                    {
                        TVP tvp = (TVP)((NodeTag)child.Tag).DbObject;
                        bool isMatchFilter = IsMatchFilter(filterSettings, tvp.tvp_name, tvp.tvp_schema);
                        if (isMatchFilter)
                            inList.Add(child);
                    }
                }

                foreach (TreeNode child in outList)
                {
                    node.Nodes.Remove(child);
                    filterSettings.Nodes.Add(child);
                }

                foreach (TreeNode child in inList)
                {
                    filterSettings.Nodes.Remove(child);
                    node.Nodes.AddSorted(child);
                }
            }
        }

        private bool IsMatchFilter(FilterSettings filterSettings, string name, string schema)
        {
            bool isMatchFilter = true;
            if (string.IsNullOrEmpty(filterSettings.FilterName.Filter) == false)
            {
                if (filterSettings.FilterName.FilterType == FilterType.Equals)
                    isMatchFilter = (string.Compare(name, filterSettings.FilterName.Filter, true) == 0);
                else if (filterSettings.FilterName.FilterType == FilterType.Contains)
                    isMatchFilter = (name.IndexOf(filterSettings.FilterName.Filter, StringComparison.CurrentCultureIgnoreCase) != -1);
                else if (filterSettings.FilterName.FilterType == FilterType.Does_Not_Contain)
                    isMatchFilter = (name.IndexOf(filterSettings.FilterName.Filter, StringComparison.CurrentCultureIgnoreCase) == -1);
            }

            if (isMatchFilter == false)
                return false;

            isMatchFilter = true;
            if (string.IsNullOrEmpty(filterSettings.FilterSchema.Filter) == false)
            {
                if (filterSettings.FilterSchema.FilterType == FilterType.Equals)
                    isMatchFilter = (string.Compare(schema, filterSettings.FilterSchema.Filter, true) == 0);
                else if (filterSettings.FilterSchema.FilterType == FilterType.Contains)
                    isMatchFilter = (schema.IndexOf(filterSettings.FilterSchema.Filter, StringComparison.CurrentCultureIgnoreCase) != -1);
                else if (filterSettings.FilterSchema.FilterType == FilterType.Does_Not_Contain)
                    isMatchFilter = (schema.IndexOf(filterSettings.FilterSchema.Filter, StringComparison.CurrentCultureIgnoreCase) == -1);
            }

            return isMatchFilter;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            DbType nodeType = ((NodeTag)node.Tag).NodeType;

            if (nodeType == DbType.Table)
            {
                Table table = (Table)((NodeTag)node.Tag).DbObject;
                DbHelper.GetTableSchema(table,
                    (IDbObject dbObject) =>
                    {
                        table.Error = null;
                    },
                    (IDbObject dbObject) =>
                    {
                        AddTableNode(node.Parent, table);
                        node.Remove();
                        WritePocoToEditor();
                    }
                );
            }
            else if (nodeType == DbType.View)
            {
                Db.DbObject.View view = (Db.DbObject.View)((NodeTag)node.Tag).DbObject;
                DbHelper.GetViewSchema(view,
                    (IDbObject dbObject) =>
                    {
                        view.Error = null;
                    },
                    (IDbObject dbObject) =>
                    {
                        AddViewNode(node.Parent, view);
                        node.Remove();
                        WritePocoToEditor();
                    }
                );
            }
            else if (nodeType == DbType.Procedure)
            {
                Procedure procedure = (Procedure)((NodeTag)node.Tag).DbObject;
                DbHelper.GetProcedureSchema(procedure,
                    (IDbObject dbObject) =>
                    {
                        procedure.Error = null;
                    },
                    (IDbObject dbObject) =>
                    {
                        AddProcedureNode(node.Parent, procedure);
                        node.Remove();
                        WritePocoToEditor();
                    }
                );
            }
            else if (nodeType == DbType.Function)
            {
                Function function = (Function)((NodeTag)node.Tag).DbObject;
                DbHelper.GetFunctionSchema(function,
                    (IDbObject dbObject) =>
                    {
                        function.Error = null;
                    },
                    (IDbObject dbObject) =>
                    {
                        AddFunctionNode(node.Parent, function);
                        node.Remove();
                        WritePocoToEditor();
                    }
                );
            }
            else if (nodeType == DbType.TVP)
            {
                TVP tvp = (TVP)((NodeTag)node.Tag).DbObject;
                DbHelper.GetTVPSchema(tvp,
                    (IDbObject dbObject) =>
                    {
                        tvp.Error = null;
                    },
                    (IDbObject dbObject) =>
                    {
                        AddTVPNode(node.Parent, tvp);
                        node.Remove();
                        WritePocoToEditor();
                    }
                );
            }
        }

        private void clearCheckBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            DbType nodeType = ((NodeTag)node.Tag).NodeType;

            if (nodeType == DbType.Database)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;

                node.Checked = false;
                foreach (TreeNode child in node.Nodes)
                {
                    child.Checked = false;
                    foreach (TreeNode child1 in child.Nodes)
                        child1.Checked = false;
                }

                WritePocoToEditor(false);

                trvServer.AfterCheck += trvServer_AfterCheck;
            }
            else if (nodeType == DbType.Tables ||
                nodeType == DbType.Views ||
                nodeType == DbType.Procedures ||
                nodeType == DbType.Functions ||
                nodeType == DbType.TVPs)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;

                node.Checked = false;
                foreach (TreeNode child in node.Nodes)
                    child.Checked = false;

                WritePocoToEditor(false);

                trvServer.AfterCheck += trvServer_AfterCheck;
            }
        }

        private void checkTablesConnectedFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(true, false, false);
        }

        private void checkTablesConnectedToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(false, true, false);
        }

        private void checkTablesConnectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(true, true, false);
        }

        private void checkRecursivelyTablesConnectedFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(true, false, true);
        }

        private void checkRecursivelyTablesConnectedToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(false, true, true);
        }

        private void checkRecursivelyTablesConnectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkTablesConnected(true, true, true);
        }

        private void checkTablesConnected(bool refFrom, bool refTo, bool recursively)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            DbType nodeType = ((NodeTag)node.Tag).NodeType;

            if (nodeType == DbType.Table)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;

                Table table = (Table)((NodeTag)node.Tag).DbObject;
                List<Table> tables = DbHelper.GetConnectedTables(table, refFrom, refTo, recursively);

                TreeNode serverNode = trvServer.Nodes[0];
                foreach (TreeNode databaseNode in serverNode.Nodes)
                {
                    if (((NodeTag)databaseNode.Tag).NodeType == DbType.Database && ((NodeTag)databaseNode.Tag).DbObject == table.Database)
                    {
                        foreach (TreeNode tablesNode in databaseNode.Nodes)
                        {
                            if (((NodeTag)tablesNode.Tag).NodeType == DbType.Tables)
                            {
                                foreach (TreeNode tableNode in tablesNode.Nodes)
                                {
                                    if (tables.Contains((Table)((NodeTag)tableNode.Tag).DbObject))
                                        tableNode.Checked = true;
                                }

                                break;
                            }
                        }

                        break;
                    }
                }

                WritePocoToEditor(false);

                trvServer.AfterCheck += trvServer_AfterCheck;
            }
        }

        #endregion

        #region POCO Options

        private void SetCheckBox(CheckBox chk, EventHandler checkedChangedHandler, bool isChecked)
        {
            chk.CheckedChanged -= checkedChangedHandler;
            chk.Checked = isChecked;
            chk.CheckedChanged += checkedChangedHandler;
        }

        private void SetRadioButton(RadioButton rdb, EventHandler checkedChangedHandler, bool isChecked)
        {
            rdb.CheckedChanged -= checkedChangedHandler;
            rdb.Checked = isChecked;
            rdb.CheckedChanged += checkedChangedHandler;
        }

        private void SetTextBox(TextBox txt, EventHandler textChangedHandler, string text)
        {
            txt.TextChanged -= textChangedHandler;
            txt.Text = text;
            txt.TextChanged += textChangedHandler;
        }

        #region POCO

        public bool IsProperties { get; set; }
        public bool IsVirtualProperties { get; set; }
        public bool IsOverrideProperties { get; set; }
        public bool IsPartialClass { get; set; }
        public bool IsAllStructNullable { get; set; }
        public bool IsComments { get; set; }
        public bool IsCommentsWithoutNull { get; set; }
        public bool IsUsing { get; set; }
        public string Namespace { get; set; }
        public string Inherit { get; set; }
        public bool IsColumnDefaults { get; set; }
        public bool IsNewLineBetweenMembers { get; set; }
        public bool IsNavigationProperties { get; set; }
        public bool IsNavigationPropertiesVirtual { get; set; }
        public bool IsNavigationPropertiesOverride { get; set; }
        public bool IsNavigationPropertiesShowJoinTable { get; set; }
        public bool IsNavigationPropertiesComments { get; set; }
        public bool IsNavigationPropertiesList { get; set; }
        public bool IsNavigationPropertiesICollection { get; set; }
        public bool IsNavigationPropertiesIEnumerable { get; set; }

        private void rdbProperties_CheckedChanged(object sender, EventArgs e)
        {
            IsProperties = rdbProperties.Checked;
            if (rdbProperties.Checked)
                WritePocoToEditor();
        }

        private void rdbDataMembers_CheckedChanged(object sender, EventArgs e)
        {
            IsProperties = !rdbDataMembers.Checked;
            if (rdbDataMembers.Checked)
                WritePocoToEditor();
        }

        private void chkVirtualProperties_CheckedChanged(object sender, EventArgs e)
        {
            IsVirtualProperties = chkVirtualProperties.Checked;

            if (IsVirtualProperties && IsOverrideProperties)
            {
                IsOverrideProperties = false;
                SetCheckBox(chkOverrideProperties, chkOverrideProperties_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkOverrideProperties_CheckedChanged(object sender, EventArgs e)
        {
            IsOverrideProperties = chkOverrideProperties.Checked;

            if (IsVirtualProperties && IsOverrideProperties)
            {
                IsVirtualProperties = false;
                SetCheckBox(chkVirtualProperties, chkVirtualProperties_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkPartialClass_CheckedChanged(object sender, EventArgs e)
        {
            IsPartialClass = chkPartialClass.Checked;
            WritePocoToEditor();
        }

        private void chkAllStructNullable_CheckedChanged(object sender, EventArgs e)
        {
            IsAllStructNullable = chkAllStructNullable.Checked;
            WritePocoToEditor();
        }

        private void chkComments_CheckedChanged(object sender, EventArgs e)
        {
            IsComments = chkComments.Checked;

            if (IsComments == false && IsCommentsWithoutNull)
            {
                IsCommentsWithoutNull = false;
                SetCheckBox(chkCommentsWithoutNull, chkCommentsWithoutNull_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkCommentsWithoutNull_CheckedChanged(object sender, EventArgs e)
        {
            IsCommentsWithoutNull = chkCommentsWithoutNull.Checked;

            if (IsCommentsWithoutNull && IsComments == false)
            {
                IsComments = true;
                SetCheckBox(chkComments, chkComments_CheckedChanged, true);
            }

            WritePocoToEditor();
        }

        private void chkUsing_CheckedChanged(object sender, EventArgs e)
        {
            IsUsing = chkUsing.Checked;
            WritePocoToEditor();
        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            Namespace = txtNamespace.Text;
            WritePocoToEditor();
        }

        private void txtInherit_TextChanged(object sender, EventArgs e)
        {
            Inherit = txtInherit.Text;
            WritePocoToEditor();
        }

        private void chkColumnDefaults_CheckedChanged(object sender, EventArgs e)
        {
            IsColumnDefaults = chkColumnDefaults.Checked;
            WritePocoToEditor();
        }

        private void chkNewLineBetweenMembers_CheckedChanged(object sender, EventArgs e)
        {
            IsNewLineBetweenMembers = chkNewLineBetweenMembers.Checked;
            WritePocoToEditor();
        }

        private void chkNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationProperties = chkNavigationProperties.Checked;

            if (IsNavigationProperties == false)
            {
                IsNavigationPropertiesVirtual = false;
                SetCheckBox(chkNavigationPropertiesVirtual, chkNavigationPropertiesVirtual_CheckedChanged, false);

                IsNavigationPropertiesOverride = false;
                SetCheckBox(chkNavigationPropertiesOverride, chkNavigationPropertiesOverride_CheckedChanged, false);

                IsNavigationPropertiesShowJoinTable = false;
                SetCheckBox(chkNavigationPropertiesShowJoinTable, chkNavigationPropertiesShowJoinTable_CheckedChanged, false);

                IsNavigationPropertiesComments = false;
                SetCheckBox(chkNavigationPropertiesComments, chkNavigationPropertiesComments_CheckedChanged, false);

                IsEFForeignKey = false;
                SetCheckBox(chkEFForeignKey, chkEFForeignKey_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkNavigationPropertiesVirtual_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesVirtual = chkNavigationPropertiesVirtual.Checked;

            if (IsNavigationPropertiesVirtual && IsNavigationProperties == false)
            {
                IsNavigationProperties = true;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);
            }

            if (IsNavigationPropertiesVirtual && IsNavigationPropertiesOverride)
            {
                IsNavigationPropertiesOverride = false;
                SetCheckBox(chkNavigationPropertiesOverride, chkNavigationPropertiesOverride_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkNavigationPropertiesOverride_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesOverride = chkNavigationPropertiesOverride.Checked;

            if (IsNavigationPropertiesOverride && IsNavigationProperties == false)
            {
                IsNavigationProperties = true;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);
            }

            if (IsNavigationPropertiesVirtual && IsNavigationPropertiesOverride)
            {
                IsNavigationPropertiesVirtual = false;
                SetCheckBox(chkNavigationPropertiesVirtual, chkNavigationPropertiesVirtual_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkNavigationPropertiesShowJoinTable_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesShowJoinTable = chkNavigationPropertiesShowJoinTable.Checked;

            if (IsNavigationPropertiesShowJoinTable && IsNavigationProperties == false)
            {
                IsNavigationProperties = true;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);
            }

            WritePocoToEditor();
        }

        private void chkNavigationPropertiesComments_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesComments = chkNavigationPropertiesComments.Checked;

            if (IsNavigationPropertiesComments && IsNavigationProperties == false)
            {
                IsNavigationProperties = true;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);
            }

            WritePocoToEditor();
        }

        private void rdbNavigationPropertiesList_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesList = rdbNavigationPropertiesList.Checked;
            IsNavigationPropertiesICollection = rdbNavigationPropertiesICollection.Checked;
            IsNavigationPropertiesIEnumerable = rdbNavigationPropertiesIEnumerable.Checked;

            if (IsNavigationProperties && rdbNavigationPropertiesList.Checked)
                WritePocoToEditor();
        }

        private void rdbNavigationPropertiesICollection_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesList = rdbNavigationPropertiesList.Checked;
            IsNavigationPropertiesICollection = rdbNavigationPropertiesICollection.Checked;
            IsNavigationPropertiesIEnumerable = rdbNavigationPropertiesIEnumerable.Checked;

            if (IsNavigationProperties && rdbNavigationPropertiesICollection.Checked)
                WritePocoToEditor();
        }

        private void rdbNavigationPropertiesIEnumerable_CheckedChanged(object sender, EventArgs e)
        {
            IsNavigationPropertiesList = rdbNavigationPropertiesList.Checked;
            IsNavigationPropertiesICollection = rdbNavigationPropertiesICollection.Checked;
            IsNavigationPropertiesIEnumerable = rdbNavigationPropertiesIEnumerable.Checked;

            if (IsNavigationProperties && rdbNavigationPropertiesIEnumerable.Checked)
                WritePocoToEditor();
        }

        #endregion

        #region Class Name

        public bool IsSingular { get; set; }
        public bool IsIncludeDB { get; set; }
        public string DBSeparator { get; set; }
        public bool IsIncludeSchema { get; set; }
        public bool IsIgnoreDboSchema { get; set; }
        public string SchemaSeparator { get; set; }
        public string WordsSeparator { get; set; }
        public bool IsCamelCase { get; set; }
        public bool IsUpperCase { get; set; }
        public bool IsLowerCase { get; set; }
        public string Search { get; set; }
        public string Replace { get; set; }
        public bool IsSearchIgnoreCase { get; set; }
        public string FixedClassName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        private void chkSingular_CheckedChanged(object sender, EventArgs e)
        {
            IsSingular = chkSingular.Checked;
            WritePocoToEditor();
        }

        private void chkIncludeDB_CheckedChanged(object sender, EventArgs e)
        {
            IsIncludeDB = chkIncludeDB.Checked;
            WritePocoToEditor();
        }

        private void txtDBSeparator_TextChanged(object sender, EventArgs e)
        {
            DBSeparator = txtDBSeparator.Text;
            WritePocoToEditor();
        }

        private void chkIncludeSchema_CheckedChanged(object sender, EventArgs e)
        {
            IsIncludeSchema = chkIncludeSchema.Checked;
            WritePocoToEditor();
        }

        private void chkIgnoreDboSchema_CheckedChanged(object sender, EventArgs e)
        {
            IsIgnoreDboSchema = chkIgnoreDboSchema.Checked;
            WritePocoToEditor();
        }

        private void txtSchemaSeparator_TextChanged(object sender, EventArgs e)
        {
            SchemaSeparator = txtSchemaSeparator.Text;
            WritePocoToEditor();
        }

        private void txtWordsSeparator_TextChanged(object sender, EventArgs e)
        {
            WordsSeparator = txtWordsSeparator.Text;
            WritePocoToEditor();
        }

        private void chkCamelCase_CheckedChanged(object sender, EventArgs e)
        {
            IsCamelCase = chkCamelCase.Checked;

            if (IsCamelCase)
            {
                IsUpperCase = false;
                SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, false);

                IsLowerCase = false;
                SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkUpperCase_CheckedChanged(object sender, EventArgs e)
        {
            IsUpperCase = chkUpperCase.Checked;

            if (IsUpperCase)
            {
                IsCamelCase = false;
                SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, false);

                IsLowerCase = false;
                SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void chkLowerCase_CheckedChanged(object sender, EventArgs e)
        {
            IsLowerCase = chkLowerCase.Checked;

            if (IsLowerCase)
            {
                IsCamelCase = false;
                SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, false);

                IsUpperCase = false;
                SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search = txtSearch.Text;
            WritePocoToEditor();
        }

        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            Replace = txtReplace.Text;
            WritePocoToEditor();
        }

        private void chkSearchIgnoreCase_CheckedChanged(object sender, EventArgs e)
        {
            IsSearchIgnoreCase = chkSearchIgnoreCase.Checked;
            WritePocoToEditor();
        }

        private void txtFixedClassName_TextChanged(object sender, EventArgs e)
        {
            FixedClassName = txtFixedClassName.Text;
            WritePocoToEditor();
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            Prefix = txtPrefix.Text;
            WritePocoToEditor();
        }

        private void txtSuffix_TextChanged(object sender, EventArgs e)
        {
            Suffix = txtSuffix.Text;
            WritePocoToEditor();
        }

        #endregion

        #region EF

        public bool IsEF { get; set; }
        public bool IsEFColumn { get; set; }
        public bool IsEFRequired { get; set; }
        public bool IsEFRequiredWithErrorMessage { get; set; }
        public bool IsEFConcurrencyCheck { get; set; }
        public bool IsEFStringLength { get; set; }
        public bool IsEFDisplay { get; set; }
        public bool IsEFDescription { get; set; }
        public bool IsEFComplexType { get; set; }
        public bool IsEFIndex { get; set; }
        public bool IsEFForeignKey { get; set; }

        private void chkEF_CheckedChanged(object sender, EventArgs e)
        {
            IsEF = chkEF.Checked;

            if (IsEF == false)
            {
                IsEFColumn = false;
                SetCheckBox(chkEFColumn, chkEFColumn_CheckedChanged, false);

                IsEFRequired = false;
                SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, false);

                IsEFRequiredWithErrorMessage = false;
                SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, false);

                IsEFConcurrencyCheck = false;
                SetCheckBox(chkEFConcurrencyCheck, chkEFConcurrencyCheck_CheckedChanged, false);

                IsEFStringLength = false;
                SetCheckBox(chkEFStringLength, chkEFStringLength_CheckedChanged, false);

                IsEFDisplay = false;
                SetCheckBox(chkEFDisplay, chkEFDisplay_CheckedChanged, false);

                IsEFDescription = false;
                SetCheckBox(chkEFDescription, chkEFDescription_CheckedChanged, false);

                IsEFComplexType = false;
                SetCheckBox(chkEFComplexType, chkEFComplexType_CheckedChanged, false);

                IsEFIndex = false;
                SetCheckBox(chkEFIndex, chkEFIndex_CheckedChanged, false);

                IsEFForeignKey = false;
                SetCheckBox(chkEFForeignKey, chkEFForeignKey_CheckedChanged, false);
            }

            WritePocoToEditor();
        }

        private void CheckEFCheckBox(bool otherEF)
        {
            if (otherEF && IsEF == false)
            {
                IsEF = true;
                SetCheckBox(chkEF, chkEF_CheckedChanged, true);
            }
        }

        private void chkEFColumn_CheckedChanged(object sender, EventArgs e)
        {
            IsEFColumn = chkEFColumn.Checked;
            CheckEFCheckBox(IsEFColumn);
            WritePocoToEditor();
        }

        private void chkEFRequired_CheckedChanged(object sender, EventArgs e)
        {
            IsEFRequired = chkEFRequired.Checked;

            if (IsEFRequired && IsEFRequiredWithErrorMessage)
            {
                IsEFRequiredWithErrorMessage = false;
                SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, false);
            }

            CheckEFCheckBox(IsEFRequired);

            WritePocoToEditor();
        }

        private void chkEFRequiredWithErrorMessage_CheckedChanged(object sender, EventArgs e)
        {
            IsEFRequiredWithErrorMessage = chkEFRequiredWithErrorMessage.Checked;

            if (IsEFRequired && IsEFRequiredWithErrorMessage)
            {
                IsEFRequired = false;
                SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, false);
            }

            CheckEFCheckBox(IsEFRequiredWithErrorMessage);

            WritePocoToEditor();
        }

        private void chkEFConcurrencyCheck_CheckedChanged(object sender, EventArgs e)
        {
            IsEFConcurrencyCheck = chkEFConcurrencyCheck.Checked;
            CheckEFCheckBox(IsEFConcurrencyCheck);
            WritePocoToEditor();
        }

        private void chkEFStringLength_CheckedChanged(object sender, EventArgs e)
        {
            IsEFStringLength = chkEFStringLength.Checked;
            CheckEFCheckBox(IsEFStringLength);
            WritePocoToEditor();
        }

        private void chkEFDisplay_CheckedChanged(object sender, EventArgs e)
        {
            IsEFDisplay = chkEFDisplay.Checked;
            CheckEFCheckBox(IsEFDisplay);
            WritePocoToEditor();
        }

        private void chkEFDescription_CheckedChanged(object sender, EventArgs e)
        {
            IsEFDescription = chkEFDescription.Checked;
            CheckEFCheckBox(IsEFDescription);
            WritePocoToEditor();
        }

        private void chkEFComplexType_CheckedChanged(object sender, EventArgs e)
        {
            IsEFComplexType = chkEFComplexType.Checked;
            CheckEFCheckBox(IsEFComplexType);
            WritePocoToEditor();
        }

        private void chkEFIndex_CheckedChanged(object sender, EventArgs e)
        {
            IsEFIndex = chkEFIndex.Checked;
            CheckEFCheckBox(IsEFIndex);
            WritePocoToEditor();
        }

        private void chkEFForeignKey_CheckedChanged(object sender, EventArgs e)
        {
            IsEFForeignKey = chkEFForeignKey.Checked;

            CheckEFCheckBox(IsEFForeignKey);

            if (IsEFForeignKey && IsNavigationProperties == false)
            {
                IsNavigationProperties = true;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);
            }

            WritePocoToEditor();
        }

        #endregion

        #endregion

        #region POCO Writer & Iterator

        private void IterateDbObjects(IDbObjectTraverse dbObject, StringBuilder sb = null)
        {
            IPOCOIterator iterator = GetPOCOIterator(new IDbObjectTraverse[] { dbObject }, sb);
            iterator.Iterate();
        }

        private void IterateDbObjects(IEnumerable<IDbObjectTraverse> dbObjects, StringBuilder sb = null)
        {
            IPOCOIterator iterator = GetPOCOIterator(dbObjects, sb);
            iterator.Iterate();
        }

        private void ClearDbObjects(StringBuilder sb = null)
        {
            IPOCOIterator iterator = GetPOCOIterator(null, sb);
            iterator.Clear();
        }

        private void WriteErrors(string objectName, IEnumerable<Exception> errors, StringBuilder sb = null)
        {
            if (errors != null && errors.Any())
            {
                IPOCOWriter pocoWriter = GetPOCOWriter(sb);

                pocoWriter.WriteLineError("/*");
                pocoWriter.WriteLineError(objectName);

                Exception lastError = errors.Last();
                foreach (Exception error in errors)
                {
                    Exception currentError = error;
                    while (currentError != null)
                    {
                        pocoWriter.WriteLineError(currentError.Message);
                        currentError = currentError.InnerException;
                    }

                    if (error != lastError)
                        pocoWriter.WriteLine();
                }

                pocoWriter.WriteLineError("*/");
            }
        }

        private IPOCOIterator GetPOCOIterator(IEnumerable<IDbObjectTraverse> dbObjects, StringBuilder sb)
        {
            IPOCOWriter pocoWriter = GetPOCOWriter(sb);
            IPOCOIterator iterator = GetPOCOIterator(dbObjects, pocoWriter);

            iterator.IsProperties = IsProperties;
            iterator.IsVirtualProperties = IsVirtualProperties;
            iterator.IsOverrideProperties = IsOverrideProperties;
            iterator.IsPartialClass = IsPartialClass;
            iterator.IsAllStructNullable = IsAllStructNullable;
            iterator.IsComments = IsComments;
            iterator.IsCommentsWithoutNull = IsCommentsWithoutNull;
            iterator.IsUsing = IsUsing;
            iterator.Namespace = Namespace;
            iterator.Inherit = Inherit;
            iterator.IsColumnDefaults = IsColumnDefaults;
            iterator.IsNewLineBetweenMembers = IsNewLineBetweenMembers;
            iterator.IsNavigationProperties = IsNavigationProperties;
            iterator.IsNavigationPropertiesVirtual = IsNavigationPropertiesVirtual;
            iterator.IsNavigationPropertiesOverride = IsNavigationPropertiesOverride;
            iterator.IsNavigationPropertiesShowJoinTable = IsNavigationPropertiesShowJoinTable;
            iterator.IsNavigationPropertiesComments = IsNavigationPropertiesComments;
            iterator.IsNavigationPropertiesList = IsNavigationPropertiesList;
            iterator.IsNavigationPropertiesICollection = IsNavigationPropertiesICollection;
            iterator.IsNavigationPropertiesIEnumerable = IsNavigationPropertiesIEnumerable;
            iterator.IsSingular = IsSingular;
            iterator.IsIncludeDB = IsIncludeDB;
            iterator.DBSeparator = DBSeparator;
            iterator.IsIncludeSchema = IsIncludeSchema;
            iterator.IsIgnoreDboSchema = IsIgnoreDboSchema;
            iterator.SchemaSeparator = SchemaSeparator;
            iterator.WordsSeparator = WordsSeparator;
            iterator.IsCamelCase = IsCamelCase;
            iterator.IsUpperCase = IsUpperCase;
            iterator.IsLowerCase = IsLowerCase;
            iterator.Search = Search;
            iterator.Replace = Replace;
            iterator.IsSearchIgnoreCase = IsSearchIgnoreCase;
            iterator.FixedClassName = FixedClassName;
            iterator.Prefix = Prefix;
            iterator.Suffix = Suffix;

            if (iterator is IEFIterator)
            {
                IEFIterator efIterator = iterator as IEFIterator;
                efIterator.IsEF = IsEF;
                efIterator.IsEFColumn = IsEFColumn;
                efIterator.IsEFRequired = IsEFRequired;
                efIterator.IsEFRequiredWithErrorMessage = IsEFRequiredWithErrorMessage;
                efIterator.IsEFConcurrencyCheck = IsEFConcurrencyCheck;
                efIterator.IsEFStringLength = IsEFStringLength;
                efIterator.IsEFDisplay = IsEFDisplay;
                efIterator.IsEFDescription = IsEFDescription;
                efIterator.IsEFComplexType = IsEFComplexType;
                efIterator.IsEFIndex = IsEFIndex;
                efIterator.IsEFForeignKey = IsEFForeignKey;
            }

            return iterator;
        }

        private IPOCOWriter GetPOCOWriter(StringBuilder sb)
        {
            if (sb == null)
                return new RichTextBoxWriterFactory(txtPocoEditor).CreatePOCOWriter();
            else
                return new StringBuilderWriterFactory(sb).CreatePOCOWriter();
        }

        private IPOCOIterator GetPOCOIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
        {
            if (IsEF)
                return new EFIteratorFactory().CreateIterator(dbObjects, pocoWriter);
            else
                return new DbIteratorFactory().CreateIterator(dbObjects, pocoWriter);
        }

        #endregion

        #region POCO Editor

        private List<IDbObjectTraverse> GetSelectedObjects()
        {
            List<IDbObjectTraverse> selectedObjects = new List<IDbObjectTraverse>();
            TreeNode serverNode = trvServer.Nodes[0];
            GetSelectedObjects(serverNode, selectedObjects);
            return selectedObjects;
        }

        private void GetSelectedObjects(TreeNode node, List<IDbObjectTraverse> selectedObjects)
        {
            DbType nodeType = ((NodeTag)node.Tag).NodeType;

            bool isDbObjectTraverse =
                nodeType == DbType.Table ||
                nodeType == DbType.View ||
                nodeType == DbType.Procedure ||
                nodeType == DbType.Function ||
                nodeType == DbType.TVP;

            if (isDbObjectTraverse && node.Checked)
                selectedObjects.Add(((NodeTag)node.Tag).DbObject as IDbObjectTraverse);

            if (isDbObjectTraverse == false)
            {
                foreach (TreeNode child in node.Nodes)
                    GetSelectedObjects(child, selectedObjects);
            }
        }

        private void trvServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            WritePocoToEditor(false);
        }

        private List<IDbObjectTraverse> selectedObjectsPrevious = new List<IDbObjectTraverse>();
        private TreeNode selectedNodePrevious;

        private void WritePocoToEditor(bool forceRefresh = true)
        {
            List<IDbObjectTraverse> selectedObjects = GetSelectedObjects();
            TreeNode selectedNode = trvServer.SelectedNode;

            if (selectedObjects.Count > 0)
            {
                if (forceRefresh || selectedObjects.Except(selectedObjectsPrevious).Any() || selectedObjectsPrevious.Except(selectedObjects).Any())
                    IterateDbObjects(selectedObjects);
            }
            else if (selectedNode != null && (forceRefresh || selectedNode != selectedNodePrevious || selectedObjectsPrevious.Count > 0))
            {
                IDbObjectTraverse dbObject = null;
                DbType nodeType = ((NodeTag)selectedNode.Tag).NodeType;
                if (nodeType == DbType.Table || nodeType == DbType.View || nodeType == DbType.Columns)
                    dbObject = (Table)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.Column)
                    dbObject = ((TableColumn)((NodeTag)selectedNode.Tag).DbObject).Table;
                else if (nodeType == DbType.Procedure || nodeType == DbType.Function || nodeType == DbType.ProcedureParameters || nodeType == DbType.ProcedureColumns)
                    dbObject = (Procedure)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.ProcedureParameter)
                    dbObject = ((ProcedureParameter)((NodeTag)selectedNode.Tag).DbObject).Procedure;
                else if (nodeType == DbType.ProcedureColumn)
                    dbObject = ((ProcedureColumn)((NodeTag)selectedNode.Tag).DbObject).Procedure;
                else if (nodeType == DbType.TVP || nodeType == DbType.TVPColumns)
                    dbObject = (TVP)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.TVPColumn)
                    dbObject = ((TVPColumn)((NodeTag)selectedNode.Tag).DbObject).TVP;

                IDbObjectTraverse dbObjectPrevious = null;
                if (selectedNodePrevious != null && forceRefresh == false && selectedObjectsPrevious.Count == 0)
                {
                    DbType nodeTypePrevious = ((NodeTag)selectedNodePrevious.Tag).NodeType;
                    if (nodeTypePrevious == DbType.Table || nodeTypePrevious == DbType.View || nodeTypePrevious == DbType.Columns)
                        dbObjectPrevious = (Table)((NodeTag)selectedNodePrevious.Tag).DbObject;
                    else if (nodeTypePrevious == DbType.Column)
                        dbObjectPrevious = ((TableColumn)((NodeTag)selectedNodePrevious.Tag).DbObject).Table;
                    else if (nodeTypePrevious == DbType.Procedure || nodeTypePrevious == DbType.Function || nodeTypePrevious == DbType.ProcedureParameters || nodeTypePrevious == DbType.ProcedureColumns)
                        dbObjectPrevious = (Procedure)((NodeTag)selectedNodePrevious.Tag).DbObject;
                    else if (nodeTypePrevious == DbType.ProcedureParameter)
                        dbObjectPrevious = ((ProcedureParameter)((NodeTag)selectedNodePrevious.Tag).DbObject).Procedure;
                    else if (nodeTypePrevious == DbType.ProcedureColumn)
                        dbObjectPrevious = ((ProcedureColumn)((NodeTag)selectedNodePrevious.Tag).DbObject).Procedure;
                    else if (nodeTypePrevious == DbType.TVP || nodeTypePrevious == DbType.TVPColumns)
                        dbObjectPrevious = (TVP)((NodeTag)selectedNodePrevious.Tag).DbObject;
                    else if (nodeTypePrevious == DbType.TVPColumn)
                        dbObjectPrevious = ((TVPColumn)((NodeTag)selectedNodePrevious.Tag).DbObject).TVP;
                }

                if (dbObject != null)
                {
                    if (dbObject != dbObjectPrevious || forceRefresh || selectedObjectsPrevious.Count > 0)
                        IterateDbObjects(dbObject);
                }
                else
                {
                    ClearDbObjects();

                    if (nodeType == DbType.Database)
                    {
                        Database database = (Database)((NodeTag)selectedNode.Tag).DbObject;
                        WriteErrors(database.ToString(), database.Errors);
                    }
                }
            }
            else if (forceRefresh || selectedNode == null)
            {
                ClearDbObjects();
            }

            selectedObjectsPrevious = selectedObjects;
            selectedNodePrevious = selectedNode;
        }

        #endregion

        #region Copy

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtPocoEditor.Text);
            }
            catch { }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtPocoEditor.SelectedText ?? string.Empty);
            }
            catch { }
            txtPocoEditor.Focus();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtPocoEditor.SelectAll();
            txtPocoEditor.Focus();
        }

        #endregion

        #region Export

        private void btnFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialogExport.SelectedPath = txtFolder.Text;
            DialogResult dr = folderBrowserDialogExport.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
                txtFolder.Text = folderBrowserDialogExport.SelectedPath;
        }

        private void chkSingleFile_CheckedChanged(object sender, EventArgs e)
        {
            txtFileName.Enabled = chkSingleFile.Checked;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            Application.DoEvents();

            if (string.IsNullOrEmpty(txtFolder.Text))
            {
                toolStripStatusLabel.Text = "Folder isn't set";
                toolStripStatusLabel.ForeColor = Color.Red;
                return;
            }

            if (Directory.Exists(txtFolder.Text) == false)
            {
                toolStripStatusLabel.Text = "Folder doens't exist";
                toolStripStatusLabel.ForeColor = Color.Red;
                return;
            }

            List<IDbObjectTraverse> exportObjects = GetExportObjects();
            if (exportObjects.Count == 0)
                return;

            toolStripStatusLabel.Text = "Exporting...";
            toolStripStatusLabel.ForeColor = Color.Black;
            Application.DoEvents();

            List<ExportResult> exportResults = WritePocoToFiles(exportObjects);

            if (exportResults.Count > 0)
            {
                var failed = exportResults.Where(r => r.Succeeded == false).ToList();

                if (failed.Count == 0)
                {
                    if (exportResults.Count > 1)
                        toolStripStatusLabel.Text = "All POCOs were exported successfully";
                    else
                        toolStripStatusLabel.Text = string.Format("Exported {0} successfully", exportResults[0].ObjectName);
                    toolStripStatusLabel.ForeColor = Color.Green;
                }
                else if (failed.Count == 1)
                {
                    var exportResult = failed[0];
                    string fileName = exportResult.FileName;
                    if (fileName == ".cs")
                        fileName = string.Empty;
                    if (string.IsNullOrEmpty(fileName))
                        toolStripStatusLabel.Text = string.Format("Failed to export {0}", exportResult.ObjectName);
                    else
                        toolStripStatusLabel.Text = string.Format("Failed to export {0} to {1}", exportResult.ObjectName, fileName);
                    toolStripStatusLabel.ForeColor = Color.Red;
                }
                else if (failed.Count < exportResults.Count)
                {
                    toolStripStatusLabel.Text = "Failed to export several POCOs";
                    toolStripStatusLabel.ForeColor = Color.Red;
                }
                else if (failed.Count == exportResults.Count)
                {
                    toolStripStatusLabel.Text = "Failed to export all POCOs";
                    toolStripStatusLabel.ForeColor = Color.Red;
                }
            }
        }

        private List<IDbObjectTraverse> GetExportObjects()
        {
            List<IDbObjectTraverse> exportResults = GetSelectedObjects();
            TreeNode selectedNode = trvServer.SelectedNode;

            if (exportResults.Count == 0 && selectedNode != null)
            {
                IDbObjectTraverse dbObject = null;
                DbType nodeType = ((NodeTag)selectedNode.Tag).NodeType;
                if (nodeType == DbType.Table || nodeType == DbType.View || nodeType == DbType.Columns)
                    dbObject = (Table)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.Column)
                    dbObject = ((TableColumn)((NodeTag)selectedNode.Tag).DbObject).Table;
                else if (nodeType == DbType.Procedure || nodeType == DbType.Function || nodeType == DbType.ProcedureParameters || nodeType == DbType.ProcedureColumns)
                    dbObject = (Procedure)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.ProcedureParameter)
                    dbObject = ((ProcedureParameter)((NodeTag)selectedNode.Tag).DbObject).Procedure;
                else if (nodeType == DbType.ProcedureColumn)
                    dbObject = ((ProcedureColumn)((NodeTag)selectedNode.Tag).DbObject).Procedure;
                else if (nodeType == DbType.TVP || nodeType == DbType.TVPColumns)
                    dbObject = (TVP)((NodeTag)selectedNode.Tag).DbObject;
                else if (nodeType == DbType.TVPColumn)
                    dbObject = ((TVPColumn)((NodeTag)selectedNode.Tag).DbObject).TVP;

                if (dbObject != null)
                    exportResults.Add(dbObject);
            }

            return exportResults;
        }

        private List<ExportResult> WritePocoToFiles(List<IDbObjectTraverse> exportObjects)
        {
            List<ExportResult> exportResults = new List<ExportResult>();

            if (exportObjects.Count > 0)
            {
                string folder = txtFolder.Text;
                bool isSingleFile = chkSingleFile.Checked;
                string fileName = txtFileName.Text;

                if (isSingleFile && string.IsNullOrEmpty(fileName) == false)
                {
                    StringBuilder sb = new StringBuilder();
                    IterateDbObjects(exportObjects, sb);

                    if (fileName.EndsWith(".cs") == false)
                        fileName += ".cs";
                    foreach (char c in Path.GetInvalidFileNameChars())
                        fileName = fileName.Replace(c.ToString(), string.Empty);

                    Exception error = null;
                    bool succeeded = WritePocoToFile(folder, fileName, sb.ToString(), true, ref error);

                    foreach (var dbObject in exportObjects)
                    {
                        exportResults.Add(new ExportResult()
                        {
                            ObjectName = string.Format("{0}.{1}.{2}", dbObject.Database.ToString(), dbObject.Schema, dbObject.Name),
                            ClassName = dbObject.ClassName,
                            FileName = fileName,
                            Succeeded = succeeded,
                            Error = error
                        });
                    }
                }
                else
                {
                    foreach (var dbObject in exportObjects)
                    {
                        StringBuilder sb = new StringBuilder();
                        IterateDbObjects(dbObject, sb);

                        fileName = dbObject.ClassName + ".cs";
                        foreach (char c in Path.GetInvalidFileNameChars())
                            fileName = fileName.Replace(c.ToString(), string.Empty);

                        Exception error = null;
                        bool succeeded = WritePocoToFile(folder, fileName, sb.ToString(), false, ref error);

                        exportResults.Add(new ExportResult()
                        {
                            ObjectName = string.Format("{0}.{1}.{2}", dbObject.Database.ToString(), dbObject.Schema, dbObject.Name),
                            ClassName = dbObject.ClassName,
                            FileName = fileName,
                            Succeeded = succeeded,
                            Error = error
                        });
                    }
                }
            }

            return exportResults;
        }

        private bool WritePocoToFile(string folder, string fileName, string content, bool isAppend, ref Exception error)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || fileName == ".cs")
                    throw new Exception("File name isn't set");

                string path = folder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + fileName;
                if (isAppend)
                    File.AppendAllText(path, content);
                else
                    File.WriteAllText(path, content);

                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }

        private class ExportResult
        {
            public string ObjectName { get; set; }
            public string ClassName { get; set; }
            public string FileName { get; set; }
            public bool Succeeded { get; set; }
            public Exception Error { get; set; }
        }

        #endregion

        #region Type Mapping

        private TypeMappingForm typeMappingForm;

        private void btnTypeMapping_Click(object sender, EventArgs e)
        {
            if (typeMappingForm == null)
                typeMappingForm = new TypeMappingForm();
            typeMappingForm.ShowDialog(this);
        }

        #endregion

        #region Command Line

        private CommandLineForm commandLineForm;

        private void btnCommandLine_Click(object sender, EventArgs e)
        {
            Options options = GetOptions();
            List<string> shortCommandParts = CommandLineParser<Options>.GetCommandLineParts(options, true);
            List<string> longCommandParts = CommandLineParser<Options>.GetCommandLineParts(options, false);
            if (commandLineForm == null)
                commandLineForm = new CommandLineForm();
            commandLineForm.SetCommandLineParts(shortCommandParts, longCommandParts);
            commandLineForm.ShowDialog(this);
        }

        private Options GetOptions(bool isIncludeObjects = true)
        {
            Options options = new Options();

            options.ConnectionString = DbHelper.ConnectionString;

            options.IsProperties = IsProperties;
            options.IsDataMembers = !IsProperties;
            options.IsVirtualProperties = IsVirtualProperties;
            options.IsOverrideProperties = IsOverrideProperties;
            options.IsPartialClass = IsPartialClass;
            options.IsAllStructNullable = IsAllStructNullable;
            options.IsComments = IsComments;
            options.IsCommentsWithoutNull = IsCommentsWithoutNull;
            options.IsUsing = IsUsing;
            options.Namespace = Namespace;
            options.Inherit = Inherit;
            options.IsColumnDefaults = IsColumnDefaults;
            options.IsNewLineBetweenMembers = IsNewLineBetweenMembers;

            if (options.IsComments == false)
                options.IsCommentsWithoutNull = false;

            options.IsNavigationProperties = IsNavigationProperties;
            options.IsNavigationPropertiesVirtual = IsNavigationPropertiesVirtual;
            options.IsNavigationPropertiesOverride = IsNavigationPropertiesOverride;
            options.IsNavigationPropertiesShowJoinTable = IsNavigationPropertiesShowJoinTable;
            options.IsNavigationPropertiesComments = IsNavigationPropertiesComments;
            options.IsNavigationPropertiesList = IsNavigationPropertiesList;
            options.IsNavigationPropertiesICollection = IsNavigationPropertiesICollection;
            options.IsNavigationPropertiesIEnumerable = IsNavigationPropertiesIEnumerable;

            if (options.IsNavigationProperties == false)
            {
                options.IsNavigationPropertiesVirtual = false;
                options.IsNavigationPropertiesOverride = false;
                options.IsNavigationPropertiesShowJoinTable = false;
                options.IsNavigationPropertiesComments = false;
                options.IsNavigationPropertiesList = false;
                options.IsNavigationPropertiesICollection = false;
                options.IsNavigationPropertiesIEnumerable = false;
                options.IsEFForeignKey = false;
            }

            options.IsSingular = IsSingular;
            options.IsIncludeDB = IsIncludeDB;
            options.DBSeparator = DBSeparator;
            options.IsIncludeSchema = IsIncludeSchema;
            options.IsIgnoreDboSchema = IsIgnoreDboSchema;
            options.SchemaSeparator = SchemaSeparator;
            options.WordsSeparator = WordsSeparator;
            options.IsCamelCase = IsCamelCase;
            options.IsUpperCase = IsUpperCase;
            options.IsLowerCase = IsLowerCase;
            options.Search = Search;
            options.Replace = Replace;
            options.IsSearchIgnoreCase = IsSearchIgnoreCase;
            options.FixedClassName = FixedClassName;
            options.Prefix = Prefix;
            options.Suffix = Suffix;

            options.IsEF = IsEF;
            options.IsEFColumn = IsEFColumn;
            options.IsEFRequired = IsEFRequired;
            options.IsEFRequiredWithErrorMessage = IsEFRequiredWithErrorMessage;
            options.IsEFConcurrencyCheck = IsEFConcurrencyCheck;
            options.IsEFStringLength = IsEFStringLength;
            options.IsEFDisplay = IsEFDisplay;
            options.IsEFDescription = IsEFDescription;
            options.IsEFComplexType = IsEFComplexType;
            options.IsEFIndex = IsEFIndex;
            options.IsEFForeignKey = IsEFForeignKey;

            if (options.IsEF == false)
            {
                options.IsEFColumn = false;
                options.IsEFRequired = false;
                options.IsEFRequiredWithErrorMessage = false;
                options.IsEFConcurrencyCheck = false;
                options.IsEFStringLength = false;
                options.IsEFDisplay = false;
                options.IsEFDescription = false;
                options.IsEFComplexType = false;
                options.IsEFIndex = false;
                options.IsEFForeignKey = false;
            }

            options.Folder = txtFolder.Text;
            options.IsSingleFile = chkSingleFile.Checked;
            options.FileName = txtFileName.Text;

            options.IsIncludeAll = false;

            options.IsIncludeAllTables = false;
            options.IsExcludeAllTables = false;
            options.IncludeTables = new List<string>();
            options.ExcludeTables = new List<string>();

            options.IsIncludeAllViews = false;
            options.IsExcludeAllViews = false;
            options.IncludeViews = new List<string>();
            options.ExcludeViews = new List<string>();

            options.IsIncludeAllStoredProcedures = false;
            options.IsExcludeAllStoredProcedures = false;
            options.IncludeStoredProcedures = new List<string>();
            options.ExcludeStoredProcedures = new List<string>();

            options.IsIncludeAllFunctions = false;
            options.IsExcludeAllFunctions = false;
            options.IncludeFunctions = new List<string>();
            options.ExcludeFunctions = new List<string>();

            options.IsIncludeAllTVPs = false;
            options.IsExcludeAllTVPs = false;
            options.IncludeTVPs = new List<string>();
            options.ExcludeTVPs = new List<string>();

            if (isIncludeObjects)
                SetIncludeObjects(options);

            return options;
        }

        private void SetIncludeObjects(Options options)
        {
            TreeNode serverNode = trvServer.Nodes[0];
            SetIncludeObjects(options, serverNode);

            options.IsIncludeAllTables = (options.IncludeTables.Count > 0 && options.ExcludeTables.Count == 0);
            options.IsExcludeAllTables = (options.IncludeTables.Count == 0 && options.ExcludeTables.Count > 0);

            options.IsIncludeAllViews = (options.IncludeViews.Count > 0 && options.ExcludeViews.Count == 0);
            options.IsExcludeAllViews = (options.IncludeViews.Count == 0 && options.ExcludeViews.Count > 0);

            options.IsIncludeAllStoredProcedures = (options.IncludeStoredProcedures.Count > 0 && options.ExcludeStoredProcedures.Count == 0);
            options.IsExcludeAllStoredProcedures = (options.IncludeStoredProcedures.Count == 0 && options.ExcludeStoredProcedures.Count > 0);

            options.IsIncludeAllFunctions = (options.IncludeFunctions.Count > 0 && options.ExcludeFunctions.Count == 0);
            options.IsExcludeAllFunctions = (options.IncludeFunctions.Count == 0 && options.ExcludeFunctions.Count > 0);

            options.IsIncludeAllTVPs = (options.IncludeTVPs.Count > 0 && options.ExcludeTVPs.Count == 0);
            options.IsExcludeAllTVPs = (options.IncludeTVPs.Count == 0 && options.ExcludeTVPs.Count > 0);

            options.IsIncludeAll =
                options.ExcludeTables.Count == 0 &&
                options.ExcludeViews.Count == 0 &&
                options.ExcludeStoredProcedures.Count == 0 &&
                options.ExcludeFunctions.Count == 0 &&
                options.ExcludeTVPs.Count == 0;

            bool isExcludeAll =
                options.IncludeTables.Count == 0 &&
                options.IncludeViews.Count == 0 &&
                options.IncludeStoredProcedures.Count == 0 &&
                options.IncludeFunctions.Count == 0 &&
                options.IncludeTVPs.Count == 0;

            if (options.IsIncludeAllTables)
                options.IncludeTables.Clear();
            if (options.IsExcludeAllTables)
                options.ExcludeTables.Clear();

            if (options.IsIncludeAllViews)
                options.IncludeViews.Clear();
            if (options.IsExcludeAllViews)
                options.ExcludeViews.Clear();

            if (options.IsIncludeAllStoredProcedures)
                options.IncludeStoredProcedures.Clear();
            if (options.IsExcludeAllStoredProcedures)
                options.ExcludeStoredProcedures.Clear();

            if (options.IsIncludeAllFunctions)
                options.IncludeFunctions.Clear();
            if (options.IsExcludeAllFunctions)
                options.ExcludeFunctions.Clear();

            if (options.IsIncludeAllTVPs)
                options.IncludeTVPs.Clear();
            if (options.IsExcludeAllTVPs)
                options.ExcludeTVPs.Clear();

            if (options.IsIncludeAll)
            {
                options.IsIncludeAllTables = false;
                options.IsIncludeAllViews = false;
                options.IsIncludeAllStoredProcedures = false;
                options.IsIncludeAllFunctions = false;
                options.IsIncludeAllTVPs = false;
            }
            else if (options.IsIncludeAll == false && isExcludeAll == false)
            {
                if (options.IsIncludeAllTables == false && options.IsExcludeAllTables == false && (options.IncludeTables.Count > 0 && options.ExcludeTables.Count > 0))
                {
                    if (1.0 * options.ExcludeTables.Count / (options.IncludeTables.Count + options.ExcludeTables.Count) < 0.5)
                    {
                        options.IsIncludeAllTables = true;
                        options.IncludeTables.Clear();
                    }
                    else
                    {
                        options.ExcludeTables.Clear();
                    }
                }

                if (options.IsIncludeAllViews == false && options.IsExcludeAllViews == false && (options.IncludeViews.Count > 0 && options.ExcludeViews.Count > 0))
                {
                    if (1.0 * options.ExcludeViews.Count / (options.IncludeViews.Count + options.ExcludeViews.Count) < 0.5)
                    {
                        options.IsIncludeAllViews = true;
                        options.IncludeViews.Clear();
                    }
                    else
                    {
                        options.ExcludeViews.Clear();
                    }
                }

                if (options.IsIncludeAllStoredProcedures == false && options.IsExcludeAllStoredProcedures == false && (options.IncludeStoredProcedures.Count > 0 && options.ExcludeStoredProcedures.Count > 0))
                {
                    if (1.0 * options.ExcludeStoredProcedures.Count / (options.IncludeStoredProcedures.Count + options.ExcludeStoredProcedures.Count) < 0.5)
                    {
                        options.IsIncludeAllStoredProcedures = true;
                        options.IncludeStoredProcedures.Clear();
                    }
                    else
                    {
                        options.ExcludeStoredProcedures.Clear();
                    }
                }

                if (options.IsIncludeAllFunctions == false && options.IsExcludeAllFunctions == false && (options.IncludeFunctions.Count > 0 && options.ExcludeFunctions.Count > 0))
                {
                    if (1.0 * options.ExcludeFunctions.Count / (options.IncludeFunctions.Count + options.ExcludeFunctions.Count) < 0.5)
                    {
                        options.IsIncludeAllFunctions = true;
                        options.IncludeFunctions.Clear();
                    }
                    else
                    {
                        options.ExcludeFunctions.Clear();
                    }
                }

                if (options.IsIncludeAllTVPs == false && options.IsExcludeAllTVPs == false && (options.IncludeTVPs.Count > 0 && options.ExcludeTVPs.Count > 0))
                {
                    if (1.0 * options.ExcludeTVPs.Count / (options.IncludeTVPs.Count + options.ExcludeTVPs.Count) < 0.5)
                    {
                        options.IsIncludeAllTVPs = true;
                        options.IncludeTVPs.Clear();
                    }
                    else
                    {
                        options.ExcludeTVPs.Clear();
                    }
                }
            }

            options.IsExcludeAllTables = false;
            options.IsExcludeAllViews = false;
            options.IsExcludeAllStoredProcedures = false;
            options.IsExcludeAllFunctions = false;
            options.IsExcludeAllTVPs = false;
        }

        private void SetIncludeObjects(Options options, TreeNode node)
        {
            DbType nodeType = ((NodeTag)node.Tag).NodeType;

            if (nodeType == DbType.Table || nodeType == DbType.View || nodeType == DbType.Procedure || nodeType == DbType.Function || nodeType == DbType.TVP)
            {
                IDbObjectTraverse dbObject = ((NodeTag)node.Tag).DbObject as IDbObjectTraverse;
                if (nodeType == DbType.Table)
                    (node.Checked ? options.IncludeTables : options.ExcludeTables).Add(dbObject.Schema == "dbo" ? dbObject.Name : dbObject.ToString());
                else if (nodeType == DbType.View)
                    (node.Checked ? options.IncludeViews : options.ExcludeViews).Add(dbObject.Schema == "dbo" ? dbObject.Name : dbObject.ToString());
                else if (nodeType == DbType.Procedure)
                    (node.Checked ? options.IncludeStoredProcedures : options.ExcludeStoredProcedures).Add(dbObject.Schema == "dbo" ? dbObject.Name : dbObject.ToString());
                else if (nodeType == DbType.Function)
                    (node.Checked ? options.IncludeFunctions : options.ExcludeFunctions).Add(dbObject.Schema == "dbo" ? dbObject.Name : dbObject.ToString());
                else if (nodeType == DbType.TVP)
                    (node.Checked ? options.IncludeTVPs : options.ExcludeTVPs).Add(dbObject.Schema == "dbo" ? dbObject.Name : dbObject.ToString());
            }

            bool isRecursion =
                nodeType == DbType.Server ||
                nodeType == DbType.Database ||
                nodeType == DbType.Tables ||
                nodeType == DbType.Views ||
                nodeType == DbType.Procedures ||
                nodeType == DbType.Functions ||
                nodeType == DbType.TVPs;

            if (isRecursion)
            {
                foreach (TreeNode child in node.Nodes)
                    SetIncludeObjects(options, child);
            }
        }

        #endregion

        #region Load & Save Options

        private readonly string settingsFileName = "POCOGenerator.settings";

        private void LoadOptions()
        {
            try
            {
                if (File.Exists(settingsFileName) == false)
                    return;

                Options options = SerializationHelper.BinaryDeserializeFromFile<Options>(settingsFileName);

                if (string.IsNullOrEmpty(options.ConnectionString) == false)
                    DbHelper.ConnectionString = options.ConnectionString;

                IsProperties = true;
                if (options.IsProperties == false && options.IsDataMembers)
                    IsProperties = false;

                SetRadioButton(rdbProperties, rdbProperties_CheckedChanged, IsProperties);

                SetRadioButton(rdbDataMembers, rdbDataMembers_CheckedChanged, !IsProperties);

                IsVirtualProperties = options.IsVirtualProperties;
                SetCheckBox(chkVirtualProperties, chkVirtualProperties_CheckedChanged, IsVirtualProperties);

                IsOverrideProperties = options.IsOverrideProperties;
                SetCheckBox(chkOverrideProperties, chkOverrideProperties_CheckedChanged, IsOverrideProperties);

                IsPartialClass = options.IsPartialClass;
                SetCheckBox(chkPartialClass, chkPartialClass_CheckedChanged, IsPartialClass);

                IsAllStructNullable = options.IsAllStructNullable;
                SetCheckBox(chkAllStructNullable, chkAllStructNullable_CheckedChanged, IsAllStructNullable);

                if (options.IsComments == false)
                    options.IsCommentsWithoutNull = false;

                IsComments = options.IsComments;
                SetCheckBox(chkComments, chkComments_CheckedChanged, IsComments);

                IsCommentsWithoutNull = options.IsCommentsWithoutNull;
                SetCheckBox(chkCommentsWithoutNull, chkCommentsWithoutNull_CheckedChanged, IsCommentsWithoutNull);

                IsUsing = options.IsUsing;
                SetCheckBox(chkUsing, chkUsing_CheckedChanged, IsUsing);

                Namespace = options.Namespace;
                SetTextBox(txtNamespace, txtNamespace_TextChanged, Namespace);

                Inherit = options.Inherit;
                SetTextBox(txtInherit, txtInherit_TextChanged, Inherit);

                IsColumnDefaults = options.IsColumnDefaults;
                SetCheckBox(chkColumnDefaults, chkColumnDefaults_CheckedChanged, IsColumnDefaults);

                IsNewLineBetweenMembers = options.IsNewLineBetweenMembers;
                SetCheckBox(chkNewLineBetweenMembers, chkNewLineBetweenMembers_CheckedChanged, IsNewLineBetweenMembers);

                if (options.IsNavigationProperties == false)
                {
                    options.IsNavigationPropertiesVirtual = false;
                    options.IsNavigationPropertiesOverride = false;
                    options.IsNavigationPropertiesShowJoinTable = false;
                    options.IsNavigationPropertiesComments = false;
                    options.IsEFForeignKey = false;
                }

                if (options.IsNavigationPropertiesList == false && options.IsNavigationPropertiesICollection == false && options.IsNavigationPropertiesIEnumerable == false)
                    options.IsNavigationPropertiesList = true;

                if (options.IsNavigationPropertiesList)
                {
                    options.IsNavigationPropertiesICollection = false;
                    options.IsNavigationPropertiesIEnumerable = false;
                }
                else if (options.IsNavigationPropertiesICollection)
                {
                    options.IsNavigationPropertiesList = false;
                    options.IsNavigationPropertiesIEnumerable = false;
                }
                else if (options.IsNavigationPropertiesIEnumerable)
                {
                    options.IsNavigationPropertiesList = false;
                    options.IsNavigationPropertiesICollection = false;
                }

                IsNavigationProperties = options.IsNavigationProperties;
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, IsNavigationProperties);

                IsNavigationPropertiesVirtual = options.IsNavigationPropertiesVirtual;
                SetCheckBox(chkNavigationPropertiesVirtual, chkNavigationPropertiesVirtual_CheckedChanged, IsNavigationPropertiesVirtual);

                IsNavigationPropertiesOverride = options.IsNavigationPropertiesOverride;
                SetCheckBox(chkNavigationPropertiesOverride, chkNavigationPropertiesOverride_CheckedChanged, IsNavigationPropertiesOverride);

                IsNavigationPropertiesShowJoinTable = options.IsNavigationPropertiesShowJoinTable;
                SetCheckBox(chkNavigationPropertiesShowJoinTable, chkNavigationPropertiesShowJoinTable_CheckedChanged, IsNavigationPropertiesShowJoinTable);

                IsNavigationPropertiesComments = options.IsNavigationPropertiesComments;
                SetCheckBox(chkNavigationPropertiesComments, chkNavigationPropertiesComments_CheckedChanged, IsNavigationPropertiesComments);

                IsNavigationPropertiesList = options.IsNavigationPropertiesList;
                SetRadioButton(rdbNavigationPropertiesList, rdbNavigationPropertiesList_CheckedChanged, IsNavigationPropertiesList);

                IsNavigationPropertiesICollection = options.IsNavigationPropertiesICollection;
                SetRadioButton(rdbNavigationPropertiesICollection, rdbNavigationPropertiesICollection_CheckedChanged, IsNavigationPropertiesICollection);

                IsNavigationPropertiesIEnumerable = options.IsNavigationPropertiesIEnumerable;
                SetRadioButton(rdbNavigationPropertiesIEnumerable, rdbNavigationPropertiesIEnumerable_CheckedChanged, IsNavigationPropertiesIEnumerable);

                IsSingular = options.IsSingular;
                SetCheckBox(chkSingular, chkSingular_CheckedChanged, IsSingular);

                IsIncludeDB = options.IsIncludeDB;
                SetCheckBox(chkIncludeDB, chkIncludeDB_CheckedChanged, IsIncludeDB);

                DBSeparator = options.DBSeparator;
                SetTextBox(txtDBSeparator, txtDBSeparator_TextChanged, DBSeparator);

                IsIncludeSchema = options.IsIncludeSchema;
                SetCheckBox(chkIncludeSchema, chkIncludeSchema_CheckedChanged, IsIncludeSchema);

                IsIgnoreDboSchema = options.IsIgnoreDboSchema;
                SetCheckBox(chkIgnoreDboSchema, chkIgnoreDboSchema_CheckedChanged, IsIgnoreDboSchema);

                SchemaSeparator = options.SchemaSeparator;
                SetTextBox(txtSchemaSeparator, txtSchemaSeparator_TextChanged, SchemaSeparator);

                WordsSeparator = options.WordsSeparator;
                SetTextBox(txtWordsSeparator, txtWordsSeparator_TextChanged, WordsSeparator);

                IsCamelCase = options.IsCamelCase;
                SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, IsCamelCase);

                IsUpperCase = options.IsUpperCase;
                SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, IsUpperCase);

                IsLowerCase = options.IsLowerCase;
                SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, IsLowerCase);

                Search = options.Search;
                SetTextBox(txtSearch, txtSearch_TextChanged, Search);

                Replace = options.Replace;
                SetTextBox(txtReplace, txtReplace_TextChanged, Replace);

                IsSearchIgnoreCase = options.IsSearchIgnoreCase;
                SetCheckBox(chkSearchIgnoreCase, chkSearchIgnoreCase_CheckedChanged, IsSearchIgnoreCase);

                FixedClassName = options.FixedClassName;
                SetTextBox(txtFixedClassName, txtFixedClassName_TextChanged, FixedClassName);

                Prefix = options.Prefix;
                SetTextBox(txtPrefix, txtPrefix_TextChanged, Prefix);

                Suffix = options.Suffix;
                SetTextBox(txtSuffix, txtSuffix_TextChanged, Suffix);

                if (options.IsEF == false)
                {
                    options.IsEFColumn = false;
                    options.IsEFRequired = false;
                    options.IsEFRequiredWithErrorMessage = false;
                    options.IsEFConcurrencyCheck = false;
                    options.IsEFStringLength = false;
                    options.IsEFDisplay = false;
                    options.IsEFDescription = false;
                    options.IsEFComplexType = false;
                    options.IsEFIndex = false;
                    options.IsEFForeignKey = false;
                }

                IsEF = options.IsEF;
                SetCheckBox(chkEF, chkEF_CheckedChanged, IsEF);

                IsEFColumn = options.IsEFColumn;
                SetCheckBox(chkEFColumn, chkEFColumn_CheckedChanged, IsEFColumn);

                IsEFRequired = options.IsEFRequired;
                SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, IsEFRequired);

                IsEFRequiredWithErrorMessage = options.IsEFRequiredWithErrorMessage;
                SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, IsEFRequiredWithErrorMessage);

                IsEFConcurrencyCheck = options.IsEFConcurrencyCheck;
                SetCheckBox(chkEFConcurrencyCheck, chkEFConcurrencyCheck_CheckedChanged, IsEFConcurrencyCheck);

                IsEFStringLength = options.IsEFStringLength;
                SetCheckBox(chkEFStringLength, chkEFStringLength_CheckedChanged, IsEFStringLength);

                IsEFDisplay = options.IsEFDisplay;
                SetCheckBox(chkEFDisplay, chkEFDisplay_CheckedChanged, IsEFDisplay);

                IsEFDescription = options.IsEFDescription;
                SetCheckBox(chkEFDescription, chkEFDescription_CheckedChanged, IsEFDescription);

                IsEFComplexType = options.IsEFComplexType;
                SetCheckBox(chkEFComplexType, chkEFComplexType_CheckedChanged, IsEFComplexType);

                IsEFIndex = options.IsEFIndex;
                SetCheckBox(chkEFIndex, chkEFIndex_CheckedChanged, IsEFIndex);

                IsEFForeignKey = options.IsEFForeignKey;
                SetCheckBox(chkEFForeignKey, chkEFForeignKey_CheckedChanged, IsEFForeignKey);

                txtFolder.Text = options.Folder;
                chkSingleFile.Checked = options.IsSingleFile;
                txtFileName.Text = options.FileName;
            }
            catch
            {
            }
        }

        private void SaveOptions()
        {
            try
            {
                Options options = GetOptions(false);
                options.IsNavigationPropertiesList = IsNavigationPropertiesList;
                options.IsNavigationPropertiesICollection = IsNavigationPropertiesICollection;
                options.IsNavigationPropertiesIEnumerable = IsNavigationPropertiesIEnumerable;
                SerializationHelper.BinarySerializeToFile(options, settingsFileName);
            }
            catch
            {
            }
        }

        #endregion
    }
}
