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
        private Thread _hiloPtv;
        private Thread _hiloGdc;
        
        private void Reload()
        {
            if (_hiloPtv != null)
            {
                _hiloPtv.Abort();
            }
            if (_hiloGdc != null)
            {
                _hiloGdc.Abort();
            }

            var delegatePtv = new ThreadStart(() => PopulateTreeView(txtBxParentFolder.Text));
            _hiloPtv = new Thread(delegatePtv);
            _hiloPtv.IsBackground = true;
            _hiloPtv.Start();
            
            dataGridView1.Rows.Clear();
        }
        
        private void ReloadChildrenOfNode(TreeNode node)
        {
            node.Nodes.Clear();
            var delegateGdc = new ThreadStart(() => GetDirectories2(((DirectoryInfo)node.Tag).GetDirectories(), node));
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
            SetTreeView1Add(rootNode);
            
            GetDirectories2(info.GetDirectories(), rootNode);

            var files = info.GetFiles();
            if (files.Length != 0)
            {
                GetFiles(files, rootNode);
            }
            SetTreeView1Expand();
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
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);

            }
        }

        private void GetFiles(IEnumerable<FileInfo> files, TreeNode nodeToAddTo)
        {
            foreach (var file in files)
            {
                TreeNode aNode;
                if (file.Extension.ToLower() != XmlInterpreter.ExtensionShortcut)
                    aNode = new TreeNode(file.Name, 1, 1) { Tag = file };
                else
                {
                    aNode = new TreeNode(file.Name.Replace(XmlInterpreter.ExtensionShortcut, ""), 0, 0) { Tag = file };
                    var shortcut = new WshShell().CreateShortcut(file.FullName) as IWshShortcut;
                    if (shortcut?.TargetPath == null) continue;
                    try
                    {
                        
                        //GetDirectories(new DirectoryInfo(shortcut.TargetPath).GetDirectories(), aNode);
                        aNode.Tag = new DirectoryInfo(shortcut.TargetPath);
                        if (((DirectoryInfo)aNode.Tag).GetDirectories().Length > 0)
                            SetLinkParentToChildrenNode(aNode, new TreeNode("Loading...", 0, 0));
                        
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
                }
                
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);
            }
        }

        private void GetDirectories2(IEnumerable<DirectoryInfo> subDirs, TreeNode nodeToAddTo)
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
                    /*
                    try
                    {
                        //GetDirectories(subSubDirs, aNode);
                        
                    }
                    catch
                    {
                        // ignored
                    }
                    */
                    SetLinkParentToChildrenNode(aNode, new TreeNode("Loading...", 0, 0));
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
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);

            }
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