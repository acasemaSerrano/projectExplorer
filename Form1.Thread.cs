using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using projectExplorer.utility;
using File = System.IO.File;

namespace projectExplorer
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This partial class takes care of everything that has to do with threads and their management
    /// </summary>
    public partial class Form1
    {
        private Thread _hiloGdc; //thread in charge of reading from disk

        /// <summary>
        /// this method is responsible for removing all threads and cleaning the interface
        /// </summary>
        private void Reload()
        {
            dataGridView1.Rows.Clear();
            HideErrorLink();
            if (_hiloGdc != null)
            {
                _hiloGdc.Abort();
            }
            PopulateTreeView(txtBxParentFolder.Text);
        }


        /// <summary>
        /// Check that the directory exists and fills it with a fake son
        /// </summary>
        /// <param name="pathFolder">the directory</param>
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
                PopulateTreeView(SettingsUtility.GetPathByFile()); // Retrieve the path from the config
                return;
            }
            if (!info.Exists) return;

            SettingsUtility.SetPathByFile(pathFolder); // Assign the route in the configuration
            SetTxtBxPath(pathFolder); // Call a method with a delegate to clean up the tree
            SetTreeView1Clear(); // Call a method with a delegate in order to add the parent node to the tree

            var rootNode = CreateTreeNode(info.Name, info, NodeType.Folder); // I assign to "tag" a corresponding DirectoryInfo
            rootNode.Nodes.Add(new TreeNode("*")); // fake child
            SetTreeView1Add(rootNode); // Call a method with a delegate in order to add the parent node to the tree
            SetTreeView1Expand(); // I force expand to jump the event and fill
        }

        /// <summary>
        /// This method creates threads to fill the children of "node"
        /// </summary>
        /// <param name="node">node to be populated</param>
        private void ReloadChildrenOfNode(TreeNode node)
        {
            node.Nodes.Clear(); //Eliminate the false son
            var delegateGdc = new ThreadStart(() => GetDirectories(node));
            _hiloGdc = new Thread(delegateGdc);
            _hiloGdc.IsBackground = true;
            _hiloGdc.Start();
        }

        #region Thread
        /// <summary>
        /// This class is responsible for filling the parent node of child nodes that are folders
        /// </summary>
        /// <param name="nodeToAddTo">Node to be populated</param>
        private void GetDirectories(TreeNode nodeToAddTo)
        {
            DirectoryInfo dir;
            switch (nodeToAddTo.Tag) // In tag has been saved both as a directory and as a shortcut
            {
                case IWshShortcut lnk: dir = new DirectoryInfo(lnk.TargetPath); break;
                case DirectoryInfo directory: dir = directory; break;
                default: return;
            }

            foreach (var subDir in dir.GetDirectories()) // We add all the files in the parent node
            {

                var aNode = CreateTreeNode(subDir.Name, subDir, NodeType.Folder); //ImageKey = @"folder"

                if (CheckAreChildren(subDir))
                    AddFakeChildren(aNode);

                // Call a method with a delegate to add the node to the parent
                SetLinkParentToChildrenNode(nodeToAddTo, aNode);
            }

            GetFiles(dir.GetFiles(), nodeToAddTo); //files to node
        }

        /// <summary>
        /// This class is responsible for filling the parent node with child nodes that are files
        /// </summary>
        /// <param name="files">Set of files </param>
        /// <param name="nodeToAddTo">Node to be populated</param>
        private void GetFiles(IEnumerable<FileInfo> files, TreeNode nodeToAddTo)
        {
            foreach (var file in files)
            {
                TreeNode aNode = null;
                
                if (file.Extension.ToLower() != XmlInterpreter.ExtensionShortcut) //Check that it is not a shortcut
                {
                    aNode = CreateTreeNode(file.Name, file, NodeType.Default);
                }
                else 
                {
                    var shortcut = new WshShell().CreateShortcut(file.FullName) as IWshShortcut;
                    if (shortcut?.TargetPath == null) continue;
                    try
                    {
                        var attr = File.GetAttributes(shortcut.TargetPath);

                        if (attr.HasFlag(FileAttributes.Directory)) // In case it is a correct shortcut
                        {
                            var info = new DirectoryInfo(shortcut.TargetPath);
                            aNode = CreateTreeNode(TrimShortcutExtension(file.Name), shortcut, NodeType.Folder);
                            if (CheckAreChildren(info))
                                AddFakeChildren(aNode);
                        }
                        else // It is a direct access to a file or web
                        {
                            var info = new FileInfo(shortcut.TargetPath);
                            aNode = CreateTreeNode(TrimShortcutExtension(file.Name), info, NodeType.Default);
                        }
                    }
                    catch (FileNotFoundException) // The directory where it points to is not found.
                    {
                        aNode = CreateTreeNode(TrimShortcutExtension(file.Name), shortcut, NodeType.Error);
                    }
                    catch (ArgumentException e) // Shortcut points to an impossible format
                    {
                        aNode = CreateTreeNode(TrimShortcutExtension(file.Name), e.ParamName, NodeType.Error);
                    }
                    catch (DirectoryNotFoundException) // It is a direct access to a faulty file or web
                    {
                        var info = new FileInfo(shortcut.TargetPath);
                        aNode = CreateTreeNode(TrimShortcutExtension(file.Name), info, NodeType.Default);
                    }
                }

                SetLinkParentToChildrenNode(nodeToAddTo, aNode);

            }
        }

        /// <summary>
        /// Check that the folder is not empty
        /// </summary>
        /// <param name="info">directory to check</param>
        /// <returns></returns>
        private static bool CheckAreChildren(DirectoryInfo info)
        {
            return info.GetDirectories().Length != 0 || info.GetFiles().Length != 0;
        }

        /// <summary>
        /// Create a fake child on the node
        /// </summary>
        /// <param name="node"></param>
        private void AddFakeChildren(TreeNode node)
        {
            SetLinkParentToChildrenNode(node, new TreeNode("@"));
        }

        public enum NodeType { Folder, File, Shortcut, Error, Default }
        private static TreeNode CreateTreeNode(string name, object tag, NodeType type)
        {
            var node = new TreeNode(name);
            node.Tag = tag;
            switch(type)
            {
                case NodeType.Folder:
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    break;
                case NodeType.File:
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    break;
                case NodeType.Shortcut:
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    break;
                case NodeType.Error:
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    break;
                case NodeType.Default:
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                    break;

            }
            return node;
        }

        private static string TrimShortcutExtension(string shortcut)
        {
            return shortcut.Replace(XmlInterpreter.ExtensionShortcut, "");
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