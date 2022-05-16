using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using projectExplorer.utility;

namespace projectExplorer
{
public partial class Form1
{
        private Thread _hiloGdc;
        
        private void Reload()
        {
            if (_hiloGdc != null)
            {
                _hiloGdc.Abort();
            }
            PopulateTreeView(txtBxParentFolder.Text);
            dataGridView1.Rows.Clear();
        }
        
        private void ReloadChildrenOfNode(TreeNode node)
        {
            node.Nodes.Clear();
            var delegateGdc = new ThreadStart(() => GetDirectories(node));
            _hiloGdc = new Thread(delegateGdc);
            _hiloGdc.IsBackground = true;
            _hiloGdc.Start();
        }

        #region Thread
        
        private void PopulateTreeView(string pathFolder)
        {
            
            if (string.IsNullOrEmpty(pathFolder)) return;
            DirectoryInfo info;
            try
            {
                info = new DirectoryInfo(pathFolder);
            }
            catch (Exception e)
            {
                MessageUtility.Error_Exception(e);
                PopulateTreeView(SettingsUtility.GetSettingsPath());
                return;
            }
            if (!info.Exists) return;
            
            SettingsUtility.UpdateSettingsPath(pathFolder);
            SetTxtBxPath(pathFolder);
            SetTreeView1Clear();
            
            var rootNode = new TreeNode(info.Name) { Tag = info };
            rootNode.Nodes.Add("*");
            SetTreeView1Add(rootNode);
            SetTreeView1Expand();
            //ReloadChildrenOfNode(rootNode);
        }

        private void GetFiles(IEnumerable<FileInfo> files, TreeNode nodeToAddTo)
        {
            foreach (var file in files)
            {
                TreeNode aNode;
                if (file.Extension.ToLower() == XmlInterpreter.ExtensionShortcut)
                {
                    aNode = new TreeNode(file.Name.Replace(XmlInterpreter.ExtensionShortcut, ""), 0, 0);
                    var shortcut = new WshShell().CreateShortcut(file.FullName) as IWshShortcut;
                    if (shortcut?.TargetPath == null) continue;
                    try
                    {
                        aNode.Tag = new DirectoryInfo(shortcut.TargetPath);
                        if (aNode.Tag is DirectoryInfo info)
                        {
                            if(info.GetDirectories().Length != 0 || info.GetFiles().Length != 0)
                                SetLinkParentToChildrenNode(aNode, new TreeNode("@"));
                            aNode.ImageKey = @"folder";
                        }
                        else
                        {
                            aNode.ImageKey = @"file";
                        }
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
                    SetLinkParentToChildrenNode(nodeToAddTo, aNode);
                    continue;
                }

                aNode = new TreeNode(file.Name, 1, 1) { Tag = file };
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);
            }
        }

        private void GetDirectories(TreeNode nodeToAddTo)
        {
            var subDirs = (nodeToAddTo.Tag as DirectoryInfo)?.GetDirectories();
            if (subDirs != null)
                foreach (var subDir in subDirs)
                {
                    var aNode = new TreeNode(subDir.Name, 0, 0)
                    {
                        Tag = subDir,
                        ImageKey = @"folder"
                    };
                    var subSubDirs = subDir.GetDirectories();
                    if (subSubDirs.Length != 0 || subDir.GetFiles().Length != 0)
                        SetLinkParentToChildrenNode(aNode, new TreeNode("*"));
                    SetLinkParentToChildrenNode(nodeToAddTo, aNode);
                }
            var subFiles = (nodeToAddTo.Tag as DirectoryInfo)?.GetFiles();
            if (subFiles == null || subFiles.Length == 0) return;
            GetFiles(subFiles, nodeToAddTo);
        }
        #endregion

        #region delegate and method

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
        public void SetTreeView1Expand()
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

        private delegate void SetTxtBxPathDelegate(string path);
        private void SetTxtBxPath(string path)
        {
            if (treeView1.InvokeRequired)
            {
                SetTxtBxPathDelegate d = SetTxtBxPath;
                treeView1.Invoke(d, path);
            }
            else
                txtBxParentFolder.Text = path;
        }

        #endregion
       
    }
}