using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace projectExplorer.utility
{
    public static class PermissionsUtility
    {
        public static IEnumerable<string[]> ShowPermissions(string path)
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

            return FormatPermissions(result);
        }

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

    }
}