using System;

namespace projectExplorer
{
    public class ExepcionXMLNotRoot : Exception
    {
        public ExepcionXMLNotRoot() : base("XML file is not root") { }
    }
    public class ExepcionXMLHasNotChildNodes : Exception
    {
        public ExepcionXMLHasNotChildNodes() : base("XML file has not child nodes") { }
        
    }
}