using System.Collections.Generic;
using System.Xml;

namespace master_project.BusinessLogic.One
{
    public class ReferenceNodes
    {
        public XmlNodeList classReferenceNodes { get; set; }
        public XmlNodeList packageReferenceNodes { get; set; }
        public XmlNodeList componentReferenceNodes { get; set; }
        public XmlNodeList objectsReferenceNodes { get; set; }
        public XmlNodeList interfaceReferenceNodes { get; set; }
        public XmlNodeList supplierDependencyNodes { get; set; }
        public XmlNodeList clientDependencyNodes { get; set; }
        public XmlNodeList actorReferenceNodes { get; set; }
        public XmlNodeList usecaseReferenceNodes { get; set; }
        public List<UMLSetereoType> setereoTypes { get; set; }
        public List<UMLDataType> dataTypes { get; set; }
        public XmlNodeList simpleStateReferenceNodes { get; set; }
        public XmlNodeList pseudoStateReferenceNodes { get; set; }
        public XmlNodeList actionStateReferenceNodes { get; set; }
    }
}