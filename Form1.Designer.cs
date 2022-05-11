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
            this.listView1 = new System.Windows.Forms.ListView();
            this.chPermissions = new System.Windows.Forms.ColumnHeader();
            this.chGrup = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "descarga (1).jpg");
            this.imageList1.Images.SetKeyName(1, "File_alt_font_awesome.svg.png");
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
            this.tableLayoutPanel2.Controls.Add(this.listView1, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 530);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.chPermissions, this.chGrup });
            this.tableLayoutPanel2.SetColumnSpan(this.listView1, 3);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(344, 33);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(237, 494);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
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
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ColumnHeader chPermissions;
        private System.Windows.Forms.ColumnHeader chGrup;

        private System.Windows.Forms.ListView listView1;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

        private System.Windows.Forms.TextBox txtBxParentFolder;

        private System.Windows.Forms.Button btCreateProject;

        private System.Windows.Forms.Button btnReloadPath;
        private System.Windows.Forms.Button btnSearchFolder;
        private System.Windows.Forms.Label lblParentFolder;

        private System.Windows.Forms.TreeView treeView1;

        public System.Windows.Forms.ImageList imageList1;

        public System.Windows.Forms.FolderBrowserDialog searchFolderDialog;

        #endregion
    }
}