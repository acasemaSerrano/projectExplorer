using System.Configuration;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This class is responsible for saving and collecting configuration information
    /// </summary>
    public static class SettingsUtility
    {

        
        private static readonly string FilePath = Application.LocalUserAppDataPath + "\\projectExplorer.txt";
        private static readonly string RegexFilePath = Resources.Regex_FilePath;

        /// <summary>
        /// Get the path saved in the config file
        /// </summary>
        public static string GetPathByFile()
        {
            if (!File.Exists(FilePath))
                using (var userData = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write))
                {
                    var writer = new StreamWriter(userData);
                    writer.Write(GetSettingsPath());
                    writer.Flush();
                }
            string path;
            using (var userData = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                var reader = new StreamReader(userData);
                path = reader.ReadToEnd();
            }

            if (Regex.IsMatch(path, RegexFilePath) || path.Equals(Resources.FilePathDefault))
                return path;
            DeleteFile();
            return GetPathByFile();

        }

        /// <summary>
        /// Write the path to the config file
        /// </summary>
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

        /// <summary>
        /// Delete a file
        /// </summary>
        private static void DeleteFile()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }


        private const string Key = "path";
        private static readonly AppSettingsReader Reader = new AppSettingsReader();
        public static string GetSettingsPath()
        {
            var path = Reader.GetValue(Key, typeof(string)).ToString();
            if (!Regex.IsMatch(path, RegexFilePath)) {
                path = Resources.FilePathDefault;
                UpdateSettingsPath(path);
            }
            return path;
        }
        public static void UpdateSettingsPath(string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[Key] == null)
                settings.Add(Key, value);
            else
                settings[Key].Value = value;

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}