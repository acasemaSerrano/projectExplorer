using System;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using IWshRuntimeLibrary;
using Microsoft.VisualBasic;
using projectExplorer.Properties;

namespace projectExplorer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InterfaceText();
            var reader = new AppSettingsReader();
            txtBxParentFolder.Text = reader.GetValue("path", typeof(string)).ToString();
            Reload();

        }

        #region events
        private void button1_Click(object sender, EventArgs e)
        {
            if (searchFolderDialog.ShowDialog() != DialogResult.OK) return;
            txtBxParentFolder.Text = searchFolderDialog.SelectedPath;
            Reload();
            UpdateSettingsPath(txtBxParentFolder.Text);
        }

        private void btnApplyPath_Click(object sender, EventArgs e)
        {
            Reload();
            UpdateSettingsPath(txtBxParentFolder.Text);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var newSelected = e.Node;
            OpenFile(newSelected.Tag);
        }

        private void txtBxParentFolder_TextChanged(object sender, EventArgs e)
        {
            Reload();
            UpdateSettingsPath(txtBxParentFolder.Text);
        }

        private void btCreateProject_Click(object sender, EventArgs e)
        {
            CreateProject();
        }
        #endregion

        #region methods
        private void Reload()
        {
            PopulateTreeView(txtBxParentFolder.Text);
        }

        private void PopulateTreeView(string pathFolder)
        {
            if (string.IsNullOrEmpty(pathFolder)) return;
            var info = new DirectoryInfo(pathFolder);
            if (!info.Exists) return;
            treeView1.Nodes.Clear();


            var rootNode = new TreeNode(info.Name) { Tag = info };

            GetDirectories(info.GetDirectories(), rootNode);

            var files = info.GetFiles();
            if (files.Length != 0)
            {
                GetFiles(files, rootNode);
            }

            treeView1.Nodes.Add(rootNode);
            treeView1.Nodes[0].Expand();
        }

        private static void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            foreach (var subDir in subDirs)
            {
                var aNode = new TreeNode(subDir.Name, 0, 0)
                {
                    Tag = subDir,
                    ImageKey = @"folder"
                };
                var subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    try
                    {
                        GetDirectories(subSubDirs, aNode);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                var files = subDir.GetFiles();
                if (files.Length != 0)
                {
                    try
                    {
                        GetFiles(files, aNode);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private static void GetFiles(FileInfo[] files, TreeNode nodeToAddTo)
        {
            foreach (var file in files)
            {
                TreeNode aNode;
                if (file.Extension.ToLower() == XmlInterpreter.ExtensionShortcut)
                {
                    aNode = new TreeNode(file.Name.Replace(XmlInterpreter.ExtensionShortcut, ""), 0, 0) { Tag = file };
                    var shortcut = new WshShell().CreateShortcut(file.FullName) as IWshShortcut;
                    if (shortcut?.TargetPath == null) continue;
                    GetDirectories(new DirectoryInfo(shortcut?.TargetPath).GetDirectories(), aNode);
                    nodeToAddTo.Nodes.Add(aNode);
                    continue;
                }

                aNode = new TreeNode(file.Name, 1, 1) { Tag = file };
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private static void UpdateSettingsPath(string value)
        {
            const string key = "path";

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null)
                settings.Add(key, value);
            else
                settings[key].Value = value;

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        private static void OpenFile(object file)
        {
            if (file is DirectoryInfo nodeDirInfo)
            {
                System.Diagnostics.Process.Start(nodeDirInfo.FullName);
            }
            else if (file is FileInfo nodeFileInfo)
            {
                System.Diagnostics.Process.Start(nodeFileInfo.FullName);
            }
        }

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
                    selectXml = OpenFileDialog_SelectXML(txtBxParentFolder.Text);
                    break;
            }

            if (selectXml == null)
            {
                Error_XMLNotFound();
                return;
            }

            var projectName = InputBox_GetNameNewProject();
            if (string.IsNullOrEmpty(projectName))
            {
                Error_NoName();
                return;
            }
            if (new DirectoryInfo(txtBxParentFolder.Text + "\\" + projectName).Exists)
            { 
                if (!Notification_ProjectExists()) return;
            }

            XmlInterpreter xml = new XmlInterpreter(selectXml.FullName, txtBxParentFolder.Text + "\\" + projectName);

            
            var caption = GetString("Form1_Error_XmlInterpreter_caption");
            try
            {
                xml.Interpreter();
            }
            catch (ExepcionXMLNotRoot)
            {
                var message = GetString("Form1_Error_XmlInterpreter_ExepcionXMLNotRoot_message");
                CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ExepcionXMLHasNotChildNodes)
            {
                var message = GetString("Form1_Error_XmlInterpreter_ExepcionXMLHasNotChildNodes_message");
                CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                var message = GetString("Form1_Error_XmlInterpreter_message");
                CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Reload();
        }
        
        private static FileInfo OpenFileDialog_SelectXML(string path)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = Resources.ResourceManager.GetString("Form1_OpenFileDialog_SelectFileXML");
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                ;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    return new FileInfo(openFileDialog.FileName);
                }
            }

            return null;
        }
        
        private void Error_XMLNotFound()
        {
            var message = GetString("Form1_Error_XMLNotFound_message");
            
            var caption = string.Format(GetString("Form1_Error_XMLNotFound_caption"),
                txtBxParentFolder.Text);

            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Error_NoName()
        {
            var message = GetString("Form1_Error_NoName_message");
            var caption = GetString("Form1_Error_NoName_caption");

            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool Notification_ProjectExists()
        {
            var message = GetString("Form1_Error_ProjectExists_message");
            var caption = GetString("Form1_Error_ProjectExists_caption");

            return CreateMessage(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) ==
                   DialogResult.Yes;
        }

        private static DialogResult CreateMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        private string InputBox_GetNameNewProject()
        {
            var message = GetString("Form1_InputBox_GetNameNewProject_message");
            var caption = GetString("Form1_InputBox_GetNameNewProject_caption");

            return Interaction.InputBox(message, caption, "", 100, 100);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var newSelected = e.Node;
            listView1.Items.Clear();
            var nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item;

            //List nodeDirInfo permissions
            foreach (var permission in nodeDirInfo.GetAccessControl(AccessControlSections.Group)
                         .GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            {
                item = new ListViewItem(permission.ToString());
                listView1.Items.Add(item);
            }
        }
        
        private void InterfaceText()
        {
            btCreateProject.Text = GetString("Form1_btCreateProject");
            btnReloadPath.Text = GetString("Form1_btnReloadPath");
            lblParentFolder.Text = GetString("Form1_lblParentFolder");
            btnSearchFolder.Text = GetString("Form1_btnSearchFolder");
            chGrup.Text = GetString("Form1_chGrup");
            chPermissions.Text = GetString("Form1_chPermissions");
            this.Text = GetString("Form1_tilte");
        }

        private static string GetString(string name)
        {
            return Resources.ResourceManager.GetString(name);
        }
        #endregion
    }
}