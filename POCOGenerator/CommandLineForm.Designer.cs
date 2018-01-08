namespace POCOGenerator
{
    partial class CommandLineForm
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
            this.txtCommandLineEditor = new System.Windows.Forms.RichTextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnToggleSwitchesSize = new System.Windows.Forms.Button();
            this.btnSaveBatFile = new System.Windows.Forms.Button();
            this.txtHelpEditor = new System.Windows.Forms.RichTextBox();
            this.groupBoxHelp = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelButtons.SuspendLayout();
            this.groupBoxHelp.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCommandLineEditor
            // 
            this.txtCommandLineEditor.BackColor = System.Drawing.Color.Black;
            this.txtCommandLineEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandLineEditor.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCommandLineEditor.ForeColor = System.Drawing.Color.White;
            this.txtCommandLineEditor.Location = new System.Drawing.Point(3, 3);
            this.txtCommandLineEditor.Name = "txtCommandLineEditor";
            this.txtCommandLineEditor.Size = new System.Drawing.Size(578, 147);
            this.txtCommandLineEditor.TabIndex = 0;
            this.txtCommandLineEditor.Text = "";
            // 
            // btnCopy
            // 
            this.btnCopy.AutoSize = true;
            this.btnCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopy.Location = new System.Drawing.Point(3, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(41, 23);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSize = true;
            this.panelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelButtons.Controls.Add(this.btnToggleSwitchesSize);
            this.panelButtons.Controls.Add(this.btnSaveBatFile);
            this.panelButtons.Controls.Add(this.btnCopy);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(3, 156);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(578, 29);
            this.panelButtons.TabIndex = 2;
            // 
            // btnToggleSwitchesSize
            // 
            this.btnToggleSwitchesSize.AutoSize = true;
            this.btnToggleSwitchesSize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnToggleSwitchesSize.Location = new System.Drawing.Point(52, 3);
            this.btnToggleSwitchesSize.Name = "btnToggleSwitchesSize";
            this.btnToggleSwitchesSize.Size = new System.Drawing.Size(88, 23);
            this.btnToggleSwitchesSize.TabIndex = 2;
            this.btnToggleSwitchesSize.Text = "Short Switches";
            this.btnToggleSwitchesSize.UseVisualStyleBackColor = true;
            this.btnToggleSwitchesSize.Click += new System.EventHandler(this.btnToggleSwitchesSize_Click);
            // 
            // btnSaveBatFile
            // 
            this.btnSaveBatFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveBatFile.AutoSize = true;
            this.btnSaveBatFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveBatFile.Location = new System.Drawing.Point(423, 3);
            this.btnSaveBatFile.Name = "btnSaveBatFile";
            this.btnSaveBatFile.Size = new System.Drawing.Size(152, 23);
            this.btnSaveBatFile.TabIndex = 3;
            this.btnSaveBatFile.Text = "Save to POCOGenerator.bat";
            this.btnSaveBatFile.UseVisualStyleBackColor = true;
            this.btnSaveBatFile.Click += new System.EventHandler(this.btnSaveBatFile_Click);
            // 
            // txtHelpEditor
            // 
            this.txtHelpEditor.BackColor = System.Drawing.Color.Black;
            this.txtHelpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelpEditor.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtHelpEditor.ForeColor = System.Drawing.Color.White;
            this.txtHelpEditor.Location = new System.Drawing.Point(3, 19);
            this.txtHelpEditor.Name = "txtHelpEditor";
            this.txtHelpEditor.ReadOnly = true;
            this.txtHelpEditor.Size = new System.Drawing.Size(572, 276);
            this.txtHelpEditor.TabIndex = 4;
            this.txtHelpEditor.Text = "";
            // 
            // groupBoxHelp
            // 
            this.groupBoxHelp.Controls.Add(this.txtHelpEditor);
            this.groupBoxHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHelp.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxHelp.Location = new System.Drawing.Point(3, 191);
            this.groupBoxHelp.Name = "groupBoxHelp";
            this.groupBoxHelp.Size = new System.Drawing.Size(578, 298);
            this.groupBoxHelp.TabIndex = 4;
            this.groupBoxHelp.TabStop = false;
            this.groupBoxHelp.Text = "Help";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.txtCommandLineEditor, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panelButtons, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.groupBoxHelp, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.5F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(584, 492);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // CommandLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 492);
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 530);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 530);
            this.Name = "CommandLineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Command Line";
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.groupBoxHelp.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtCommandLineEditor;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.RichTextBox txtHelpEditor;
        private System.Windows.Forms.GroupBox groupBoxHelp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnSaveBatFile;
        private System.Windows.Forms.Button btnToggleSwitchesSize;
    }
}