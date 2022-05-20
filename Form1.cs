using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using projectExplorer.Properties;
using projectExplorer.utility;

namespace projectExplorer
{
    /// <summary>
    /// author: acasema
    /// date of creation: I don't remember
    /// 
    /// modified by: acasema
    /// date modified: 16/05/2022
    /// cause: only reload if you have deployed that branch, but only direct children
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InterfaceText();
            var settingsPath = SettingsUtility.GetPathByFile();
            txtBxParentFolder.Text = string.IsNullOrEmpty(settingsPath) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : settingsPath;
            Reload();
        }

        #region events
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (searchFolderDialog.ShowDialog() != DialogResult.OK) return;
            txtBxParentFolder.Text = searchFolderDialog.SelectedPath;
            Reload();
        }

        private void btnApplyPath_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var newSelected = e.Node;
            FileUtility.OpenFile(newSelected.Tag as FileSystemInfo);
        }

        private void txtBxParentFolder_TextChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void btCreateProject_Click(object sender, EventArgs e)
        {
            CreateProject();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var newSelected = e.Node.Tag;

            if (newSelected is FileSystemInfo dirInfo)
                ShowPermissions(dirInfo);
        }
        
        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ReloadChildrenOfNode(e.Node);
        }
        #endregion
        
        #region methods
        private void CreateProject()
        {
            var files = new DirectoryInfo(txtBxParentFolder.Text).GetFiles();
            files = files.Where(x => x.Extension == ".xml").ToArray();
            FileInfo selectXml;

            switch (files.Length)
            {
                case 0:
                    selectXml = null;
                    break;
                case 1:
                    selectXml = files[0];
                    break;
                default:
                    selectXml = FileUtility.OpenFileDialog_SelectXML(txtBxParentFolder.Text);
                    break;
            }

            if (selectXml == null)
            {
                MessageUtility.Error_XMLNotFound(txtBxParentFolder.Text);
                return;
            }

            var projectName = MessageUtility.InputBox_GetNameNewProject();
            if (string.IsNullOrEmpty(projectName))
            {
                MessageUtility.Error_NoName();
                return;
            }
            if (new DirectoryInfo(txtBxParentFolder.Text + "\\" + projectName).Exists)
            { 
                if (!MessageUtility.Notification_ProjectExists()) return;
            }

            var xml = new XmlInterpreter(selectXml.FullName, txtBxParentFolder.Text + "\\" + projectName);

            try { xml.Interpreter(); }
            catch (ExceptionXmlNotRoot) { MessageUtility.Error_XMLNotRoot(); return;}
            catch (ExceptionXmlHasNotChildNodes) { MessageUtility.Error_XmlInterpreter(); return;}
            catch { MessageUtility.Error_XMLHasNotChildNodes(); return; }
            Reload();
        }

        private void InterfaceText()
        {
            btCreateProject.Text = Resources.ResourceManager.GetString("Form1_btCreateProject");
            btnReloadPath.Text = Resources.ResourceManager.GetString("Form1_btnReloadPath");
            lblParentFolder.Text = Resources.ResourceManager.GetString("Form1_lblParentFolder");
            btnSearchFolder.Text = Resources.ResourceManager.GetString("Form1_btnSearchFolder");
            ClGrup.HeaderText = Resources.ResourceManager.GetString("Form1_chGroup");
            clPermissions.HeaderText = Resources.ResourceManager.GetString("Form1_chPermissions");
            Text = Resources.ResourceManager.GetString("Form1_title");
        }
        
        private void ShowPermissions(FileSystemInfo dirInfo)
        {
            dataGridView1.Rows.Clear();

            var permissions = PermissionsUtility.ShowPermissions(dirInfo.FullName);
            foreach (var permission in permissions)
            {
                var permit = PermissionsUtility.PermissionTypeInterpretation(permission[1]);
                dataGridView1.Rows.Add(permission[0], permit);
            }
        }
        
        #endregion


    }
}