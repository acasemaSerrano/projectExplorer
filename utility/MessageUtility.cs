using System.Windows.Forms;
using Microsoft.VisualBasic;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    public static class MessageUtility
    {
        public static void Error_XMLNotFound(string path)
        {
            var message = GetString("Form1_Error_XMLNotFound_message");

            var caption = GetString("Form1_Error_XMLNotFound_caption") + path;

            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error_NoName()
        {
            var message = GetString("Form1_Error_NoName_message");
            var caption = GetString("Form1_Error_NoName_caption");

            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Notification_ProjectExists()
        {
            var message = GetString("Form1_Error_ProjectExists_message");
            var caption = GetString("Form1_Error_ProjectExists_caption");

            return CreateMessage(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) ==
                   DialogResult.Yes;
        }

        public static void Error_XMLNotRoot()
        {
            var caption = GetString("Form1_Error_XmlInterpreter_caption");
            var message = GetString("Form1_Error_XmlInterpreter_ExeptionXMLNotRoot_message");
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error_XMLHasNotChildNodes()
        {
            var caption = GetString("Form1_Error_XmlInterpreter_caption");
            var message = GetString("Form1_Error_XmlInterpreter_ExeptionXMLHasNotChildNodes_message");
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error_XmlInterpreter()
        {
            var caption = GetString("Form1_Error_XmlInterpreter_caption");
            var message = GetString("Form1_Error_XmlInterpreter_message");
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static DialogResult CreateMessage(string message, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        public static string InputBox_GetNameNewProject()
        {
            var message = GetString("Form1_InputBox_GetNameNewProject_message");
            var caption = GetString("Form1_InputBox_GetNameNewProject_caption");

            return Interaction.InputBox(message, caption, "", 100, 100);
        }

        private static string GetString(string name)
        {
            return Resources.ResourceManager.GetString(name);
        }
    }
}