using System.Collections.Generic;
using System.Xml;

namespace master_project.BusinessLogic.Two
{
    public class ReferenceNodes
    {
        public List<XmlNode> classReferenceNodes{get;set;}
        public List<XmlNode> packageReferenceNodes{get;set;}
        public List<XmlNode> componentReferenceNodes{get;set;}
        public List<XmlNode> attributesReferenceNodes{get;set;}
        public List<XmlNode> propertiesReferenceNodes{get;set;}
        public List<XmlNode> interfaceReferenceNodes{get;set;}
        public List<XmlNode> associationReferenceNodes{get;set;}
        public List<XmlNode> objectAttributesReferenceNodes{get;set;}
        public List<XmlNode> objectReferenceNodes{get;set;}
        public List<XmlNode> actorReferenceNodes{get;set;}
        public List<XmlNode> usecaseReferenceNodes{get;set;}
        public List<XmlNode> fragmentReferenceNodes{get;set;}
        public List<XmlNode> lifeLineReferenceNodes{get;set;}
        public List<XmlNode> messageReferenceNodes{get;set;}
        public List<XmlNode> stateReferenceNodes{get;set;}
        public List<XmlNode> triggerReferenceNodes{get;set;}
        public List<XmlNode> connectorsReferenceNodes{get;set;}
        public List<XmlNode> requiredReferenceNodes{get;set;}
        public List<XmlNode> providedReferenceNodes{get;set;}
        public List<XmlNode> portsReferenceNodes{get;set;}
        public List<XmlNode> informationFlowReferenceNodes{get;set;}
        public List<XmlNode> collaborationUseNodes{get;set;}
        public List<XmlNode> nodeReferenceNodes{get;set;}
        public List<XmlNode> outputReferenceNodes{get;set;}
        public List<XmlNode> inputReferenceNodes{get;set;}
        public List<XmlNode> edgeReferenceNodes{get;set;}
        public List<XmlNode> exceptionHandlerReferenceNodes{get;set;}
        public List<XmlNode> exceptionInputReferenceNodes{get;set;}
        public List<XmlNode> acceptEventActionReferenceNodes{get;set;}
        public List<XmlNode> interruptingEdgeReferenceNodes{get;set;}
        public List<UMLDataType> dataTypes{get;set;}
        public List<UMLDataType> primitiveTypes{get;set;}
    }

}