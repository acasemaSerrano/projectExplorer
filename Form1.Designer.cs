using System.Windows.Forms;

namespace projectExplorer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.searchFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnSearchFolder = new System.Windows.Forms.Button();
            this.btnReloadPath = new System.Windows.Forms.Button();
            this.lblParentFolder = new System.Windows.Forms.Label();
            this.btCreateProject = new System.Windows.Forms.Button();
            this.txtBxParentFolder = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ClGrup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clPermissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // searchFolderDialog
            // 
            this.searchFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "descarga (1).jpg");
            this.imageList1.Images.SetKeyName(1, "File_alt_font_awesome.svg.png");
            this.imageList1.Images.SetKeyName(2, "descarga (3).jpg");
            // 
            // treeView1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.treeView1, 2);
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 33);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(335, 494);
            this.treeView1.TabIndex = 5;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // btnSearchFolder
            // 
            this.btnSearchFolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearchFolder.Location = new System.Drawing.Point(344, 3);
            this.btnSearchFolder.Name = "btnSearchFolder";
            this.btnSearchFolder.Size = new System.Drawing.Size(75, 24);
            this.btnSearchFolder.TabIndex = 2;
            this.btnSearchFolder.UseVisualStyleBackColor = true;
            this.btnSearchFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnReloadPath
            // 
            this.btnReloadPath.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReloadPath.Location = new System.Drawing.Point(425, 3);
            this.btnReloadPath.Name = "btnReloadPath";
            this.btnReloadPath.Size = new System.Drawing.Size(75, 24);
            this.btnReloadPath.TabIndex = 3;
            this.btnReloadPath.UseVisualStyleBackColor = true;
            this.btnReloadPath.Click += new System.EventHandler(this.btnApplyPath_Click);
            // 
            // lblParentFolder
            // 
            this.lblParentFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParentFolder.Location = new System.Drawing.Point(3, 0);
            this.lblParentFolder.Name = "lblParentFolder";
            this.lblParentFolder.Size = new System.Drawing.Size(159, 30);
            this.lblParentFolder.TabIndex = 2;
            this.lblParentFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btCreateProject
            // 
            this.btCreateProject.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCreateProject.Location = new System.Drawing.Point(506, 3);
            this.btCreateProject.Name = "btCreateProject";
            this.btCreateProject.Size = new System.Drawing.Size(75, 24);
            this.btCreateProject.TabIndex = 4;
            this.btCreateProject.UseVisualStyleBackColor = true;
            this.btCreateProject.Click += new System.EventHandler(this.btCreateProject_Click);
            // 
            // txtBxParentFolder
            // 
            this.txtBxParentFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBxParentFolder.Location = new System.Drawing.Point(168, 5);
            this.txtBxParentFolder.Name = "txtBxParentFolder";
            this.txtBxParentFolder.Size = new System.Drawing.Size(170, 20);
            this.txtBxParentFolder.TabIndex = 1;
            this.txtBxParentFolder.TextChanged += new System.EventHandler(this.txtBxParentFolder_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblParentFolder, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btCreateProject, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.treeView1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtBxParentFolder, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSearchFolder, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnReloadPath, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 530);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.CausesValidation = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.ClGrup, this.clPermissions });
            this.tableLayoutPanel2.SetColumnSpan(this.dataGridView1, 3);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(344, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(237, 494);
            this.dataGridView1.TabIndex = 6;
            // 
            // ClGrup
            // 
            this.ClGrup.Frozen = true;
            this.ClGrup.HeaderText = "Grup";
            this.ClGrup.Name = "ClGrup";
            this.ClGrup.ReadOnly = true;
            // 
            // clPermissions
            // 
            this.clPermissions.Frozen = true;
            this.clPermissions.HeaderText = "Permissions";
            this.clPermissions.Name = "clPermissions";
            this.clPermissions.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(584, 530);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Location = new System.Drawing.Point(15, 15);
            this.MinimumSize = new System.Drawing.Size(600, 100);
            this.Name = "Form1";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridViewTextBoxColumn ClGrup;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPermissions;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

        protected System.Windows.Forms.TextBox txtBxParentFolder;

        private System.Windows.Forms.Button btCreateProject;

        private System.Windows.Forms.Button btnReloadPath;
        private System.Windows.Forms.Button btnSearchFolder;
        private System.Windows.Forms.Label lblParentFolder;

        protected System.Windows.Forms.TreeView treeView1;

        public System.Windows.Forms.ImageList imageList1;

        private System.Windows.Forms.FolderBrowserDialog searchFolderDialog;

        #endregion
    }
}