namespace POCOGenerator
{
    partial class POCOGeneratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POCOGeneratorForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trvServer = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCheckBoxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListDbObjects = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtPocoEditor = new System.Windows.Forms.RichTextBox();
            this.contextMenuPocoEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.chkColumnDefaults = new System.Windows.Forms.CheckBox();
            this.chkNavigationPropertiesOverride = new System.Windows.Forms.CheckBox();
            this.chkOverrideProperties = new System.Windows.Forms.CheckBox();
            this.chkEFDescription = new System.Windows.Forms.CheckBox();
            this.txtInherit = new System.Windows.Forms.TextBox();
            this.lblInherit = new System.Windows.Forms.Label();
            this.btnCommandLine = new System.Windows.Forms.Button();
            this.chkNavigationPropertiesShowJoinTable = new System.Windows.Forms.CheckBox();
            this.chkNavigationPropertiesComments = new System.Windows.Forms.CheckBox();
            this.chkEFForeignKey = new System.Windows.Forms.CheckBox();
            this.panelNavigationProperties1 = new System.Windows.Forms.Panel();
            this.rdbNavigationPropertiesIEnumerable = new System.Windows.Forms.RadioButton();
            this.rdbNavigationPropertiesICollection = new System.Windows.Forms.RadioButton();
            this.rdbNavigationPropertiesList = new System.Windows.Forms.RadioButton();
            this.chkNavigationPropertiesVirtual = new System.Windows.Forms.CheckBox();
            this.chkNavigationProperties = new System.Windows.Forms.CheckBox();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.rdbProperties = new System.Windows.Forms.RadioButton();
            this.rdbDataMembers = new System.Windows.Forms.RadioButton();
            this.chkEFIndex = new System.Windows.Forms.CheckBox();
            this.chkEFComplexType = new System.Windows.Forms.CheckBox();
            this.lblEF = new System.Windows.Forms.Label();
            this.chkEFRequiredWithErrorMessage = new System.Windows.Forms.CheckBox();
            this.chkNewLineBetweenMembers = new System.Windows.Forms.CheckBox();
            this.chkEFConcurrencyCheck = new System.Windows.Forms.CheckBox();
            this.chkEFStringLength = new System.Windows.Forms.CheckBox();
            this.chkEFDisplay = new System.Windows.Forms.CheckBox();
            this.chkSearchIgnoreCase = new System.Windows.Forms.CheckBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.lblReplace = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblSingularDesc = new System.Windows.Forms.Label();
            this.lblEFAnnotationsTables = new System.Windows.Forms.Label();
            this.chkEFColumn = new System.Windows.Forms.CheckBox();
            this.chkEFRequired = new System.Windows.Forms.CheckBox();
            this.chkEF = new System.Windows.Forms.CheckBox();
            this.chkPartialClass = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEFAnnotations = new System.Windows.Forms.Label();
            this.chkSingular = new System.Windows.Forms.CheckBox();
            this.chkUsing = new System.Windows.Forms.CheckBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.lblNamespace = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtSchemaSeparator = new System.Windows.Forms.TextBox();
            this.lblSchemaSeparator = new System.Windows.Forms.Label();
            this.chkIgnoreDboSchema = new System.Windows.Forms.CheckBox();
            this.chkIncludeSchema = new System.Windows.Forms.CheckBox();
            this.txtDBSeparator = new System.Windows.Forms.TextBox();
            this.lblDBSeparator = new System.Windows.Forms.Label();
            this.chkIncludeDB = new System.Windows.Forms.CheckBox();
            this.lblFixedName = new System.Windows.Forms.Label();
            this.lblWordsSeparatorDesc = new System.Windows.Forms.Label();
            this.btnTypeMapping = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.chkSingleFile = new System.Windows.Forms.CheckBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lblExport = new System.Windows.Forms.Label();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.txtWordsSeparator = new System.Windows.Forms.TextBox();
            this.lblWordsSeparator = new System.Windows.Forms.Label();
            this.chkLowerCase = new System.Windows.Forms.CheckBox();
            this.chkUpperCase = new System.Windows.Forms.CheckBox();
            this.chkCamelCase = new System.Windows.Forms.CheckBox();
            this.lblPOCO = new System.Windows.Forms.Label();
            this.chkCommentsWithoutNull = new System.Windows.Forms.CheckBox();
            this.chkComments = new System.Windows.Forms.CheckBox();
            this.txtFixedClassName = new System.Windows.Forms.TextBox();
            this.chkAllStructNullable = new System.Windows.Forms.CheckBox();
            this.chkVirtualProperties = new System.Windows.Forms.CheckBox();
            this.lblClassName = new System.Windows.Forms.Label();
            this.folderBrowserDialogExport = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkTablesConnectedFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkTablesConnectedToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkTablesConnectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkRecursivelyTablesConnectedToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkRecursivelyTablesConnectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuPocoEditor.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelNavigationProperties1.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.contextMenuTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvServer);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 650;
            this.splitContainer1.Size = new System.Drawing.Size(1084, 712);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;
            // 
            // trvServer
            // 
            this.trvServer.CheckBoxes = true;
            this.trvServer.ContextMenuStrip = this.contextMenu;
            this.trvServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvServer.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.trvServer.HideSelection = false;
            this.trvServer.ImageIndex = 0;
            this.trvServer.ImageList = this.imageListDbObjects;
            this.trvServer.Location = new System.Drawing.Point(0, 0);
            this.trvServer.Name = "trvServer";
            this.trvServer.SelectedImageIndex = 0;
            this.trvServer.Size = new System.Drawing.Size(400, 712);
            this.trvServer.TabIndex = 1;
            this.trvServer.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvServer_AfterCheck);
            this.trvServer.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvServer_DrawNode);
            this.trvServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvServer_AfterSelect);
            this.trvServer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trvServer_MouseUp);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFilterToolStripMenuItem,
            this.filterSettingsToolStripMenuItem,
            this.clearCheckBoxesToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.contextMenu.Name = "contextMenuServerTree";
            this.contextMenu.Size = new System.Drawing.Size(168, 92);
            // 
            // removeFilterToolStripMenuItem
            // 
            this.removeFilterToolStripMenuItem.Name = "removeFilterToolStripMenuItem";
            this.removeFilterToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.removeFilterToolStripMenuItem.Text = "Remove Filter";
            this.removeFilterToolStripMenuItem.Click += new System.EventHandler(this.removeFilterToolStripMenuItem_Click);
            // 
            // filterSettingsToolStripMenuItem
            // 
            this.filterSettingsToolStripMenuItem.Name = "filterSettingsToolStripMenuItem";
            this.filterSettingsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.filterSettingsToolStripMenuItem.Text = "Filter Settings";
            this.filterSettingsToolStripMenuItem.Click += new System.EventHandler(this.filterSettingsToolStripMenuItem_Click);
            // 
            // clearCheckBoxesToolStripMenuItem
            // 
            this.clearCheckBoxesToolStripMenuItem.Name = "clearCheckBoxesToolStripMenuItem";
            this.clearCheckBoxesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.clearCheckBoxesToolStripMenuItem.Text = "Clear Checkboxes";
            this.clearCheckBoxesToolStripMenuItem.Click += new System.EventHandler(this.clearCheckBoxesToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // imageListDbObjects
            // 
            this.imageListDbObjects.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDbObjects.ImageStream")));
            this.imageListDbObjects.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDbObjects.Images.SetKeyName(0, "Server.gif");
            this.imageListDbObjects.Images.SetKeyName(1, "Database.gif");
            this.imageListDbObjects.Images.SetKeyName(2, "Folder.gif");
            this.imageListDbObjects.Images.SetKeyName(3, "Table.gif");
            this.imageListDbObjects.Images.SetKeyName(4, "View.gif");
            this.imageListDbObjects.Images.SetKeyName(5, "Procedure.gif");
            this.imageListDbObjects.Images.SetKeyName(6, "Function.gif");
            this.imageListDbObjects.Images.SetKeyName(7, "Column.gif");
            this.imageListDbObjects.Images.SetKeyName(8, "PK.gif");
            this.imageListDbObjects.Images.SetKeyName(9, "FK.gif");
            this.imageListDbObjects.Images.SetKeyName(10, "UK.gif");
            this.imageListDbObjects.Images.SetKeyName(11, "Index.gif");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtPocoEditor);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelMain);
            this.splitContainer2.Size = new System.Drawing.Size(680, 712);
            this.splitContainer2.SplitterDistance = 348;
            this.splitContainer2.TabIndex = 0;
            // 
            // txtPocoEditor
            // 
            this.txtPocoEditor.BackColor = System.Drawing.Color.White;
            this.txtPocoEditor.ContextMenuStrip = this.contextMenuPocoEditor;
            this.txtPocoEditor.DetectUrls = false;
            this.txtPocoEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPocoEditor.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtPocoEditor.Location = new System.Drawing.Point(0, 0);
            this.txtPocoEditor.Name = "txtPocoEditor";
            this.txtPocoEditor.ReadOnly = true;
            this.txtPocoEditor.Size = new System.Drawing.Size(680, 348);
            this.txtPocoEditor.TabIndex = 0;
            this.txtPocoEditor.Text = "";
            this.txtPocoEditor.WordWrap = false;
            // 
            // contextMenuPocoEditor
            // 
            this.contextMenuPocoEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.contextMenuPocoEditor.Name = "contextMenuPocoEditor";
            this.contextMenuPocoEditor.Size = new System.Drawing.Size(123, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.chkColumnDefaults);
            this.panelMain.Controls.Add(this.chkNavigationPropertiesOverride);
            this.panelMain.Controls.Add(this.chkOverrideProperties);
            this.panelMain.Controls.Add(this.chkEFDescription);
            this.panelMain.Controls.Add(this.txtInherit);
            this.panelMain.Controls.Add(this.lblInherit);
            this.panelMain.Controls.Add(this.btnCommandLine);
            this.panelMain.Controls.Add(this.chkNavigationPropertiesShowJoinTable);
            this.panelMain.Controls.Add(this.chkNavigationPropertiesComments);
            this.panelMain.Controls.Add(this.chkEFForeignKey);
            this.panelMain.Controls.Add(this.panelNavigationProperties1);
            this.panelMain.Controls.Add(this.chkNavigationPropertiesVirtual);
            this.panelMain.Controls.Add(this.chkNavigationProperties);
            this.panelMain.Controls.Add(this.panelProperties);
            this.panelMain.Controls.Add(this.chkEFIndex);
            this.panelMain.Controls.Add(this.chkEFComplexType);
            this.panelMain.Controls.Add(this.lblEF);
            this.panelMain.Controls.Add(this.chkEFRequiredWithErrorMessage);
            this.panelMain.Controls.Add(this.chkNewLineBetweenMembers);
            this.panelMain.Controls.Add(this.chkEFConcurrencyCheck);
            this.panelMain.Controls.Add(this.chkEFStringLength);
            this.panelMain.Controls.Add(this.chkEFDisplay);
            this.panelMain.Controls.Add(this.chkSearchIgnoreCase);
            this.panelMain.Controls.Add(this.txtReplace);
            this.panelMain.Controls.Add(this.lblReplace);
            this.panelMain.Controls.Add(this.txtSearch);
            this.panelMain.Controls.Add(this.lblSearch);
            this.panelMain.Controls.Add(this.lblSingularDesc);
            this.panelMain.Controls.Add(this.lblEFAnnotationsTables);
            this.panelMain.Controls.Add(this.chkEFColumn);
            this.panelMain.Controls.Add(this.chkEFRequired);
            this.panelMain.Controls.Add(this.chkEF);
            this.panelMain.Controls.Add(this.chkPartialClass);
            this.panelMain.Controls.Add(this.statusStrip);
            this.panelMain.Controls.Add(this.lblEFAnnotations);
            this.panelMain.Controls.Add(this.chkSingular);
            this.panelMain.Controls.Add(this.chkUsing);
            this.panelMain.Controls.Add(this.txtNamespace);
            this.panelMain.Controls.Add(this.lblNamespace);
            this.panelMain.Controls.Add(this.btnCopy);
            this.panelMain.Controls.Add(this.txtSchemaSeparator);
            this.panelMain.Controls.Add(this.lblSchemaSeparator);
            this.panelMain.Controls.Add(this.chkIgnoreDboSchema);
            this.panelMain.Controls.Add(this.chkIncludeSchema);
            this.panelMain.Controls.Add(this.txtDBSeparator);
            this.panelMain.Controls.Add(this.lblDBSeparator);
            this.panelMain.Controls.Add(this.chkIncludeDB);
            this.panelMain.Controls.Add(this.lblFixedName);
            this.panelMain.Controls.Add(this.lblWordsSeparatorDesc);
            this.panelMain.Controls.Add(this.btnTypeMapping);
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Controls.Add(this.btnExport);
            this.panelMain.Controls.Add(this.btnFolder);
            this.panelMain.Controls.Add(this.chkSingleFile);
            this.panelMain.Controls.Add(this.txtFileName);
            this.panelMain.Controls.Add(this.txtFolder);
            this.panelMain.Controls.Add(this.lblExport);
            this.panelMain.Controls.Add(this.txtSuffix);
            this.panelMain.Controls.Add(this.lblSuffix);
            this.panelMain.Controls.Add(this.txtPrefix);
            this.panelMain.Controls.Add(this.lblPrefix);
            this.panelMain.Controls.Add(this.txtWordsSeparator);
            this.panelMain.Controls.Add(this.lblWordsSeparator);
            this.panelMain.Controls.Add(this.chkLowerCase);
            this.panelMain.Controls.Add(this.chkUpperCase);
            this.panelMain.Controls.Add(this.chkCamelCase);
            this.panelMain.Controls.Add(this.lblPOCO);
            this.panelMain.Controls.Add(this.chkCommentsWithoutNull);
            this.panelMain.Controls.Add(this.chkComments);
            this.panelMain.Controls.Add(this.txtFixedClassName);
            this.panelMain.Controls.Add(this.chkAllStructNullable);
            this.panelMain.Controls.Add(this.chkVirtualProperties);
            this.panelMain.Controls.Add(this.lblClassName);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(680, 360);
            this.panelMain.TabIndex = 0;
            // 
            // chkColumnDefaults
            // 
            this.chkColumnDefaults.AutoSize = true;
            this.chkColumnDefaults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkColumnDefaults.Location = new System.Drawing.Point(4, 145);
            this.chkColumnDefaults.Name = "chkColumnDefaults";
            this.chkColumnDefaults.Size = new System.Drawing.Size(103, 17);
            this.chkColumnDefaults.TabIndex = 11;
            this.chkColumnDefaults.Text = "Column Defaults";
            this.chkColumnDefaults.UseVisualStyleBackColor = true;
            this.chkColumnDefaults.CheckedChanged += new System.EventHandler(this.chkColumnDefaults_CheckedChanged);
            // 
            // chkNavigationPropertiesOverride
            // 
            this.chkNavigationPropertiesOverride.AutoSize = true;
            this.chkNavigationPropertiesOverride.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNavigationPropertiesOverride.Location = new System.Drawing.Point(213, 170);
            this.chkNavigationPropertiesOverride.Name = "chkNavigationPropertiesOverride";
            this.chkNavigationPropertiesOverride.Size = new System.Drawing.Size(66, 17);
            this.chkNavigationPropertiesOverride.TabIndex = 15;
            this.chkNavigationPropertiesOverride.Text = "Override";
            this.chkNavigationPropertiesOverride.UseVisualStyleBackColor = true;
            this.chkNavigationPropertiesOverride.CheckedChanged += new System.EventHandler(this.chkNavigationPropertiesOverride_CheckedChanged);
            // 
            // chkOverrideProperties
            // 
            this.chkOverrideProperties.AutoSize = true;
            this.chkOverrideProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOverrideProperties.Location = new System.Drawing.Point(117, 51);
            this.chkOverrideProperties.Name = "chkOverrideProperties";
            this.chkOverrideProperties.Size = new System.Drawing.Size(116, 17);
            this.chkOverrideProperties.TabIndex = 3;
            this.chkOverrideProperties.Text = "Override Properties";
            this.chkOverrideProperties.UseVisualStyleBackColor = true;
            this.chkOverrideProperties.CheckedChanged += new System.EventHandler(this.chkOverrideProperties_CheckedChanged);
            // 
            // chkEFDescription
            // 
            this.chkEFDescription.AutoSize = true;
            this.chkEFDescription.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFDescription.Location = new System.Drawing.Point(573, 264);
            this.chkEFDescription.Name = "chkEFDescription";
            this.chkEFDescription.Size = new System.Drawing.Size(79, 17);
            this.chkEFDescription.TabIndex = 47;
            this.chkEFDescription.Text = "Description";
            this.chkEFDescription.UseVisualStyleBackColor = true;
            this.chkEFDescription.CheckedChanged += new System.EventHandler(this.chkEFDescription_CheckedChanged);
            // 
            // txtInherit
            // 
            this.txtInherit.Location = new System.Drawing.Point(187, 120);
            this.txtInherit.Name = "txtInherit";
            this.txtInherit.Size = new System.Drawing.Size(75, 20);
            this.txtInherit.TabIndex = 10;
            this.txtInherit.TextChanged += new System.EventHandler(this.txtInherit_TextChanged);
            // 
            // lblInherit
            // 
            this.lblInherit.AutoSize = true;
            this.lblInherit.Location = new System.Drawing.Point(151, 122);
            this.lblInherit.Name = "lblInherit";
            this.lblInherit.Size = new System.Drawing.Size(36, 13);
            this.lblInherit.TabIndex = 0;
            this.lblInherit.Text = "Inherit";
            // 
            // btnCommandLine
            // 
            this.btnCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommandLine.AutoSize = true;
            this.btnCommandLine.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCommandLine.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCommandLine.Location = new System.Drawing.Point(443, 310);
            this.btnCommandLine.Name = "btnCommandLine";
            this.btnCommandLine.Size = new System.Drawing.Size(87, 23);
            this.btnCommandLine.TabIndex = 52;
            this.btnCommandLine.Text = "Command Line";
            this.btnCommandLine.UseVisualStyleBackColor = true;
            this.btnCommandLine.Click += new System.EventHandler(this.btnCommandLine_Click);
            // 
            // chkNavigationPropertiesShowJoinTable
            // 
            this.chkNavigationPropertiesShowJoinTable.AutoSize = true;
            this.chkNavigationPropertiesShowJoinTable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNavigationPropertiesShowJoinTable.Location = new System.Drawing.Point(4, 191);
            this.chkNavigationPropertiesShowJoinTable.Name = "chkNavigationPropertiesShowJoinTable";
            this.chkNavigationPropertiesShowJoinTable.Size = new System.Drawing.Size(175, 17);
            this.chkNavigationPropertiesShowJoinTable.TabIndex = 16;
            this.chkNavigationPropertiesShowJoinTable.Text = "Show Many-to-Many Join Table";
            this.chkNavigationPropertiesShowJoinTable.UseVisualStyleBackColor = true;
            this.chkNavigationPropertiesShowJoinTable.CheckedChanged += new System.EventHandler(this.chkNavigationPropertiesShowJoinTable_CheckedChanged);
            // 
            // chkNavigationPropertiesComments
            // 
            this.chkNavigationPropertiesComments.AutoSize = true;
            this.chkNavigationPropertiesComments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNavigationPropertiesComments.Location = new System.Drawing.Point(187, 191);
            this.chkNavigationPropertiesComments.Name = "chkNavigationPropertiesComments";
            this.chkNavigationPropertiesComments.Size = new System.Drawing.Size(75, 17);
            this.chkNavigationPropertiesComments.TabIndex = 17;
            this.chkNavigationPropertiesComments.Text = "Comments";
            this.chkNavigationPropertiesComments.UseVisualStyleBackColor = true;
            this.chkNavigationPropertiesComments.CheckedChanged += new System.EventHandler(this.chkNavigationPropertiesComments_CheckedChanged);
            // 
            // chkEFForeignKey
            // 
            this.chkEFForeignKey.AutoSize = true;
            this.chkEFForeignKey.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFForeignKey.Location = new System.Drawing.Point(472, 289);
            this.chkEFForeignKey.Name = "chkEFForeignKey";
            this.chkEFForeignKey.Size = new System.Drawing.Size(165, 17);
            this.chkEFForeignKey.TabIndex = 50;
            this.chkEFForeignKey.Text = "ForeignKey && InverseProperty";
            this.chkEFForeignKey.UseVisualStyleBackColor = true;
            this.chkEFForeignKey.CheckedChanged += new System.EventHandler(this.chkEFForeignKey_CheckedChanged);
            // 
            // panelNavigationProperties1
            // 
            this.panelNavigationProperties1.AutoSize = true;
            this.panelNavigationProperties1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelNavigationProperties1.Controls.Add(this.rdbNavigationPropertiesIEnumerable);
            this.panelNavigationProperties1.Controls.Add(this.rdbNavigationPropertiesICollection);
            this.panelNavigationProperties1.Controls.Add(this.rdbNavigationPropertiesList);
            this.panelNavigationProperties1.Location = new System.Drawing.Point(4, 215);
            this.panelNavigationProperties1.Name = "panelNavigationProperties1";
            this.panelNavigationProperties1.Size = new System.Drawing.Size(218, 20);
            this.panelNavigationProperties1.TabIndex = 18;
            this.panelNavigationProperties1.TabStop = true;
            // 
            // rdbNavigationPropertiesIEnumerable
            // 
            this.rdbNavigationPropertiesIEnumerable.AutoSize = true;
            this.rdbNavigationPropertiesIEnumerable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbNavigationPropertiesIEnumerable.Location = new System.Drawing.Point(131, 0);
            this.rdbNavigationPropertiesIEnumerable.Name = "rdbNavigationPropertiesIEnumerable";
            this.rdbNavigationPropertiesIEnumerable.Size = new System.Drawing.Size(84, 17);
            this.rdbNavigationPropertiesIEnumerable.TabIndex = 3;
            this.rdbNavigationPropertiesIEnumerable.TabStop = true;
            this.rdbNavigationPropertiesIEnumerable.Text = "IEnumerable";
            this.rdbNavigationPropertiesIEnumerable.UseVisualStyleBackColor = true;
            this.rdbNavigationPropertiesIEnumerable.CheckedChanged += new System.EventHandler(this.rdbNavigationPropertiesIEnumerable_CheckedChanged);
            // 
            // rdbNavigationPropertiesICollection
            // 
            this.rdbNavigationPropertiesICollection.AutoSize = true;
            this.rdbNavigationPropertiesICollection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbNavigationPropertiesICollection.Location = new System.Drawing.Point(49, 0);
            this.rdbNavigationPropertiesICollection.Name = "rdbNavigationPropertiesICollection";
            this.rdbNavigationPropertiesICollection.Size = new System.Drawing.Size(74, 17);
            this.rdbNavigationPropertiesICollection.TabIndex = 2;
            this.rdbNavigationPropertiesICollection.TabStop = true;
            this.rdbNavigationPropertiesICollection.Text = "ICollection";
            this.rdbNavigationPropertiesICollection.UseVisualStyleBackColor = true;
            this.rdbNavigationPropertiesICollection.CheckedChanged += new System.EventHandler(this.rdbNavigationPropertiesICollection_CheckedChanged);
            // 
            // rdbNavigationPropertiesList
            // 
            this.rdbNavigationPropertiesList.AutoSize = true;
            this.rdbNavigationPropertiesList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbNavigationPropertiesList.Checked = true;
            this.rdbNavigationPropertiesList.Location = new System.Drawing.Point(0, 0);
            this.rdbNavigationPropertiesList.Name = "rdbNavigationPropertiesList";
            this.rdbNavigationPropertiesList.Size = new System.Drawing.Size(41, 17);
            this.rdbNavigationPropertiesList.TabIndex = 1;
            this.rdbNavigationPropertiesList.TabStop = true;
            this.rdbNavigationPropertiesList.Text = "List";
            this.rdbNavigationPropertiesList.UseVisualStyleBackColor = true;
            this.rdbNavigationPropertiesList.CheckedChanged += new System.EventHandler(this.rdbNavigationPropertiesList_CheckedChanged);
            // 
            // chkNavigationPropertiesVirtual
            // 
            this.chkNavigationPropertiesVirtual.AutoSize = true;
            this.chkNavigationPropertiesVirtual.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNavigationPropertiesVirtual.Location = new System.Drawing.Point(156, 170);
            this.chkNavigationPropertiesVirtual.Name = "chkNavigationPropertiesVirtual";
            this.chkNavigationPropertiesVirtual.Size = new System.Drawing.Size(55, 17);
            this.chkNavigationPropertiesVirtual.TabIndex = 14;
            this.chkNavigationPropertiesVirtual.Text = "Virtual";
            this.chkNavigationPropertiesVirtual.UseVisualStyleBackColor = true;
            this.chkNavigationPropertiesVirtual.CheckedChanged += new System.EventHandler(this.chkNavigationPropertiesVirtual_CheckedChanged);
            // 
            // chkNavigationProperties
            // 
            this.chkNavigationProperties.AutoSize = true;
            this.chkNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNavigationProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNavigationProperties.Location = new System.Drawing.Point(4, 170);
            this.chkNavigationProperties.Name = "chkNavigationProperties";
            this.chkNavigationProperties.Size = new System.Drawing.Size(148, 17);
            this.chkNavigationProperties.TabIndex = 13;
            this.chkNavigationProperties.Text = "Navigation Properties";
            this.chkNavigationProperties.UseVisualStyleBackColor = true;
            this.chkNavigationProperties.CheckedChanged += new System.EventHandler(this.chkNavigationProperties_CheckedChanged);
            // 
            // panelProperties
            // 
            this.panelProperties.AutoSize = true;
            this.panelProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelProperties.Controls.Add(this.rdbProperties);
            this.panelProperties.Controls.Add(this.rdbDataMembers);
            this.panelProperties.Location = new System.Drawing.Point(4, 24);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(177, 20);
            this.panelProperties.TabIndex = 1;
            this.panelProperties.TabStop = true;
            // 
            // rdbProperties
            // 
            this.rdbProperties.AutoSize = true;
            this.rdbProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbProperties.Checked = true;
            this.rdbProperties.Location = new System.Drawing.Point(0, 0);
            this.rdbProperties.Name = "rdbProperties";
            this.rdbProperties.Size = new System.Drawing.Size(72, 17);
            this.rdbProperties.TabIndex = 1;
            this.rdbProperties.TabStop = true;
            this.rdbProperties.Text = "Properties";
            this.rdbProperties.UseVisualStyleBackColor = true;
            this.rdbProperties.CheckedChanged += new System.EventHandler(this.rdbProperties_CheckedChanged);
            // 
            // rdbDataMembers
            // 
            this.rdbDataMembers.AutoSize = true;
            this.rdbDataMembers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbDataMembers.Location = new System.Drawing.Point(80, 0);
            this.rdbDataMembers.Name = "rdbDataMembers";
            this.rdbDataMembers.Size = new System.Drawing.Size(94, 17);
            this.rdbDataMembers.TabIndex = 2;
            this.rdbDataMembers.TabStop = true;
            this.rdbDataMembers.Text = "Data Members";
            this.rdbDataMembers.UseVisualStyleBackColor = true;
            this.rdbDataMembers.CheckedChanged += new System.EventHandler(this.rdbDataMembers_CheckedChanged);
            // 
            // chkEFIndex
            // 
            this.chkEFIndex.AutoSize = true;
            this.chkEFIndex.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFIndex.Location = new System.Drawing.Point(384, 289);
            this.chkEFIndex.Name = "chkEFIndex";
            this.chkEFIndex.Size = new System.Drawing.Size(80, 17);
            this.chkEFIndex.TabIndex = 49;
            this.chkEFIndex.Text = "Index (EF6)";
            this.chkEFIndex.UseVisualStyleBackColor = true;
            this.chkEFIndex.CheckedChanged += new System.EventHandler(this.chkEFIndex_CheckedChanged);
            // 
            // chkEFComplexType
            // 
            this.chkEFComplexType.AutoSize = true;
            this.chkEFComplexType.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFComplexType.Location = new System.Drawing.Point(286, 289);
            this.chkEFComplexType.Name = "chkEFComplexType";
            this.chkEFComplexType.Size = new System.Drawing.Size(90, 17);
            this.chkEFComplexType.TabIndex = 48;
            this.chkEFComplexType.Text = "ComplexType";
            this.chkEFComplexType.UseVisualStyleBackColor = true;
            this.chkEFComplexType.CheckedChanged += new System.EventHandler(this.chkEFComplexType_CheckedChanged);
            // 
            // lblEF
            // 
            this.lblEF.AutoSize = true;
            this.lblEF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lblEF.Location = new System.Drawing.Point(333, 216);
            this.lblEF.Name = "lblEF";
            this.lblEF.Size = new System.Drawing.Size(278, 13);
            this.lblEF.TabIndex = 0;
            this.lblEF.Text = "(Table, Key, MaxLength, Timestamp, DatabaseGenerated)";
            // 
            // chkEFRequiredWithErrorMessage
            // 
            this.chkEFRequiredWithErrorMessage.AutoSize = true;
            this.chkEFRequiredWithErrorMessage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFRequiredWithErrorMessage.Location = new System.Drawing.Point(432, 239);
            this.chkEFRequiredWithErrorMessage.Name = "chkEFRequiredWithErrorMessage";
            this.chkEFRequiredWithErrorMessage.Size = new System.Drawing.Size(159, 17);
            this.chkEFRequiredWithErrorMessage.TabIndex = 43;
            this.chkEFRequiredWithErrorMessage.Text = "Required with ErrorMessage";
            this.chkEFRequiredWithErrorMessage.UseVisualStyleBackColor = true;
            this.chkEFRequiredWithErrorMessage.CheckedChanged += new System.EventHandler(this.chkEFRequiredWithErrorMessage_CheckedChanged);
            // 
            // chkNewLineBetweenMembers
            // 
            this.chkNewLineBetweenMembers.AutoSize = true;
            this.chkNewLineBetweenMembers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNewLineBetweenMembers.Location = new System.Drawing.Point(115, 145);
            this.chkNewLineBetweenMembers.Name = "chkNewLineBetweenMembers";
            this.chkNewLineBetweenMembers.Size = new System.Drawing.Size(162, 17);
            this.chkNewLineBetweenMembers.TabIndex = 12;
            this.chkNewLineBetweenMembers.Text = "New Line Between Members";
            this.chkNewLineBetweenMembers.UseVisualStyleBackColor = true;
            this.chkNewLineBetweenMembers.CheckedChanged += new System.EventHandler(this.chkNewLineBetweenMembers_CheckedChanged);
            // 
            // chkEFConcurrencyCheck
            // 
            this.chkEFConcurrencyCheck.AutoSize = true;
            this.chkEFConcurrencyCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFConcurrencyCheck.Location = new System.Drawing.Point(286, 264);
            this.chkEFConcurrencyCheck.Name = "chkEFConcurrencyCheck";
            this.chkEFConcurrencyCheck.Size = new System.Drawing.Size(117, 17);
            this.chkEFConcurrencyCheck.TabIndex = 44;
            this.chkEFConcurrencyCheck.Text = "ConcurrencyCheck";
            this.chkEFConcurrencyCheck.UseVisualStyleBackColor = true;
            this.chkEFConcurrencyCheck.CheckedChanged += new System.EventHandler(this.chkEFConcurrencyCheck_CheckedChanged);
            // 
            // chkEFStringLength
            // 
            this.chkEFStringLength.AutoSize = true;
            this.chkEFStringLength.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFStringLength.Location = new System.Drawing.Point(411, 264);
            this.chkEFStringLength.Name = "chkEFStringLength";
            this.chkEFStringLength.Size = new System.Drawing.Size(86, 17);
            this.chkEFStringLength.TabIndex = 45;
            this.chkEFStringLength.Text = "StringLength";
            this.chkEFStringLength.UseVisualStyleBackColor = true;
            this.chkEFStringLength.CheckedChanged += new System.EventHandler(this.chkEFStringLength_CheckedChanged);
            // 
            // chkEFDisplay
            // 
            this.chkEFDisplay.AutoSize = true;
            this.chkEFDisplay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFDisplay.Location = new System.Drawing.Point(505, 264);
            this.chkEFDisplay.Name = "chkEFDisplay";
            this.chkEFDisplay.Size = new System.Drawing.Size(60, 17);
            this.chkEFDisplay.TabIndex = 46;
            this.chkEFDisplay.Text = "Display";
            this.chkEFDisplay.UseVisualStyleBackColor = true;
            this.chkEFDisplay.CheckedChanged += new System.EventHandler(this.chkEFDisplay_CheckedChanged);
            // 
            // chkSearchIgnoreCase
            // 
            this.chkSearchIgnoreCase.AutoSize = true;
            this.chkSearchIgnoreCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSearchIgnoreCase.Location = new System.Drawing.Point(540, 145);
            this.chkSearchIgnoreCase.Name = "chkSearchIgnoreCase";
            this.chkSearchIgnoreCase.Size = new System.Drawing.Size(83, 17);
            this.chkSearchIgnoreCase.TabIndex = 36;
            this.chkSearchIgnoreCase.Text = "Ignore Case";
            this.chkSearchIgnoreCase.UseVisualStyleBackColor = true;
            this.chkSearchIgnoreCase.CheckedChanged += new System.EventHandler(this.chkSearchIgnoreCase_CheckedChanged);
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(457, 143);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(75, 20);
            this.txtReplace.TabIndex = 35;
            this.txtReplace.TextChanged += new System.EventHandler(this.txtReplace_TextChanged);
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(410, 147);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(47, 13);
            this.lblReplace.TabIndex = 0;
            this.lblReplace.Text = "Replace";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(327, 143);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(75, 20);
            this.txtSearch.TabIndex = 34;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(286, 147);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(41, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search";
            // 
            // lblSingularDesc
            // 
            this.lblSingularDesc.AutoSize = true;
            this.lblSingularDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lblSingularDesc.Location = new System.Drawing.Point(358, 28);
            this.lblSingularDesc.Name = "lblSingularDesc";
            this.lblSingularDesc.Size = new System.Drawing.Size(107, 13);
            this.lblSingularDesc.TabIndex = 0;
            this.lblSingularDesc.Text = "(Tables, Views, TVPs)";
            // 
            // lblEFAnnotationsTables
            // 
            this.lblEFAnnotationsTables.AutoSize = true;
            this.lblEFAnnotationsTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lblEFAnnotationsTables.Location = new System.Drawing.Point(440, 193);
            this.lblEFAnnotationsTables.Name = "lblEFAnnotationsTables";
            this.lblEFAnnotationsTables.Size = new System.Drawing.Size(43, 13);
            this.lblEFAnnotationsTables.TabIndex = 0;
            this.lblEFAnnotationsTables.Text = "(Tables)";
            // 
            // chkEFColumn
            // 
            this.chkEFColumn.AutoSize = true;
            this.chkEFColumn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFColumn.Location = new System.Drawing.Point(286, 239);
            this.chkEFColumn.Name = "chkEFColumn";
            this.chkEFColumn.Size = new System.Drawing.Size(61, 17);
            this.chkEFColumn.TabIndex = 41;
            this.chkEFColumn.Text = "Column";
            this.chkEFColumn.UseVisualStyleBackColor = true;
            this.chkEFColumn.CheckedChanged += new System.EventHandler(this.chkEFColumn_CheckedChanged);
            // 
            // chkEFRequired
            // 
            this.chkEFRequired.AutoSize = true;
            this.chkEFRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEFRequired.Location = new System.Drawing.Point(355, 239);
            this.chkEFRequired.Name = "chkEFRequired";
            this.chkEFRequired.Size = new System.Drawing.Size(69, 17);
            this.chkEFRequired.TabIndex = 42;
            this.chkEFRequired.Text = "Required";
            this.chkEFRequired.UseVisualStyleBackColor = true;
            this.chkEFRequired.CheckedChanged += new System.EventHandler(this.chkEFRequired_CheckedChanged);
            // 
            // chkEF
            // 
            this.chkEF.AutoSize = true;
            this.chkEF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEF.Location = new System.Drawing.Point(286, 214);
            this.chkEF.Name = "chkEF";
            this.chkEF.Size = new System.Drawing.Size(39, 17);
            this.chkEF.TabIndex = 40;
            this.chkEF.Text = "EF";
            this.chkEF.UseVisualStyleBackColor = true;
            this.chkEF.CheckedChanged += new System.EventHandler(this.chkEF_CheckedChanged);
            // 
            // chkPartialClass
            // 
            this.chkPartialClass.AutoSize = true;
            this.chkPartialClass.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPartialClass.Location = new System.Drawing.Point(4, 76);
            this.chkPartialClass.Name = "chkPartialClass";
            this.chkPartialClass.Size = new System.Drawing.Size(83, 17);
            this.chkPartialClass.TabIndex = 4;
            this.chkPartialClass.Text = "Partial Class";
            this.chkPartialClass.UseVisualStyleBackColor = true;
            this.chkPartialClass.CheckedChanged += new System.EventHandler(this.chkPartialClass_CheckedChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 338);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(680, 22);
            this.statusStrip.TabIndex = 34;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // lblEFAnnotations
            // 
            this.lblEFAnnotations.AutoSize = true;
            this.lblEFAnnotations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblEFAnnotations.Location = new System.Drawing.Point(286, 193);
            this.lblEFAnnotations.Name = "lblEFAnnotations";
            this.lblEFAnnotations.Size = new System.Drawing.Size(154, 13);
            this.lblEFAnnotations.TabIndex = 0;
            this.lblEFAnnotations.Text = "EF Code-First Annotations";
            // 
            // chkSingular
            // 
            this.chkSingular.AutoSize = true;
            this.chkSingular.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSingular.Location = new System.Drawing.Point(286, 26);
            this.chkSingular.Name = "chkSingular";
            this.chkSingular.Size = new System.Drawing.Size(64, 17);
            this.chkSingular.TabIndex = 24;
            this.chkSingular.Text = "Singular";
            this.chkSingular.UseVisualStyleBackColor = true;
            this.chkSingular.CheckedChanged += new System.EventHandler(this.chkSingular_CheckedChanged);
            // 
            // chkUsing
            // 
            this.chkUsing.AutoSize = true;
            this.chkUsing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUsing.Location = new System.Drawing.Point(177, 99);
            this.chkUsing.Name = "chkUsing";
            this.chkUsing.Size = new System.Drawing.Size(51, 17);
            this.chkUsing.TabIndex = 8;
            this.chkUsing.Text = "using";
            this.chkUsing.UseVisualStyleBackColor = true;
            this.chkUsing.CheckedChanged += new System.EventHandler(this.chkUsing_CheckedChanged);
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(68, 120);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(75, 20);
            this.txtNamespace.TabIndex = 9;
            this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
            // 
            // lblNamespace
            // 
            this.lblNamespace.AutoSize = true;
            this.lblNamespace.Location = new System.Drawing.Point(4, 124);
            this.lblNamespace.Name = "lblNamespace";
            this.lblNamespace.Size = new System.Drawing.Size(64, 13);
            this.lblNamespace.TabIndex = 0;
            this.lblNamespace.Text = "Namespace";
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.AutoSize = true;
            this.btnCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopy.Location = new System.Drawing.Point(631, 5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(41, 23);
            this.btnCopy.TabIndex = 51;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtSchemaSeparator
            // 
            this.txtSchemaSeparator.Location = new System.Drawing.Point(619, 74);
            this.txtSchemaSeparator.Name = "txtSchemaSeparator";
            this.txtSchemaSeparator.Size = new System.Drawing.Size(45, 20);
            this.txtSchemaSeparator.TabIndex = 29;
            this.txtSchemaSeparator.TextChanged += new System.EventHandler(this.txtSchemaSeparator_TextChanged);
            // 
            // lblSchemaSeparator
            // 
            this.lblSchemaSeparator.AutoSize = true;
            this.lblSchemaSeparator.Location = new System.Drawing.Point(524, 78);
            this.lblSchemaSeparator.Name = "lblSchemaSeparator";
            this.lblSchemaSeparator.Size = new System.Drawing.Size(95, 13);
            this.lblSchemaSeparator.TabIndex = 0;
            this.lblSchemaSeparator.Text = "Schema Separator";
            // 
            // chkIgnoreDboSchema
            // 
            this.chkIgnoreDboSchema.AutoSize = true;
            this.chkIgnoreDboSchema.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIgnoreDboSchema.Location = new System.Drawing.Point(397, 76);
            this.chkIgnoreDboSchema.Name = "chkIgnoreDboSchema";
            this.chkIgnoreDboSchema.Size = new System.Drawing.Size(119, 17);
            this.chkIgnoreDboSchema.TabIndex = 28;
            this.chkIgnoreDboSchema.Text = "Ignore dbo Schema";
            this.chkIgnoreDboSchema.UseVisualStyleBackColor = true;
            this.chkIgnoreDboSchema.CheckedChanged += new System.EventHandler(this.chkIgnoreDboSchema_CheckedChanged);
            // 
            // chkIncludeSchema
            // 
            this.chkIncludeSchema.AutoSize = true;
            this.chkIncludeSchema.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeSchema.Location = new System.Drawing.Point(286, 76);
            this.chkIncludeSchema.Name = "chkIncludeSchema";
            this.chkIncludeSchema.Size = new System.Drawing.Size(103, 17);
            this.chkIncludeSchema.TabIndex = 27;
            this.chkIncludeSchema.Text = "Include Schema";
            this.chkIncludeSchema.UseVisualStyleBackColor = true;
            this.chkIncludeSchema.CheckedChanged += new System.EventHandler(this.chkIncludeSchema_CheckedChanged);
            // 
            // txtDBSeparator
            // 
            this.txtDBSeparator.Location = new System.Drawing.Point(444, 49);
            this.txtDBSeparator.Name = "txtDBSeparator";
            this.txtDBSeparator.Size = new System.Drawing.Size(45, 20);
            this.txtDBSeparator.TabIndex = 26;
            this.txtDBSeparator.TextChanged += new System.EventHandler(this.txtDBSeparator_TextChanged);
            // 
            // lblDBSeparator
            // 
            this.lblDBSeparator.AutoSize = true;
            this.lblDBSeparator.Location = new System.Drawing.Point(373, 53);
            this.lblDBSeparator.Name = "lblDBSeparator";
            this.lblDBSeparator.Size = new System.Drawing.Size(71, 13);
            this.lblDBSeparator.TabIndex = 0;
            this.lblDBSeparator.Text = "DB Separator";
            // 
            // chkIncludeDB
            // 
            this.chkIncludeDB.AutoSize = true;
            this.chkIncludeDB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeDB.Location = new System.Drawing.Point(286, 51);
            this.chkIncludeDB.Name = "chkIncludeDB";
            this.chkIncludeDB.Size = new System.Drawing.Size(79, 17);
            this.chkIncludeDB.TabIndex = 25;
            this.chkIncludeDB.Text = "Include DB";
            this.chkIncludeDB.UseVisualStyleBackColor = true;
            this.chkIncludeDB.CheckedChanged += new System.EventHandler(this.chkIncludeDB_CheckedChanged);
            // 
            // lblFixedName
            // 
            this.lblFixedName.AutoSize = true;
            this.lblFixedName.Location = new System.Drawing.Point(286, 172);
            this.lblFixedName.Name = "lblFixedName";
            this.lblFixedName.Size = new System.Drawing.Size(63, 13);
            this.lblFixedName.TabIndex = 0;
            this.lblFixedName.Text = "Fixed Name";
            // 
            // lblWordsSeparatorDesc
            // 
            this.lblWordsSeparatorDesc.AutoSize = true;
            this.lblWordsSeparatorDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblWordsSeparatorDesc.Location = new System.Drawing.Point(426, 101);
            this.lblWordsSeparatorDesc.Name = "lblWordsSeparatorDesc";
            this.lblWordsSeparatorDesc.Size = new System.Drawing.Size(214, 13);
            this.lblWordsSeparatorDesc.TabIndex = 0;
            this.lblWordsSeparatorDesc.Text = "(Words between _ and words in CamelCase)";
            // 
            // btnTypeMapping
            // 
            this.btnTypeMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTypeMapping.AutoSize = true;
            this.btnTypeMapping.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTypeMapping.Location = new System.Drawing.Point(538, 310);
            this.btnTypeMapping.Name = "btnTypeMapping";
            this.btnTypeMapping.Size = new System.Drawing.Size(85, 23);
            this.btnTypeMapping.TabIndex = 53;
            this.btnTypeMapping.Text = "Type Mapping";
            this.btnTypeMapping.UseVisualStyleBackColor = true;
            this.btnTypeMapping.Click += new System.EventHandler(this.btnTypeMapping_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(629, 310);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(43, 23);
            this.btnClose.TabIndex = 54;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = true;
            this.btnExport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExport.Location = new System.Drawing.Point(4, 310);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(47, 23);
            this.btnExport.TabIndex = 23;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.AutoSize = true;
            this.btnFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFolder.Location = new System.Drawing.Point(4, 261);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(46, 23);
            this.btnFolder.TabIndex = 19;
            this.btnFolder.Text = "Folder";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // chkSingleFile
            // 
            this.chkSingleFile.AutoSize = true;
            this.chkSingleFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSingleFile.Location = new System.Drawing.Point(4, 289);
            this.chkSingleFile.Name = "chkSingleFile";
            this.chkSingleFile.Size = new System.Drawing.Size(94, 17);
            this.chkSingleFile.TabIndex = 21;
            this.chkSingleFile.Text = "Append to File";
            this.chkSingleFile.UseVisualStyleBackColor = true;
            this.chkSingleFile.CheckedChanged += new System.EventHandler(this.chkSingleFile_CheckedChanged);
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(106, 287);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(156, 20);
            this.txtFileName.TabIndex = 22;
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(58, 262);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(204, 20);
            this.txtFolder.TabIndex = 20;
            // 
            // lblExport
            // 
            this.lblExport.AutoSize = true;
            this.lblExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblExport.Location = new System.Drawing.Point(4, 241);
            this.lblExport.Name = "lblExport";
            this.lblExport.Size = new System.Drawing.Size(88, 13);
            this.lblExport.TabIndex = 0;
            this.lblExport.Text = "Export to Files";
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(581, 168);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(75, 20);
            this.txtSuffix.TabIndex = 39;
            this.txtSuffix.TextChanged += new System.EventHandler(this.txtSuffix_TextChanged);
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(548, 172);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(33, 13);
            this.lblSuffix.TabIndex = 0;
            this.lblSuffix.Text = "Suffix";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(465, 168);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(75, 20);
            this.txtPrefix.TabIndex = 38;
            this.txtPrefix.TextChanged += new System.EventHandler(this.txtPrefix_TextChanged);
            // 
            // lblPrefix
            // 
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(432, 172);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(33, 13);
            this.lblPrefix.TabIndex = 0;
            this.lblPrefix.Text = "Prefix";
            // 
            // txtWordsSeparator
            // 
            this.txtWordsSeparator.Location = new System.Drawing.Point(373, 97);
            this.txtWordsSeparator.Name = "txtWordsSeparator";
            this.txtWordsSeparator.Size = new System.Drawing.Size(45, 20);
            this.txtWordsSeparator.TabIndex = 30;
            this.txtWordsSeparator.TextChanged += new System.EventHandler(this.txtWordsSeparator_TextChanged);
            // 
            // lblWordsSeparator
            // 
            this.lblWordsSeparator.AutoSize = true;
            this.lblWordsSeparator.Location = new System.Drawing.Point(286, 101);
            this.lblWordsSeparator.Name = "lblWordsSeparator";
            this.lblWordsSeparator.Size = new System.Drawing.Size(87, 13);
            this.lblWordsSeparator.TabIndex = 0;
            this.lblWordsSeparator.Text = "Words Separator";
            // 
            // chkLowerCase
            // 
            this.chkLowerCase.AutoSize = true;
            this.chkLowerCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLowerCase.Location = new System.Drawing.Point(475, 122);
            this.chkLowerCase.Name = "chkLowerCase";
            this.chkLowerCase.Size = new System.Drawing.Size(77, 17);
            this.chkLowerCase.TabIndex = 33;
            this.chkLowerCase.Text = "lower case";
            this.chkLowerCase.UseVisualStyleBackColor = true;
            this.chkLowerCase.CheckedChanged += new System.EventHandler(this.chkLowerCase_CheckedChanged);
            // 
            // chkUpperCase
            // 
            this.chkUpperCase.AutoSize = true;
            this.chkUpperCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUpperCase.Location = new System.Drawing.Point(373, 122);
            this.chkUpperCase.Name = "chkUpperCase";
            this.chkUpperCase.Size = new System.Drawing.Size(94, 17);
            this.chkUpperCase.TabIndex = 32;
            this.chkUpperCase.Text = "UPPER CASE";
            this.chkUpperCase.UseVisualStyleBackColor = true;
            this.chkUpperCase.CheckedChanged += new System.EventHandler(this.chkUpperCase_CheckedChanged);
            // 
            // chkCamelCase
            // 
            this.chkCamelCase.AutoSize = true;
            this.chkCamelCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCamelCase.Location = new System.Drawing.Point(286, 122);
            this.chkCamelCase.Name = "chkCamelCase";
            this.chkCamelCase.Size = new System.Drawing.Size(79, 17);
            this.chkCamelCase.TabIndex = 31;
            this.chkCamelCase.Text = "CamelCase";
            this.chkCamelCase.UseVisualStyleBackColor = true;
            this.chkCamelCase.CheckedChanged += new System.EventHandler(this.chkCamelCase_CheckedChanged);
            // 
            // lblPOCO
            // 
            this.lblPOCO.AutoSize = true;
            this.lblPOCO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblPOCO.Location = new System.Drawing.Point(4, 5);
            this.lblPOCO.Name = "lblPOCO";
            this.lblPOCO.Size = new System.Drawing.Size(41, 13);
            this.lblPOCO.TabIndex = 0;
            this.lblPOCO.Text = "POCO";
            // 
            // chkCommentsWithoutNull
            // 
            this.chkCommentsWithoutNull.AutoSize = true;
            this.chkCommentsWithoutNull.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCommentsWithoutNull.Location = new System.Drawing.Point(87, 99);
            this.chkCommentsWithoutNull.Name = "chkCommentsWithoutNull";
            this.chkCommentsWithoutNull.Size = new System.Drawing.Size(82, 17);
            this.chkCommentsWithoutNull.TabIndex = 7;
            this.chkCommentsWithoutNull.Text = "Without null";
            this.chkCommentsWithoutNull.UseVisualStyleBackColor = true;
            this.chkCommentsWithoutNull.CheckedChanged += new System.EventHandler(this.chkCommentsWithoutNull_CheckedChanged);
            // 
            // chkComments
            // 
            this.chkComments.AutoSize = true;
            this.chkComments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkComments.Location = new System.Drawing.Point(4, 99);
            this.chkComments.Name = "chkComments";
            this.chkComments.Size = new System.Drawing.Size(75, 17);
            this.chkComments.TabIndex = 6;
            this.chkComments.Text = "Comments";
            this.chkComments.UseVisualStyleBackColor = true;
            this.chkComments.CheckedChanged += new System.EventHandler(this.chkComments_CheckedChanged);
            // 
            // txtFixedClassName
            // 
            this.txtFixedClassName.Location = new System.Drawing.Point(349, 168);
            this.txtFixedClassName.Name = "txtFixedClassName";
            this.txtFixedClassName.Size = new System.Drawing.Size(75, 20);
            this.txtFixedClassName.TabIndex = 37;
            this.txtFixedClassName.TextChanged += new System.EventHandler(this.txtFixedClassName_TextChanged);
            // 
            // chkAllStructNullable
            // 
            this.chkAllStructNullable.AutoSize = true;
            this.chkAllStructNullable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllStructNullable.Location = new System.Drawing.Point(95, 76);
            this.chkAllStructNullable.Name = "chkAllStructNullable";
            this.chkAllStructNullable.Size = new System.Drawing.Size(127, 17);
            this.chkAllStructNullable.TabIndex = 5;
            this.chkAllStructNullable.Text = "Struct Types Nullable";
            this.chkAllStructNullable.UseVisualStyleBackColor = true;
            this.chkAllStructNullable.CheckedChanged += new System.EventHandler(this.chkAllStructNullable_CheckedChanged);
            // 
            // chkVirtualProperties
            // 
            this.chkVirtualProperties.AutoSize = true;
            this.chkVirtualProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkVirtualProperties.Location = new System.Drawing.Point(4, 51);
            this.chkVirtualProperties.Name = "chkVirtualProperties";
            this.chkVirtualProperties.Size = new System.Drawing.Size(105, 17);
            this.chkVirtualProperties.TabIndex = 2;
            this.chkVirtualProperties.Text = "Virtual Properties";
            this.chkVirtualProperties.UseVisualStyleBackColor = true;
            this.chkVirtualProperties.CheckedChanged += new System.EventHandler(this.chkVirtualProperties_CheckedChanged);
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblClassName.Location = new System.Drawing.Point(286, 5);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(73, 13);
            this.lblClassName.TabIndex = 0;
            this.lblClassName.Text = "Class Name";
            // 
            // contextMenuTable
            // 
            this.contextMenuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkTablesConnectedFromToolStripMenuItem,
            this.checkTablesConnectedToToolStripMenuItem,
            this.checkTablesConnectedToolStripMenuItem,
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem,
            this.checkRecursivelyTablesConnectedToToolStripMenuItem,
            this.checkRecursivelyTablesConnectedToolStripMenuItem,
            this.refreshTableToolStripMenuItem});
            this.contextMenuTable.Name = "contextMenuTable";
            this.contextMenuTable.Size = new System.Drawing.Size(349, 158);
            // 
            // checkTablesConnectedFromToolStripMenuItem
            // 
            this.checkTablesConnectedFromToolStripMenuItem.Name = "checkTablesConnectedFromToolStripMenuItem";
            this.checkTablesConnectedFromToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkTablesConnectedFromToolStripMenuItem.Text = "Check Connected From This Table (FK This -> To)";
            this.checkTablesConnectedFromToolStripMenuItem.Click += new System.EventHandler(this.checkTablesConnectedFromToolStripMenuItem_Click);
            // 
            // checkTablesConnectedToToolStripMenuItem
            // 
            this.checkTablesConnectedToToolStripMenuItem.Name = "checkTablesConnectedToToolStripMenuItem";
            this.checkTablesConnectedToToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkTablesConnectedToToolStripMenuItem.Text = "Check Connected To This Table (FK From -> This)";
            this.checkTablesConnectedToToolStripMenuItem.Click += new System.EventHandler(this.checkTablesConnectedToToolStripMenuItem_Click);
            // 
            // checkTablesConnectedToolStripMenuItem
            // 
            this.checkTablesConnectedToolStripMenuItem.Name = "checkTablesConnectedToolStripMenuItem";
            this.checkTablesConnectedToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkTablesConnectedToolStripMenuItem.Text = "Check Connected From && To This Table";
            this.checkTablesConnectedToolStripMenuItem.Click += new System.EventHandler(this.checkTablesConnectedToolStripMenuItem_Click);
            // 
            // checkRecursivelyTablesConnectedFromToolStripMenuItem
            // 
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem.Name = "checkRecursivelyTablesConnectedFromToolStripMenuItem";
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem.Text = "Check Recursively Connected From This Table";
            this.checkRecursivelyTablesConnectedFromToolStripMenuItem.Click += new System.EventHandler(this.checkRecursivelyTablesConnectedFromToolStripMenuItem_Click);
            // 
            // checkRecursivelyTablesConnectedToToolStripMenuItem
            // 
            this.checkRecursivelyTablesConnectedToToolStripMenuItem.Name = "checkRecursivelyTablesConnectedToToolStripMenuItem";
            this.checkRecursivelyTablesConnectedToToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkRecursivelyTablesConnectedToToolStripMenuItem.Text = "Check Recursively Connected To This Table";
            this.checkRecursivelyTablesConnectedToToolStripMenuItem.Click += new System.EventHandler(this.checkRecursivelyTablesConnectedToToolStripMenuItem_Click);
            // 
            // checkRecursivelyTablesConnectedToolStripMenuItem
            // 
            this.checkRecursivelyTablesConnectedToolStripMenuItem.Name = "checkRecursivelyTablesConnectedToolStripMenuItem";
            this.checkRecursivelyTablesConnectedToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.checkRecursivelyTablesConnectedToolStripMenuItem.Text = "Check Recursively Connected From && To This Table";
            this.checkRecursivelyTablesConnectedToolStripMenuItem.Click += new System.EventHandler(this.checkRecursivelyTablesConnectedToolStripMenuItem_Click);
            // 
            // refreshTableToolStripMenuItem
            // 
            this.refreshTableToolStripMenuItem.Name = "refreshTableToolStripMenuItem";
            this.refreshTableToolStripMenuItem.Size = new System.Drawing.Size(348, 22);
            this.refreshTableToolStripMenuItem.Text = "Refresh";
            this.refreshTableToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // POCOGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1084, 712);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(1100, 750);
            this.Name = "POCOGeneratorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POCO Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POCOGeneratorForm_FormClosing);
            this.Load += new System.EventHandler(this.POCOGeneratorForm_Load);
            this.Shown += new System.EventHandler(this.POCOGeneratorForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuPocoEditor.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelNavigationProperties1.ResumeLayout(false);
            this.panelNavigationProperties1.PerformLayout();
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextMenuTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView trvServer;
        private System.Windows.Forms.ImageList imageListDbObjects;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox txtPocoEditor;
        private System.Windows.Forms.ContextMenuStrip contextMenuPocoEditor;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdbProperties;
        private System.Windows.Forms.RadioButton rdbDataMembers;
        private System.Windows.Forms.CheckBox chkVirtualProperties;
        private System.Windows.Forms.CheckBox chkAllStructNullable;
        private System.Windows.Forms.CheckBox chkComments;
        private System.Windows.Forms.CheckBox chkCommentsWithoutNull;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.TextBox txtFixedClassName;
        private System.Windows.Forms.CheckBox chkLowerCase;
        private System.Windows.Forms.CheckBox chkUpperCase;
        private System.Windows.Forms.CheckBox chkCamelCase;
        private System.Windows.Forms.TextBox txtWordsSeparator;
        private System.Windows.Forms.Label lblWordsSeparator;
        private System.Windows.Forms.Label lblPOCO;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.CheckBox chkSingleFile;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label lblExport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnTypeMapping;
        private System.Windows.Forms.Label lblWordsSeparatorDesc;
        private System.Windows.Forms.Label lblFixedName;
        private System.Windows.Forms.TextBox txtDBSeparator;
        private System.Windows.Forms.Label lblDBSeparator;
        private System.Windows.Forms.CheckBox chkIncludeDB;
        private System.Windows.Forms.TextBox txtSchemaSeparator;
        private System.Windows.Forms.Label lblSchemaSeparator;
        private System.Windows.Forms.CheckBox chkIgnoreDboSchema;
        private System.Windows.Forms.CheckBox chkIncludeSchema;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label lblNamespace;
        private System.Windows.Forms.CheckBox chkUsing;
        private System.Windows.Forms.CheckBox chkSingular;
        private System.Windows.Forms.Label lblEFAnnotations;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.CheckBox chkPartialClass;
        private System.Windows.Forms.CheckBox chkEF;
        private System.Windows.Forms.CheckBox chkEFRequired;
        private System.Windows.Forms.CheckBox chkEFColumn;
        private System.Windows.Forms.Label lblEFAnnotationsTables;
        private System.Windows.Forms.Label lblSingularDesc;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterSettingsToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSearchIgnoreCase;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox chkEFDisplay;
        private System.Windows.Forms.CheckBox chkEFStringLength;
        private System.Windows.Forms.CheckBox chkEFConcurrencyCheck;
        private System.Windows.Forms.CheckBox chkNewLineBetweenMembers;
        private System.Windows.Forms.CheckBox chkEFRequiredWithErrorMessage;
        private System.Windows.Forms.Label lblEF;
        private System.Windows.Forms.CheckBox chkEFComplexType;
        private System.Windows.Forms.CheckBox chkEFIndex;
        private System.Windows.Forms.ToolStripMenuItem clearCheckBoxesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuTable;
        private System.Windows.Forms.ToolStripMenuItem checkTablesConnectedFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkTablesConnectedToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkTablesConnectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkRecursivelyTablesConnectedFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkRecursivelyTablesConnectedToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkRecursivelyTablesConnectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshTableToolStripMenuItem;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.CheckBox chkNavigationProperties;
        private System.Windows.Forms.CheckBox chkNavigationPropertiesVirtual;
        private System.Windows.Forms.Panel panelNavigationProperties1;
        private System.Windows.Forms.RadioButton rdbNavigationPropertiesIEnumerable;
        private System.Windows.Forms.RadioButton rdbNavigationPropertiesICollection;
        private System.Windows.Forms.RadioButton rdbNavigationPropertiesList;
        private System.Windows.Forms.CheckBox chkEFForeignKey;
        private System.Windows.Forms.CheckBox chkNavigationPropertiesComments;
        private System.Windows.Forms.CheckBox chkNavigationPropertiesShowJoinTable;
        private System.Windows.Forms.Button btnCommandLine;
        private System.Windows.Forms.TextBox txtInherit;
        private System.Windows.Forms.Label lblInherit;
        private System.Windows.Forms.CheckBox chkEFDescription;
        private System.Windows.Forms.CheckBox chkOverrideProperties;
        private System.Windows.Forms.CheckBox chkNavigationPropertiesOverride;
        private System.Windows.Forms.CheckBox chkColumnDefaults;
    }
}