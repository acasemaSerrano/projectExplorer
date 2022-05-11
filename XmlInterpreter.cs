using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using IWshRuntimeLibrary;


namespace projectExplorer
{

    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// </summary>
    public class XmlInterpreter
    {
        private readonly string _xmlFilePath, _projectFilePath;
        private const string XmlTextNodeName = "#text";
        private const string XmlReplacerByRoodName = "[root]";
        public const string ExtensionShortcut = ".lnk";
        private string _roodName = "";

        public XmlInterpreter(string xmlFilePath, string projectFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _projectFilePath = projectFilePath;
        }

        public void Interpreter()
        {
            var doc = new XmlDocument();
            doc.Load(_xmlFilePath);

            if (doc.DocumentElement != null && doc.DocumentElement.Name != "root" )
            {
                throw new ExepcionXMLNotRoot();
            }

            //check that the root has children
            if (!doc.DocumentElement.HasChildNodes)
            {
                throw new ExepcionXMLHasNotChildNodes();
            }

            //create a new folder named root
            Directory.CreateDirectory(_projectFilePath);
            _roodName = new DirectoryInfo(_projectFilePath).Name;
            //loop through all the child nodes
            ChildNodeLoop(doc.DocumentElement, _projectFilePath);
        }

        private void ChildNodeLoop(XmlNode parentNode, string parentFilePath)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {

                if (childNode.Name == XmlTextNodeName) continue;
                var childFilePath = parentFilePath + "\\" + childNode.Name;
                if (childNode.ChildNodes[0]?.Name == XmlTextNodeName)
                {
                    CreateDirectAccess(childNode, childNode.ChildNodes[0].Value, childFilePath);
                    continue;
                }

                if (childNode.Attributes?.Count > 0 && string.IsNullOrEmpty(childNode.Attributes[0]?.Value) && childNode.Attributes[0].Name != "DirectAccess")
                {
                    CreateDirectAccess(childNode, childNode.Attributes[0].Value, childFilePath);
                    continue;
                }

                Directory.CreateDirectory(childFilePath);

                if (childNode.HasChildNodes)
                    ChildNodeLoop(childNode, childFilePath);

            }
        }

        private void CreateDirectAccess(XmlNode childNode, string link, string pathDestiny)
        {
            var filePath = link.TrimEnd().Replace(XmlReplacerByRoodName, _roodName);
            Directory.CreateDirectory(filePath);
            MakeShortcut(pathDestiny, filePath);
            ChildNodeLoop(childNode, filePath);
        }

        /// <summary>
        /// 
        /// puede dar un error si el archivo ya existe
        /// puede no acer los aceso directo solunion;
        /// 
        /// 1. Accede a las políticas de seguridad de Windows desde Inicio -> Ejecutar (Win+R) -> escribe “secpol.msc” -> Aceptar
        /// 
        /// 2. Selecciona Directivas locales -> Opciones de seguridad -> “Control de cuentas de usuario: ejecutar todos los administradores en Modo de aprobación de administrador” -> “Deshabilitar”
        /// 
        /// 3. Reinicia el ordenador para que los cambios surjan efecto
        /// </summary>
        /// <param name="shortcutFileName">donde lo quieres mas nombre</param>
        /// <param name="targetFileName">de donde quieres el enlace</param>
        private static void MakeShortcut(string shortcutFileName, string targetFileName)
        {
            var shell = new WshShell();
            var link = (IWshShortcut)shell.CreateShortcut(shortcutFileName + ExtensionShortcut);
            link.TargetPath = targetFileName;
            link.Save();
        }
    }
}
