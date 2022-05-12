using System.Configuration;

namespace projectExplorer.utility
{
    public static class SettingsUtility
    {
        
        private const string Key = "path";
        private static readonly AppSettingsReader Reader = new AppSettingsReader();
        
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

        public static string GetSettingsPath()
        {
            return Reader.GetValue(Key, typeof(string)).ToString();
        }
    }
}