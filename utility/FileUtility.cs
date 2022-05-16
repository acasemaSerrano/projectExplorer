using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    public static class FileUtility
    {
        public static void OpenFile(FileSystemInfo file)
        {
            try
            {
                Process.Start(file.FullName);
            }
            catch (Win32Exception e)
            {
                MessageUtility.Error_Exception(e);
            }
        }
        
            public static FileInfo OpenFileDialog_SelectXML(string path)
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
    }
}