using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This class handles permissions as much as seeing them as interpreting them.
    /// </summary>
    public static class PermissionsUtility
    {
        /// <summary>
        /// Run an Icacls command and 
        /// </summary>
        public static IEnumerable<string[]> GetPermissions(string path)
        {
            return FormatPermissions(ShowPermission(path));
        }

        /// <summary>
        /// Run an Icacls command and interpret the output
        /// </summary>
        private static string ShowPermission(string path)
        {
            var command = "Icacls \"" + path + "\"";
            var procStartInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c" + command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            var proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd().Replace(path, "");

            return result;
        }

        /// <summary>
        /// Interpret the output of the "Icacls" command response.
        /// Returning a collection of names with a collection of permissions each of each.
        /// </summary>
        private static IEnumerable<string[]> FormatPermissions(string stringNotFormatted)
        {
            stringNotFormatted = stringNotFormatted.Replace("\r\n", "\n");
            stringNotFormatted = stringNotFormatted.Replace("\n\n", "\n");
            stringNotFormatted = stringNotFormatted.Replace("\t", string.Empty);
            var preFormatted = stringNotFormatted.Split('\n');
            var formatted = new List<string[]>();
            foreach (var t in preFormatted)
            {
                var listTemp = t.Split(':');
                if (string.IsNullOrWhiteSpace(t) || listTemp.Length <= 1) continue;
                listTemp[0] = listTemp[0].Split('\\').Last().Trim();
                var tmp = listTemp[1].Split('(');
                listTemp[1] = tmp.Last().Replace(")", string.Empty).Trim();
                formatted.Add(listTemp);
            }
            return formatted;
        }

        /// <summary>
        /// Interpret a permissions code by a text
        /// </summary>
        public static string PermissionTypeInterpretation(string type)
        {
            switch (type)
            {   
                case "N" : return Resources.Form1_permission_n;
                case "F" : return Resources.Form1_permission_f;
                case "M" : return Resources.Form1_permission_m;
                case "RX" : return Resources.Form1_permission_rx;
                case "R" : return Resources.Form1_permission_r;
                case "W" : return Resources.Form1_permission_w;
                case "D" : return Resources.Form1_permission_d;
                default: return type;
            }
        }
    }
}