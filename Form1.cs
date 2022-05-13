using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using IWshRuntimeLibrary;
using projectExplorer.Properties;
using projectExplorer.utility;

namespace projectExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InterfaceText();
            var settingsPath = SettingsUtility.GetSettingsPath();
            txtBxParentFolder.Text = string.IsNullOrEmpty(settingsPath) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : settingsPath;
            Reload();
        }

        #region events
        private void button1_Click(object sender, EventArgs e)
        {
            if (searchFolderDialog.ShowDialog() != DialogResult.OK) return;
            txtBxParentFolder.Text = searchFolderDialog.SelectedPath;
            Reload();
            SettingsUtility.UpdateSettingsPath(txtBxParentFolder.Text);
        }

        private void btnApplyPath_Click(object sender, EventArgs e)
        {
            Reload();
            SettingsUtility.UpdateSettingsPath(txtBxParentFolder.Text);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var newSelected = e.Node;
            OpenFile(newSelected.Tag);
        }

        private void txtBxParentFolder_TextChanged(object sender, EventArgs e)
        {
            Reload();
            SettingsUtility.UpdateSettingsPath(txtBxParentFolder.Text);
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
        #endregion

        #region methods

        private Thread _hiloPtv;
        private void Reload()
        {
            
            //TODO: thead.Start => PopulateTreeView(txtBxParentFolder.Text);
            //odl: PopulateTreeView(txtBxParentFolder.Text);
            var delegatePtv = new ThreadStart(() => PopulateTreeView(txtBxParentFolder.Text));
            _hiloPtv = new Thread(delegatePtv);
            _hiloPtv.Start();
            
            dataGridView1.Rows.Clear();
        }
        
        private void GetDirectories(IEnumerable<DirectoryInfo> subDirs, TreeNode nodeToAddTo)
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

                //TODO: (solucionado) nodeToAddTo add aNode (error)
                //Odl: nodeToAddTo.Nodes.Add(aNode);
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);

            }
        }

        private void GetFiles(IEnumerable<FileInfo> files, TreeNode nodeToAddTo)
        {
            foreach (var file in files)
            {
                TreeNode aNode;
                if (file.Extension.ToLower() == XmlInterpreter.ExtensionShortcut)
                {
                    aNode = new TreeNode(file.Name.Replace(XmlInterpreter.ExtensionShortcut, ""), 0, 0) { Tag = file };
                    var shortcut = new WshShell().CreateShortcut(file.FullName) as IWshShortcut;
                    if (shortcut?.TargetPath == null) continue;
                    try
                    {
                        GetDirectories(new DirectoryInfo(shortcut.TargetPath).GetDirectories(), aNode);
                    }
                    catch (ArgumentException)
                    {
                        aNode.ImageIndex = 2;
                        aNode.SelectedImageIndex = 2;
                    }
                    catch (IOException)
                    {
                        aNode.ImageIndex = 2;
                        aNode.SelectedImageIndex = 2;
                    }
                    //TODO: (solucionado) nodeToAddTo add aNode (error)
                    //Odl: nodeToAddTo.Nodes.Add(aNode);
                    SetLinkParentToChildrenNode(nodeToAddTo, aNode);
                    continue;
                }

                aNode = new TreeNode(file.Name, 1, 1) { Tag = file };
                //TODO: (solucionado) nodeToAddTo add aNode (error)
                //Odl: nodeToAddTo.Nodes.Add(aNode);
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);
            }
        }

        private static void OpenFile(object file)
        {
            Process.Start((file as FileSystemInfo)?.FullName ?? string.Empty);
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
            catch (ExceptionXmlNotRoot) { MessageUtility.Error_XMLNotRoot(); }
            catch (ExceptionXmlHasNotChildNodes) { MessageUtility.Error_XmlInterpreter(); }
            catch { MessageUtility.Error_XMLHasNotChildNodes(); }
            finally { Reload(); }
        }
        
        private static FileInfo OpenFileDialog_SelectXML(string path)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = Resources.ResourceManager.GetString("Form1_OpenFileDialog_SelectFileXML");
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return new FileInfo(openFileDialog.FileName);
                }
            }
            return null;
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
                string permit;
                switch (permission[1])
                {   
                    case "N" : permit = Resources.ResourceManager.GetString("Form1_permission_n"); break;
                    case "F" : permit = Resources.ResourceManager.GetString("Form1_permission_f"); break;
                    case "M" : permit = Resources.ResourceManager.GetString("Form1_permission_m"); break;
                    case "RX" : permit = Resources.ResourceManager.GetString("Form1_permission_rx"); break;
                    case "R" : permit = Resources.ResourceManager.GetString("Form1_permission_r"); break;
                    case "W" : permit = Resources.ResourceManager.GetString("Form1_permission_w"); break;
                    case "D" : permit = Resources.ResourceManager.GetString("Form1_permission_d"); break;
                    default: permit = permission[1]; break;
                }
                dataGridView1.Rows.Add(permission[0], permit);
            }
        }
        
        private void PopulateTreeView(string pathFolder)
        {
            
            if (string.IsNullOrEmpty(pathFolder)) return;
            var info = new DirectoryInfo(pathFolder);
            if (!info.Exists) return;
            
            // TODO: (solucionado) ERROR no previsto de antemano
            //Odl: treeView1.Nodes.Clear();
            SetTreeView1Clear();


            var rootNode = new TreeNode(info.Name) { Tag = info };
            //TODO: (solucionado) treeView1 add rootNode (error)
            //Odl: treeView1.Nodes.Add(rootNode);
            SetTreeView1Add(rootNode);
            
            GetDirectories(info.GetDirectories(), rootNode);

            var files = info.GetFiles();
            if (files.Length != 0)
            {
                GetFiles(files, rootNode);
            }

            //TODO: (solucionado) error no se puede aceder a esto por que esta en otro hilo
            //odl: treeView1.Nodes[0].Expand();
            SetTreeView1Expand();
        }
        #endregion

        #region Delegate and its methods
        private delegate void SetLinkParentToChildrenNodeDelegate(TreeNode parentNode, TreeNode childNode);
        private void SetLinkParentToChildrenNode(TreeNode parentNode, TreeNode childNode)
        {
            if (treeView1.InvokeRequired)
            {
                SetLinkParentToChildrenNodeDelegate d = SetLinkParentToChildrenNode;
                treeView1.Invoke(d, parentNode, childNode);
            }
            else
                parentNode.Nodes.Add(childNode);
        }
        
        private delegate void SetTreeView1ExpandDelegate();
        private void SetTreeView1Expand()
        {
            if (treeView1.InvokeRequired)
            {
                SetTreeView1ExpandDelegate d = SetTreeView1Expand;
                treeView1.Invoke(d);
            }
            else
                treeView1.Nodes[0].Expand();
        }
        
        private delegate void SetTreeView1ClearDelegate();
        private void SetTreeView1Clear()
        {
            if (treeView1.InvokeRequired)
            {
                SetTreeView1ClearDelegate d = SetTreeView1Clear;
                treeView1.Invoke(d);
            }
            else
                treeView1.Nodes.Clear();
        }
        
        private delegate void SetTreeView1AddDelegate(TreeNode node);
        private void SetTreeView1Add(TreeNode childNode)
        {
            if (treeView1.InvokeRequired)
            {
                SetTreeView1AddDelegate d = SetTreeView1Add;
                treeView1.Invoke(d, childNode);
            }
            else
                treeView1.Nodes.Add(childNode);
        }
        #endregion
    }
}