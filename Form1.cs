using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;
using IWshRuntimeLibrary;
using projectExplorer.Properties;
using projectExplorer.utility;

namespace projectExplorer
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: events and methods de Form1
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HideErrorLink();
            InterfaceText();
            var settingsPath = SettingsUtility.GetPathByFile(); // Pick up the route saved in the configuration folder
            //In the case of empty text, it collects it from My Documents
            txtBxParentFolder.Text = string.IsNullOrEmpty(settingsPath) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : settingsPath;
            Reload();
        }

        #region events
        /// <summary>
        /// When clicking the button, it searches for a folder to put it as the root of the tree
        /// </summary>
        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            if (searchFolderDialog.ShowDialog() != DialogResult.OK) return; // Find a folder
            txtBxParentFolder.Text = searchFolderDialog.SelectedPath; // Assign a path to the text field
            Reload();
        }

        /// <summary>
        /// Click to reload
        /// </summary>
        private void btnApplyPath_Click(object sender, EventArgs e)
        {
            Reload();
        }

        /// <summary>
        /// Double-clicking a node to open it
        /// </summary>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var newSelected = e.Node;
            switch (newSelected.Tag)
            {
                case IWshShortcut lnk:
                    FileUtility.OpenFile(new DirectoryInfo(lnk.FullName));
                    break;
                case FileSystemInfo dirInfo:
                    FileUtility.OpenFile(dirInfo);
                    break;
            }
        }

        /// <summary>
        /// An event for when the text changes, it reloads the interface with the new information
        /// </summary>
        private void txtBxParentFolder_TextChanged(object sender, EventArgs e)
        {
            Reload();
        }

        /// <summary>
        /// An event to: create a new project in the root folder of the node
        /// </summary>
        private void btCreateProject_Click(object sender, EventArgs e)
        {
            CreateProject();
        }

        /// <summary>
        /// An event that fires after selecting a node
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            HideErrorLink();
            var newSelected = e.Node.Tag;
            dataGridView1.Rows.Clear();

            switch (newSelected)
            {
                case IWshShortcut lnk:
                    ShowErrorLink(lnk.TargetPath, lnk);
                    return;
                case string _:
                    return;
                case FileSystemInfo dirInfo:
                    ShowPermissions(dirInfo);
                    break;
            }
        }

        /// <summary>
        /// An event that fires after the node is expanded
        /// </summary>
        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ReloadChildrenOfNode(e.Node);
        }

        /// <summary>
        /// Solution 1 for corrupted link, recreate the folder and all its children.
        /// </summary>
        private void btnReCreateFolder_Click(object sender, EventArgs e)
        {
            var pathXml = SelectXml(txtBxParentFolder.Text);
            if (string.IsNullOrWhiteSpace(pathXml)) return;

            var projectName = MessageUtility.InputBox_GetNameNewProject();
            if (string.IsNullOrWhiteSpace(projectName)) return;

            var shortcut = ((Button)sender).Tag as IWshShortcut;

            try
            {
                new XmlInterpreter(projectName).RecreateFolder(shortcut.FullName, pathXml);
                Reload();

            }
            catch (ExceptionXmlNodeNotFound)
            {
                MessageUtility.Error_XmlNodeNotFound();
            }
        }

        /// <summary>
        /// Solution 2 for broken link, points to another folder
        /// </summary>
        private void btnTagAnotherFolder_Click(object sender, EventArgs e)
        {
            
            var path = ((Button)sender).Tag as IWshShortcut;

            searchFolderDialog.SelectedPath = path.TargetPath.Substring(0, path.TargetPath.LastIndexOf('\\'));

            if (searchFolderDialog.ShowDialog() != DialogResult.OK) return;

            path.TargetPath = searchFolderDialog.SelectedPath;
            path.Save();
            Reload();
        }
        #endregion

        #region methods
        /// <summary>
        /// Create a project
        /// </summary>
        private void CreateProject()
        {
            // Select a project name
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

            var xml = new XmlInterpreter(SelectXml(txtBxParentFolder.Text), txtBxParentFolder.Text + "\\" + projectName);

            try { xml.Interpreter(); }
            catch (ExceptionXmlNotRoot) { MessageUtility.Error_XMLNotRoot(); return;}
            catch (ExceptionXmlHasNotChildNodes) { MessageUtility.Error_XmlInterpreter(); return;}
            catch { MessageUtility.Error_XMLHasNotChildNodes(); return; }
            Reload();
        }

        /// <summary>
        /// Selecting an XML file from the project root folder,
        /// in case of more than one allows you to choose between them.
        /// </summary>
        /// <param name="path">Root folder path</param>
        /// <returns>Path of the selected file or an empty text in case nothing is found</returns>
        private static string SelectXml(string path)
        {
            var files = new DirectoryInfo(path).GetFiles();
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
                    selectXml = FileUtility.OpenFileDialog_SelectXML(path);
                    break;
            }
            if (selectXml == null)
            {
                MessageUtility.Error_XMLNotFound(path);
                return "";
            }
            return selectXml.FullName;

        }

        /// <summary>
        /// Assigns the text of the program resources to be able to have metalanguage.
        /// </summary>
        private void InterfaceText()
        {
            btCreateProject.Text = Resources.Form1_btCreateProject;
            btnReloadPath.Text = Resources.Form1_btnReloadPath;
            lblParentFolder.Text = Resources.Form1_lblParentFolder;
            btnSearchFolder.Text = Resources.Form1_btnSearchFolder;
            ClGrup.HeaderText = Resources.Form1_chGroup;
            clPermissions.HeaderText = Resources.Form1_chPermissions;
            Text = Resources.Form1_title;
            btnReCreateFolder.Text = Resources.Form1_btnReCreateFolder;
            btnTagAnotherFolder.Text = Resources.Form1_btnTagAnotherFolder;
            lbErrorLink.Text = Resources.Form1_lbErrorLink;
        }

        /// <summary>
        /// Show dirInfo permissions.
        /// </summary>
        /// <param name="dirInfo">Corresponding folder or document </param>
        private void ShowPermissions(FileSystemInfo dirInfo)
        {
            dataGridView1.Rows.Clear();

            var permissions = PermissionsUtility.GetPermissions(dirInfo.FullName);
            foreach (var permission in permissions)
            {
                var permit = PermissionsUtility.PermissionTypeInterpretation(permission[1]);
                dataGridView1.Rows.Add(permission[0], permit);
            }
        }

        /// <summary>
        /// Hide buttons and error information from corrupted shortcut.
        /// </summary>
        private void HideErrorLink()
        {
            tableLayoutPanel2.RowStyles[1].Height = 0;
        }

        /// <summary>
        /// Show buttons and shortcut error information.
        /// When there is a corrupted link you have two options: recreate the folder or point to another folder.
        /// </summary>
        /// <param name="error">path of the failed route</param>
        /// <param name="lnk">Direct access</param>
        private void ShowErrorLink(string error, IWshShortcut lnk)
        {
            lbErrorLink.Text = Resources.ResourceManager.GetString("Form1_lbErrorLink") + error;
            btnReCreateFolder.Tag = lnk;
            btnTagAnotherFolder.Tag = lnk;
            tableLayoutPanel2.RowStyles[1].Height = 60;
        }
        #endregion
    }
}