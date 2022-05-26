using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This class is responsible for managing how to open a file and search for it.
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Open a file
        /// </summary>
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
            catch (NullReferenceException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// find a file in the path
        /// </summary>
        /// <param name="path">Path where to look for a file</param>
        /// <returns>returns a file</returns>
        public static FileInfo OpenFileDialog_SelectXML(string path)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = Resources.Form1_OpenFileDialog_SelectFileXML;
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