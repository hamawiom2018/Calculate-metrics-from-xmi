using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models.UML;
using master_project.Utils;

namespace master_project.BusinessLogic.Two
{


    public class StateMachine
    {
        private ReferenceNodes _referenceNodes;
        public StateMachine(ReferenceNodes referenceNodes)
        {
            _referenceNodes = referenceNodes;
        }

        public List<UMLElement> buildStateMachine(XmlNode activityModelNode)
        {

            List<UMLElement> compositeStates = new List<UMLElement>();
            List<XmlNode> regionNodes = activityModelNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "region").ToList();

            foreach (XmlNode regionNode in regionNodes)
            {
                UMLElement compositeStateElement = new UMLElement();
                compositeStateElement.Type = (int)AppEnums.Type.CompositeState;
                compositeStateElement.XmiId = regionNode.Attributes["xmi:id"]?.Value;
                compositeStateElement.Name = regionNode.Attributes["name"]?.Value;
                compositeStateElement.Visibility = regionNode.Attributes["visibility"]?.Value;

                List<UMLTransition> tarnsitions = new List<UMLTransition>();
                List<XmlNode> transitionNodes = regionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "transition").ToList();
                List<XmlNode> stateNodes = regionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "subvertex").ToList();
                List<UMLState> stateElements = new List<UMLState>();
                foreach (XmlNode transitionNode in transitionNodes)
                {

                    UMLTransition transitionElement = new UMLTransition();
                    transitionElement.XmiId = transitionNode.Attributes["xmi:id"]?.Value;
                    transitionElement.Name = transitionNode.Attributes["name"]?.Value;
                    transitionElement.Visibility = transitionNode.Attributes["visibility"]?.Value;
                    transitionElement.Type = (int)AppEnums.Type.Transtion;
                    if (transitionNode.Attributes["source"] != null && transitionNode.Attributes["target"] != null)
                    {
                        int sourceType = 0;
                        transitionElement.SourceRef = transitionNode.Attributes["source"].Value;
                        transitionElement.SourceName = XmiVersionTwoBL.GetNameAndType(transitionElement.SourceRef, out sourceType, _referenceNodes);
                        transitionElement.SourceType = sourceType;
                        int targetType = 0;
                        transitionElement.TargetRef = transitionNode.Attributes["target"].Value;
                        transitionElement.TargetName = XmiVersionTwoBL.GetNameAndType(transitionElement.TargetRef, out targetType, _referenceNodes);
                        transitionElement.TargetType = targetType;

                    }
                    else
                    {
                        List<XmlNode> stateNode2 = stateNodes.Where(node => node.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Attributes["xmi:idref"].Value == transitionElement.XmiId).Any()).ToList();
                        foreach (var transitionStateNode in stateNode2)
                        {
                            XmlNode targetTransitionNode = transitionStateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Attributes["xmi:idref"].Value == transitionElement.XmiId).FirstOrDefault();
                            if (targetTransitionNode != null)
                            {
                                if (targetTransitionNode.Name == "outgoing")
                                {
                                    int sourceType = 0;
                                    transitionElement.SourceRef = transitionStateNode.Attributes["xmi:id"].Value;
                                    transitionElement.SourceName = XmiVersionTwoBL.GetNameAndType(transitionElement.SourceRef, out sourceType, _referenceNodes);
                                    transitionElement.SourceType = sourceType;
                                }
                                else if (targetTransitionNode.Name == "incoming")
                                {
                                    int targetType = 0;
                                    transitionElement.TargetRef = transitionStateNode.Attributes["xmi:id"].Value;
                                    transitionElement.TargetName = XmiVersionTwoBL.GetNameAndType(transitionElement.TargetRef, out targetType, _referenceNodes);
                                    transitionElement.TargetType = targetType;
                                }
                            }
                        }
                    }
                    XmlNode triggerNode = transitionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "trigger".ToLower()).FirstOrDefault();
                    if (triggerNode != null)
                    {

                        string triggerRef = triggerNode.Attributes["xmi:idref"]?.Value;
                        XmlNode triggerReferenceNode = _referenceNodes.triggerReferenceNodes.Where(node => node.Attributes["xmi:id"].Value == triggerRef).FirstOrDefault();
                        if (triggerReferenceNode != null)
                        {
                            transitionElement.Trigger = triggerReferenceNode.Attributes["name"]?.Value;
                        }
                    }
                    XmlNode gaurdNode = transitionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "guard").FirstOrDefault();

                    if (gaurdNode != null)
                    {
                        XmlNode specificationNode = gaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "specification".ToLower()).FirstOrDefault();
                        if (specificationNode != null)
                        {
                            transitionElement.Guard = specificationNode.Attributes["body"]?.Value;
                        }
                    }




                    tarnsitions.Add(transitionElement);


                }
                foreach (XmlNode stateNode in stateNodes)
                {
                    UMLState stateElement = new UMLState();
                    stateElement.XmiId = stateNode.Attributes["xmi:id"]?.Value;
                    stateElement.Name = stateNode.Attributes["name"]?.Value;
                    stateElement.Visibility = stateNode.Attributes["visibility"]?.Value;
                    if (stateNode.Attributes["xmi:type"]?.Value == "uml:State")
                    {
                        stateElement.Type = (int)AppEnums.Type.SimpleState;
                    }
                    else if (stateNode.Attributes["xmi:type"]?.Value == "uml:Pseudostate")
                    {
                        stateElement.Type = (int)AppEnums.Type.PseudoState;
                    }
                    else if (stateNode.Attributes["xmi:type"]?.Value == "uml:FinalState")
                    {
                        stateElement.Type = (int)AppEnums.Type.FinalState;
                    }
                    stateElement.Kind = stateNode.Attributes["kind"]?.Value;

                    List<UMLTransition> outgoingTransitionElements = tarnsitions.Where(tarnsition => tarnsition.SourceRef == stateElement.XmiId).ToList();
                    if (outgoingTransitionElements.Count > 0)
                    {
                        stateElement.Childs = new List<UMLElement>();
                        stateElement.Childs.AddRange(outgoingTransitionElements);
                    }
                    XmlNode entryNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "entry").FirstOrDefault();
                    if (entryNode != null)
                    {
                        if (entryNode.Attributes["name"] != null)
                        {
                            stateElement.Entry = entryNode.Attributes["name"]?.Value;
                        }else{
                            XmlNode ownedOperationNode = entryNode.ChildNodes.Cast<XmlNode>().Where(node=>node.Name=="ownedOperation").FirstOrDefault();
                            if(ownedOperationNode!=null){
                                stateElement.Entry = ownedOperationNode.Attributes["name"]?.Value;
                            }
                        }

                    }

                    XmlNode doNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "doActivity").FirstOrDefault();
                    if (doNode != null)
                    {
                        if (doNode.Attributes["name"] != null)
                        {
                            stateElement.Do = doNode.Attributes["name"]?.Value;
                        }else{
                            XmlNode ownedOperationNode = doNode.ChildNodes.Cast<XmlNode>().Where(node=>node.Name=="ownedOperation").FirstOrDefault();
                            if(ownedOperationNode!=null){
                                stateElement.Do = ownedOperationNode.Attributes["name"]?.Value;
                            }
                        }
                    }

                    XmlNode exitNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "exit").FirstOrDefault();
                    if (exitNode != null)
                    {
                        if (exitNode.Attributes["name"] != null)
                        {
                            stateElement.Exit = exitNode.Attributes["name"]?.Value;
                        }else{
                            XmlNode ownedOperationNode = exitNode.ChildNodes.Cast<XmlNode>().Where(node=>node.Name=="ownedOperation").FirstOrDefault();
                            if(ownedOperationNode!=null){
                                stateElement.Exit = ownedOperationNode.Attributes["name"]?.Value;
                            }
                        }
                    }
                    XmlNode subRegionNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name == "region").FirstOrDefault();
                    if (subRegionNode != null)
                    {
                        if (stateElement.Childs == null)
                        {
                            stateElement.Childs = new List<UMLElement>();
                        }
                        stateElement.Childs.AddRange(buildStateMachine(stateNode));
                    }
                    stateElements.Add(stateElement);
                }
                if (stateElements.Count > 0)
                {
                    compositeStateElement.Childs = new List<UMLElement>();
                    compositeStateElement.Childs.AddRange(stateElements);
                }
                compositeStates.Add(compositeStateElement);


            }


            return compositeStates;

        }
    }
}