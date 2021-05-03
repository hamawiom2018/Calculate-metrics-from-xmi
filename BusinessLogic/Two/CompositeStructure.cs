using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models.UML;
using master_project.Utils;

namespace master_project.BusinessLogic.Two
{


    public class CompositeStructure
    {
        private ReferenceNodes _referenceNodes;
        public CompositeStructure(ReferenceNodes referenceNodes)
        {
            _referenceNodes = referenceNodes;
        }

        public List<UMLElement> GetProvidedRequired(XmlNode qualifierNode)
        {
            List<UMLElement> result = new List<UMLElement>();
            List<XmlNode> providedNodes = qualifierNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "provided").ToList();

            if (providedNodes.Count > 0)
            {
                foreach (XmlNode providedNode in providedNodes)
                {
                    UMLProvided providedElement = new UMLProvided();
                    providedElement.XmiId = providedNode.Attributes["xmi:id"]?.Value;
                    providedElement.Name = providedNode.Attributes["name"]?.Value;
                    providedElement.Visibility = providedNode.Attributes["visibility"]?.Value;

                    providedElement.Type = (int)AppEnums.Type.Provided;

                    providedElement.ProvidedRef = providedNode.Attributes["xmi:idref"]?.Value;
                    if (providedElement.ProvidedRef != null)
                    {
                        int providedType = 0;
                        providedElement.ProvidedName = XmiVersionTwoBL.GetNameAndType(providedElement.ProvidedRef, out providedType, _referenceNodes);
                        providedElement.ProvidedType = providedType;
                    }

                    //Get Connectors
                    List<UMLElement> connectors = GetConnectors(providedElement.XmiId);
                    if (connectors.Count > 0)
                    {
                        providedElement.Childs = connectors;
                    }

                    //Get InformationFlow
                    List<UMLElement> informationFlows = GetInformationFlow(providedElement.XmiId);
                    if (informationFlows.Count > 0)
                    {
                        if(providedElement.Childs==null){
                            providedElement.Childs=new List<UMLElement>();
                        }
                        providedElement.Childs.AddRange( informationFlows);
                    }
                    result.Add(providedElement);
                }
            }
            List<XmlNode> requiredNodes = qualifierNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "required").ToList();

            if (requiredNodes.Count > 0)
            {
                foreach (XmlNode requiredNode in requiredNodes)
                {
                    UMLRequired requiredElement = new UMLRequired();
                    requiredElement.XmiId = requiredNode.Attributes["xmi:id"]?.Value;
                    requiredElement.Name = requiredNode.Attributes["name"]?.Value;
                    requiredElement.Visibility = requiredNode.Attributes["visibility"]?.Value;

                    requiredElement.Type = (int)AppEnums.Type.Required;

                    requiredElement.RequiredRef = requiredNode.Attributes["xmi:idref"]?.Value;
                    if (requiredElement.RequiredRef != null)
                    {
                        int requiredType = 0;
                        requiredElement.RequiredName = XmiVersionTwoBL.GetNameAndType(requiredElement.RequiredRef, out requiredType, _referenceNodes);
                        requiredElement.RequiredType = requiredType;
                    }
                    //Get Connectors
                    List<UMLElement> connectors = GetConnectors(requiredElement.XmiId);
                    if (connectors.Count > 0)
                    {
                        requiredElement.Childs = connectors;
                    }

                    //Get InformationFlow
                    List<UMLElement> informationFlows = GetInformationFlow(requiredElement.XmiId);
                    if (informationFlows.Count > 0)
                    {
                        if(requiredElement.Childs==null){
                            requiredElement.Childs=new List<UMLElement>();
                        }
                        requiredElement.Childs.AddRange( informationFlows);
                    }
                    result.Add(requiredElement);
                }
            }
            return result;
        }

        public List<UMLElement> GetInformationFlow(string reference)
        {
            List<UMLElement> result = new List<UMLElement>();
            List<XmlNode> informationFlowNodes = _referenceNodes.informationFlowReferenceNodes.Where(node => node.Attributes["informationSource"]?.Value == reference ||
            node.Attributes["informationTarget"]?.Value == reference).ToList();

            if (informationFlowNodes.Count > 0)
            {
                foreach (XmlNode informationFlowNode in informationFlowNodes)
                {
                    UMLInformationFlow informationFlowElement = new UMLInformationFlow();
                    informationFlowElement.Type=(int)AppEnums.Type.InformationFlow;
                    informationFlowElement.XmiId = informationFlowNode.Attributes["xmi:id"]?.Value;
                    informationFlowElement.SourceRef = informationFlowNode.Attributes["informationSource"]?.Value;
                    if (informationFlowElement.SourceRef != null)
                    {
                        int sourceType = 0;
                        informationFlowElement.SourceName = XmiVersionTwoBL.GetNameAndType(reference, out sourceType, _referenceNodes);
                        informationFlowElement.SourceType = sourceType;
                    }

                    informationFlowElement.TargetRef=informationFlowNode.Attributes["informationTarget"]?.Value;
                    if(informationFlowElement.TargetRef!=null){
                         int targetType = 0;
                        informationFlowElement.TargetName = XmiVersionTwoBL.GetNameAndType(reference, out targetType, _referenceNodes);
                        informationFlowElement.TargetType = targetType;
                    }
                    result.Add(informationFlowElement);
                }
            }

            return result;

        }
        public List<UMLElement> GetConnectors(string reference)
        {
            List<UMLElement> result = new List<UMLElement>();
            List<XmlNode> connectorNodes = _referenceNodes.connectorsReferenceNodes.Where(node => node.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Attributes["role"]?.Value == reference).Any()).ToList();
            if (connectorNodes.Count > 0)
            {
                foreach (XmlNode connectorNode in connectorNodes)
                {
                    UMLConnector connectorElement = new UMLConnector();
                    connectorElement.XmiId = connectorNode.Attributes["xmi:id"]?.Value;
                    connectorElement.Name = connectorNode.Attributes["name"]?.Value;
                    connectorElement.Visibility = connectorNode.Attributes["visibility"]?.Value;
                    connectorElement.Kind = connectorNode.Attributes["kind"]?.Value;
                    connectorElement.Type = (int)AppEnums.Type.Connector;
                    List<XmlNode> endNodes = connectorNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "end").ToList();
                    if (endNodes.Count > 0)
                    {
                        connectorElement.Element1Ref = endNodes[0].Attributes["role"]?.Value;
                        if (connectorElement.Element1Ref != null)
                        {
                            int element1Type = 0;
                            connectorElement.Element1Name = XmiVersionTwoBL.GetNameAndType(connectorElement.Element1Ref, out element1Type, _referenceNodes);
                            connectorElement.Element1Type = element1Type;
                        }

                    }
                    if (endNodes.Count > 1)
                    {
                        connectorElement.Element2Ref = endNodes[1].Attributes["role"]?.Value;
                        if (connectorElement.Element2Ref != null)
                        {
                            int element2Type = 0;
                            connectorElement.Element2Name = XmiVersionTwoBL.GetNameAndType(connectorElement.Element2Ref, out element2Type, _referenceNodes);
                            connectorElement.Element2Type = element2Type;
                        }

                    }
                    result.Add(connectorElement);

                }
            }

            return result;

        }
        public UMLPort BuildPort(XmlNode portNode)
        {
            UMLPort portElement = new UMLPort();
            portElement.XmiId = portNode.Attributes["xmi:id"]?.Value;
            portElement.Name = portNode.Attributes["name"]?.Value;
            portElement.Visibility = portNode.Attributes["visibility"]?.Value;

            portElement.Type = (int)AppEnums.Type.Port;
            portElement.Aggregation = portNode.Attributes["aggregation"]?.Value;
            portElement.Childs = GetProvidedRequired(portNode);
            //Get Connectors
            List<UMLElement> connectors = GetConnectors(portElement.XmiId);
            if (connectors.Count > 0)
            {
                if (portElement.Childs == null)
                {
                    portElement.Childs = connectors;
                }
                else
                {
                    portElement.Childs.AddRange(connectors);
                }

            }
            return (portElement);
        }
        public List<UMLElement> builCompositeStructure(XmlNode classNode)
        {
            List<UMLElement> result = new List<UMLElement>();
            List<XmlNode> portNodes = classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "ownedAttribute" && node.Attributes["xmi:type"]?.Value == "uml:Port").ToList();
            if (portNodes.Count > 0)
            {
                foreach (XmlNode portNode in portNodes)
                {


                    result.Add(BuildPort(portNode));
                }
            }
            List<XmlNode> propertyNodes = classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "ownedAttribute" && node.Attributes["xmi:type"]?.Value == "uml:Property"
            && node.Attributes["aggregation"] != null).ToList();
            if (propertyNodes.Count > 0)
            {
                foreach (XmlNode propertyNode in propertyNodes)
                {
                    UMLProperty propertyElement = new UMLProperty();
                    propertyElement.XmiId = propertyNode.Attributes["xmi:id"]?.Value;
                    propertyElement.Name = propertyNode.Attributes["name"]?.Value;
                    propertyElement.Visibility = propertyNode.Attributes["visibility"]?.Value;

                    propertyElement.Type = (int)AppEnums.Type.Property;

                    string typeReference = XmiVersionTwoBL.GetTypeReference(propertyNode);
                    if (!string.IsNullOrEmpty(typeReference))
                    {
                        propertyElement.PropertyRef = typeReference;
                        int propertyTypeType = 0;
                        propertyElement.PropertyName = XmiVersionTwoBL.GetNameAndType(typeReference, out propertyTypeType, _referenceNodes);
                        propertyElement.PropertyType = propertyTypeType;
                    }

                    List<XmlNode> qualifierNodes = classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "qualifier").ToList();
                    if (qualifierNodes.Count > 0)
                    {
                        propertyElement.Childs = new List<UMLElement>();
                        foreach (XmlNode qualifierNode in qualifierNodes)
                        {
                            propertyElement.Childs.Add(BuildPort(qualifierNode));
                        }
                    }
                    //Get Connectors
                    List<UMLElement> connectors = GetConnectors(propertyElement.XmiId);
                    if (connectors.Count > 0)
                    {
                        if (propertyElement.Childs == null)
                        {
                            propertyElement.Childs = connectors;
                        }
                        else
                        {
                            propertyElement.Childs.AddRange(connectors);
                        }

                    }

                    result.Add(propertyElement);
                }

            }

            return result;


        }
    }
}