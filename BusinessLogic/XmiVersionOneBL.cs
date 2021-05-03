using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.BusinessLogic.One;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;

namespace master_project.BusinessLogic
{
    internal class XmiVersionOneBL
    {
        private ApplicationUser _identity;
        private XmlNode mainModelNode;
        private ReferenceNodes _referenceNodes;
        public XmiVersionOneBL(ApplicationUser Identity)
        {
            _identity = Identity;
        }

        public XmiVersionOneBL()
        {

        }
        private bool validateContent(string content, out XmlNode modelNode, out XmlNode contentModel)
        {
            modelNode = null;
            contentModel = null;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(content);
            XmlNodeList XMI = xDoc.GetElementsByTagName("XMI");
            if (XMI.Count == 0)
            {
                return false;

            }
            XmlNode xmlRootNode = XMI[0];
            XmlAttribute xmlAttribute = xmlRootNode.Attributes["xmi.version"];
            if (xmlAttribute == null)
            {
                return false;
            }
            string version = xmlAttribute.Value;
            if (version != "1.1" && version != "1.2")
            {
                return false;
            }

            XmlNode childContent = xmlRootNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "xmi.content").FirstOrDefault();
            if (childContent == null)
            {
                return false;
            }
            contentModel = childContent;
            modelNode = childContent.FirstChild;
            if (modelNode == null)
            {
                return false;
            }
            if (modelNode.Name.ToLower() != "uml:model")
            {
                return false;
            }
            XmlNode ownedElementNode = modelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "uml:namespace.ownedelement").FirstOrDefault();
            if (ownedElementNode == null)
            {
                return false;
            }
            modelNode = ownedElementNode;
            return true;
        }
        public bool readContent(string content, out List<UMLElement> resultElements)
        {
            XmlNode modelNode = null;
            XmlNode contentNode = null;
            resultElements = null;
            bool isValid = validateContent(content, out modelNode, out contentNode);
            mainModelNode = modelNode;
            if (isValid)
            {
                _referenceNodes = new ReferenceNodes();
                _referenceNodes.classReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:Class");
                _referenceNodes.actorReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:Actor");
                _referenceNodes.interfaceReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:Interface");
                _referenceNodes.supplierDependencyNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:ModelElement.supplierDependency");
                _referenceNodes.packageReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:Package");
                _referenceNodes.componentReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:Component");
                _referenceNodes.clientDependencyNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:ModelElement.clientDependency");
                _referenceNodes.objectsReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:ClassifierRole");
                _referenceNodes.usecaseReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:UseCase");
                _referenceNodes.simpleStateReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:SimpleState");
                _referenceNodes.pseudoStateReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:PseudoState");
                _referenceNodes.actionStateReferenceNodes = mainModelNode.OwnerDocument.GetElementsByTagName("UML:ActionState");
                _referenceNodes.dataTypes = getDataTypes(modelNode, contentNode);
                _referenceNodes.setereoTypes = GetSetereoTypes(modelNode, contentNode);

                resultElements = buildStructure(modelNode);
            }
            return isValid;
        }
        private List<UMLSetereoType> GetSetereoTypes(XmlNode modelNode, XmlNode contentNode)
        {
            List<UMLSetereoType> setereoTypes = new List<UMLSetereoType>();
            List<XmlNode> stereoTypeNodes = modelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:StereoType".ToLower()).ToList();
            foreach (XmlNode stereoTypeNode in stereoTypeNodes)
            {
                UMLSetereoType stereoTypeElelement = new UMLSetereoType();
                if (stereoTypeNode.Attributes["xmi.id"] != null)
                {
                    stereoTypeElelement.XmiId = stereoTypeNode.Attributes["xmi.id"].Value;
                }
                if (stereoTypeNode.Attributes["name"] != null)
                {
                    stereoTypeElelement.Name = stereoTypeNode.Attributes["name"].Value;
                }
                if (stereoTypeNode.Attributes["visibility"] != null)
                {
                    stereoTypeElelement.Visibility = stereoTypeNode.Attributes["visibility"].Value;
                }

                XmlNode extendedElementNode = stereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype.extendedElement".ToLower()).FirstOrDefault();
                if (extendedElementNode != null)
                {
                    List<string> ElementsRef = new List<string>();
                    foreach (XmlNode childExtendedElementNode in extendedElementNode.ChildNodes)
                    {
                        if (childExtendedElementNode.Attributes["xmi.idref"] != null)
                        {
                            ElementsRef.Add(childExtendedElementNode.Attributes["xmi.idref"].Value);
                        }


                    }
                    stereoTypeElelement.ElementsRef = ElementsRef;
                }
                setereoTypes.Add(stereoTypeElelement);

            }
            stereoTypeNodes = contentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:StereoType".ToLower()).ToList();
            foreach (XmlNode stereoTypeNode in stereoTypeNodes)
            {
                UMLSetereoType stereoTypeElelement = new UMLSetereoType();
                if (stereoTypeNode.Attributes["xmi.id"] != null)
                {
                    stereoTypeElelement.XmiId = stereoTypeNode.Attributes["xmi.id"].Value;
                }
                if (stereoTypeNode.Attributes["name"] != null)
                {
                    stereoTypeElelement.Name = stereoTypeNode.Attributes["name"].Value;
                }
                if (stereoTypeNode.Attributes["visibility"] != null)
                {
                    stereoTypeElelement.Visibility = stereoTypeNode.Attributes["visibility"].Value;
                }

                XmlNode extendedElementNode = stereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype.extendedElement".ToLower()).FirstOrDefault();
                if (extendedElementNode != null)
                {
                    List<string> ElementsRef = new List<string>();
                    foreach (XmlNode childExtendedElementNode in extendedElementNode.ChildNodes)
                    {
                        if (childExtendedElementNode.Attributes["xmi.idref"] != null)
                        {
                            ElementsRef.Add(childExtendedElementNode.Attributes["xmi.idref"].Value);
                        }


                    }
                    stereoTypeElelement.ElementsRef = ElementsRef;
                }
                setereoTypes.Add(stereoTypeElelement);

            }
            return setereoTypes;

        }
        private List<UMLDataType> getDataTypes(XmlNode modelNode, XmlNode contentNode)
        {
            List<UMLDataType> dataTypes = new List<UMLDataType>();
            List<XmlNode> dataTypeNodes = modelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:DataType".ToLower()).ToList();
            foreach (XmlNode dataTypeNode in dataTypeNodes)
            {
                UMLDataType dataTypeElelement = new UMLDataType();
                if (dataTypeNode.Attributes["xmi.id"] != null)
                {
                    dataTypeElelement.XmiId = dataTypeNode.Attributes["xmi.id"].Value;
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
            dataTypeNodes = contentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:DataType".ToLower()).ToList();
            foreach (XmlNode dataTypeNode in dataTypeNodes)
            {
                UMLDataType dataTypeElelement = new UMLDataType();
                if (dataTypeNode.Attributes["xmi.id"] != null)
                {
                    dataTypeElelement.XmiId = dataTypeNode.Attributes["xmi.id"].Value;
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
        private List<UMLOperation> GetOperations(List<XmlNode> operationNodes, List<UMLDataType> dataTypes)
        {
            List<UMLOperation> umlOperations = new List<UMLOperation>();
            foreach (XmlNode operationNode in operationNodes)
            {
                UMLOperation umlOperation = new UMLOperation();
                if (operationNode.Attributes["xmi.id"] != null)
                {
                    umlOperation.XmiId = operationNode.Attributes["xmi.id"].Value;
                }
                if (operationNode.Attributes["name"] != null)
                {
                    umlOperation.Name = operationNode.Attributes["name"].Value;
                }
                if (operationNode.Attributes["visibility"] != null)
                {
                    umlOperation.Visibility = operationNode.Attributes["visibility"].Value;
                }

                XmlNode behaviouralFeaturesNode = operationNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:BehavioralFeature.parameter".ToLower()).FirstOrDefault();

                if (behaviouralFeaturesNode != null)
                {
                    List<UMLOperationParameter> operationParameters = new List<UMLOperationParameter>();
                    foreach (XmlNode parameterNode in behaviouralFeaturesNode.ChildNodes)
                    {
                        if (parameterNode.Name.ToLower() == "UML:Parameter".ToLower())
                        {
                            //get parameter kind
                            string kind = "";
                            if (parameterNode.Attributes["kind"] != null)
                            {
                                kind = parameterNode.Attributes["kind"].Value;
                            }
                            else
                            {
                                XmlNode kindNode = parameterNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Parameter.kind".ToLower()).FirstOrDefault();
                                if (kindNode != null && kindNode.Attributes["xmi.value"] != null)
                                {
                                    kind = kindNode.Attributes["xmi.value"].Value;
                                }
                            }
                            if (kind != "")
                            {
                                if (kind == "return")
                                {
                                    //Get return Data Type
                                    XmlNode parameterTypeNode = parameterNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Parameter.type".ToLower()).FirstOrDefault();
                                    if (parameterTypeNode != null && parameterTypeNode.HasChildNodes)
                                    {
                                        XmlNode parameterTypeReferenceNode = parameterTypeNode.FirstChild;
                                        if (parameterTypeReferenceNode.Attributes["xmi.idref"] != null)
                                        {
                                            string parameterTypeReference = parameterTypeReferenceNode.Attributes["xmi.idref"].Value;
                                            UMLDataType dataType = dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == parameterTypeReference).FirstOrDefault();
                                            if (dataType != null)
                                            {
                                                umlOperation.IsClassDataType = false;
                                                umlOperation.DataTypeName = dataType.Name;
                                            }
                                            else
                                            {
                                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == parameterTypeReference).FirstOrDefault();
                                                if (classReferenceNode != null)
                                                {
                                                    umlOperation.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                                    umlOperation.IsClassDataType = true;

                                                }
                                            }
                                        }

                                    }

                                }
                                else
                                {
                                    XmlNode parameterTypeNode = parameterNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Parameter.type".ToLower()).FirstOrDefault();
                                    UMLOperationParameter umlOperationParameter = new UMLOperationParameter();
                                    if (parameterNode.Attributes["xmi.id"] != null)
                                    {
                                        umlOperationParameter.XmiId = parameterNode.Attributes["xmi.id"].Value;
                                    }
                                    if (parameterNode.Attributes["name"] != null)
                                    {
                                        umlOperationParameter.Name = parameterNode.Attributes["name"].Value;
                                    }
                                    if (parameterNode.Attributes["visibility"] != null)
                                    {
                                        umlOperationParameter.Visibility = parameterNode.Attributes["visibility"].Value;
                                    }
                                    umlOperationParameter.Kind = kind;
                                    if (parameterTypeNode != null && parameterTypeNode.HasChildNodes)
                                    {
                                        XmlNode parameterTypeReferenceNode = parameterTypeNode.FirstChild;
                                        if (parameterTypeReferenceNode.Attributes["xmi.idref"] != null)
                                        {
                                            string parameterTypeReference = parameterTypeReferenceNode.Attributes["xmi.idref"].Value;
                                            UMLDataType dataType = dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == parameterTypeReference).FirstOrDefault();

                                            if (dataType != null)
                                            {
                                                umlOperationParameter.IsClassDataType = false;
                                                umlOperationParameter.DataTypeName = dataType.Name;
                                            }
                                            else
                                            {
                                                XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == parameterTypeReference).FirstOrDefault();
                                                if (classReferenceNode != null)
                                                {
                                                    umlOperationParameter.DataTypeName = classReferenceNode.Name;
                                                    umlOperationParameter.IsClassDataType = true;

                                                }
                                            }
                                        }

                                    }
                                    operationParameters.Add(umlOperationParameter);
                                }
                            }

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
        public static string GetNameAndType(string reference, out int type,
        ReferenceNodes referenceNodes)
        {
            string name = "";
            type = 0;
            XmlNode classReferenceNode = referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
            if (classReferenceNode != null)
            {
                name = classReferenceNode.Attributes["name"]?.Value;
                type = (int)AppEnums.Type.Class;
            }
            else
            {
                XmlNode interfaceReferenceNode = referenceNodes.interfaceReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                if (interfaceReferenceNode != null)
                {
                    name = interfaceReferenceNode.Attributes["name"]?.Value;
                    type = (int)AppEnums.Type.Interface;
                }
                else
                {
                    XmlNode objectReferenceNode = referenceNodes.objectsReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                    if (objectReferenceNode != null)
                    {
                        name = objectReferenceNode.Attributes["name"]?.Value;
                        type = (int)AppEnums.Type.Object;
                    }
                    else
                    {
                        XmlNode packageReferenceNode = referenceNodes.packageReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                        if (packageReferenceNode != null)
                        {
                            name = packageReferenceNode.Attributes["name"]?.Value;
                            type = (int)AppEnums.Type.Package;
                        }
                        else
                        {
                            XmlNode actorReferenceNode = referenceNodes.actorReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                            if (actorReferenceNode != null)
                            {
                                name = actorReferenceNode.Attributes["name"]?.Value;
                                type = (int)AppEnums.Type.Actor;
                            }
                            else
                            {
                                XmlNode usecaseReferenceNode = referenceNodes.usecaseReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                                if (usecaseReferenceNode != null)
                                {
                                    name = usecaseReferenceNode.Attributes["name"]?.Value;
                                    type = (int)AppEnums.Type.UseCase;
                                }
                                else
                                {
                                    XmlNode componetReferenceNode = referenceNodes.componentReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                                    if (componetReferenceNode != null)
                                    {
                                        name = componetReferenceNode.Attributes["name"]?.Value;
                                        type = (int)AppEnums.Type.Component;
                                    }
                                    else
                                    {
                                        XmlNode simpleStateReferenceNode = referenceNodes.simpleStateReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                                        if (simpleStateReferenceNode != null)
                                        {
                                            name = simpleStateReferenceNode.Attributes["name"]?.Value;
                                            type = (int)AppEnums.Type.SimpleState;
                                        }
                                        else
                                        {
                                            XmlNode pseudoStateReferenceNode = referenceNodes.pseudoStateReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                                            if (pseudoStateReferenceNode != null)
                                            {
                                                name = pseudoStateReferenceNode.Attributes["name"]?.Value;
                                                type = (int)AppEnums.Type.PseudoState;
                                            }
                                            else
                                            {
                                                XmlNode actionStateReferenceNode = referenceNodes.actionStateReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == reference).FirstOrDefault();
                                                if (actionStateReferenceNode != null)
                                                {
                                                    name = actionStateReferenceNode.Attributes["name"]?.Value;
                                                    type = (int)AppEnums.Type.ActionState;
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
        private List<UMLAttribute> GetAttributes(List<XmlNode> attributeNodes, List<UMLDataType> dataTypes)
        {
            List<UMLAttribute> umlAttributes = new List<UMLAttribute>();
            foreach (XmlNode attributeNode in attributeNodes)
            {
                UMLAttribute umlAttribute = new UMLAttribute();
                if (attributeNode.Attributes["xmi.id"] != null)
                {
                    umlAttribute.XmiId = attributeNode.Attributes["xmi.id"].Value;
                }
                if (attributeNode.Attributes["name"] != null)
                {
                    umlAttribute.Name = attributeNode.Attributes["name"].Value;
                }
                if (attributeNode.Attributes["visibility"] != null)
                {
                    umlAttribute.Visibility = attributeNode.Attributes["visibility"].Value;
                }
                //Get DataType
                XmlNode structuralFeatureNode = attributeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:StructuralFeature.type".ToLower()).FirstOrDefault();
                if (structuralFeatureNode != null && structuralFeatureNode.HasChildNodes)
                {
                    XmlNode attributeTypeNode = structuralFeatureNode.FirstChild;
                    if (attributeTypeNode.Attributes["xmi.idref"] != null)
                    {
                        string typeReference = attributeTypeNode.Attributes["xmi.idref"].Value;
                        //check from dataTypes list
                        UMLDataType dataType = dataTypes.Where(dataTypeElement => dataTypeElement.XmiId == typeReference).FirstOrDefault();
                        if (dataType != null)
                        {
                            umlAttribute.IsClassDataType = false;
                            umlAttribute.DataTypeName = dataType.Name;
                        }
                        else
                        {
                            XmlNode classReferenceNode = _referenceNodes.classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == typeReference).FirstOrDefault();
                            if (classReferenceNode != null)
                            {
                                umlAttribute.DataTypeName = classReferenceNode.Attributes["name"].Value;
                                umlAttribute.IsClassDataType = true;

                            }
                        }

                    }
                }
                umlAttributes.Add(umlAttribute);
            }
            return umlAttributes;
        }
        private List<UMLElement> GetInnerClasses(List<XmlNode> innerClassNodes)
        {
            List<UMLElement> innerClasses = new List<UMLElement>();
            foreach (XmlNode classNode in innerClassNodes)
            {
                if (classNode.Name.ToLower() == "uml:class")
                {
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.InnerClass;
                    if (classNode.Attributes["xmi.id"] != null)
                    {
                        classElelement.XmiId = classNode.Attributes["xmi.id"].Value;
                    }
                    if (classNode.Attributes["name"] != null)
                    {
                        classElelement.Name = classNode.Attributes["name"].Value;
                    }
                    if (classNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = classNode.Attributes["visibility"].Value;
                    }
                    if (classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault() != null)
                    {
                        XmlNode featuresNode = classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault();
                        //check inner classes
                        List<XmlNode> innerClassesNode = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Class".ToLower()).ToList();
                        if (innerClassesNode.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassesNode);
                        }

                        //ChildNodes
                        //Attributes
                        List<XmlNode> attributeNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Attribute".ToLower()).ToList();
                        if (attributeNodes.Count > 0)
                        {

                            classElelement.Attributes = GetAttributes(attributeNodes, _referenceNodes.dataTypes);
                        }

                        //Operations
                        List<XmlNode> operationNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Operation".ToLower()).ToList();
                        if (operationNodes.Count > 0)
                        {

                            classElelement.Operations = GetOperations(operationNodes, _referenceNodes.dataTypes);
                        }

                    }
                    if (classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                        {
                            classElelement.Childs.AddRange(childElements);
                        }
                        else
                        {
                            classElelement.Childs = childElements;
                        }
                    }
                    innerClasses.Add(classElelement);
                }
                else if (classNode.Name.ToLower() == "UML:Interface".ToLower())
                {
                    //build the class
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.InnerInterface;

                    if (classNode.Attributes["xmi.id"] != null)
                    {
                        classElelement.XmiId = classNode.Attributes["xmi.id"].Value;
                    }
                    if (classNode.Attributes["name"] != null)
                    {
                        classElelement.Name = classNode.Attributes["name"].Value;
                    }
                    if (classNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = classNode.Attributes["visibility"].Value;
                    }
                    if (classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault() != null)
                    {
                        XmlNode featuresNode = classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault();
                        //check inner classes
                        List<XmlNode> innerClassesNode = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Class".ToLower() || node.Name.ToLower() == "UML:Interface".ToLower()).ToList();
                        if (innerClassesNode.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassesNode);
                        }

                        //ChildNodes
                        //Attributes
                        List<XmlNode> attributeNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Attribute".ToLower()).ToList();
                        if (attributeNodes.Count > 0)
                        {

                            classElelement.Attributes = GetAttributes(attributeNodes, _referenceNodes.dataTypes);
                        }

                        //Operations
                        List<XmlNode> operationNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Operation".ToLower()).ToList();
                        if (operationNodes.Count > 0)
                        {

                            classElelement.Operations = GetOperations(operationNodes, _referenceNodes.dataTypes);
                        }

                    }
                    if (classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(classNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        if (classElelement.Childs != null && classElelement.Childs.Count > 0)
                        {
                            classElelement.Childs.AddRange(childElements);
                        }
                        else
                        {
                            classElelement.Childs = childElements;
                        }
                    }

                    innerClasses.Add(classElelement);
                    //classItem.Attributes
                    //result.Add(classItem);
                }
            }
            return innerClasses;
        }
        private List<UMLElement> buildStructure(XmlNode modelNode)
        {
            List<UMLElement> result = new List<UMLElement>();
            foreach (XmlNode childNode in modelNode)
            {
                if (childNode.Name.ToLower() == "uml:class")
                {
                    //build the class
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.Class;
                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        classElelement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        classElelement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault() != null)
                    {
                        XmlNode featuresNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault();
                        //check inner classes
                        List<XmlNode> innerClassesNode = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Class".ToLower() || node.Name.ToLower() == "UML:Interface".ToLower()).ToList();
                        if (innerClassesNode.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassesNode);
                        }

                        //ChildNodes
                        //Attributes
                        List<XmlNode> attributeNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Attribute".ToLower()).ToList();
                        if (attributeNodes.Count > 0)
                        {

                            classElelement.Attributes = GetAttributes(attributeNodes, _referenceNodes.dataTypes);
                        }

                        //Operations
                        List<XmlNode> operationNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Operation".ToLower()).ToList();
                        if (operationNodes.Count > 0)
                        {

                            classElelement.Operations = GetOperations(operationNodes, _referenceNodes.dataTypes);
                        }

                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
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
                    //classItem.Attributes
                    //result.Add(classItem);
                }
                else if (childNode.Name.ToLower() == "UML:Component".ToLower())
                {
                    //build the class
                    UMLClass componentElelement = new UMLClass();
                    componentElelement.Type = (int)AppEnums.Type.Component;
                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        componentElelement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        componentElelement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        componentElelement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault() != null)
                    {
                        XmlNode featuresNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault();
                        //check inner classes
                        List<XmlNode> innerClassesNode = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Class".ToLower() || node.Name.ToLower() == "UML:Interface".ToLower()).ToList();
                        if (innerClassesNode.Count > 0)
                        {
                            componentElelement.Childs = GetInnerClasses(innerClassesNode);
                        }

                        //ChildNodes
                        //Attributes
                        List<XmlNode> attributeNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Attribute".ToLower()).ToList();
                        if (attributeNodes.Count > 0)
                        {

                            componentElelement.Attributes = GetAttributes(attributeNodes, _referenceNodes.dataTypes);
                        }

                        //Operations
                        List<XmlNode> operationNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Operation".ToLower()).ToList();
                        if (operationNodes.Count > 0)
                        {

                            componentElelement.Operations = GetOperations(operationNodes, _referenceNodes.dataTypes);
                        }

                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        if (componentElelement.Childs != null && componentElelement.Childs.Count > 0)
                        {
                            componentElelement.Childs.AddRange(childElements);
                        }
                        else
                        {
                            componentElelement.Childs = childElements;
                        }

                    }
                    result.Add(componentElelement);
                    //classItem.Attributes
                    //result.Add(classItem);
                }
                else if (childNode.Name.ToLower() == "UML:Interface".ToLower())
                {
                    //build the class
                    UMLClass classElelement = new UMLClass();
                    classElelement.Type = (int)AppEnums.Type.Interface;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        classElelement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        classElelement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        classElelement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault() != null)
                    {
                        XmlNode featuresNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier.feature".ToLower()).FirstOrDefault();
                        //check inner classes
                        List<XmlNode> innerClassesNode = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Class".ToLower() || node.Name.ToLower() == "UML:Interface".ToLower()).ToList();
                        if (innerClassesNode.Count > 0)
                        {
                            classElelement.Childs = GetInnerClasses(innerClassesNode);
                        }

                        //ChildNodes
                        //Attributes
                        List<XmlNode> attributeNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Attribute".ToLower()).ToList();
                        if (attributeNodes.Count > 0)
                        {

                            classElelement.Attributes = GetAttributes(attributeNodes, _referenceNodes.dataTypes);
                        }

                        //Operations
                        List<XmlNode> operationNodes = featuresNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Operation".ToLower()).ToList();
                        if (operationNodes.Count > 0)
                        {

                            classElelement.Operations = GetOperations(operationNodes, _referenceNodes.dataTypes);
                        }

                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
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
                    //classItem.Attributes
                    //result.Add(classItem);
                }
                else if (childNode.Name.ToLower() == "UML:Package".ToLower())
                {
                    UMLPackage packageElement = new UMLPackage();
                    packageElement.Type = (int)AppEnums.Type.Package;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        packageElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        packageElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        packageElement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        packageElement.Childs = childElements;
                    }

                    result.Add(packageElement);
                }
                else if (childNode.Name.ToLower() == "UML:Association".ToLower())
                {

                    UMLAssociation associationElement = new UMLAssociation();
                    associationElement.Type = (int)AppEnums.Type.Association;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        associationElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        associationElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        associationElement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    XmlNode associationConnectionNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Association.connection".ToLower()).FirstOrDefault();
                    string associationStereoType = "";
                    string class1Ref = "", class2Ref = "";
                    if (associationConnectionNode != null)
                    {
                        List<XmlNode> associationEndNodes = associationConnectionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd".ToLower()).ToList();
                        if (associationEndNodes.Count > 0)
                        {
                            if (associationEndNodes[0].Attributes["type"] != null)
                            {
                                class1Ref = associationEndNodes[0].Attributes["type"].Value;

                            }
                            else
                            {
                                XmlNode typeRefNode = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.type".ToLower()).FirstOrDefault();
                                if (typeRefNode != null)
                                {
                                    XmlNode typeRefClassNode = typeRefNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier".ToLower()).FirstOrDefault();
                                    if (typeRefClassNode != null && typeRefClassNode.Attributes["xmi.idref"] != null)
                                    {
                                        class1Ref = typeRefClassNode.Attributes["xmi.idref"].Value;

                                    }
                                }
                            }

                            XmlNode associationEndStereoType = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (associationEndStereoType != null)
                            {
                                XmlNode associationEndStereoTypeChildNode = associationEndStereoType.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (associationEndStereoTypeChildNode != null && associationEndStereoTypeChildNode.Attributes["name"] != null)
                                {
                                    associationStereoType = associationEndStereoTypeChildNode.Attributes["name"].Value;

                                }
                            }
                            if (associationEndNodes[0].Attributes["aggregation"] != null)
                            {
                                associationElement.Element1Aggregation = associationEndNodes[0].Attributes["aggregation"].Value;

                            }
                            else
                            {
                                XmlNode aggregationNode = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.aggregation".ToLower()).FirstOrDefault();
                                if (aggregationNode != null && aggregationNode.Attributes["xmi.value"] != null)
                                {
                                    associationElement.Element1Aggregation = aggregationNode.Attributes["xmi.value"].Value;
                                }
                            }
                            if (associationEndNodes[0].Attributes["multiplicity"] != null)
                            {
                                associationElement.Element1Multiplicity = associationEndNodes[0].Attributes["multiplicity"].Value;

                            }
                            else
                            {
                                XmlNode multiplicityNode = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.multiplicity".ToLower()).FirstOrDefault();
                                if (multiplicityNode != null)
                                {

                                    // XmlNode multiplicityRangeNode= multiplicityNode.SelectSingleNode("UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange");
                                    XmlNode multiplicityChildNode = multiplicityNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity".ToLower()).FirstOrDefault();
                                    if (multiplicityChildNode != null)
                                    {
                                        XmlNode multiplicityRangeNode = multiplicityChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity.range".ToLower()).FirstOrDefault();
                                        if (multiplicityRangeNode != null)
                                        {
                                            XmlNode multiplicityRenageChildNode = multiplicityRangeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange".ToLower()).FirstOrDefault();
                                            if (multiplicityRenageChildNode != null)
                                            {
                                                if (multiplicityRenageChildNode.Attributes["lower"] != null && multiplicityRenageChildNode.Attributes["upper"] != null)
                                                {
                                                    string lower = multiplicityRenageChildNode.Attributes["lower"].Value;
                                                    string upper = multiplicityRenageChildNode.Attributes["upper"].Value;
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
                                                        associationElement.Element1Multiplicity = lower;
                                                    }
                                                    else
                                                    {
                                                        associationElement.Element1Multiplicity = lower + "..." + upper;
                                                    }

                                                }
                                                else
                                                {
                                                    XmlNode multiplicityRenageChildLowerNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.lower".ToLower()).FirstOrDefault();
                                                    XmlNode multiplicityRenageChildUpperNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.upper".ToLower()).FirstOrDefault();
                                                    if (multiplicityRenageChildLowerNode != null && multiplicityRenageChildUpperNode != null)
                                                    {
                                                        string lower = multiplicityRenageChildLowerNode.InnerText;
                                                        string upper = multiplicityRenageChildUpperNode.InnerText;
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
                                                            associationElement.Element1Multiplicity = lower;
                                                        }
                                                        else
                                                        {
                                                            associationElement.Element1Multiplicity = lower + "..." + upper;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                            }

                        }
                        if (associationEndNodes.Count > 1)
                        {

                            if (associationEndNodes[1].Attributes["type"] != null)
                            {
                                class2Ref = associationEndNodes[1].Attributes["type"].Value;


                            }
                            else
                            {
                                XmlNode typeRefNode = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.type".ToLower()).FirstOrDefault();
                                if (typeRefNode != null)
                                {
                                    XmlNode typeRefClassNode = typeRefNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Classifier".ToLower()).FirstOrDefault();
                                    if (typeRefClassNode != null && typeRefClassNode.Attributes["xmi.idref"] != null)
                                    {
                                        class2Ref = typeRefClassNode.Attributes["xmi.idref"].Value;

                                    }
                                }
                            }

                            XmlNode associationEndStereoType = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (associationEndStereoType != null)
                            {
                                XmlNode associationEndStereoTypeChildNode = associationEndStereoType.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (associationEndStereoTypeChildNode != null && associationEndStereoTypeChildNode.Attributes["name"] != null)
                                {
                                    associationStereoType = associationEndStereoTypeChildNode.Attributes["name"].Value;

                                }
                            }
                            if (associationEndNodes[1].Attributes["aggregation"] != null)
                            {
                                associationElement.Element2Aggregation = associationEndNodes[1].Attributes["aggregation"].Value;

                            }
                            else
                            {
                                XmlNode aggregationNode = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.aggregation".ToLower()).FirstOrDefault();
                                if (aggregationNode != null && aggregationNode.Attributes["xmi.value"] != null)
                                {
                                    associationElement.Element2Aggregation = aggregationNode.Attributes["xmi.value"].Value;
                                }
                            }
                            if (associationEndNodes[1].Attributes["multiplicity"] != null)
                            {
                                associationElement.Element2Multiplicity = associationEndNodes[1].Attributes["multiplicity"].Value;

                            }
                            else
                            {
                                XmlNode multiplicityNode = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.multiplicity".ToLower()).FirstOrDefault();
                                if (multiplicityNode != null)
                                {

                                    // XmlNode multiplicityRangeNode= multiplicityNode.SelectSingleNode("UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange");
                                    XmlNode multiplicityChildNode = multiplicityNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity".ToLower()).FirstOrDefault();
                                    if (multiplicityChildNode != null)
                                    {
                                        XmlNode multiplicityRangeNode = multiplicityChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity.range".ToLower()).FirstOrDefault();
                                        if (multiplicityRangeNode != null)
                                        {
                                            XmlNode multiplicityRenageChildNode = multiplicityRangeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange".ToLower()).FirstOrDefault();
                                            if (multiplicityRenageChildNode != null)
                                            {
                                                if (multiplicityRenageChildNode.Attributes["lower"] != null && multiplicityRenageChildNode.Attributes["upper"] != null)
                                                {
                                                    string lower = multiplicityRenageChildNode.Attributes["lower"].Value;
                                                    string upper = multiplicityRenageChildNode.Attributes["upper"].Value;
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
                                                        associationElement.Element2Multiplicity = lower;
                                                    }
                                                    else
                                                    {
                                                        associationElement.Element2Multiplicity = lower + "..." + upper;
                                                    }

                                                }
                                                else
                                                {
                                                    XmlNode multiplicityRenageChildLowerNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.lower".ToLower()).FirstOrDefault();
                                                    XmlNode multiplicityRenageChildUpperNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.upper".ToLower()).FirstOrDefault();
                                                    if (multiplicityRenageChildLowerNode != null && multiplicityRenageChildUpperNode != null)
                                                    {
                                                        string lower = multiplicityRenageChildLowerNode.InnerText;
                                                        string upper = multiplicityRenageChildUpperNode.InnerText;
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
                                                            associationElement.Element2Multiplicity = lower;
                                                        }
                                                        else
                                                        {
                                                            associationElement.Element2Multiplicity = lower + "..." + upper;
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
                    if (associationStereoType != "")
                    {
                        associationElement.StereoType = associationStereoType;
                    }
                    else
                    {
                        UMLSetereoType setereoType = _referenceNodes.setereoTypes.Where(setereoTypeItem => setereoTypeItem.ElementsRef != null && setereoTypeItem.ElementsRef.Where(elementref => elementref == associationElement.XmiId).Any()).FirstOrDefault();
                        if (setereoType != null)
                        {
                            associationElement.StereoType = setereoType.Name;
                        }
                        else
                        {
                            XmlNode modelElementStereoTypeNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (modelElementStereoTypeNode != null)
                            {
                                XmlNode steroTypeNode = modelElementStereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (steroTypeNode != null)
                                {
                                    if (steroTypeNode.Attributes["name"] != null)
                                    {
                                        associationElement.StereoType = steroTypeNode.Attributes["name"].Value;
                                    }
                                    else if (steroTypeNode.Attributes["xmi.idref"] != null)
                                    {
                                        UMLSetereoType setereoTypeElement = _referenceNodes.setereoTypes.Where(stereoTypeItem => stereoTypeItem.XmiId == steroTypeNode.Attributes["xmi.idref"].Value).FirstOrDefault();
                                        if (setereoTypeElement != null)
                                        {
                                            associationElement.StereoType = setereoTypeElement.Name;

                                        }
                                    }

                                }
                            }
                        }

                    }

                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        associationElement.Childs = childElements;
                    }
                    if (class1Ref != "" && class2Ref != "")
                    {
                        int element1Type = 0;
                        associationElement.Element1Ref = class1Ref;
                        associationElement.Element1Name = GetNameAndType(class1Ref, out element1Type, _referenceNodes
                        );
                        associationElement.Element1Type = element1Type;

                        int element2Type = 0;
                        associationElement.Element2Ref = class2Ref;
                        associationElement.Element2Name = GetNameAndType(class2Ref, out element2Type, _referenceNodes);
                        associationElement.Element2Type = element2Type;



                    }

                    result.Add(associationElement);


                }
                else if (childNode.Name.ToLower() == "UML:Dependency".ToLower())
                {
                    UMLDependency dependencyElement = new UMLDependency();
                    dependencyElement.Type = (int)AppEnums.Type.Dependency;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        dependencyElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        dependencyElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        dependencyElement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    string clientReference = "";
                    string supplierReference = "";

                    if (childNode.Attributes["client"] != null)
                    {
                        clientReference = childNode.Attributes["client"].Value;
                    }
                    else if (_referenceNodes.clientDependencyNodes.Count > 0)
                    {
                        XmlNode clientDependencyNode = _referenceNodes.clientDependencyNodes.Cast<XmlNode>().Where(node => node.ChildNodes.Cast<XmlNode>().Where(childeNodeInner => childeNodeInner.Attributes["xmi.idref"] != null && childeNodeInner.Attributes["xmi.idref"].Value == dependencyElement.XmiId).Any()).FirstOrDefault();
                        if (clientDependencyNode != null)
                        {
                            if (clientDependencyNode.ParentNode.Attributes["xmi.id"] != null)
                            {
                                clientReference = clientDependencyNode.ParentNode.Attributes["xmi.id"].Value;
                            }
                        }
                    }

                    if (childNode.Attributes["supplier"] != null)
                    {
                        supplierReference = childNode.Attributes["supplier"].Value;

                    }
                    else if (_referenceNodes.supplierDependencyNodes.Count > 0)
                    {

                        XmlNode supplierDependencyNode = _referenceNodes.supplierDependencyNodes.Cast<XmlNode>().Where(node => node.ChildNodes.Cast<XmlNode>().Where(childeNodeInner => childeNodeInner.Attributes["xmi.idref"] != null && childeNodeInner.Attributes["xmi.idref"].Value == dependencyElement.XmiId).Any()).FirstOrDefault();
                        if (supplierDependencyNode != null)
                        {
                            if (supplierDependencyNode.ParentNode.Attributes["xmi.id"] != null)
                            {
                                supplierReference = supplierDependencyNode.ParentNode.Attributes["xmi.id"].Value;

                            }
                        }
                    }

                    //check stereoType
                    UMLSetereoType setereoType = _referenceNodes.setereoTypes.Where(setereoTypeItem => setereoTypeItem.ElementsRef != null && setereoTypeItem.ElementsRef.Where(elementref => elementref == dependencyElement.XmiId).Any()).FirstOrDefault();
                    if (setereoType != null)
                    {
                        dependencyElement.StereoType = setereoType.Name;
                    }
                    else
                    {
                        XmlNode modelElementStereoTypeNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                        if (modelElementStereoTypeNode != null)
                        {
                            XmlNode steroTypeNode = modelElementStereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                            if (steroTypeNode != null)
                            {
                                if (steroTypeNode.Attributes["name"] != null)
                                {
                                    dependencyElement.StereoType = steroTypeNode.Attributes["name"].Value;
                                }
                                else if (steroTypeNode.Attributes["xmi.idref"] != null)
                                {
                                    UMLSetereoType setereoTypeElement = _referenceNodes.setereoTypes.Where(stereoTypeItem => stereoTypeItem.XmiId == steroTypeNode.Attributes["xmi.idref"].Value).FirstOrDefault();
                                    if (setereoTypeElement != null)
                                    {
                                        dependencyElement.StereoType = setereoTypeElement.Name;

                                    }
                                }

                            }
                        }
                    }
                    if (clientReference != "" && supplierReference != "")
                    {
                        int clientType = 0;
                        dependencyElement.ClientRef = clientReference;
                        dependencyElement.ClientName = GetNameAndType(clientReference, out clientType, _referenceNodes);
                        dependencyElement.ClientType = clientType;

                        int supplierType = 0;
                        dependencyElement.SupplierRef = supplierReference;
                        dependencyElement.SupplierName = GetNameAndType(supplierReference, out supplierType, _referenceNodes);
                        dependencyElement.SupplierType = supplierType;


                    }

                    result.Add(dependencyElement);



                }
                else if (childNode.Name.ToLower() == "UML:Generalization".ToLower())
                {
                    UMLGeneralization generalizationElement = new UMLGeneralization();
                    generalizationElement.Type = (int)AppEnums.Type.Generalization;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        generalizationElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        generalizationElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        generalizationElement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    string parentRef = "", childRef = "";
                    if (childNode.Attributes["subtype"] != null)
                    {
                        childRef = childNode.Attributes["subtype"].Value;

                    }
                    else
                    {
                        XmlNode generalizationChildNode = childNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Generalization.child".ToLower()).FirstOrDefault();
                        if (generalizationChildNode != null)
                        {
                            XmlNode generalizationChildNodeChild = generalizationChildNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:GeneralizableElement".ToLower()).FirstOrDefault();
                            if (generalizationChildNodeChild != null)
                            {
                                if (generalizationChildNodeChild.Attributes["xmi.idref"] != null)
                                {
                                    childRef = generalizationChildNodeChild.Attributes["xmi.idref"].Value;
                                }
                            }
                            else
                            {
                                if (generalizationChildNode.HasChildNodes)
                                {
                                    generalizationChildNodeChild = generalizationChildNode.FirstChild;
                                    if (generalizationChildNodeChild.Attributes["xmi.idref"] != null)
                                    {
                                        childRef = generalizationChildNodeChild.Attributes["xmi.idref"].Value;
                                    }

                                }
                            }
                        }
                    }

                    if (childNode.Attributes["supertype"] != null)
                    {
                        parentRef = childNode.Attributes["supertype"].Value;
                    }
                    else
                    {
                        XmlNode generalizationParentNode = childNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Generalization.parent".ToLower()).FirstOrDefault();
                        if (generalizationParentNode != null)
                        {
                            XmlNode generalizationParentNodeChild = generalizationParentNode.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:GeneralizableElement".ToLower()).FirstOrDefault();
                            if (generalizationParentNodeChild != null)
                            {
                                if (generalizationParentNodeChild.Attributes["xmi.idref"] != null)
                                {
                                    parentRef = generalizationParentNodeChild.Attributes["xmi.idref"].Value;
                                }
                            }
                            else
                            {
                                if (generalizationParentNode.HasChildNodes)
                                {
                                    generalizationParentNodeChild = generalizationParentNode.FirstChild;
                                    if (generalizationParentNodeChild.Attributes["xmi.idref"] != null)
                                    {
                                        parentRef = generalizationParentNodeChild.Attributes["xmi.idref"].Value;


                                    }

                                }
                            }


                        }
                    }
                    if (parentRef != "" && childRef != "")
                    {
                        generalizationElement.ParentRef = parentRef;
                        int parentType = 0;
                        generalizationElement.ParentName = GetNameAndType(parentRef, out parentType, _referenceNodes);
                        generalizationElement.ParentType = parentType;
                        int childType = 0;
                        generalizationElement.ChildRef = childRef;
                        generalizationElement.ChildName = GetNameAndType(childRef, out childType, _referenceNodes);
                        generalizationElement.ChildType = childType;
                    }




                    result.Add(generalizationElement);

                }
                else if (childNode.Name.ToLower() == "UML:Collaboration".ToLower())
                {
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        result.AddRange(childElements);
                        //associationElement.Childs = childElements;
                    }
                    XmlNode interactionNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Collaboration.interaction".ToLower()).FirstOrDefault();
                    if (interactionNode != null)
                    {
                        List<UMLElement> childElements = buildStructure(interactionNode);
                        result.AddRange(childElements);
                    }
                }
                else if (childNode.Name.ToLower() == "UML:Interaction".ToLower())
                {
                    UMLElement interactionElement = new UMLElement();
                    interactionElement.Type = (int)AppEnums.Type.Interaction;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        interactionElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    interactionElement.Name = "";
                    if (childNode.Attributes["name"] != null)
                    {
                        //interactionElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        interactionElement.Visibility = childNode.Attributes["visibility"].Value;
                    }

                    XmlNode interactionMessageNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Interaction.message".ToLower()).FirstOrDefault();
                    if (interactionMessageNode != null)
                    {
                        interactionElement.Childs = new List<UMLElement>();
                        List<XmlNode> messageNodes = interactionMessageNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Message".ToLower()).ToList();
                        List<string> lifeLineRefs = new List<string>();

                        foreach (XmlNode messageNode in messageNodes)
                        {
                            if (messageNode.Attributes["sender"] != null)
                            {
                                if (lifeLineRefs.Where(lifeLineRef => lifeLineRef == messageNode.Attributes["sender"].Value).ToList().Count == 0)
                                {
                                    lifeLineRefs.Add(messageNode.Attributes["sender"].Value);
                                }

                            }
                            if (messageNode.Attributes["receiver"] != null)
                            {
                                if (lifeLineRefs.Where(lifeLineRef => lifeLineRef == messageNode.Attributes["receiver"].Value).ToList().Count == 0)
                                {
                                    lifeLineRefs.Add(messageNode.Attributes["receiver"].Value);
                                }
                            }
                        }
                        List<UMLLifeLine> lifeLineElements = new List<UMLLifeLine>();
                        foreach (string lifeLineRef in lifeLineRefs)
                        {
                            UMLLifeLine lifeLineElement = new UMLLifeLine();

                            lifeLineElement.Type = (int)AppEnums.Type.LifeLine;
                            lifeLineElement.ElementRef = lifeLineRef;
                            int lifeLineElementType = 0;
                            lifeLineElement.ElementName = GetNameAndType(lifeLineRef, out lifeLineElementType, _referenceNodes);
                            lifeLineElement.ElmenetType = lifeLineElementType;
                            lifeLineElement.Name = lifeLineElement.ElementName;
                            List<UMLMessage> incomingMessages = new List<UMLMessage>();
                            List<UMLMessage> outgoingMessages = new List<UMLMessage>();
                            foreach (XmlNode messageNode in messageNodes)
                            {
                                if (messageNode.Attributes["sender"]?.Value == lifeLineRef)
                                {
                                    UMLMessage outgoingMessageElement = new UMLMessage();
                                    outgoingMessageElement.Type = (int)AppEnums.Type.Message;
                                    int elementFromType = 0;
                                    outgoingMessageElement.ElementFromRef = messageNode.Attributes["sender"]?.Value;
                                    outgoingMessageElement.ElementFromName = GetNameAndType(messageNode.Attributes["sender"]?.Value, out elementFromType, _referenceNodes);
                                    outgoingMessageElement.ElementFromType = elementFromType;

                                    int elementToType = 0;
                                    outgoingMessageElement.ElementToRef = messageNode.Attributes["receiver"]?.Value;
                                    outgoingMessageElement.ElementToName = GetNameAndType(messageNode.Attributes["receiver"]?.Value, out elementToType, _referenceNodes);
                                    outgoingMessageElement.ElementToType = elementToType;

                                    outgoingMessages.Add(outgoingMessageElement);

                                }

                                if (messageNode.Attributes["receiver"]?.Value == lifeLineRef)
                                {
                                    UMLMessage incomingMessageElement = new UMLMessage();
                                    incomingMessageElement.Type = (int)AppEnums.Type.Message;
                                    int elementFromType = 0;
                                    incomingMessageElement.ElementFromRef = messageNode.Attributes["receiver"]?.Value;
                                    incomingMessageElement.ElementFromName = GetNameAndType(messageNode.Attributes["sender"]?.Value, out elementFromType, _referenceNodes);
                                    incomingMessageElement.ElementFromType = elementFromType;

                                    int elementToType = 0;
                                    incomingMessageElement.ElementToRef = messageNode.Attributes["receiver"]?.Value;
                                    incomingMessageElement.ElementToName = GetNameAndType(messageNode.Attributes["receiver"]?.Value, out elementToType, _referenceNodes);
                                    incomingMessageElement.ElementToType = elementToType;
                                    incomingMessages.Add(incomingMessageElement);

                                }
                            }

                            lifeLineElement.IncomingMessages = incomingMessages;
                            lifeLineElement.OutgoingMessages = outgoingMessages;
                            lifeLineElements.Add(lifeLineElement);
                            //interactionElement.Childs.Add(lifeLineElement);
                        }
                        List<UMLMessage> messagesElements = new List<UMLMessage>();
                        foreach (XmlNode messageNode in messageNodes)
                        {
                            UMLMessage outgoingMessageElement = new UMLMessage();
                            outgoingMessageElement.Type = (int)AppEnums.Type.Message;
                            int elementFromType = 0;
                            outgoingMessageElement.ElementFromRef = messageNode.Attributes["sender"]?.Value;
                            outgoingMessageElement.ElementFromName = GetNameAndType(messageNode.Attributes["sender"]?.Value, out elementFromType, _referenceNodes);
                            outgoingMessageElement.ElementFromType = elementFromType;

                            int elementToType = 0;
                            outgoingMessageElement.ElementToRef = messageNode.Attributes["receiver"]?.Value;
                            outgoingMessageElement.ElementToName = GetNameAndType(messageNode.Attributes["receiver"]?.Value, out elementToType, _referenceNodes);
                            outgoingMessageElement.ElementToType = elementToType;
                            outgoingMessageElement.Childs = new List<UMLElement>();
                            foreach (UMLLifeLine lifeLine in lifeLineElements)
                            {
                                if (outgoingMessageElement.ElementFromRef == lifeLine.ElementRef)
                                {
                                    UMLLifeLine lifeLineElement = new UMLLifeLine();
                                    lifeLineElement.XmiId = lifeLine.XmiId;
                                    lifeLineElement.Name = lifeLine.Name;
                                    lifeLineElement.Visibility = lifeLine.Visibility;
                                    lifeLineElement.ElementName = lifeLine.ElementName;
                                    lifeLineElement.ElementRef = lifeLine.ElementRef;
                                    lifeLineElement.ElmenetType = lifeLine.ElmenetType;
                                    lifeLineElement.Type = lifeLine.Type;
                                    outgoingMessageElement.Childs.Add(lifeLineElement);
                                }

                            }
                            foreach (UMLLifeLine lifeLine in lifeLineElements)
                            {
                                if (outgoingMessageElement.ElementToRef == lifeLine.ElementRef)
                                {
                                    UMLLifeLine lifeLineElement = new UMLLifeLine();
                                    lifeLineElement.XmiId = lifeLine.XmiId;
                                    lifeLineElement.Name = lifeLine.Name;
                                    lifeLineElement.Visibility = lifeLine.Visibility;
                                    lifeLineElement.ElementName = lifeLine.ElementName;
                                    lifeLineElement.ElementRef = lifeLine.ElementRef;
                                    lifeLineElement.ElmenetType = lifeLine.ElmenetType;
                                    lifeLineElement.Type = lifeLine.Type;
                                    outgoingMessageElement.Childs.Add(lifeLineElement);
                                }

                            }
                            messagesElements.Add(outgoingMessageElement);
                        }
                        interactionElement.Childs = new List<UMLElement>();
                        UMLElement LifeLinesElement = new UMLElement();
                        LifeLinesElement.Name = "Life Lines";
                        LifeLinesElement.Type = 0;
                        LifeLinesElement.Childs = new List<UMLElement>();
                        LifeLinesElement.Childs.AddRange(lifeLineElements);

                        UMLElement MessagesElement = new UMLElement();
                        MessagesElement.Name = "Messages";
                        LifeLinesElement.Type = 0;

                        MessagesElement.Childs = new List<UMLElement>();
                        MessagesElement.Childs.AddRange(messagesElements);
                        interactionElement.Childs.Add(LifeLinesElement);
                        interactionElement.Childs.Add(MessagesElement);

                    }
                    result.Add(interactionElement);
                }
                else if (childNode.Name.ToLower() == "UML:ClassifierRole".ToLower())
                {

                    UMLObject objectElement = new UMLObject();

                    objectElement.Type = (int)AppEnums.Type.Object;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        objectElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        objectElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        objectElement.Visibility = childNode.Attributes["visibility"].Value;
                    }
                    /*if (childNode.Attributes["base"] != null)
                    {
                        string classReference = childNode.Attributes["base"].Value;

                        XmlNode classReferenceNode = classReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == classReference).FirstOrDefault();
                        if (classReferenceNode != null)
                        {
                            objectElement.InstancedClass = classReferenceNode.Attributes["name"].Value;
                            

                        }
                    }*/
                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        objectElement.Childs = childElements;
                    }

                    result.Add(objectElement);
                }
                else if (childNode.Name.ToLower() == "UML:AssociationRole".ToLower())
                {
                    UMLAssociation associationElement = new UMLAssociation();
                    associationElement.Type = (int)AppEnums.Type.Association;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        associationElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        associationElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        associationElement.Visibility = childNode.Attributes["visibility"].Value;
                    }

                    XmlNode associationConnectionNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Association.connection".ToLower()).FirstOrDefault();
                    string associationStereoType = "";
                    if (associationConnectionNode != null)
                    {
                        List<XmlNode> associationEndNodes = associationConnectionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEndRole".ToLower()).ToList();
                        if (associationEndNodes.Count > 0)
                        {
                            if (associationEndNodes[0].Attributes["type"] != null)
                            {
                                string object1Ref = associationEndNodes[0].Attributes["type"].Value;
                                XmlNode objectReferenceNode = _referenceNodes.objectsReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == object1Ref).FirstOrDefault();
                                if (objectReferenceNode != null)
                                {
                                    if (objectReferenceNode.Attributes["name"] != null)
                                    {
                                        associationElement.Element1Ref = associationEndNodes[0].Attributes["type"].Value;
                                        associationElement.Element1Name = objectReferenceNode.Attributes["name"].Value;
                                        associationElement.Element1Type = (int)AppEnums.Type.Object;
                                    }
                                }
                            }


                            XmlNode associationEndStereoType = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (associationEndStereoType != null)
                            {
                                XmlNode associationEndStereoTypeChildNode = associationEndStereoType.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (associationEndStereoTypeChildNode != null && associationEndStereoTypeChildNode.Attributes["name"] != null)
                                {
                                    associationStereoType = associationEndStereoTypeChildNode.Attributes["name"].Value;

                                }
                            }
                            if (associationEndNodes[0].Attributes["aggregation"] != null)
                            {
                                associationElement.Element1Aggregation = associationEndNodes[0].Attributes["aggregation"].Value;

                            }
                            else
                            {
                                XmlNode aggregationNode = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.aggregation".ToLower()).FirstOrDefault();
                                if (aggregationNode != null && aggregationNode.Attributes["xmi.value"] != null)
                                {
                                    associationElement.Element1Aggregation = aggregationNode.Attributes["xmi.value"].Value;
                                }
                            }
                            if (associationEndNodes[0].Attributes["multiplicity"] != null)
                            {
                                associationElement.Element1Multiplicity = associationEndNodes[0].Attributes["multiplicity"].Value;

                            }
                            else
                            {
                                XmlNode multiplicityNode = associationEndNodes[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.multiplicity".ToLower()).FirstOrDefault();
                                if (multiplicityNode != null)
                                {

                                    // XmlNode multiplicityRangeNode= multiplicityNode.SelectSingleNode("UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange");
                                    XmlNode multiplicityChildNode = multiplicityNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity".ToLower()).FirstOrDefault();
                                    if (multiplicityChildNode != null)
                                    {
                                        XmlNode multiplicityRangeNode = multiplicityChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity.range".ToLower()).FirstOrDefault();
                                        if (multiplicityRangeNode != null)
                                        {
                                            XmlNode multiplicityRenageChildNode = multiplicityRangeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange".ToLower()).FirstOrDefault();
                                            if (multiplicityRenageChildNode != null)
                                            {
                                                if (multiplicityRenageChildNode.Attributes["lower"] != null && multiplicityRenageChildNode.Attributes["upper"] != null)
                                                {
                                                    string lower = multiplicityRenageChildNode.Attributes["lower"].Value;
                                                    string upper = multiplicityRenageChildNode.Attributes["upper"].Value;
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
                                                        associationElement.Element1Multiplicity = lower;
                                                    }
                                                    else
                                                    {
                                                        associationElement.Element1Multiplicity = lower + "..." + upper;
                                                    }

                                                }
                                                else
                                                {
                                                    XmlNode multiplicityRenageChildLowerNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.lower".ToLower()).FirstOrDefault();
                                                    XmlNode multiplicityRenageChildUpperNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.upper".ToLower()).FirstOrDefault();
                                                    if (multiplicityRenageChildLowerNode != null && multiplicityRenageChildUpperNode != null)
                                                    {
                                                        string lower = multiplicityRenageChildLowerNode.InnerText;
                                                        string upper = multiplicityRenageChildUpperNode.InnerText;
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
                                                            associationElement.Element1Multiplicity = lower;
                                                        }
                                                        else
                                                        {
                                                            associationElement.Element1Multiplicity = lower + "..." + upper;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                            }

                        }
                        if (associationEndNodes.Count > 1)
                        {

                            if (associationEndNodes[1].Attributes["type"] != null)
                            {
                                string object2Ref = associationEndNodes[1].Attributes["type"].Value;

                                XmlNode objectReferenceNode = _referenceNodes.objectsReferenceNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi.id"] != null && node.Attributes["xmi.id"].Value == object2Ref).FirstOrDefault();
                                if (objectReferenceNode != null)
                                {
                                    if (objectReferenceNode.Attributes["name"] != null)
                                    {
                                        associationElement.Element2Ref = associationEndNodes[1].Attributes["type"].Value;
                                        associationElement.Element2Name = objectReferenceNode.Attributes["name"].Value;
                                        associationElement.Element2Type = (int)AppEnums.Type.Object;

                                    }
                                }
                            }

                            XmlNode associationEndStereoType = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (associationEndStereoType != null)
                            {
                                XmlNode associationEndStereoTypeChildNode = associationEndStereoType.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (associationEndStereoTypeChildNode != null && associationEndStereoTypeChildNode.Attributes["name"] != null)
                                {
                                    associationStereoType = associationEndStereoTypeChildNode.Attributes["name"].Value;

                                }
                            }
                            if (associationEndNodes[1].Attributes["aggregation"] != null)
                            {
                                associationElement.Element2Aggregation = associationEndNodes[1].Attributes["aggregation"].Value;

                            }
                            else
                            {
                                XmlNode aggregationNode = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.aggregation".ToLower()).FirstOrDefault();
                                if (aggregationNode != null && aggregationNode.Attributes["xmi.value"] != null)
                                {
                                    associationElement.Element2Aggregation = aggregationNode.Attributes["xmi.value"].Value;
                                }
                            }
                            if (associationEndNodes[1].Attributes["multiplicity"] != null)
                            {
                                associationElement.Element2Multiplicity = associationEndNodes[1].Attributes["multiplicity"].Value;

                            }
                            else
                            {
                                XmlNode multiplicityNode = associationEndNodes[1].ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:AssociationEnd.multiplicity".ToLower()).FirstOrDefault();
                                if (multiplicityNode != null)
                                {

                                    // XmlNode multiplicityRangeNode= multiplicityNode.SelectSingleNode("UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange");
                                    XmlNode multiplicityChildNode = multiplicityNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity".ToLower()).FirstOrDefault();
                                    if (multiplicityChildNode != null)
                                    {
                                        XmlNode multiplicityRangeNode = multiplicityChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Multiplicity.range".ToLower()).FirstOrDefault();
                                        if (multiplicityRangeNode != null)
                                        {
                                            XmlNode multiplicityRenageChildNode = multiplicityRangeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange".ToLower()).FirstOrDefault();
                                            if (multiplicityRenageChildNode != null)
                                            {
                                                if (multiplicityRenageChildNode.Attributes["lower"] != null && multiplicityRenageChildNode.Attributes["upper"] != null)
                                                {
                                                    string lower = multiplicityRenageChildNode.Attributes["lower"].Value;
                                                    string upper = multiplicityRenageChildNode.Attributes["upper"].Value;
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
                                                        associationElement.Element2Multiplicity = lower;
                                                    }
                                                    else
                                                    {
                                                        associationElement.Element2Multiplicity = lower + "..." + upper;
                                                    }

                                                }
                                                else
                                                {
                                                    XmlNode multiplicityRenageChildLowerNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.lower".ToLower()).FirstOrDefault();
                                                    XmlNode multiplicityRenageChildUpperNode = multiplicityRenageChildNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:MultiplicityRange.upper".ToLower()).FirstOrDefault();
                                                    if (multiplicityRenageChildLowerNode != null && multiplicityRenageChildUpperNode != null)
                                                    {
                                                        string lower = multiplicityRenageChildLowerNode.InnerText;
                                                        string upper = multiplicityRenageChildUpperNode.InnerText;
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
                                                            associationElement.Element2Multiplicity = lower;
                                                        }
                                                        else
                                                        {
                                                            associationElement.Element2Multiplicity = lower + "..." + upper;
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
                    if (associationStereoType != "")
                    {
                        associationElement.StereoType = associationStereoType;
                    }
                    else
                    {
                        UMLSetereoType setereoType = _referenceNodes.setereoTypes.Where(setereoTypeItem => setereoTypeItem.ElementsRef != null && setereoTypeItem.ElementsRef.Where(elementref => elementref == associationElement.XmiId).Any()).FirstOrDefault();
                        if (setereoType != null)
                        {
                            associationElement.StereoType = setereoType.Name;
                        }
                        else
                        {
                            XmlNode modelElementStereoTypeNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                            if (modelElementStereoTypeNode != null)
                            {
                                XmlNode steroTypeNode = modelElementStereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                                if (steroTypeNode != null)
                                {
                                    if (steroTypeNode.Attributes["name"] != null)
                                    {
                                        associationElement.StereoType = steroTypeNode.Attributes["name"].Value;
                                    }
                                    else if (steroTypeNode.Attributes["xmi.idref"] != null)
                                    {
                                        UMLSetereoType setereoTypeElement = _referenceNodes.setereoTypes.Where(stereoTypeItem => stereoTypeItem.XmiId == steroTypeNode.Attributes["xmi.idref"].Value).FirstOrDefault();
                                        if (setereoTypeElement != null)
                                        {
                                            associationElement.StereoType = setereoTypeElement.Name;

                                        }
                                    }

                                }
                            }
                        }
                    }

                    if (childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault() != null)
                    {
                        List<UMLElement> childElements = buildStructure(childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Namespace.ownedElement".ToLower()).FirstOrDefault());
                        associationElement.Childs = childElements;
                    }

                    result.Add(associationElement);
                }
                else if (childNode.Name.ToLower() == "UML:Actor".ToLower())
                {
                    UMLElement actorElement = new UMLElement();
                    actorElement.Type = (int)AppEnums.Type.Actor;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        actorElement.XmiId = childNode.Attributes["xmi.id"].Value;
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
                else if (childNode.Name.ToLower() == "UML:UseCase".ToLower())
                {
                    UMLUseCase usecaseElement = new UMLUseCase();
                    usecaseElement.Type = (int)AppEnums.Type.UseCase;

                    if (childNode.Attributes["xmi.id"] != null)
                    {
                        usecaseElement.XmiId = childNode.Attributes["xmi.id"].Value;
                    }
                    if (childNode.Attributes["name"] != null)
                    {
                        usecaseElement.Name = childNode.Attributes["name"].Value;
                    }
                    if (childNode.Attributes["visibility"] != null)
                    {
                        usecaseElement.Visibility = childNode.Attributes["visibility"].Value;
                    }

                    //check stereoType
                    UMLSetereoType setereoType = _referenceNodes.setereoTypes.Where(setereoTypeItem => setereoTypeItem.ElementsRef != null && setereoTypeItem.ElementsRef.Where(elementref => elementref == usecaseElement.XmiId).Any()).FirstOrDefault();
                    if (setereoType != null)
                    {
                        usecaseElement.StereoType = setereoType.Name;
                    }
                    else
                    {
                        XmlNode modelElementStereoTypeNode = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:ModelElement.stereotype".ToLower()).FirstOrDefault();
                        if (modelElementStereoTypeNode != null)
                        {
                            XmlNode steroTypeNode = modelElementStereoTypeNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Stereotype".ToLower()).FirstOrDefault();
                            if (steroTypeNode != null)
                            {
                                if (steroTypeNode.Attributes["name"] != null)
                                {
                                    usecaseElement.StereoType = steroTypeNode.Attributes["name"].Value;
                                }
                                else if (steroTypeNode.Attributes["xmi.idref"] != null)
                                {
                                    UMLSetereoType setereoTypeElement = _referenceNodes.setereoTypes.Where(stereoTypeItem => stereoTypeItem.XmiId == steroTypeNode.Attributes["xmi.idref"].Value).FirstOrDefault();
                                    if (setereoTypeElement != null)
                                    {
                                        usecaseElement.StereoType = setereoTypeElement.Name;

                                    }
                                }

                            }
                        }
                    }
                    result.Add(usecaseElement);
                }
                else if (childNode.Name.ToLower() == "UML:Namespace.ownedElement".ToLower())
                {
                    result.AddRange(buildStructure(childNode));
                }
                else if (childNode.Name.ToLower() == "UML:ActivityModel".ToLower())
                {
                    StateMachine stateMachine = new StateMachine(_referenceNodes);
                    result.AddRange(stateMachine.buildStateMachine(childNode));
                }
            }

            return result;
        }

    }
}