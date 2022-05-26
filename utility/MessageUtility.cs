using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using projectExplorer.Properties;

namespace projectExplorer.utility
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This class handles the messages that are given to the user
    /// </summary>
    public static class MessageUtility
    {
        /// <summary>
        /// XML not found error
        /// </summary>
        /// <param name="path">the path of where the XML must be</param>
        public static void Error_XMLNotFound(string path)
        {
            var message = Resources.Form1_Error_XMLNotFound_message;
            var caption = Resources.Form1_Error_XMLNotFound_caption + path;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Unspecified name error (null or empty case)
        /// </summary>
        public static void Error_NoName()
        {
            var message = Resources.Form1_Error_NoName_message;
            var caption = Resources.Form1_Error_NoName_caption;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// You are notified that the project already exists and if you want to overwrite it
        /// </summary>
        /// <returns></returns>
        public static bool Notification_ProjectExists()
        {
            var message = Resources.Form1_Error_ProjectExists_message;
            var caption = Resources.Form1_Error_ProjectExists_caption;
            return CreateMessage(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
        }

        /// <summary>
        /// Error the XML has no root
        /// </summary>
        public static void Error_XMLNotRoot()
        {
            var caption = Resources.Form1_Error_XmlInterpreter_caption;
            var message = Resources.Form1_Error_XmlInterpreter_ExeptionXMLNotRoot_message;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// XML error does not have a child node
        /// </summary>
        public static void Error_XMLHasNotChildNodes()
        {
            var caption = Resources.Form1_Error_XmlInterpreter_caption;
            var message = Resources.Form1_Error_XmlInterpreter_ExeptionXMLHasNotChildNodes_message;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// There was an error executing the XML. Unknown error
        /// </summary>
        public static void Error_XmlInterpreter()
        {
            var caption = Resources.Form1_Error_XmlInterpreter_caption;
            var message = Resources.Form1_Error_XmlInterpreter_message;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Method to communicate with the user
        /// </summary>
        private static DialogResult CreateMessage(string message, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        /// <summary>
        /// Method to prompt the user for a project name
        /// </summary>
        public static string InputBox_GetNameNewProject()
        {
            var message = Resources.Form1_InputBox_GetNameNewProject_message;
            var caption = Resources.Form1_InputBox_GetNameNewProject_caption;
            return Interaction.InputBox(message, caption, "", 100, 100);
        }

        /// <summary>
        /// Generic method for exceptions
        /// </summary>
        /// <param name="e">Exception</param>
        public static void Error_Exception(Exception e)
        {
            var caption = Resources.Form1_Error_Exception_caption;
            var message = e.Message; 
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Error XML node with the same name as the shortcut
        /// </summary>
        public static void Error_XmlNodeNotFound()
        {
            var caption = Resources.Form1_Error_Exception_caption;
            var message = Resources.Form1_Error_XmlNodeNotFound_message;
            CreateMessage(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}