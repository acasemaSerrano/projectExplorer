using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    public static class SettingsUtility
    {
        
        private static readonly string FilePath = Application.LocalUserAppDataPath + "\\projectExplorer.txt";
        private static readonly string FilePathDefault = Resources.ResourceManager.GetString("FilePathDefault");
        private static readonly string RegexFilePath = Resources.ResourceManager.GetString("Regex_FilePath");
        
        public static string GetPathByFile()
        {
            if (!File.Exists(FilePath))
                using (var userData = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write))
                {
                    var writer = new StreamWriter(userData);
                    writer.Write(FilePathDefault);
                    writer.Flush();
                }
            string path;
            using (var userData = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                var reader = new StreamReader(userData);
                path = reader.ReadToEnd();
            }
            
            if (!Regex.IsMatch (path, RegexFilePath) && !path.Equals(FilePathDefault))
            {
                DeleteFile();
                return GetPathByFile();
            }
            
            return path;
        }
        
        public static void SetPathByFile(string path)
        {
            if (!File.Exists(FilePath))
                using (var userData = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write))
                {
                    var writer = new StreamWriter(userData);
                    writer.Write(path);
                    writer.Flush();
                    return;
                }
            using (var userData = new FileStream(FilePath, FileMode.Truncate, FileAccess.Write))
            {
                var writer = new StreamWriter(userData);
                writer.Write(path);
                writer.Flush();
            }
        }

        private static void DeleteFile()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}