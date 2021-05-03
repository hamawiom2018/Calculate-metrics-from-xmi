using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.BusinessLogic.Two;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft;
using Newtonsoft.Json;

namespace master_project.BusinessLogic
{
    internal class XmiVersionTwoBL
    {
        private ApplicationUser _identity;
        private XmlNode mainModelNode;

        private ReferenceNodes _referenceNodes;
        public XmiVersionTwoBL(ApplicationUser Identity)
        {
            _identity = Identity;
        }

        private bool validateContent(string content, out XmlNode modelNode)
        {
            modelNode = null;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(content);
            XmlNodeList XMI = xDoc.GetElementsByTagName("uml:Model");
            if (XMI.Count == 0)
            {
                return false;
            }

            modelNode = XMI[0];

            if (modelNode == null)
            {
                return false;
            }


            return true;
        }

        public bool readContent(string content, out List<UMLElement> resultElements)
        {
            XmlNode modelNode = null;
            bool isValid = validateContent(content, out modelNode);
            mainModelNode = modelNode;
            resultElements = null;
            if (isValid)
            {
                _referenceNodes = new ReferenceNodes();
                List<XmlNode> packagedElementNodes = mainModelNode.OwnerDocument.GetElementsByTagName("packagedElement").Cast<XmlNode>().ToList();
                List<XmlNode> fragmentNodes = mainModelNode.OwnerDocument.GetElementsByTagName("fragment").Cast<XmlNode>().ToList();
                List<XmlNode> ownedAttributeNodes = mainModelNode.OwnerDocument.GetElementsByTagName("ownedAttribute").Cast<XmlNode>().ToList();
                List<XmlNode> ownedMemberNodes = mainModelNode.OwnerDocument.GetElementsByTagName("ownedMember").Cast<XmlNode>().ToList();
                List<XmlNode> ownedConnectorNodes = mainModelNode.OwnerDocument.GetElementsByTagName("ownedConnector").Cast<XmlNode>().ToList();
                List<XmlNode> qualifierNodes = mainModelNode.OwnerDocument.GetElementsByTagName("qualifier").Cast<XmlNode>().ToList();
                List<XmlNode> ownedElementNodes = mainModelNode.OwnerDocument.GetElementsByTagName("ownedElement").Cast<XmlNode>().ToList();
                _referenceNodes.stateReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("subvertex").Cast<XmlNode>().ToList();
                _referenceNodes.fragmentReferenceNodes = fragmentNodes.Where(node => node.Attributes["xmi:type"] != null && (node.Attributes["xmi:type"].Value.ToLower() == "uml:OccurrenceSpecification".ToLower()
                || node.Attributes["xmi:type"].Value.ToLower() == "uml:MessageOccurrenceSpecification".ToLower())).ToList();
                _referenceNodes.classReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:Class".ToLower()).ToList();
                _referenceNodes.packageReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:Package".ToLower()).ToList();
                _referenceNodes.componentReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:Component".ToLower()).ToList();
                _referenceNodes.requiredReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("required").Cast<XmlNode>().ToList();
                _referenceNodes.providedReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("provided").Cast<XmlNode>().ToList();
                _referenceNodes.outputReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("output").Cast<XmlNode>().ToList();
                _referenceNodes.inputReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("input").Cast<XmlNode>().ToList();
                _referenceNodes.edgeReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("edge").Cast<XmlNode>().ToList();
                _referenceNodes.exceptionHandlerReferenceNodes = ownedElementNodes.Where(node => node.Attributes["xmi:type"]?.Value == "uml:ExceptionHandler").ToList();
                _referenceNodes.exceptionInputReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("exceptionInput").Cast<XmlNode>().ToList();
                _referenceNodes.acceptEventActionReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("containedNode").Cast<XmlNode>().ToList();
                _referenceNodes.interruptingEdgeReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("interruptingEdge").Cast<XmlNode>().ToList();

                _referenceNodes.collaborationUseNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:CollaborationUse".ToLower()).ToList();

                //portNodes
                _referenceNodes.portsReferenceNodes = qualifierNodes;
                _referenceNodes.portsReferenceNodes.AddRange(ownedAttributeNodes.Where(node => node.Attributes["xmi:type"]?.Value == "uml:Port").ToList());

                //informationFlow
                _referenceNodes.informationFlowReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:InformationFlow".ToLower()).ToList();
                _referenceNodes.interfaceReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:Interface".ToLower()).ToList();
                _referenceNodes.objectReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:InstanceSpecification".ToLower()).ToList();
                _referenceNodes.actorReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:Actor".ToLower()).ToList();
                _referenceNodes.usecaseReferenceNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:UseCase".ToLower()).ToList();
                _referenceNodes.connectorsReferenceNodes = ownedConnectorNodes;
                _referenceNodes.messageReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("message").Cast<XmlNode>().ToList();
                _referenceNodes.lifeLineReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("lifeline").Cast<XmlNode>().ToList();
                _referenceNodes.nodeReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("node").Cast<XmlNode>().ToList();
                _referenceNodes.associationReferenceNodes = ownedAttributeNodes.Where(node => node.Attributes["association"] != null).ToList();
                _referenceNodes.triggerReferenceNodes = ownedMemberNodes.Where(node => node.Attributes["xmi:type"]?.Value == "uml:Trigger").ToList();
                _referenceNodes.attributesReferenceNodes = ownedAttributeNodes.Where(node => node.Attributes["aggregation"] == null).ToList(); ;
                _referenceNodes.propertiesReferenceNodes = ownedAttributeNodes.Where(node => node.Attributes["aggregation"] != null && node.Attributes["xmi:type"]?.Value == "uml:Property").ToList(); ;
                _referenceNodes.objectAttributesReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("slot").Cast<XmlNode>().ToList();
                List<XmlNode> dataTypeNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:DataType".ToLower()).ToList();
                _referenceNodes.dataTypes = getDataTypes(dataTypeNodes);
                List<XmlNode> primitiveTypeNodes = packagedElementNodes.Where(node => node.Attributes["xmi:type"] != null && node.Attributes["xmi:type"].Value.ToLower() == "uml:PrimitiveType".ToLower()).ToList();
                _referenceNodes.primitiveTypes = getDataTypes(primitiveTypeNodes);

                resultElements = buildStructure(modelNode);
            }
            return isValid;
        }
        private List<UMLDataType> getDataTypes(List<XmlNode> dataTypeNodes)
        {
            List<UMLDataType> dataTypes = new List<UMLDataType>();
            foreach (XmlNode dataTypeNode in dataTypeNodes)
            {
                UMLDataType dataTypeElelement = new UMLDataType();
                if (dataTypeNode.Attributes["xmi:id"] != null)
                {
                    dataTypeElelement.XmiId = dataTypeNode.Attributes["xmi:id"].Value;
                }
                if (dataTypeNode.Attributes["name"] != null)
                {
                    dataTypeElelement.Name = dataTypeNode.Attributes["name"].Value;
                }
                if (dataTypeNode.Attributes["visibility"] != null)
                {
                    dataTypeElelement.Visibility = dataTypeNode.Attributes["visibility"].Value;
                }

                dataTypes.Add(dataTypeElelement);
            }

            return dataTypes;
        }
        private UMLAssociation GetTypeReference(UMLAssociation associationElement, XmlNode associationReferenceNode, int classNumber, out string typeRef)
        {
            typeRef = null;
            if (associationReferenceNode.Attributes["type"] != null)
            {
                string typeReference = associationReferenceNode.Attributes["type"].Value;
                typeRef = associationReferenceNode.Attributes["type"].Value;


            }
            else
            {
                XmlNode attributeTypeNode = associationReferenceNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "type").FirstOrDefault();
                if (attributeTypeNode != null)
                {

                    if (attributeTypeNode.Attributes["xmi:idref"] != null)
                    {
                        string typeReference = attributeTypeNode.Attributes["xmi:idref"].Value;
                        typeRef = typeReference;
                    }
                }
            }


            if (typeRef != null)
            {
                string typeReference = typeRef;
                int typeNumber = 0;
                if (classNumber == 1)
                {
                    associationElement.Element1Ref = typeRef;
                    associationElement.Element1Name = GetNameAndType(typeReference, out typeNumber, _referenceNodes);
                    associationElement.Element1Type = typeNumber;
                }
                else if (classNumber == 2)
                {
                    associationElement.Element2Ref = typeRef;
                    associationElement.Element2Name = GetNameAndType(typeReference, out typeNumber, _referenceNodes);
                    associationElement.Element2Type = typeNumber;
                }
            }
            return associationElement;
        }
        public static string GetTypeReference(XmlNode associationReferenceNode)
        {
            string typeRef = null;
            if (associationReferenceNode.Attributes["type"] != null)
            {
                string typeReference = associationReferenceNode.Attributes["type"].Value;
                typeRef = associationReferenceNode.Attributes["type"].Value;


            }
            else
            {
                XmlNode attributeTypeNode = associationReferenceNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "type").FirstOrDefault();
                if (attributeTypeNode != null)
                {

                    if (attributeTypeNode.Attributes["xmi:idref"] != null)
                    {
                        string typeReference = attributeTypeNode.Attributes["xmi:idref"].Value;
                        typeRef = typeReference;
                    }
                }
            }



            return typeRef;
        }
        public static string GetNameAndType(string reference, out int type, ReferenceNodes referenceNodes)
        {
            string name = "";
            type = 0;
            XmlNode classReferenceNode = referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
            if (classReferenceNode != null)
            {
                name = classReferenceNode.Attributes["name"]?.Value;
                type = (int)AppEnums.Type.Class;
            }
            else
            {
                XmlNode interfaceReferenceNode = referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                if (interfaceReferenceNode != null)
                {
                    name = interfaceReferenceNode.Attributes["name"]?.Value;
                    type = (int)AppEnums.Type.Interface;
                }
                else
                {
                    XmlNode objectReferenceNode = referenceNodes.objectReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                    if (objectReferenceNode != null)
                    {
                        name = objectReferenceNode.Attributes["name"]?.Value;
                        type = (int)AppEnums.Type.Object;
                    }
                    else
                    {
                        XmlNode packageReferenceNode = referenceNodes.packageReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                        if (packageReferenceNode != null)
                        {
                            name = packageReferenceNode.Attributes["name"]?.Value;
                            type = (int)AppEnums.Type.Package;
                        }
                        else
                        {
                            XmlNode actorReferenceNode = referenceNodes.actorReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                            if (actorReferenceNode != null)
                            {
                                name = actorReferenceNode.Attributes["name"]?.Value;
                                type = (int)AppEnums.Type.Actor;
                            }
                            else
                            {
                                XmlNode usecaseReferenceNode = referenceNodes.usecaseReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                if (usecaseReferenceNode != null)
                                {
                                    name = usecaseReferenceNode.Attributes["name"]?.Value;
                                    type = (int)AppEnums.Type.UseCase;
                                }
                                else
                                {
                                    XmlNode componentReferenceNode = referenceNodes.componentReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                    if (componentReferenceNode != null)
                                    {
                                        name = componentReferenceNode.Attributes["name"]?.Value;
                                        type = (int)AppEnums.Type.Component;
                                    }
                                    else
                                    {
                                        XmlNode stateReferenceNode = referenceNodes.stateReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                        if (stateReferenceNode != null && stateReferenceNode.Attributes["xmi:type"].Value == "uml:State")
                                        {
                                            name = stateReferenceNode.Attributes["name"]?.Value;
                                            type = (int)AppEnums.Type.SimpleState;
                                        }
                                        else if (stateReferenceNode != null && stateReferenceNode.Attributes["xmi:type"].Value == "uml:Pseudostate")
                                        {
                                            name = stateReferenceNode.Attributes["name"]?.Value;
                                            type = (int)AppEnums.Type.PseudoState;
                                        }
                                        else if (stateReferenceNode != null && stateReferenceNode.Attributes["xmi:type"].Value == "uml:FinalState")
                                        {
                                            name = stateReferenceNode.Attributes["name"]?.Value;
                                            type = (int)AppEnums.Type.FinalState;
                                        }
                                        else
                                        {
                                            XmlNode providedReferenceNode = referenceNodes.providedReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                            if (providedReferenceNode != null)
                                            {
                                                name = providedReferenceNode.Attributes["name"]?.Value;
                                                type = (int)AppEnums.Type.Provided;
                                            }
                                            else
                                            {
                                                XmlNode requiredReferenceNode = referenceNodes.requiredReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                if (requiredReferenceNode != null)
                                                {
                                                    name = requiredReferenceNode.Attributes["name"]?.Value;
                                                    type = (int)AppEnums.Type.Required;
                                                }
                                                else
                                                {
                                                    XmlNode propertReferenceNode = referenceNodes.propertiesReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                    if (propertReferenceNode != null)
                                                    {
                                                        name = propertReferenceNode.Attributes["name"]?.Value;
                                                        type = (int)AppEnums.Type.Property;
                                                    }
                                                    else
                                                    {
                                                        XmlNode portReferenceNode = referenceNodes.portsReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                        if (portReferenceNode != null)
                                                        {
                                                            name = portReferenceNode.Attributes["name"]?.Value;
                                                            type = (int)AppEnums.Type.Port;
                                                        }
                                                        else
                                                        {
                                                            XmlNode collaborationUseReferenceNode = referenceNodes.collaborationUseNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                            if (collaborationUseReferenceNode != null)
                                                            {
                                                                name = collaborationUseReferenceNode.Attributes["name"]?.Value;
                                                                type = (int)AppEnums.Type.CollaborationUse;
                                                            }
                                                            else
                                                            {
                                                                XmlNode nodeReferenceNode = referenceNodes.nodeReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                if (nodeReferenceNode != null)
                                                                {
                                                                    name = nodeReferenceNode.Attributes["name"]?.Value;
                                                                    if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:Action")
                                                                    {
                                                                        type = (int)AppEnums.Type.Action;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:DecisionNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.DecisionNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:ActivityFinalNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.ActivityFinalNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:InitialNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.InitialNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:ForkNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.ForkNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:FlowFinalNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.FlowFinalNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:DataStoreNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.DataStoreNode;
                                                                    }
                                                                    else if (nodeReferenceNode != null && nodeReferenceNode.Attributes["xmi:type"].Value == "uml:CentralBufferNode")
                                                                    {
                                                                        type = (int)AppEnums.Type.CentralBufferNode;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    XmlNode outputReferenceNode = referenceNodes.outputReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                    if (outputReferenceNode != null)
                                                                    {
                                                                        name = outputReferenceNode.Attributes["name"]?.Value;
                                                                        type = (int)AppEnums.Type.OutputPin;
                                                                    }
                                                                    else
                                                                    {
                                                                        XmlNode inputReferenceNode = referenceNodes.inputReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                        if (inputReferenceNode != null)
                                                                        {
                                                                            name = inputReferenceNode.Attributes["name"]?.Value;
                                                                            type = (int)AppEnums.Type.InputPin;
                                                                        }
                                                                        else
                                                                        {
                                                                            XmlNode exceptionInputReferenceNode = referenceNodes.exceptionInputReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                            if (exceptionInputReferenceNode != null)
                                                                            {
                                                                                name = exceptionInputReferenceNode.Attributes["name"]?.Value;
                                                                                type = (int)AppEnums.Type.ExceptionInput;
                                                                            }
                                                                            else
                                                                            {
                                                                                XmlNode exceptionHandlerReferenceNode = referenceNodes.exceptionHandlerReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                                if (exceptionHandlerReferenceNode != null)
                                                                                {
                                                                                    name = exceptionHandlerReferenceNode.Attributes["name"]?.Value;
                                                                                    type = (int)AppEnums.Type.ExceptionHandler;
                                                                                }
                                                                                else
                                                                                {
                                                                                    XmlNode acceptEventActionReferenceNode = referenceNodes.acceptEventActionReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                                    if (acceptEventActionReferenceNode != null)
                                                                                    {
                                                                                        name = acceptEventActionReferenceNode.Attributes["name"]?.Value;
                                                                                        type = (int)AppEnums.Type.AcceptEventAction;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        XmlNode interruptingEdgeReferenceNode = referenceNodes.interruptingEdgeReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == reference).FirstOrDefault();
                                                                                        if (interruptingEdgeReferenceNode != null)
                                                                                        {
                                                                                            name = interruptingEdgeReferenceNode.Attributes["name"]?.Value;
                                                                                            type = (int)AppEnums.Type.InterruptingEdge;
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }


                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return name;
        }
        private string GetMultiplicity(XmlNode attributeNode)
        {
            string multiplicityResult = "";
            XmlNode attributeUpperNode = attributeNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "upperValue".ToLower()).FirstOrDefault();
            XmlNode attributeLowerNode = attributeNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "lowerValue".ToLower()).FirstOrDefault();
            if (attributeLowerNode != null && attributeUpperNode != null)
            {
                if (attributeUpperNode.Attributes["value"] != null && attributeLowerNode.Attributes["value"] != null)
                {
                    string lower = attributeUpperNode.Attributes["value"].Value;
                    string upper = attributeLowerNode.Attributes["value"].Value;
                    if (lower == "-1")
                    {
                        lower = "*";

                    }


                    if (upper == "-1")
                    {
                        upper = "*";
                    }


                    if (lower == upper)
                    {
                        multiplicityResult = lower;
                    }
                    else
                    {
                        multiplicityResult = lower + "..." + upper;
                    }
                }
                else if (attributeUpperNode.Attributes["value"] != null)
                {
                    multiplicityResult = attributeUpperNode.Attributes["value"].Value;

                }
                else if (attributeLowerNode.Attributes["value"] != null)
                {

                    multiplicityResult = attributeLowerNode.Attributes["value"].Value;

                }


            }
            return multiplicityResult;
        }
        private List<UMLAttribute> GetAttributes(List<XmlNode> attributeNodes)
        {
            List<UMLAttribute> umlAttributes = new List<UMLAttribute>();
            foreach (XmlNode attributeNode in attributeNodes)
            {
                UMLAttribute umlAttribute = new UMLAttribute();
                if (attributeNode.Attributes["xmi:id"] != null)
                {
                    umlAttribute.XmiId = attributeNode.Attributes["xmi:id"].Value;
                }
                if (attributeNode.Attributes["name"] != null)
                {
                    umlAttribute.Name = attributeNode.Attributes["name"].Value;
                }
                if (attributeNode.Attributes["visibility"] != null)
                {
                    umlAttribute.Visibility = attributeNode.Attributes["visibility"].Value;
                }else{
                    umlAttribute.Visibility="public";
                }


                //Get DataType
                if (attributeNode.Attributes["type"] != null)
                {
                    string typeReference = attributeNode.Attributes["type"].Value;
                    UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                    UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                    if (dataType != null)
                    {
                        umlAttribute.IsClassDataType = false;
                        umlAttribute.DataTypeName = dataType.Name;
                    }
                    else if (primitiveType != null)
                    {
                        umlAttribute.IsClassDataType = false;
                        umlAttribute.DataTypeName = primitiveType.Name;
                    }
                    else
                    {
                        XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                        if (classReferenceNode != null)
                        {
                            umlAttribute.DataTypeName = classReferenceNode.Attributes["name"].Value;
                            umlAttribute.IsClassDataType = true;

                        }
                    }
                }
                else
                {
                    XmlNode attributeTypeNode = attributeNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "type").FirstOrDefault();
                    if (attributeTypeNode != null)
                    {

                        if (attributeTypeNode.Attributes["xmi:type"] != null && attributeTypeNode.Attributes["xmi:type"].Value == "uml:Class" && attributeTypeNode.Attributes["xmi:idref"] != null)
                        {
                            string classReference = attributeTypeNode.Attributes["xmi:idref"].Value;
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == classReference).FirstOrDefault();
                            if (classReferenceNode != null)
                            {
                                umlAttribute.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                umlAttribute.IsClassDataType = true;
                            }

                        }
                        else if (attributeTypeNode.Attributes["href"] != null)
                        {
                            string type = attributeTypeNode.Attributes["href"].Value;
                            type = type.Substring(type.IndexOf('#'), type.Length - type.IndexOf('#'));
                            umlAttribute.IsClassDataType = false;
                            umlAttribute.DataTypeName = type;

                        }
                        else if (attributeTypeNode.Attributes["xmi:idref"] != null)
                        {
                            string typeReference = attributeTypeNode.Attributes["xmi:idref"].Value;
                            UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                            UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();

                            if (dataType != null)
                            {
                                umlAttribute.IsClassDataType = false;
                                umlAttribute.DataTypeName = dataType.Name;
                            }
                            else if (primitiveType != null)
                            {
                                umlAttribute.IsClassDataType = false;
                                umlAttribute.DataTypeName = primitiveType.Name;
                            }
                            else
                            {
                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                                if (classReferenceNode != null)
                                {
                                    umlAttribute.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                    umlAttribute.IsClassDataType = true;

                                }
                            }
                        }

                    }
                }
                XmlNode attributeUpperNode = attributeNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "upperValue".ToLower()).FirstOrDefault();
                XmlNode attributeLowerNode = attributeNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "lowerValue".ToLower()).FirstOrDefault();
                if (attributeLowerNode != null && attributeUpperNode != null)
                {
                    if (attributeUpperNode.Attributes["value"] != null && attributeLowerNode.Attributes["value"] != null)
                    {
                        string lower = attributeUpperNode.Attributes["value"].Value;
                        string upper = attributeLowerNode.Attributes["value"].Value;
                        if (lower == "-1")
                        {
                            lower = "*";

                        }


                        if (upper == "-1")
                        {
                            upper = "*";
                        }
                        if (upper == "*" || lower == "*")
                        {
                            umlAttribute.IsArray = true;

                        }

                        if (lower == upper)
                        {
                            umlAttribute.Multiplicity = lower;
                        }
                        else
                        {
                            umlAttribute.Multiplicity = lower + "..." + upper;
                        }
                    }
                    else if (attributeUpperNode.Attributes["value"] != null)
                    {
                        umlAttribute.Multiplicity = attributeUpperNode.Attributes["value"].Value;
                        if (attributeUpperNode.Attributes["value"].Value == "*")
                        {
                            umlAttribute.IsArray = true;
                        }
                    }
                    else if (attributeLowerNode.Attributes["value"] != null)
                    {

                        umlAttribute.Multiplicity = attributeLowerNode.Attributes["value"].Value;
                        if (attributeLowerNode.Attributes["value"].Value == "*")
                        {
                            umlAttribute.IsArray = true;
                        }
                    }


                }
                umlAttributes.Add(umlAttribute);
            }
            return umlAttributes;
        }
        private List<UMLOperation> GetOperations(List<XmlNode> operationNodes)
        {
            List<UMLOperation> umlOperations = new List<UMLOperation>();
            foreach (XmlNode operationNode in operationNodes)
            {
                UMLOperation umlOperation = new UMLOperation();
                if (operationNode.Attributes["xmi:id"] != null)
                {
                    umlOperation.XmiId = operationNode.Attributes["xmi:id"].Value;
                }
                if (operationNode.Attributes["name"] != null)
                {
                    umlOperation.Name = operationNode.Attributes["name"].Value;
                }
                if (operationNode.Attributes["visibility"] != null)
                {
                    umlOperation.Visibility = operationNode.Attributes["visibility"].Value;
                }

                List<XmlNode> operationParameterNodes = operationNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedParameter".ToLower()).ToList();


                if (operationParameterNodes.Count > 0)
                {
                    List<UMLOperationParameter> operationParameters = new List<UMLOperationParameter>();
                    foreach (XmlNode parameterNode in operationParameterNodes)
                    {

                        //get parameter kind
                        string direction = "";
                        if (parameterNode.Attributes["direction"] != null)
                        {
                            direction = parameterNode.Attributes["direction"].Value;
                        }


                        if (direction == "return")
                        {
                            //Get return Data Type
                            if (parameterNode.Attributes["type"] != null)
                            {
                                string typeReference = parameterNode.Attributes["type"].Value;
                                UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                                UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();

                                if (dataType != null)
                                {
                                    umlOperation.IsClassDataType = false;
                                    umlOperation.DataTypeName = dataType.Name;
                                }
                                else if (primitiveType != null)
                                {
                                    umlOperation.IsClassDataType = false;
                                    umlOperation.DataTypeName = primitiveType.Name;
                                }
                                else
                                {
                                    XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                                    if (classReferenceNode != null)
                                    {
                                        umlOperation.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                        umlOperation.IsClassDataType = true;

                                    }
                                }
                            }
                            else
                            {
                                XmlNode parameterTypeNode = parameterNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "type").FirstOrDefault();
                                if (parameterTypeNode != null)
                                {

                                    if (parameterTypeNode.Attributes["xmi:type"] != null && parameterTypeNode.Attributes["xmi:type"].Value == "uml:Class" && parameterTypeNode.Attributes["xmi:idref"] != null)
                                    {
                                        string classReference = parameterTypeNode.Attributes["xmi:idref"].Value;
                                        XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == classReference).FirstOrDefault();
                                        if (classReferenceNode != null)
                                        {
                                            umlOperation.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                            umlOperation.IsClassDataType = true;
                                        }

                                    }
                                    else if (parameterTypeNode.Attributes["href"] != null)
                                    {
                                        string type = parameterTypeNode.Attributes["href"].Value;
                                        type = type.Substring(type.IndexOf('#'), type.Length - type.IndexOf('#'));
                                        umlOperation.IsClassDataType = false;
                                        umlOperation.DataTypeName = type;

                                    }
                                    else if (parameterTypeNode.Attributes["xmi:idref"] != null)
                                    {
                                        string typeReference = parameterTypeNode.Attributes["xmi:idref"].Value;
                                        UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                                        UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();

                                        if (dataType != null)
                                        {
                                            umlOperation.IsClassDataType = false;
                                            umlOperation.DataTypeName = dataType.Name;
                                        }
                                        else if (primitiveType != null)
                                        {
                                            umlOperation.IsClassDataType = false;
                                            umlOperation.DataTypeName = primitiveType.Name;
                                        }
                                        else
                                        {
                                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                                            if (classReferenceNode != null)
                                            {
                                                umlOperation.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                                umlOperation.IsClassDataType = true;

                                            }
                                        }
                                    }

                                }
                            }
                            string multiplicity = GetMultiplicity(parameterNode);
                            if (multiplicity == "*")
                            {
                                umlOperation.IsArray = true;
                            }

                        }
                        else
                        {
                            UMLOperationParameter umlOperationParameter = new UMLOperationParameter();
                            if (parameterNode.Attributes["xmi:id"] != null)
                            {
                                umlOperationParameter.XmiId = parameterNode.Attributes["xmi:id"].Value;
                            }
                            if (parameterNode.Attributes["name"] != null)
                            {
                                umlOperationParameter.Name = parameterNode.Attributes["name"].Value;
                            }
                            if (parameterNode.Attributes["visibility"] != null)
                            {
                                umlOperationParameter.Visibility = parameterNode.Attributes["visibility"].Value;
                            }
                            umlOperationParameter.Kind = direction;
                            if (parameterNode.Attributes["type"] != null)
                            {
                                string typeReference = parameterNode.Attributes["type"].Value;
                                UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                                UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();

                                if (dataType != null)
                                {
                                    umlOperationParameter.IsClassDataType = false;
                                    umlOperationParameter.DataTypeName = dataType.Name;
                                }
                                else if (primitiveType != null)
                                {
                                    umlOperationParameter.IsClassDataType = false;
                                    umlOperationParameter.DataTypeName = primitiveType.Name;
                                }
                                else
                                {
                                    XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                                    if (classReferenceNode != null)
                                    {
                                        umlOperationParameter.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                        umlOperationParameter.IsClassDataType = true;

                                    }
                                }
                            }
                            else
                            {
                                XmlNode parameterTypeNode = parameterNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "type").FirstOrDefault();
                                if (parameterTypeNode != null)
                                {
                                    if (parameterTypeNode.Attributes["xmi:type"] != null && parameterTypeNode.Attributes["xmi:type"].Value == "uml:Class" && parameterTypeNode.Attributes["xmi:idref"] != null)
                                    {
                                        string classReference = parameterTypeNode.Attributes["xmi:idref"].Value;
                                        XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == classReference).FirstOrDefault();
                                        if (classReferenceNode != null)
                                        {
                                            umlOperationParameter.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                            umlOperationParameter.IsClassDataType = true;
                                        }

                                    }
                                    else if (parameterTypeNode.Attributes["href"] != null)
                                    {
                                        string type = parameterTypeNode.Attributes["href"].Value;
                                        type = type.Substring(type.IndexOf('#'), type.Length - type.IndexOf('#'));
                                        umlOperationParameter.IsClassDataType = false;
                                        umlOperationParameter.DataTypeName = type;

                                    }
                                    else if (parameterTypeNode.Attributes["xmi:idref"] != null)
                                    {
                                        string typeReference = parameterTypeNode.Attributes["xmi:idref"].Value;
                                        UMLDataType dataType = _referenceNodes.dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                                        UMLDataType primitiveType = _referenceNodes.primitiveTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();

                                        if (dataType != null)
                                        {
                                            umlOperationParameter.IsClassDataType = false;
                                            umlOperationParameter.DataTypeName = dataType.Name;
                                        }
                                        else if (primitiveType != null)
                                        {
                                            umlOperationParameter.IsClassDataType = false;
                                            umlOperationParameter.DataTypeName = primitiveType.Name;
                                        }
                                        else
                                        {
                                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == typeReference).FirstOrDefault();
                                            if (classReferenceNode != null)
                                            {
                                                umlOperationParameter.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                                umlOperationParameter.IsClassDataType = true;

                                            }
                                        }
                                    }

                                }
                            }
                            string multiplicity = GetMultiplicity(parameterNode);
                            if (multiplicity == "*")
                            {
                                umlOperationParameter.IsArray = true;
                            }
                            operationParameters.Add(umlOperationParameter);
                        }




                    }
                    if (operationParameters.Count > 0)
                    {
                        umlOperation.Parameters = operationParameters;
                    }

                }

                umlOperations.Add(umlOperation);
            }
            return umlOperations;
        }
        private List<UMLElement> GetInnerClasses(List<XmlNode> innerClassNodes)
        {
            List<UMLElement> result = new List<UMLElement>();
            foreach (XmlNode childNode in innerClassNodes)
            {
                if (childNode.Name.ToLower() == "nestedClassifier".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Class")
                {
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.InnerClass;
                    if (childNode.Attributes["xmi:id"] != null)
                    {
                        classElelement.XmiId = childNode.Attributes["xmi:id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        classElelement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    List<XmlNode> innerClassNodes2 = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "nestedClassifier".ToLower()).ToList();
                    if (innerClassNodes2.Count > 0)
                    {
                        classElelement.Childs = GetInnerClasses(innerClassNodes2);
                    }
                    List<XmlNode> ownedAttributeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower() && node.Attributes["association"] == null && node.Attributes["aggregation"] == null).ToList();
                    if (ownedAttributeNodes.Count > 0)
                    {
                        classElelement.Attributes = GetAttributes(ownedAttributeNodes);
                    }
                    List<XmlNode> ownedOperationNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedOperation".ToLower()).ToList();
                    if (ownedOperationNodes.Count > 0)
                    {
                        classElelement.Operations = GetOperations(ownedOperationNodes);
                    }

                    List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                    if (childPackage != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode);
                        if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                        {
                            classElelement.Childs.AddRange(childElements);
                        }
                        else
                        {
                            classElelement.Childs = childElements;
                        }


                    }
                    XmlNode generalizagtionNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "generalization".ToLower()).FirstOrDefault();
                    if (generalizagtionNode != null)
                    {
                        UMLGeneralization generalizationElement = new UMLGeneralization();
                        generalizationElement.Type = (int)AppEnums.Type.Generalization;
                        if (generalizagtionNode.Attributes["xmi:id"] != null)
                        {
                            generalizationElement.XmiId = generalizagtionNode.Attributes["xmi:id"].Value;
                        }

                        generalizationElement.ChildRef = classElelement.XmiId;
                        generalizationElement.ChildName = classElelement.Name;
                        generalizationElement.ChildType = (int)AppEnums.Type.Class;

                        if (generalizagtionNode.Attributes["general"] != null)
                        {
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => generalizagtionNode.Attributes["general"] != null && generalizagtionNode.Attributes["general"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                            if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                            {
                                generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                generalizationElement.ParentName = classReferenceNode.Attributes["name"].Value;
                                generalizationElement.ParentType = (int)AppEnums.Type.Class;
                            }
                            else
                            {
                                XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                                if (_referenceNodes.interfaceReferenceNodes != null && interfaceReferenceNode.Attributes["name"] != null)
                                {
                                    generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                    generalizationElement.ParentName = interfaceReferenceNode.Attributes["name"].Value;
                                    generalizationElement.ParentType = (int)AppEnums.Type.Interface;
                                }
                            }
                        }
                        result.Add(generalizationElement);
                    }
                    result.Add(classElelement);

                }
                else if (childNode.Name.ToLower() == "nestedClassifier".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Interface")
                {
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.InnerInterface;
                    if (childNode.Attributes["xmi:id"] != null)
                    {
                        classElelement.XmiId = childNode.Attributes["xmi:id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        classElelement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    List<XmlNode> innerClassNodes2 = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "nestedClassifier".ToLower()).ToList();
                    if (innerClassNodes2.Count > 0)
                    {
                        classElelement.Childs = GetInnerClasses(innerClassNodes2);
                    }
                    List<XmlNode> ownedAttributeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower() && node.Attributes["association"] == null && node.Attributes["aggregation"] == null).ToList();
                    if (ownedAttributeNodes.Count > 0)
                    {
                        classElelement.Attributes = GetAttributes(ownedAttributeNodes);
                    }
                    List<XmlNode> ownedOperationNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedOperation".ToLower()).ToList();
                    if (ownedOperationNodes.Count > 0)
                    {
                        classElelement.Operations = GetOperations(ownedOperationNodes);
                    }

                    List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                    if (childPackage != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode);
                        if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                        {
                            classElelement.Childs.AddRange(childElements);
                        }
                        else
                        {
                            classElelement.Childs = childElements;
                        }
                    }

                    result.Add(classElelement);

                }
            }

            return result;
        }
        private List<UMLElement> buildStructure(XmlNode modelNode)
        {
            List<UMLElement> result = new List<UMLElement>();
            foreach (XmlNode childNode in modelNode)
            {
                if (childNode.Attributes["xmi:type"] != null)
                {
                    if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Package")
                    {
                        UMLPackage packageElement = new UMLPackage();
                        packageElement.Type = (int)AppEnums.Type.Package;

                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            packageElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            packageElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            packageElement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                        if (childPackage != null)
                        {
                            List<UMLElement> childElements = buildStructure(childNode);
                            packageElement.Childs = childElements;
                        }

                        result.Add(packageElement);
                    }
                    else if (childNode.Name.ToLower() == "packageImport".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:PackageImport")
                    {
                        UMLPackageImport packageImportElement = new UMLPackageImport();
                        packageImportElement.Type = (int)AppEnums.Type.PackageImport;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            packageImportElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            packageImportElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            packageImportElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        if (childNode.Attributes["importedPackage"] != null)
                        {
                            XmlNode packageReferenceNode = _referenceNodes.packageReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["importedPackage"].Value).FirstOrDefault();
                            if (packageReferenceNode != null && packageReferenceNode.Attributes["name"] != null)
                            {
                                packageImportElement.ImportedPackageName = packageReferenceNode.Attributes["name"].Value;
                                packageImportElement.ImportedPackageRef = packageReferenceNode.Attributes["xmi:id"].Value;
                            }
                        }

                        result.Add(packageImportElement);

                    }
                    else if (childNode.Name.ToLower() == "packageMerge".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:PackageMerge")
                    {
                        UMLPackageMerge packageMergeElement = new UMLPackageMerge();
                        packageMergeElement.Type = (int)AppEnums.Type.PackageMerge;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            packageMergeElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            packageMergeElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            packageMergeElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        if (childNode.Attributes["mergedPackage"] != null)
                        {
                            XmlNode packageReferenceNode = _referenceNodes.packageReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["mergedPackage"].Value).FirstOrDefault();
                            if (packageReferenceNode != null && packageReferenceNode.Attributes["name"] != null)
                            {
                                packageMergeElement.MergedPackageName = packageReferenceNode.Attributes["name"].Value;
                                packageMergeElement.MergedPackageRef = packageReferenceNode.Attributes["xmi:id"].Value;
                            }
                        }

                        result.Add(packageMergeElement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Class")
                    {
                        UMLClass classElelement = new UMLClass();
                        classElelement.Type = (int)AppEnums.Type.Class;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            classElelement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            classElelement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            classElelement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        List<XmlNode> innerClassNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "nestedClassifier".ToLower()).ToList();
                        if (innerClassNodes.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassNodes);
                        }
                        List<XmlNode> ownedAttributeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower() && node.Attributes["association"] == null
                        && node.Attributes["aggregation"] == null).ToList();
                        if (ownedAttributeNodes.Count > 0)
                        {
                            classElelement.Attributes = GetAttributes(ownedAttributeNodes);
                        }
                        List<XmlNode> ownedOperationNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedOperation".ToLower()).ToList();
                        if (ownedOperationNodes.Count > 0)
                        {
                            classElelement.Operations = GetOperations(ownedOperationNodes);
                        }

                        List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                        if (childPackage != null)
                        {
                            List<UMLElement> childElements = buildStructure(childNode);
                            if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                            {
                                classElelement.Childs.AddRange(childElements);
                            }
                            else
                            {
                                classElelement.Childs = childElements;
                            }


                        }
                        List<XmlNode> generalizagtionNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "generalization".ToLower()).ToList();
                        foreach (XmlNode generalizagtionNode in generalizagtionNodes)
                        {
                            if (generalizagtionNode != null)
                            {
                                UMLGeneralization generalizationElement = new UMLGeneralization();
                                generalizationElement.Type = (int)AppEnums.Type.Generalization;
                                if (generalizagtionNode.Attributes["xmi:id"] != null)
                                {
                                    generalizationElement.XmiId = generalizagtionNode.Attributes["xmi:id"].Value;
                                }

                                generalizationElement.ChildRef = classElelement.XmiId;
                                generalizationElement.ChildName = classElelement.Name;
                                generalizationElement.ChildType = classElelement.Type;

                                if (generalizagtionNode.Attributes["general"] != null)
                                {
                                    XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => generalizagtionNode.Attributes["general"] != null && generalizagtionNode.Attributes["general"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                                    if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                                    {
                                        generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                        generalizationElement.ParentName = classReferenceNode.Attributes["name"].Value;
                                        generalizationElement.ParentType = (int)AppEnums.Type.Class;
                                    }
                                    else
                                    {
                                        XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                                        if (_referenceNodes.interfaceReferenceNodes != null && interfaceReferenceNode.Attributes["name"] != null)
                                        {
                                            generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                            generalizationElement.ParentName = interfaceReferenceNode.Attributes["name"].Value;
                                            generalizationElement.ParentType = (int)AppEnums.Type.Interface;
                                        }
                                    }
                                }
                                result.Add(generalizationElement);
                            }
                        }

                        CompositeStructure compositeStructure = new CompositeStructure(_referenceNodes);
                        List<UMLElement> compositeStructureElements = compositeStructure.builCompositeStructure(childNode);
                        if (compositeStructureElements.Count > 0)
                        {
                            if (classElelement.Childs == null)
                            {
                                classElelement.Childs = new List<UMLElement>();
                            }
                            classElelement.Childs.AddRange(compositeStructureElements);
                        }
                        result.Add(classElelement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Component")
                    {
                        UMLClass classElelement = new UMLClass();
                        classElelement.Type = (int)AppEnums.Type.Component;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            classElelement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            classElelement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            classElelement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        List<XmlNode> innerClassNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "nestedClassifier".ToLower()).ToList();
                        if (innerClassNodes.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassNodes);
                        }
                        List<XmlNode> ownedAttributeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower() && node.Attributes["association"] == null
                        && node.Attributes["aggregation"] == null).ToList();
                        if (ownedAttributeNodes.Count > 0)
                        {
                            classElelement.Attributes = GetAttributes(ownedAttributeNodes);
                        }
                        List<XmlNode> ownedOperationNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedOperation".ToLower()).ToList();
                        if (ownedOperationNodes.Count > 0)
                        {
                            classElelement.Operations = GetOperations(ownedOperationNodes);
                        }

                        List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                        if (childPackage != null)
                        {
                            List<UMLElement> childElements = buildStructure(childNode);
                            if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                            {
                                classElelement.Childs.AddRange(childElements);
                            }
                            else
                            {
                                classElelement.Childs = childElements;
                            }


                        }
                        List<XmlNode> generalizagtionNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "generalization".ToLower()).ToList();
                        foreach (XmlNode generalizagtionNode in generalizagtionNodes)
                        {
                            if (generalizagtionNode != null)
                            {
                                UMLGeneralization generalizationElement = new UMLGeneralization();
                                generalizationElement.Type = (int)AppEnums.Type.Generalization;
                                if (generalizagtionNode.Attributes["xmi:id"] != null)
                                {
                                    generalizationElement.XmiId = generalizagtionNode.Attributes["xmi:id"].Value;
                                }

                                generalizationElement.ChildRef = classElelement.XmiId;
                                generalizationElement.ChildName = classElelement.Name;
                                generalizationElement.ChildType = classElelement.Type;

                                if (generalizagtionNode.Attributes["general"] != null)
                                {
                                    XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => generalizagtionNode.Attributes["general"] != null && generalizagtionNode.Attributes["general"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                                    if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                                    {
                                        generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                        generalizationElement.ParentName = classReferenceNode.Attributes["name"].Value;
                                        generalizationElement.ParentType = (int)AppEnums.Type.Class;
                                    }
                                    else
                                    {
                                        XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == generalizagtionNode.Attributes["general"].Value).FirstOrDefault();
                                        if (_referenceNodes.interfaceReferenceNodes != null && interfaceReferenceNode.Attributes["name"] != null)
                                        {
                                            generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                            generalizationElement.ParentName = interfaceReferenceNode.Attributes["name"].Value;
                                            generalizationElement.ParentType = (int)AppEnums.Type.Interface;
                                        }
                                    }
                                }
                                result.Add(generalizationElement);
                            }
                        }
                        CompositeStructure compositeStructure = new CompositeStructure(_referenceNodes);
                        List<UMLElement> compositeStructureElements = compositeStructure.builCompositeStructure(childNode);
                        if (compositeStructureElements.Count > 0)
                        {
                            if (classElelement.Childs == null)
                            {
                                classElelement.Childs = new List<UMLElement>();
                            }
                            classElelement.Childs.AddRange(compositeStructureElements);
                        }
                        result.Add(classElelement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Interface")
                    {
                        UMLClass classElelement = new UMLClass();
                        classElelement.Type = (int)AppEnums.Type.Interface;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            classElelement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            classElelement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            classElelement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        List<XmlNode> innerClassNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "nestedClassifier".ToLower()).ToList();
                        if (innerClassNodes.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassNodes);
                        }
                        List<XmlNode> ownedAttributeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower() && node.Attributes["association"] == null).ToList();
                        if (ownedAttributeNodes.Count > 0)
                        {
                            classElelement.Attributes = GetAttributes(ownedAttributeNodes);
                        }
                        List<XmlNode> ownedOperationNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedOperation".ToLower()).ToList();
                        if (ownedOperationNodes.Count > 0)
                        {
                            classElelement.Operations = GetOperations(ownedOperationNodes);
                        }

                        List<XmlNode> childPackage = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "packagedElement".ToLower()).ToList();
                        if (childPackage != null)
                        {
                            List<UMLElement> childElements = buildStructure(childNode);
                            if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                            {
                                classElelement.Childs.AddRange(childElements);
                            }
                            else
                            {
                                classElelement.Childs = childElements;
                            }
                        }

                        result.Add(classElelement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Association")
                    {
                        string typeRef1 = "";
                        string typeRef2 = "";
                        UMLAssociation associationElement = new UMLAssociation();
                        associationElement.Type = (int)AppEnums.Type.Association;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            associationElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            associationElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            associationElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        List<XmlNode> memebrEndNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "memberEnd".ToLower()).ToList();
                        if (memebrEndNodes.Count > 0)
                        {
                            //for class1
                            XmlNode associationReferenceNode = _referenceNodes.associationReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == memebrEndNodes[0].Attributes["xmi:idref"].Value).FirstOrDefault();
                            if (associationReferenceNode != null)
                            {
                                //multiplicity
                                associationElement.Element1Multiplicity = GetMultiplicity(associationReferenceNode);
                                //aggregation
                                if (associationReferenceNode.Attributes["aggregation"] != null)
                                {
                                    associationElement.Element1Aggregation = associationReferenceNode.Attributes["aggregation"].Value;
                                }
                                //Get DataType
                                associationElement = GetTypeReference(associationElement, associationReferenceNode, 1, out typeRef1);
                            }
                            else
                            {
                                XmlNode ownedEndNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedEnd".ToLower() && node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == memebrEndNodes[0].Attributes["xmi:idref"].Value).FirstOrDefault();
                                if (ownedEndNode != null)
                                {
                                    associationElement.Element1Multiplicity = GetMultiplicity(ownedEndNode);

                                    //aggregation
                                    if (ownedEndNode.Attributes["aggregation"] != null)
                                    {
                                        associationElement.Element1Aggregation = ownedEndNode.Attributes["aggregation"].Value;
                                    }
                                    //Get DataType
                                    associationElement = GetTypeReference(associationElement, ownedEndNode, 1, out typeRef1);

                                }

                            }
                            if (memebrEndNodes.Count > 1)
                            {
                                //for class2
                                associationReferenceNode = _referenceNodes.associationReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == memebrEndNodes[1].Attributes["xmi:idref"].Value).FirstOrDefault();
                                if (associationReferenceNode != null)
                                {
                                    //multiplicity
                                    associationElement.Element2Multiplicity = GetMultiplicity(associationReferenceNode);
                                    //aggregation
                                    if (associationReferenceNode.Attributes["aggregation"] != null)
                                    {
                                        associationElement.Element2Aggregation = associationReferenceNode.Attributes["aggregation"].Value;
                                    }
                                    //Get DataType
                                    associationElement = GetTypeReference(associationElement, associationReferenceNode, 2, out typeRef2);
                                }
                                else
                                {
                                    XmlNode ownedEndNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedEnd".ToLower() && node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == memebrEndNodes[1].Attributes["xmi:idref"].Value).FirstOrDefault();
                                    if (ownedEndNode != null)
                                    {
                                        associationElement.Element2Multiplicity = GetMultiplicity(ownedEndNode);

                                        //aggregation
                                        if (ownedEndNode.Attributes["aggregation"] != null)
                                        {
                                            associationElement.Element2Aggregation = ownedEndNode.Attributes["aggregation"].Value;
                                        }
                                        //Get DataType
                                        associationElement = GetTypeReference(associationElement, ownedEndNode, 2, out typeRef2);

                                    }

                                }
                            }

                        }
                        else
                        {
                            List<XmlNode> ownedEndNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedEnd".ToLower()).ToList();
                            if (ownedEndNodes.Count > 0)
                            {
                                associationElement.Element1Multiplicity = GetMultiplicity(ownedEndNodes[0]);

                                //aggregation
                                if (ownedEndNodes[0].Attributes["aggregation"] != null)
                                {
                                    associationElement.Element1Aggregation = ownedEndNodes[0].Attributes["aggregation"].Value;
                                }
                                //Get DataType
                                associationElement = GetTypeReference(associationElement, ownedEndNodes[0], 1, out typeRef1);

                            }
                            if (ownedEndNodes.Count > 1)
                            {
                                //class 2
                                associationElement.Element2Multiplicity = GetMultiplicity(ownedEndNodes[1]);

                                //aggregation
                                if (ownedEndNodes[1].Attributes["aggregation"] != null)
                                {
                                    associationElement.Element2Aggregation = ownedEndNodes[1].Attributes["aggregation"].Value;
                                }
                                //Get DataType
                                associationElement = GetTypeReference(associationElement, ownedEndNodes[1], 2, out typeRef2);

                            }
                        }

                        result.Add(associationElement);



                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Dependency")
                    {
                        UMLDependency dependencyElement = new UMLDependency();

                        dependencyElement.Type = (int)AppEnums.Type.Dependency;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            dependencyElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            dependencyElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            dependencyElement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        string clientReference = "", supplierReference = "";
                        if (childNode.Attributes["client"] != null)
                        {
                            clientReference = childNode.Attributes["client"].Value;

                        }

                        if (childNode.Attributes["supplier"] != null)
                        {
                            supplierReference = childNode.Attributes["supplier"].Value;

                        }
                        if (clientReference != "" && supplierReference != "")
                        {

                            int clientType = 0;
                            dependencyElement.ClientRef = childNode.Attributes["client"].Value;
                            dependencyElement.ClientName = GetNameAndType(clientReference, out clientType, _referenceNodes);
                            dependencyElement.ClientType = clientType;

                            int supplierType = 0;
                            dependencyElement.SupplierRef = childNode.Attributes["supplier"].Value;
                            dependencyElement.SupplierName = GetNameAndType(supplierReference, out supplierType, _referenceNodes);
                            dependencyElement.SupplierType = supplierType;
                        }

                        result.Add(dependencyElement);



                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Realization")
                    {
                        UMLRelization relizationElement = new UMLRelization();
                        relizationElement.Type = (int)AppEnums.Type.Realization;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            relizationElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            relizationElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            relizationElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        if (childNode.Attributes["client"] != null)
                        {
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["client"].Value).FirstOrDefault();
                            if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                            {
                                relizationElement.ClientRef = childNode.Attributes["client"].Value;
                                relizationElement.ClientName = classReferenceNode.Attributes["name"].Value;
                                relizationElement.ClientType = (int)AppEnums.Type.Class;
                            }
                            else
                            {
                                XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["client"].Value).FirstOrDefault();
                                if (interfaceReferenceNode != null && interfaceReferenceNode.Attributes["name"] != null)
                                {
                                    relizationElement.ClientRef = childNode.Attributes["client"].Value;
                                    relizationElement.ClientName = interfaceReferenceNode.Attributes["name"].Value;
                                    relizationElement.ClientType = (int)AppEnums.Type.Interface;

                                }
                            }
                        }

                        if (childNode.Attributes["supplier"] != null)
                        {
                            XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["supplier"].Value).FirstOrDefault();
                            if (interfaceReferenceNode != null && interfaceReferenceNode.Attributes["name"] != null)
                            {
                                relizationElement.SupplierRef = childNode.Attributes["supplier"].Value;
                                relizationElement.SupplierName = interfaceReferenceNode.Attributes["name"].Value;
                                relizationElement.SupplierType = (int)AppEnums.Type.Interface;
                            }
                            else
                            {
                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["supplier"].Value).FirstOrDefault();
                                if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                                {
                                    relizationElement.SupplierRef = childNode.Attributes["supplier"].Value;
                                    relizationElement.SupplierName = classReferenceNode.Attributes["name"].Value;
                                    relizationElement.SupplierType = (int)AppEnums.Type.Class;
                                }
                            }
                        }

                        result.Add(relizationElement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Usage")
                    {
                        UMLUsage usageElement = new UMLUsage();
                        usageElement.Type = (int)AppEnums.Type.Usage;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            usageElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            usageElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            usageElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        if (childNode.Attributes["client"] != null)
                        {
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["client"].Value).FirstOrDefault();
                            if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                            {
                                usageElement.ClientRef = childNode.Attributes["client"].Value;
                                usageElement.ClientName = classReferenceNode.Attributes["name"].Value;
                                usageElement.ClientType = (int)AppEnums.Type.Class;
                            }
                            else
                            {
                                XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["client"].Value).FirstOrDefault();
                                if (interfaceReferenceNode != null && interfaceReferenceNode.Attributes["name"] != null)
                                {
                                    usageElement.ClientRef = childNode.Attributes["client"].Value;
                                    usageElement.ClientName = interfaceReferenceNode.Attributes["name"].Value;
                                    usageElement.ClientType = (int)AppEnums.Type.Interface;

                                }
                                else
                                {
                                    XmlNode packageReferenceNode = _referenceNodes.packageReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["client"].Value).FirstOrDefault();
                                    if (packageReferenceNode != null && packageReferenceNode.Attributes["name"] != null)
                                    {
                                        usageElement.ClientRef = childNode.Attributes["client"].Value;
                                        usageElement.ClientName = packageReferenceNode.Attributes["name"].Value;
                                        usageElement.ClientType = (int)AppEnums.Type.Package;
                                    }
                                }
                            }
                        }

                        if (childNode.Attributes["supplier"] != null)
                        {
                            XmlNode interfaceReferenceNode = _referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["supplier"].Value).FirstOrDefault();
                            if (interfaceReferenceNode != null && interfaceReferenceNode.Attributes["name"] != null)
                            {
                                usageElement.SupplierRef = childNode.Attributes["supplier"].Value;
                                usageElement.SupplierName = interfaceReferenceNode.Attributes["name"].Value;
                                usageElement.SupplierType = (int)AppEnums.Type.Interface;
                            }
                            else
                            {
                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["supplier"].Value).FirstOrDefault();
                                if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                                {
                                    usageElement.SupplierRef = childNode.Attributes["supplier"].Value;
                                    usageElement.SupplierName = classReferenceNode.Attributes["name"].Value;
                                    usageElement.SupplierType = (int)AppEnums.Type.Class;
                                }
                                else
                                {
                                    XmlNode packageReferenceNode = _referenceNodes.packageReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == childNode.Attributes["supplier"].Value).FirstOrDefault();
                                    if (packageReferenceNode != null && packageReferenceNode.Attributes["name"] != null)
                                    {
                                        usageElement.SupplierRef = childNode.Attributes["supplier"].Value;
                                        usageElement.SupplierName = packageReferenceNode.Attributes["name"].Value;
                                        usageElement.SupplierType = (int)AppEnums.Type.Package;
                                    }
                                }
                            }
                        }

                        result.Add(usageElement);

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:InstanceSpecification")
                    {
                        UMLObject objectElement = new UMLObject();
                        objectElement.Type = (int)AppEnums.Type.Object;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            objectElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            objectElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            objectElement.Visibility = childNode.Attributes["visibility"].Value;
                        }

                        if (childNode.Attributes["classifier"] != null)
                        {
                            string classReference = childNode.Attributes["classifier"].Value;
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == classReference).FirstOrDefault();
                            if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                            {
                                objectElement.InstancedClassName = classReferenceNode.Attributes["name"].Value;
                                objectElement.InstancedClassRef = classReference;
                            }
                        }
                        else
                        {
                            XmlNode classifierNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "classifier").FirstOrDefault();
                            if (classifierNode != null && classifierNode.Attributes["xmi:idref"] != null)
                            {
                                string classReference = classifierNode.Attributes["xmi:idref"].Value;
                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == classReference).FirstOrDefault();
                                if (classReferenceNode != null && classReferenceNode.Attributes["name"] != null)
                                {
                                    objectElement.InstancedClassName = classReferenceNode.Attributes["name"].Value;
                                    objectElement.InstancedClassRef = classReference;
                                }
                            }
                        }

                        List<XmlNode> slotNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "slot").ToList();
                        if (slotNodes.Count > 0)
                        {
                            XmlNode associationNode = null;
                            objectElement.Attributes = GetObjectAttributes(slotNodes, out associationNode);
                            if (associationNode != null)
                            {
                                if (associationNode.Attributes["value"] != null)
                                {
                                    string objectReference = associationNode.Attributes["value"].Value;
                                    XmlNode objectReferenceAssociationNode = _referenceNodes.objectReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == objectReference).FirstOrDefault();

                                    UMLAssociationObject associationObject = new UMLAssociationObject();
                                    associationObject.Object1Ref = objectElement.XmiId;
                                    associationObject.Object1Name = objectElement.Name;
                                    associationObject.Type = (int)AppEnums.Type.AssociationObject;
                                    if (objectReferenceAssociationNode != null)
                                    {
                                        if (objectReferenceAssociationNode.Attributes["name"] != null)
                                        {
                                            associationObject.Object2Name = objectReferenceAssociationNode.Attributes["name"].Value;
                                        }

                                        if (objectReferenceAssociationNode.Attributes["xmi:id"] != null)
                                        {
                                            associationObject.Object2Ref = objectReferenceAssociationNode.Attributes["xmi:id"].Value;
                                        }

                                    }
                                    result.Add(associationObject);

                                }
                            }
                        }
                        Activity activity = new Activity(_referenceNodes);
                        List<UMLElement> outgoingEdges = activity.GetOutgouingEdges(objectElement.XmiId);
                        if (outgoingEdges.Count > 0)
                        {
                            if (objectElement.Childs == null)
                            {
                                objectElement.Childs = new List<UMLElement>();
                            }
                            objectElement.Childs.AddRange(outgoingEdges);
                        }

                        result.Add(objectElement);





                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Actor")
                    {
                        UMLElement actorElement = new UMLElement();
                        actorElement.Type = (int)AppEnums.Type.Actor;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            actorElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            actorElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            actorElement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        result.Add(actorElement);
                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:UseCase")
                    {
                        UMLElement usecaseElement = new UMLElement();
                        usecaseElement.Type = (int)AppEnums.Type.UseCase;
                        if (childNode.Attributes["xmi:id"] != null)
                        {
                            usecaseElement.XmiId = childNode.Attributes["xmi:id"].Value;
                        }
                        if (childNode.Attributes["name"] != null)
                        {
                            usecaseElement.Name = childNode.Attributes["name"].Value;
                        }
                        if (childNode.Attributes["visibility"] != null)
                        {
                            usecaseElement.Visibility = childNode.Attributes["visibility"].Value;
                        }
                        List<XmlNode> extendedNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "extend".ToLower()).ToList();
                        foreach (XmlNode extendNode in extendedNodes)
                        {
                            if (extendNode.Attributes["extendedCase"] != null)
                            {
                                UMLAssociation associationElement = new UMLAssociation();
                                associationElement.Type = (int)AppEnums.Type.Association;
                                associationElement.XmiId = extendNode.Attributes["xmi:id"]?.Value;
                                associationElement.Element1Name = usecaseElement.Name;
                                associationElement.Element1Ref = usecaseElement.XmiId;
                                associationElement.Element1Type = (int)AppEnums.Type.UseCase;

                                associationElement.Element2Ref = extendNode.Attributes["extendedCase"].Value;
                                int clientType = 0;
                                associationElement.Element2Name = GetNameAndType(extendNode.Attributes["extendedCase"].Value, out clientType, _referenceNodes);
                                associationElement.Element2Type = clientType;

                                associationElement.StereoType = "extend";

                                result.Add(associationElement);
                            }
                        }

                        List<XmlNode> includeNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "include".ToLower()).ToList();
                        foreach (XmlNode includeNode in includeNodes)
                        {
                            if (includeNode.Attributes["addition"] != null)
                            {
                                UMLAssociation associationElement = new UMLAssociation();
                                associationElement.Type = (int)AppEnums.Type.Association;
                                associationElement.XmiId = includeNode.Attributes["xmi:id"]?.Value;
                                associationElement.Element1Name = usecaseElement.Name;
                                associationElement.Element1Ref = usecaseElement.XmiId;
                                associationElement.Element1Type = (int)AppEnums.Type.UseCase;

                                associationElement.Element2Ref = includeNode.Attributes["addition"].Value;
                                int element2Type = 0;
                                associationElement.Element2Name = GetNameAndType(includeNode.Attributes["addition"].Value, out element2Type, _referenceNodes);
                                associationElement.Element2Type = element2Type;

                                associationElement.StereoType = "include";

                                result.Add(associationElement);
                            }
                        }
                        List<XmlNode> generalizagtionNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "generalization".ToLower()).ToList();
                        foreach (XmlNode generalizagtionNode in generalizagtionNodes)
                        {
                            UMLGeneralization generalizationElement = new UMLGeneralization();
                            generalizationElement.Type = (int)AppEnums.Type.Generalization;
                            if (generalizagtionNode.Attributes["xmi:id"] != null)
                            {
                                generalizationElement.XmiId = generalizagtionNode.Attributes["xmi:id"].Value;
                            }

                            generalizationElement.ChildRef = usecaseElement.XmiId;
                            generalizationElement.ChildName = usecaseElement.Name;
                            generalizationElement.ChildType = (int)AppEnums.Type.Class;

                            if (generalizagtionNode.Attributes["general"] != null)
                            {
                                int parentType = 0;
                                generalizationElement.ParentRef = generalizagtionNode.Attributes["general"].Value;
                                generalizationElement.ParentName = GetNameAndType(generalizagtionNode.Attributes["general"].Value, out parentType, _referenceNodes);
                                generalizationElement.ParentType = parentType;
                            }
                            result.Add(generalizationElement);
                        }
                        result.Add(usecaseElement);
                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Collaboration")
                    {
                        List<XmlNode> interactionNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedBehavior".ToLower() && node.Attributes["xmi:type"]?.Value == "uml:Interaction").ToList();

                        result.AddRange(buildInteraction(interactionNodes));

                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Interaction")
                    {
                        List<XmlNode> interactionNodes = new List<XmlNode>();
                        interactionNodes.Add(childNode);
                        result.AddRange(buildInteraction(interactionNodes));
                    }
                    else if ((childNode.Name.ToLower() == "packagedElement".ToLower() || childNode.Name.ToLower() == "ownedBehavior".ToLower()) && childNode.Attributes["xmi:type"].Value == "uml:StateMachine")
                    {
                        StateMachine stateMachine = new StateMachine(_referenceNodes);
                        result.AddRange(stateMachine.buildStateMachine(childNode));
                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:CollaborationUse")
                    {
                        UMLElement collaborationElement = new UMLElement();
                        collaborationElement.XmiId = childNode.Attributes["xmi:id"]?.Value;
                        collaborationElement.Name = childNode.Attributes["name"]?.Value;
                        collaborationElement.Visibility = childNode.Attributes["visibility"]?.Value;
                        collaborationElement.Type = (int)AppEnums.Type.CollaborationUse;
                        result.Add(collaborationElement);
                    }
                    else if (childNode.Name.ToLower() == "packagedElement".ToLower() && childNode.Attributes["xmi:type"].Value == "uml:Activity")
                    {
                        Activity activity = new Activity(_referenceNodes);
                        result.Add(activity.buildActivity(childNode));
                    }

                }

            }

            return result;
        }
        private List<UMLMessage> buildMessages(List<UMLLifeLine> lifeLines)
        {
            List<UMLMessage> messagesElementsGroup = new List<UMLMessage>();
            foreach (var lifeLine in lifeLines)
            {
                foreach (var message in lifeLine.IncomingMessages)
                {
                    if (!messagesElementsGroup.Where(d => d.XmiId == message.XmiId).Any())
                    {
                        string sMessage = JsonConvert.SerializeObject(message);
                        UMLMessage messageElement = JsonConvert.DeserializeObject<UMLMessage>(sMessage);
                        messageElement.Childs = new List<UMLElement>();
                        UMLLifeLine fromLifeLine = lifeLines.Where(element => element.XmiId == messageElement.ElementFromRef).FirstOrDefault();
                        if (fromLifeLine != null)
                        {
                            string sFromLifeLine = JsonConvert.SerializeObject(fromLifeLine);
                            fromLifeLine = JsonConvert.DeserializeObject<UMLLifeLine>(sFromLifeLine);
                            fromLifeLine.IncomingMessages = null;
                            fromLifeLine.OutgoingMessages = null;
                            if (fromLifeLine != null)
                            {
                                messageElement.Childs.Add(fromLifeLine);
                            }
                        }


                        UMLLifeLine toLifeLine = lifeLines.Where(element => element.XmiId == messageElement.ElementToRef).FirstOrDefault();
                        string sToLifeLine = JsonConvert.SerializeObject(toLifeLine);
                        toLifeLine = JsonConvert.DeserializeObject<UMLLifeLine>(sToLifeLine);
                        toLifeLine.IncomingMessages = null;
                        toLifeLine.OutgoingMessages = null;
                        if (fromLifeLine != null)
                        {
                            messageElement.Childs.Add(toLifeLine);
                        }
                        messagesElementsGroup.Add(messageElement);
                    }
                }
            }
            return messagesElementsGroup;
        }
        private List<UMLElement> buildInteraction(List<XmlNode> interactionNodes)
        {
            List<UMLElement> interactionElements = new List<UMLElement>();

            //get attribute LifeLines
            foreach (XmlNode interactionNode in interactionNodes)
            {
                List<XmlNode> attributesLifeLines = interactionNode.ParentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower()).ToList();
                if (attributesLifeLines.Count == 0)
                {
                    attributesLifeLines = interactionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "ownedAttribute".ToLower()).ToList();
                }
                UMLElement interactionElement = new UMLElement();
                interactionElement.Type = (int)AppEnums.Type.Interaction;
                if (interactionNode.Attributes["xmi:id"] != null)
                {
                    interactionElement.XmiId = interactionNode.Attributes["xmi:id"].Value;
                }
                if (interactionNode.Attributes["name"] != null)
                {
                    interactionElement.Name = interactionNode.Attributes["name"].Value;
                }
                if (interactionNode.Attributes["visibility"] != null)
                {
                    interactionElement.Visibility = interactionNode.Attributes["visibility"].Value;
                }
                interactionElement.Childs = new List<UMLElement>();
                //get fragment condition
                List<XmlNode> combinedFragmentNodes = interactionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "fragment".ToLower() && node.Attributes["xmi:type"]?.Value == "uml:CombinedFragment").ToList();
                if (combinedFragmentNodes.Count > 0)
                {
                    foreach (XmlNode combinedFragmentNode in combinedFragmentNodes)
                    {
                        UMLCombinedFragment combinedFragmentElement = new UMLCombinedFragment();
                        combinedFragmentElement.Type = (int)AppEnums.Type.CombinedFragment;
                        combinedFragmentElement.XmiId = combinedFragmentNode.Attributes["xmi:id"]?.Value;
                        combinedFragmentElement.Name = combinedFragmentNode.Attributes["name"]?.Value;
                        combinedFragmentElement.Visibility = combinedFragmentNode.Attributes["visibility"]?.Value;

                        combinedFragmentElement.InteractionOperator = combinedFragmentNode.Attributes["interactionOperator"]?.Value;
                        combinedFragmentElement.Childs = new List<UMLElement>();
                        //get fragment lifelines
                        List<XmlNode> coveredNodes = combinedFragmentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "covered".ToLower()).ToList();
                        List<UMLLifeLine> lifeLines = new List<UMLLifeLine>();
                        foreach (XmlNode coveredNode in coveredNodes)
                        {
                            string typeRef = coveredNode.Attributes["xmi:idref"]?.Value;
                            XmlNode attributeNode = _referenceNodes.attributesReferenceNodes.Where(node => GetTypeReference(node) == typeRef).FirstOrDefault();
                            if (attributeNode != null)
                            {
                                XmlNode lifeLineNode = _referenceNodes.lifeLineReferenceNodes.Where(node => node.Attributes["represents"]?.Value == attributeNode.Attributes["xmi:id"]?.Value).FirstOrDefault();
                                if (lifeLineNode != null)
                                {
                                    UMLLifeLine lifeLineElement = new UMLLifeLine();
                                    lifeLineElement.Type = (int)AppEnums.Type.LifeLine;
                                    if (lifeLineNode.Attributes["xmi:id"] != null)
                                    {
                                        lifeLineElement.XmiId = lifeLineNode.Attributes["xmi:id"].Value;
                                    }
                                    if (lifeLineNode.Attributes["name"] != null)
                                    {
                                        lifeLineElement.Name = lifeLineNode.Attributes["name"].Value;
                                    }
                                    if (lifeLineNode.Attributes["visibility"] != null)
                                    {
                                        lifeLineElement.Visibility = lifeLineNode.Attributes["visibility"].Value;
                                    }
                                    int elementType = 0;
                                    lifeLineElement.ElementName = GetNameAndType(typeRef, out elementType, _referenceNodes);
                                    lifeLineElement.ElementRef = typeRef;
                                    lifeLineElement.ElmenetType = elementType;
                                    lifeLines.Add(lifeLineElement);
                                }
                            }
                        }
                        List<XmlNode> operandNodes = combinedFragmentNode.Cast<XmlNode>().Where(node => node.Name == "operand").ToList();
                        List<UMLInteractionOperand> interactionOperands = new List<UMLInteractionOperand>();

                        foreach (XmlNode operandNode in operandNodes)
                        {
                            string slifeLines = JsonConvert.SerializeObject(lifeLines);
                            List<UMLLifeLine> copiedLifeLines = JsonConvert.DeserializeObject<List<UMLLifeLine>>(slifeLines);
                            string expression = "";
                            XmlNode gaurdNode = operandNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "guard").FirstOrDefault();
                            if (gaurdNode != null)
                            {
                                XmlNode specificationNode = gaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "specification").FirstOrDefault();
                                expression = specificationNode?.Attributes["body"]?.Value;

                            }
                            UMLInteractionOperand interactionOperand = new UMLInteractionOperand();
                            interactionOperand.Type = (int)AppEnums.Type.InteractionOperand;
                            interactionOperand.XmiId = operandNode.Attributes["xmi:id"]?.Value;
                            interactionOperand.Name = operandNode.Attributes["name"]?.Value;
                            interactionOperand.Visibility = operandNode.Attributes["visibility"]?.Value;
                            interactionOperand.Expression = expression;
                            List<XmlNode> fragmentNodes = operandNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "fragment".ToLower() && (node.Attributes["xmi:type"].Value == "uml:OccurrenceSpecification"
                            || node.Attributes["xmi:type"].Value == "uml:MessageOccurrenceSpecification")).ToList();
                            foreach (UMLLifeLine lifeLine in copiedLifeLines)
                            {
                                List<UMLMessage> incomingMessages = new List<UMLMessage>();
                                List<UMLMessage> outgoingMessages = new List<UMLMessage>();
                                foreach (XmlNode messageNode in _referenceNodes.messageReferenceNodes)
                                {
                                    XmlNode relatedFragmentIncoming = fragmentNodes.Where(node => node.Attributes["xmi:id"]?.Value == messageNode.Attributes["receiveEvent"]?.Value).FirstOrDefault();
                                    XmlNode relatedFragmentOutgoing = fragmentNodes.Where(node => node.Attributes["xmi:id"]?.Value == messageNode.Attributes["sendEvent"]?.Value).FirstOrDefault();
                                    if (relatedFragmentIncoming != null && relatedFragmentOutgoing != null)
                                    {
                                        UMLMessage incomingMessageElement = new UMLMessage();
                                        incomingMessageElement.Type = (int)AppEnums.Type.Message;
                                        if (messageNode.Attributes["xmi:id"] != null)
                                        {
                                            incomingMessageElement.XmiId = messageNode.Attributes["xmi:id"].Value;
                                        }
                                        if (messageNode.Attributes["name"] != null)
                                        {
                                            incomingMessageElement.Name = messageNode.Attributes["name"].Value;
                                        }
                                        if (messageNode.Attributes["visibility"] != null)
                                        {
                                            incomingMessageElement.Visibility = messageNode.Attributes["visibility"].Value;
                                        }
                                        incomingMessageElement.MessgaeKind = messageNode.Attributes["messageKind"]?.Value;
                                        incomingMessageElement.MessgaeSort = messageNode.Attributes["messageSort"]?.Value;
                                        if (relatedFragmentIncoming != null)
                                        {
                                            UMLLifeLine toLifeLine = (UMLLifeLine)copiedLifeLines.Where(lifeLine => lifeLine.XmiId == relatedFragmentIncoming.Attributes["covered"].Value).FirstOrDefault();
                                            if (toLifeLine?.ElementName != null)
                                            {
                                                incomingMessageElement.ElementToRef = toLifeLine.XmiId;
                                                incomingMessageElement.ElementToName = toLifeLine.ElementName;
                                                incomingMessageElement.ElementToType = toLifeLine.ElmenetType;
                                            }
                                            else
                                            {
                                                incomingMessageElement.ElementToRef = toLifeLine.XmiId;
                                                incomingMessageElement.ElementToName = toLifeLine.Name;
                                                incomingMessageElement.ElementToType = toLifeLine.Type;
                                            }
                                        }
                                        if (relatedFragmentOutgoing != null)
                                        {
                                            UMLLifeLine fromLifeLine = (UMLLifeLine)copiedLifeLines.Where(lifeLine => lifeLine.XmiId == relatedFragmentOutgoing.Attributes["covered"].Value).FirstOrDefault();
                                            if (fromLifeLine.ElementName != null)
                                            {
                                                incomingMessageElement.ElementFromRef = fromLifeLine.XmiId;
                                                incomingMessageElement.ElementFromName = fromLifeLine.ElementName;
                                                incomingMessageElement.ElementFromType = fromLifeLine.Type;
                                            }
                                            else
                                            {
                                                incomingMessageElement.ElementFromRef = fromLifeLine.XmiId;
                                                incomingMessageElement.ElementFromName = fromLifeLine.Name;
                                                incomingMessageElement.ElementFromType = fromLifeLine.Type;
                                            }
                                        }
                                        if (relatedFragmentIncoming != null && relatedFragmentIncoming.Attributes["covered"]?.Value == lifeLine.XmiId)
                                        {
                                            incomingMessages.Add(incomingMessageElement);
                                        }

                                        if (relatedFragmentOutgoing != null && relatedFragmentOutgoing.Attributes["covered"]?.Value == lifeLine.XmiId)
                                        {
                                            outgoingMessages.Add(incomingMessageElement);
                                        }
                                    }
                                }
                                lifeLine.IncomingMessages = incomingMessages;
                                lifeLine.OutgoingMessages = outgoingMessages;

                            }
                            if (copiedLifeLines.Count > 0)
                            {
                                interactionOperand.Childs = new List<UMLElement>();

                                UMLElement LifeLinesElementGroup = new UMLElement();
                                LifeLinesElementGroup.Name = "Life Lines";
                                LifeLinesElementGroup.Type = 0;
                                LifeLinesElementGroup.XmiId = Guid.NewGuid().ToString();
                                LifeLinesElementGroup.Childs = new List<UMLElement>();
                                LifeLinesElementGroup.Childs.AddRange(copiedLifeLines);
                                UMLElement MessagesElementGroup = new UMLElement();
                                MessagesElementGroup.Name = "Messages";
                                MessagesElementGroup.Type = 0;
                                MessagesElementGroup.XmiId = Guid.NewGuid().ToString();
                                MessagesElementGroup.Childs = new List<UMLElement>();
                                MessagesElementGroup.Childs.AddRange(buildMessages(copiedLifeLines));

                                interactionOperand.Childs.Add(LifeLinesElementGroup);
                                interactionOperand.Childs.Add(MessagesElementGroup);
                            }


                            combinedFragmentElement.Childs.Add(interactionOperand);


                        }

                        interactionElement.Childs.Add(combinedFragmentElement);

                    }
                }
                //get lifelines
                List<XmlNode> lifeLineNodes = interactionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "lifeline".ToLower()).ToList();

                List<XmlNode> messageNodes = interactionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "message".ToLower()).ToList();

                //List<XmlNode> fragmentNodes = interactionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "fragment".ToLower() && node.Attributes["xmi:type"].Value == "uml:OccurrenceSpecification").ToList();
                //interactionElement.Childs = new List<UMLElement>();
                List<UMLLifeLine> childElements = new List<UMLLifeLine>();
                foreach (XmlNode lifeLineNode in lifeLineNodes)
                {
                    UMLLifeLine lifeLineElement = new UMLLifeLine();
                    lifeLineElement.Type = (int)AppEnums.Type.LifeLine;
                    if (lifeLineNode.Attributes["xmi:id"] != null)
                    {
                        lifeLineElement.XmiId = lifeLineNode.Attributes["xmi:id"].Value;
                    }
                    if (lifeLineNode.Attributes["name"] != null)
                    {
                        lifeLineElement.Name = lifeLineNode.Attributes["name"].Value;
                    }
                    if (lifeLineNode.Attributes["visibility"] != null)
                    {
                        lifeLineElement.Visibility = lifeLineNode.Attributes["visibility"].Value;
                    }

                    //get reletedElement
                    if (lifeLineNode.Attributes["represents"] != null)
                    {
                        XmlNode relatedAttributeNode = attributesLifeLines.Where(node => node.Attributes["xmi:id"]?.Value == lifeLineNode.Attributes["represents"].Value).FirstOrDefault();
                        if (relatedAttributeNode != null)
                        {
                            string attributeRef = GetTypeReference(relatedAttributeNode);
                            if (attributeRef != null)
                            {
                                int elementType = 0;
                                lifeLineElement.ElementName = GetNameAndType(attributeRef, out elementType, _referenceNodes);
                                lifeLineElement.ElementRef = attributeRef;
                                lifeLineElement.ElmenetType = elementType;


                            }
                        }
                    }
                    childElements.Add(lifeLineElement);
                }

                foreach (UMLLifeLine lifeLineElement in childElements)
                {
                    List<UMLMessage> incomingMessages = new List<UMLMessage>();
                    List<UMLMessage> outgoingMessages = new List<UMLMessage>();
                    foreach (XmlNode messageNode in messageNodes)
                    {
                        XmlNode relatedFragmentIncoming = _referenceNodes.fragmentReferenceNodes.Where(node => node.Attributes["xmi:id"]?.Value == messageNode.Attributes["receiveEvent"]?.Value).FirstOrDefault();
                        XmlNode relatedFragmentOutgoing = _referenceNodes.fragmentReferenceNodes.Where(node => node.Attributes["xmi:id"]?.Value == messageNode.Attributes["sendEvent"]?.Value).FirstOrDefault();

                        UMLMessage incomingMessageElement = new UMLMessage();
                        incomingMessageElement.Type = (int)AppEnums.Type.Message;
                        if (messageNode.Attributes["xmi:id"] != null)
                        {
                            incomingMessageElement.XmiId = messageNode.Attributes["xmi:id"].Value;
                        }
                        if (messageNode.Attributes["name"] != null)
                        {
                            incomingMessageElement.Name = messageNode.Attributes["name"].Value;
                        }
                        if (messageNode.Attributes["visibility"] != null)
                        {
                            incomingMessageElement.Visibility = messageNode.Attributes["visibility"].Value;
                        }
                        incomingMessageElement.MessgaeKind = messageNode.Attributes["messageKind"]?.Value;
                        incomingMessageElement.MessgaeSort = messageNode.Attributes["messageSort"]?.Value;
                        if (relatedFragmentIncoming != null)
                        {


                            UMLLifeLine toLifeLine = (UMLLifeLine)childElements.Where(lifeLine => lifeLine.XmiId == relatedFragmentIncoming.Attributes["covered"].Value).FirstOrDefault();
                            if (toLifeLine == null)
                            {
                                XmlNode relatedAttribute = attributesLifeLines.Where(node => GetTypeReference(node) == relatedFragmentIncoming.Attributes["covered"].Value).FirstOrDefault();
                                if (relatedAttribute != null)
                                {
                                    XmlNode relatedLifeLineNode = lifeLineNodes.Where(node => node.Attributes["represents"].Value == relatedAttribute.Attributes["xmi:id"].Value).FirstOrDefault();
                                    if (relatedLifeLineNode != null)
                                    {
                                        toLifeLine = (UMLLifeLine)childElements.Where(lifeLine => lifeLine.XmiId == relatedLifeLineNode.Attributes["xmi:id"].Value).FirstOrDefault();
                                    }
                                }
                            }
                            if (toLifeLine.ElementName != null)
                            {
                                incomingMessageElement.ElementToRef = toLifeLine.XmiId;
                                incomingMessageElement.ElementToName = toLifeLine.ElementName;
                                incomingMessageElement.ElementToType = toLifeLine.ElmenetType;
                            }
                            else
                            {
                                incomingMessageElement.ElementToRef = toLifeLine.XmiId;
                                incomingMessageElement.ElementToName = toLifeLine.Name;
                                incomingMessageElement.ElementToType = toLifeLine.Type;
                            }
                        }
                        if (relatedFragmentOutgoing != null)
                        {
                            UMLLifeLine fromLifeLine = (UMLLifeLine)childElements.Where(lifeLine => lifeLine.XmiId == relatedFragmentOutgoing.Attributes["covered"].Value).FirstOrDefault();
                            if (fromLifeLine == null)
                            {
                                XmlNode relatedAttribute = attributesLifeLines.Where(node => GetTypeReference(node) == relatedFragmentOutgoing.Attributes["covered"].Value).FirstOrDefault();
                                if (relatedAttribute != null)
                                {
                                    XmlNode relatedLifeLineNode = lifeLineNodes.Where(node => node.Attributes["represents"].Value == relatedAttribute.Attributes["xmi:id"].Value).FirstOrDefault();
                                    if (relatedLifeLineNode != null)
                                    {
                                        fromLifeLine = (UMLLifeLine)childElements.Where(lifeLine => lifeLine.XmiId == relatedLifeLineNode.Attributes["xmi:id"].Value).FirstOrDefault();
                                    }
                                }
                            }
                            if (fromLifeLine.ElementName != null)
                            {
                                incomingMessageElement.ElementFromRef = fromLifeLine.XmiId;
                                incomingMessageElement.ElementFromName = fromLifeLine.ElementName;
                                incomingMessageElement.ElementFromType = fromLifeLine.Type;
                            }
                            else
                            {
                                incomingMessageElement.ElementFromRef = fromLifeLine.XmiId;
                                incomingMessageElement.ElementFromName = fromLifeLine.Name;
                                incomingMessageElement.ElementFromType = fromLifeLine.Type;
                            }
                        }


                        if (relatedFragmentIncoming != null && relatedFragmentIncoming.Attributes["covered"]?.Value == lifeLineElement.XmiId)
                        {
                            incomingMessages.Add(incomingMessageElement);
                        }

                        if (relatedFragmentOutgoing != null && relatedFragmentOutgoing.Attributes["covered"]?.Value == lifeLineElement.XmiId)
                        {
                            outgoingMessages.Add(incomingMessageElement);
                        }

                    }
                    lifeLineElement.IncomingMessages = incomingMessages;
                    lifeLineElement.OutgoingMessages = outgoingMessages;

                }

                UMLElement LifeLinesElement = new UMLElement();
                LifeLinesElement.Name = "Life Lines";
                LifeLinesElement.XmiId = Guid.NewGuid().ToString();

                LifeLinesElement.Type = 0;
                LifeLinesElement.Childs = new List<UMLElement>();
                LifeLinesElement.Childs.AddRange(childElements);

                UMLElement MessagesElement = new UMLElement();
                MessagesElement.Name = "Messages";
                MessagesElement.Type = 0;
                MessagesElement.XmiId = Guid.NewGuid().ToString();

                MessagesElement.Childs = new List<UMLElement>();
                MessagesElement.Childs.AddRange(buildMessages(childElements));

                if (LifeLinesElement.Childs?.Count > 0)
                {
                    interactionElement.Childs.Add(LifeLinesElement);
                }
                if (MessagesElement.Childs?.Count > 0)
                {
                    interactionElement.Childs.Add(MessagesElement);
                }

                interactionElements.Add(interactionElement);

            }
            return interactionElements;
        }

        private List<UMLObjectAttribute> GetObjectAttributes(List<XmlNode> slotNodes, out XmlNode associationNode)
        {
            associationNode = null;
            List<UMLObjectAttribute> objectAttributes = new List<UMLObjectAttribute>();
            foreach (XmlNode slotNode in slotNodes)
            {
                UMLObjectAttribute objectAttributeElement = new UMLObjectAttribute();
                if (slotNode.Attributes["xmi:id"] != null)
                {
                    objectAttributeElement.XmiId = slotNode.Attributes["xmi:id"].Value;
                }
                if (slotNode.Attributes["visibility"] != null)
                {
                    objectAttributeElement.Visibility = slotNode.Attributes["visibility"].Value;
                }
                if (slotNode.Attributes["definingFeature"] != null)
                {
                    string attributeReference = slotNode.Attributes["definingFeature"].Value;
                    //objectAttributeElement.Name = slotNode.Attributes["name"].Value;
                    XmlNode attributeReferenceNode = _referenceNodes.attributesReferenceNodes.Where(node => node.Attributes["xmi:id"] != null && node.Attributes["xmi:id"].Value == attributeReference).FirstOrDefault();
                    if (attributeReferenceNode != null && attributeReferenceNode.Attributes["name"] != null && attributeReferenceNode.Attributes["association"] == null)
                    {
                        objectAttributeElement.Name = attributeReferenceNode.Attributes["name"].Value;
                        XmlNode slotValueNode = slotNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "value".ToLower()).FirstOrDefault();
                        if (slotNode != null && slotValueNode.Attributes["value"] != null)
                        {
                            objectAttributeElement.Value = slotValueNode.Attributes["value"].Value;
                            objectAttributeElement.isBodyValue = false;

                        }
                        else if (slotNode != null && slotValueNode.Attributes["body"] != null)
                        {
                            objectAttributeElement.Value = slotValueNode.Attributes["body"].Value;
                            objectAttributeElement.isBodyValue = true;
                        }
                        objectAttributes.Add(objectAttributeElement);
                    }
                    else if (attributeReferenceNode != null && attributeReferenceNode.Attributes["name"] != null && attributeReferenceNode.Attributes["association"] != null)
                    {
                        associationNode = slotNode;

                    }
                }

            }
            return objectAttributes;
        }
    }

}