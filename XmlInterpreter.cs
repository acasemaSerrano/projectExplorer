using System;
using System.IO;
using System.Linq;
using System.Xml;
using IWshRuntimeLibrary;


namespace projectExplorer
{
    /// <summary>
    /// author: acasema
    /// email: acasema201@gmail.com
    /// definition: This class is used to parse an XML into a folder tree
    /// </summary>
    public class XmlInterpreter
    {
        #region constant
        private const string XmlTextNodeName = "#text";
        private const string XmlReplacerByRoodName = "[root]";
        public const string ExtensionShortcut = ".lnk";
        #endregion
        private readonly string _xmlFilePath;
        private readonly string _projectFilePath;
        private string _roodName;

        #region builders
        public XmlInterpreter(string xmlFilePath, string projectFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _projectFilePath = projectFilePath;
        }

        public XmlInterpreter(string projectName)
        {
            _roodName = projectName;
        }
        #endregion

        /// <summary>
        /// Method to start parsing the XML to a folder tree
        /// </summary>
        /// <exception cref="ExceptionXmlNotRoot">Incorrect XML format</exception>
        /// <exception cref="ExceptionXmlHasNotChildNodes">Incorrect XML format</exception>
        public void Interpreter()
        {
            var doc = new XmlDocument();
            doc.Load(_xmlFilePath);

            if (doc.DocumentElement != null && doc.DocumentElement.Name != "root" )
            {
                throw new ExceptionXmlNotRoot();
            }

            //check that the root has children
            if (doc.DocumentElement != null && !doc.DocumentElement.HasChildNodes)
            {
                throw new ExceptionXmlHasNotChildNodes();
            }

            //create a new folder named root
            Directory.CreateDirectory(_projectFilePath);
            _roodName = new DirectoryInfo(_projectFilePath).Name;
            //loop through all the child nodes
            ChildNodeLoop(doc.DocumentElement, _projectFilePath);
        }

        /// <summary>
        /// Main loop of that goes through all the children
        /// </summary>
        /// <param name="parentNode">Top node</param>
        /// <param name="parentFilePath">Parent folder path</param>
        private void ChildNodeLoop(XmlNode parentNode, string parentFilePath)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                CreateChildNode(childNode, parentFilePath + "\\" + childNode.Name);
            }
        }

        /// <summary>
        /// Choose whether to just create the folder or a shortcut
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="childFilePath">Folder path</param>
        private void CreateChildNode(XmlNode childNode, string childFilePath)
        {
            if (childNode.Name == XmlTextNodeName) return;
            if (childNode.ChildNodes[0]?.Name == XmlTextNodeName)
            {
                CreateDirectAccess(childNode, childNode.ChildNodes[0].Value, childFilePath);
                return;
            }

            if (childNode.Attributes?.Count > 0 && string.IsNullOrEmpty(childNode.Attributes[0]?.Value) && childNode.Attributes[0].Name != "DirectAccess")
            {
                CreateDirectAccess(childNode, childNode.Attributes[0].Value, childFilePath);
                return;
            }

            Directory.CreateDirectory(childFilePath);

            //In case of having children, the node will call ChildNodeLoop
            if (childNode.HasChildNodes)
                ChildNodeLoop(childNode, childFilePath);
        }

        /// <summary>
        /// Create the folder before creating the shortcut
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="link">Shortcut Path</param>
        /// <param name="pathDestiny">Folder path</param>
        private void CreateDirectAccess(XmlNode childNode, string link, string pathDestiny)
        {
            var filePath = link.TrimEnd().Replace(XmlReplacerByRoodName, _roodName);
            Directory.CreateDirectory(filePath);
            MakeShortcut(pathDestiny, filePath);
            ChildNodeLoop(childNode, filePath);
        }

        /// <summary>
        /// Make Shortcut.
        /// 
        /// Can give an error if the file already exists.
        /// May not do direct access solunion;
        /// 1. Access Windows security policies from Start -> Run (Win+R) -> type “secpol.msc” -> OK
        /// 2. Select Local Policies -> Security Options -> “User Account Control: Run all administrators in Admin Approval Mode” -> “Disable”
        /// 3. Reboot the computer for the changes to take effect
        /// 
        /// Puede dar un error si el archivo ya existe.
        /// Puede no acer los aceso directo solunion;
        /// 1. Accede a las políticas de seguridad de Windows desde Inicio -> Ejecutar (Win+R) -> escribe “secpol.msc” -> Aceptar
        /// 2. Selecciona Directivas locales -> Opciones de seguridad -> “Control de cuentas de usuario: ejecutar todos los administradores en Modo de aprobación de administrador” -> “Deshabilitar”
        /// 3. Reinicia el ordenador para que los cambios surjan efecto
        /// </summary>
        /// <param name="shortcutFileName">Folder path</param>
        /// <param name="targetFileName">Shortcut Path</param>
        public static void MakeShortcut(string shortcutFileName, string targetFileName)
        {
            var shell = new WshShell();
            var link = (IWshShortcut)shell.CreateShortcut(shortcutFileName + ExtensionShortcut);
            link.TargetPath = targetFileName;
            link.Save();
        }

        /// <summary>
        /// Recreate deleted folder
        /// </summary>
        /// <param name="pathFolder">Direct access address</param>
        /// <param name="xmlPath">XML Address</param>
        /// <exception cref="ExceptionXmlNodeNotFound">Exception, Xml node not found</exception>
        public void RecreateFolder(string pathFolder, string xmlPath)
        {
            var doc = new XmlDocument();
            doc.Load(xmlPath);

            var nameFolder = pathFolder.Split('\\').Last().Replace(ExtensionShortcut, "");

            var nodeToRecreate = FindNode(nameFolder, doc.DocumentElement, true);
            if (nodeToRecreate == null)
            {
                throw new ExceptionXmlNodeNotFound();
            }
            CreateChildNode(nodeToRecreate, pathFolder.Replace(ExtensionShortcut, ""));
        }

        /// <summary>
        /// Search in a node of a specific child XML
        /// </summary>
        /// <param name="searchNameNode">Name to be found</param>
        /// <param name="parentNode">Father of the children to be compared</param>
        /// <param name="hasText">I need to have to text</param>
        /// <returns>Found node. If it is null, there has been no success</returns>
        private static XmlNode FindNode(string searchNameNode, XmlNode parentNode, bool hasText)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                 if (childNode.Name == searchNameNode && (!hasText || childNode.ChildNodes.Count > 0 && childNode.ChildNodes[0].Name == XmlTextNodeName))
                 {
                     return childNode;
                 }

                 var foundNode = FindNode(searchNameNode, childNode, hasText);
                 if (foundNode == null) continue;
                 return foundNode;
            }
            return null;
        }
    }

    #region MyException
    public class ExceptionXmlNotRoot : Exception
    {
        public ExceptionXmlNotRoot() : base("XML file is not root") { }
    }
    public class ExceptionXmlHasNotChildNodes : Exception
    {
        public ExceptionXmlHasNotChildNodes() : base("XML file has not child nodes") { }

    }
    public class ExceptionXmlNodeNotFound : Exception
    {
        public ExceptionXmlNodeNotFound() : base("Exception Xml Node Not Found") { }

    }
    #endregion

}
