using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models.UML;
using master_project.Utils;

namespace master_project.BusinessLogic.Two
{


    public class Activity
    {
        private ReferenceNodes _referenceNodes;
        public Activity(ReferenceNodes referenceNodes)
        {
            _referenceNodes = referenceNodes;
        }

        public UMLElement buildActivity(XmlNode activityModelNode)
        {

            UMLElement activityElement = new UMLElement();
            activityElement.XmiId = activityModelNode.Attributes["xmi:id"]?.Value;
            activityElement.Name = activityModelNode.Attributes["name"]?.Value;
            activityElement.Visibility = activityModelNode.Attributes["visibility"]?.Value;
            activityElement.Type = (int)AppEnums.Type.Activity;


            List<XmlNode> nodes = activityModelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "node").ToList();
            List<XmlNode> edges = activityModelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "edge").ToList();
            List<XmlNode> groups = activityModelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "group").ToList();
            List<XmlNode> exceptionHandleNodes = activityModelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "ownedElement" && node.Attributes["xmi:type"]?.Value == "uml:ExceptionHandler").ToList();

            List<UMLElement> actionElements = new List<UMLElement>();
            List<UMLTransition> edgeElements = new List<UMLTransition>();


            edgeElements.AddRange(buildEdges(edges));
            foreach (var node in nodes)
            {
                UMLElement actionElement = new UMLElement();
                actionElement.XmiId = node.Attributes["xmi:id"]?.Value;
                actionElement.Name = node.Attributes["name"]?.Value;
                actionElement.Visibility = node.Attributes["visibility"]?.Value;
                if (node.Attributes["xmi:type"]?.Value == "uml:DecisionNode")
                {
                    actionElement.Type = (int)AppEnums.Type.DecisionNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:InitialNode")
                {
                    actionElement.Type = (int)AppEnums.Type.InitialNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:ActivityFinalNode")
                {
                    actionElement.Type = (int)AppEnums.Type.ActivityFinalNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:Action")
                {
                    actionElement.Type = (int)AppEnums.Type.Action;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:ForkNode")
                {
                    actionElement.Type = (int)AppEnums.Type.ForkNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:FlowFinalNode")
                {
                    actionElement.Type = (int)AppEnums.Type.FlowFinalNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:DataStoreNode")
                {
                    actionElement.Type = (int)AppEnums.Type.DataStoreNode;
                }
                else if (node.Attributes["xmi:type"]?.Value == "uml:CentralBufferNode")
                {
                    actionElement.Type = (int)AppEnums.Type.CentralBufferNode;
                }
                List<UMLTransition> outgoingTransitionElements = edgeElements.Where(tarnsition => tarnsition.SourceRef == actionElement.XmiId).ToList();
                if (outgoingTransitionElements.Count > 0)
                {
                    actionElement.Childs = new List<UMLElement>();
                    actionElement.Childs.AddRange(outgoingTransitionElements);
                }
                List<XmlNode> outputNodes = node.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "output".ToLower()).ToList();
                if (outputNodes.Count > 0)
                {
                    if (actionElement.Childs == null)
                    {
                        actionElement.Childs = new List<UMLElement>();
                    }
                    foreach (var outputNode in outputNodes)
                    {
                        UMLElement outputElement = new UMLElement();
                        outputElement.XmiId = outputNode.Attributes["xmi:id"]?.Value;
                        outputElement.Name = outputNode.Attributes["name"]?.Value;
                        outputElement.Visibility = outputNode.Attributes["visibility"]?.Value;
                        outputElement.Type = (int)AppEnums.Type.OutputPin;
                        List<UMLTransition> outgoingTransitionOutputElements = edgeElements.Where(tarnsition => tarnsition.SourceRef == outputElement.XmiId).ToList();
                        if (outgoingTransitionOutputElements.Count > 0)
                        {
                            outputElement.Childs = new List<UMLElement>();
                            outputElement.Childs.AddRange(outgoingTransitionOutputElements);
                        }
                        actionElement.Childs.Add(outputElement);
                    }
                }

                List<XmlNode> inputNodes = node.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "input".ToLower()).ToList();
                if (inputNodes.Count > 0)
                {
                    if (actionElement.Childs == null)
                    {
                        actionElement.Childs = new List<UMLElement>();
                    }
                    foreach (var inputNode in inputNodes)
                    {
                        UMLElement inputElement = new UMLElement();
                        inputElement.XmiId = inputNode.Attributes["xmi:id"]?.Value;
                        inputElement.Name = inputNode.Attributes["name"]?.Value;
                        inputElement.Visibility = inputNode.Attributes["visibility"]?.Value;
                        inputElement.Type = (int)AppEnums.Type.InputPin;
                        List<UMLTransition> outgoingTransitioninputElements = edgeElements.Where(tarnsition => tarnsition.SourceRef == inputElement.XmiId).ToList();
                        if (outgoingTransitioninputElements.Count > 0)
                        {
                            inputElement.Childs = new List<UMLElement>();
                            inputElement.Childs.AddRange(outgoingTransitioninputElements);
                        }
                        actionElement.Childs.Add(inputElement);
                    }
                }
                actionElements.Add(actionElement);
            }
            List<UMLElement> exceptionHandlers = new List<UMLElement>();

            if (exceptionHandleNodes.Count > 0)
            {
                foreach (XmlNode exceptionHandleNode in exceptionHandleNodes)
                {
                    UMLElement exceptionHandlerElement = new UMLElement();
                    exceptionHandlerElement.XmiId = exceptionHandleNode.Attributes["xmi:id"]?.Value;
                    exceptionHandlerElement.Name = exceptionHandleNode.Attributes["name"]?.Value;
                    exceptionHandlerElement.Visibility = exceptionHandleNode.Attributes["visibility"]?.Value;
                    exceptionHandlerElement.Type = (int)AppEnums.Type.ExceptionHandler;
                    XmlNode handlerBodyNode = exceptionHandleNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "handlerBody".ToLower()).FirstOrDefault();
                    if (handlerBodyNode != null)
                    {
                        exceptionHandlerElement.Name = handlerBodyNode.Attributes["name"]?.Value;
                    }
                    List<UMLTransition> outgoingTransitionExceptionHandlerElements = edgeElements.Where(tarnsition => tarnsition.SourceRef == exceptionHandlerElement.XmiId).ToList();
                    if (outgoingTransitionExceptionHandlerElements.Count > 0)
                    {
                        exceptionHandlerElement.Childs = new List<UMLElement>();
                        exceptionHandlerElement.Childs.AddRange(outgoingTransitionExceptionHandlerElements);
                    }
                    List<XmlNode> inputNodes = exceptionHandleNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "exceptionInput".ToLower()).ToList();
                    if (inputNodes.Count > 0)
                    {
                        if (exceptionHandlerElement.Childs == null)
                        {
                            exceptionHandlerElement.Childs = new List<UMLElement>();
                        }
                        foreach (var inputNode in inputNodes)
                        {
                            UMLElement inputElement = new UMLElement();
                            inputElement.XmiId = inputNode.Attributes["xmi:id"]?.Value;
                            inputElement.Name = inputNode.Attributes["name"]?.Value;
                            inputElement.Visibility = inputNode.Attributes["visibility"]?.Value;
                            inputElement.Type = (int)AppEnums.Type.ExceptionInput;
                            List<UMLTransition> outgoingTransitioninputElements = edgeElements.Where(tarnsition => tarnsition.SourceRef == inputElement.XmiId).ToList();
                            if (outgoingTransitioninputElements.Count > 0)
                            {
                                inputElement.Childs = new List<UMLElement>();
                                inputElement.Childs.AddRange(outgoingTransitioninputElements);
                            }
                            exceptionHandlerElement.Childs.Add(inputElement);
                        }
                    }
                    exceptionHandlers.Add(exceptionHandlerElement);

                }
            }



            if (groups.Count > 0)
            {
                List<UMLElement> addedElements = new List<UMLElement>();
                List<UMLElement> partitionElements = new List<UMLElement>();
                foreach (XmlNode group in groups)
                {
                    if (group.Attributes["xmi:type"]?.Value == "uml:ActivityPartition")
                    {
                        UMLElement partitionElement = new UMLElement();
                        partitionElement.XmiId = group.Attributes["xmi:id"]?.Value;
                        partitionElement.Name = group.Attributes["name"]?.Value;
                        partitionElement.Visibility = group.Attributes["visibility"]?.Value;
                        partitionElement.Type = (int)AppEnums.Type.ActivityPartition;

                        List<XmlNode> partitionNodes = group.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "node").ToList();
                        if (partitionNodes.Count > 0)
                        {
                            partitionElement.Childs = new List<UMLElement>();
                            foreach (var partitionNode in partitionNodes)
                            {
                                var actionElement = actionElements.Where(element => element.XmiId == partitionNode.Attributes["xmi:idref"]?.Value).FirstOrDefault();
                                if (actionElement != null)
                                {
                                    partitionElement.Childs.Add(actionElement);
                                    addedElements.Add(actionElement);
                                }
                                else
                                {
                                    var exceptionHandlerElement = exceptionHandlers.Where(element => element.XmiId == partitionNode.Attributes["xmi:idref"]?.Value).FirstOrDefault();
                                    if (exceptionHandlerElement != null)
                                    {
                                        partitionElement.Childs.Add(exceptionHandlerElement);
                                        addedElements.Add(exceptionHandlerElement);
                                    }
                                }

                            }
                        }

                        partitionElements.Add(partitionElement);
                    }
                    else if (group.Attributes["xmi:type"]?.Value == "uml:InterruptibleActivityRegion")
                    {

                        UMLElement interruotibleRegionElement = new UMLElement();
                        interruotibleRegionElement.XmiId = group.Attributes["xmi:id"]?.Value;
                        interruotibleRegionElement.Type=(int)AppEnums.Type.InterruptibleActivityRegion;
                        interruotibleRegionElement.Childs=new List<UMLElement>();
                        List<XmlNode> acceptEventNodes = group.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "containedNode" && node.Attributes["xmi:type"]?.Value == "uml:AcceptEventAction").ToList();
                        List<XmlNode> interruptingEdgeNodes = group.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "interruptingEdge" && node.Attributes["xmi:type"]?.Value == "uml:ControlFlow").ToList();
                        List<UMLElement> acceptEventElements = new List<UMLElement>();
                        List<UMLTransition> interruptingEdgeElements = buildInterruptedEdges(interruptingEdgeNodes);

                        foreach (XmlNode acceptEventNode in acceptEventNodes)
                        {
                            UMLElement acceptEventElement = new UMLElement();
                            acceptEventElement.XmiId = acceptEventNode.Attributes["xmi:id"]?.Value;
                            acceptEventElement.Name = acceptEventNode.Attributes["name"]?.Value;
                            acceptEventElement.Visibility = acceptEventNode.Attributes["visibility"]?.Value;
                            acceptEventElement.Type = (int)AppEnums.Type.AcceptEventAction;
                            List<UMLTransition> outgoingTransitionAcceptEventElements = interruptingEdgeElements.Where(tarnsition => tarnsition.SourceRef == acceptEventElement.XmiId).ToList();
                            if (outgoingTransitionAcceptEventElements.Count > 0)
                            {
                                acceptEventElement.Childs = new List<UMLElement>();
                                acceptEventElement.Childs.AddRange(outgoingTransitionAcceptEventElements);
                            }

                            acceptEventElements.Add(acceptEventElement);

                        }
                        interruotibleRegionElement.Childs.AddRange(acceptEventElements);
                        partitionElements.Add(interruotibleRegionElement);
                    }

                }

                var notAdded = actionElements.Where(element => !addedElements.Where(added => added.XmiId == element.XmiId).Any()).ToList();
                if (notAdded.Count > 0)
                {
                    partitionElements.AddRange(notAdded);
                }
                var notAddedExceptionHandler = exceptionHandlers.Where(element => !addedElements.Where(added => added.XmiId == element.XmiId).Any()).ToList();
                if (notAddedExceptionHandler.Count > 0)
                {
                    partitionElements.AddRange(notAddedExceptionHandler);
                }
                activityElement.Childs = partitionElements;
            }
            else
            {
                activityElement.Childs = new List<UMLElement>();

                activityElement.Childs.AddRange(actionElements);
                activityElement.Childs.AddRange(exceptionHandlers);

            }

            return activityElement;




        }
        public List<UMLElement> GetOutgouingEdges(string elementRef)
        {
            List<UMLElement> result = new List<UMLElement>();
            List<XmlNode> relatedEdges = _referenceNodes.edgeReferenceNodes.Where(node => node.Attributes["source"]?.Value == elementRef).ToList();
            result.AddRange(buildEdges(relatedEdges));

            return result;
        }
        private List<UMLTransition> buildEdges(List<XmlNode> edges)
        {
            List<UMLTransition> result = new List<UMLTransition>();
            foreach (var edge in edges)
            {
                UMLTransition transitionElement = new UMLTransition();
                transitionElement.XmiId = edge.Attributes["xmi:id"]?.Value;
                transitionElement.Name = edge.Attributes["name"]?.Value;
                transitionElement.Visibility = edge.Attributes["visibility"]?.Value;
                if (edge.Attributes["xmi:type"]?.Value == "uml:ControlFlow")
                {
                    transitionElement.Type = (int)AppEnums.Type.ControlFlow;
                }
                else if (edge.Attributes["xmi:type"]?.Value == "uml:ObjectFlow")
                {
                    transitionElement.Type = (int)AppEnums.Type.ObjectFlow;
                }

                if (edge.Attributes["source"] != null && edge.Attributes["target"] != null)
                {
                    int sourceType = 0;
                    transitionElement.SourceRef = edge.Attributes["source"].Value;
                    transitionElement.SourceName = XmiVersionTwoBL.GetNameAndType(transitionElement.SourceRef, out sourceType, _referenceNodes);
                    transitionElement.SourceType = sourceType;
                    int targetType = 0;
                    transitionElement.TargetRef = edge.Attributes["target"].Value;
                    transitionElement.TargetName = XmiVersionTwoBL.GetNameAndType(transitionElement.TargetRef, out targetType, _referenceNodes);
                    transitionElement.TargetType = targetType;

                }
                XmlNode gaurdNode = edge.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "guard").FirstOrDefault();

                if (gaurdNode != null)
                {
                    XmlNode specificationNode = gaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "specification".ToLower()).FirstOrDefault();
                    if (specificationNode != null)
                    {
                        transitionElement.Guard = specificationNode.Attributes["body"]?.Value;
                    }
                    else
                    {
                        transitionElement.Guard = gaurdNode.Attributes["body"]?.Value;
                    }
                }




                result.Add(transitionElement);

            }

            return result;

        }
        private List<UMLTransition> buildInterruptedEdges(List<XmlNode> interruptedEdged)
        {
            List<UMLTransition> result = new List<UMLTransition>();
            foreach (var edge in interruptedEdged)
            {
                UMLTransition transitionElement = new UMLTransition();
                transitionElement.XmiId = edge.Attributes["xmi:id"]?.Value;
                transitionElement.Name = edge.Attributes["name"]?.Value;
                transitionElement.Visibility = edge.Attributes["visibility"]?.Value;
                transitionElement.Type = (int)AppEnums.Type.InterruptingEdge;

                if (edge.Attributes["source"] != null && edge.Attributes["target"] != null)
                {
                    int sourceType = 0;
                    transitionElement.SourceRef = edge.Attributes["source"].Value;
                    transitionElement.SourceName = XmiVersionTwoBL.GetNameAndType(transitionElement.SourceRef, out sourceType, _referenceNodes);
                    transitionElement.SourceType = sourceType;
                    int targetType = 0;
                    transitionElement.TargetRef = edge.Attributes["target"].Value;
                    transitionElement.TargetName = XmiVersionTwoBL.GetNameAndType(transitionElement.TargetRef, out targetType, _referenceNodes);
                    transitionElement.TargetType = targetType;

                }
                XmlNode gaurdNode = edge.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "guard").FirstOrDefault();

                if (gaurdNode != null)
                {
                    XmlNode specificationNode = gaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "specification".ToLower()).FirstOrDefault();
                    if (specificationNode != null)
                    {
                        transitionElement.Guard = specificationNode.Attributes["body"]?.Value;
                    }
                    else
                    {
                        transitionElement.Guard = gaurdNode.Attributes["body"]?.Value;
                    }
                }




                result.Add(transitionElement);

            }

            return result;

        }
    }
}