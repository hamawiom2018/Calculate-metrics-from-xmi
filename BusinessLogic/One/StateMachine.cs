using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models.UML;
using master_project.Utils;

namespace master_project.BusinessLogic.One
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
            List<UMLElement> result = new List<UMLElement>();
            List<UMLTransition> tarnsitions = new List<UMLTransition>();
            List<UMLElement> compositeStates = new List<UMLElement>();


            foreach (XmlNode childNode in activityModelNode.ChildNodes)
            {
                if (childNode.Name.ToLower() == "UML:StateMachine.transitions".ToLower())
                {
                    foreach (XmlNode transitionNode in childNode.ChildNodes)
                    {
                        //transitions
                        UMLTransition transition = new UMLTransition();
                        transition.Type = (int)AppEnums.Type.Transtion;
                        transition.XmiId = transitionNode.Attributes["xmi.id"]?.Value;
                        transition.Name = transitionNode.Attributes["name"]?.Value;
                        transition.Visibility = transitionNode.Attributes["visibility"]?.Value;

                        int sourceType = 0;
                        transition.SourceRef = transitionNode.Attributes["source"]?.Value;
                        transition.SourceName = XmiVersionOneBL.GetNameAndType(transition.SourceRef, out sourceType, _referenceNodes);

                        transition.SourceType = sourceType;

                        int targetType = 0;
                        transition.TargetRef = transitionNode.Attributes["target"]?.Value;
                        transition.TargetName = XmiVersionOneBL.GetNameAndType(transition.TargetRef, out targetType, _referenceNodes);
                        transition.TargetType = targetType;
                        XmlNode triggerNode = transitionNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(node => node.Name.ToLower() == "UML:Transition.trigger".ToLower());
                        if (triggerNode != null)
                        {
                            XmlNode triggerEventNode = triggerNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(node => node.Name.ToLower() == "UML:Event".ToLower());
                            if (triggerEventNode != null)
                            {
                                transition.Trigger = triggerEventNode.Attributes["name"]?.Value;
                            }
                        }
                        XmlNode tranditionGaurdNode = transitionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Transition.guard".ToLower()).FirstOrDefault();
                        if (tranditionGaurdNode != null)
                        {
                            XmlNode gaurdNode = tranditionGaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Guard".ToLower()).FirstOrDefault();
                            if (gaurdNode != null)
                            {
                                XmlNode gaurdExpressionNode = gaurdNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:Guard.expression".ToLower()).FirstOrDefault();
                                if (gaurdExpressionNode != null)
                                {
                                    XmlNode booleanExpressionNode = gaurdExpressionNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:BooleanExpression".ToLower()).FirstOrDefault();
                                    if (booleanExpressionNode != null)
                                    {
                                        transition.Guard = booleanExpressionNode.Attributes["body"]?.Value;
                                    }

                                }

                            }

                        }

                        tarnsitions.Add(transition);
                    }


                }
                else if (childNode.Name.ToLower() == "UML:StateMachine.top".ToLower())
                {
                    List<XmlNode> compositeStatesNodes = childNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:CompositeState".ToLower()).ToList();
                    foreach (XmlNode compositeStatesNode in compositeStatesNodes)
                    {
                        UMLElement compositeStateElement = new UMLElement();
                        compositeStateElement.XmiId = compositeStatesNode.Attributes["xmi.id"]?.Value;
                        compositeStateElement.Name = compositeStatesNode.Attributes["name"]?.Value;
                        compositeStateElement.Visibility = compositeStatesNode.Attributes["visibility"]?.Value;
                        compositeStateElement.Type = (int)AppEnums.Type.CompositeState;

                        XmlNode compositeSubStatesNode = compositeStatesNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:CompositeState.substate".ToLower()).FirstOrDefault();
                        if (compositeSubStatesNode != null)
                        {

                            compositeStateElement.Childs = new List<UMLElement>();
                            foreach (XmlNode stateNode in compositeSubStatesNode.ChildNodes)
                            {
                                UMLState stateElement = new UMLState();
                                stateElement.XmiId = stateNode.Attributes["xmi.id"]?.Value;
                                stateElement.Name = stateNode.Attributes["name"]?.Value;
                                stateElement.Visibility = stateNode.Attributes["visibility"]?.Value;
                                stateElement.Type = 0;
                                if (stateNode.Name.ToLower() == "UML:SimpleState".ToLower())
                                {
                                    stateElement.Type = (int)AppEnums.Type.SimpleState;
                                }
                                else if (stateNode.Name.ToLower() == "UML:PseudoState".ToLower())
                                {
                                    stateElement.Type = (int)AppEnums.Type.PseudoState;

                                }else if (stateNode.Name.ToLower() == "UML:ActionState".ToLower())
                                {
                                    stateElement.Type = (int)AppEnums.Type.ActionState;
                                }
                                stateElement.Kind = stateNode.Attributes["kind"]?.Value;

                                XmlNode stateEntryNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:State.entry".ToLower()).FirstOrDefault();

                                if (stateEntryNode != null)
                                {
                                    List<string> nodeNames = new List<string>();
                                    nodeNames.Add("UML:ActionSequence");
                                    nodeNames.Add("UML:ActionSequence.action");
                                    nodeNames.Add("UML:UninterpretedAction");

                                    XmlNode uninterpretedActionNode = null;
                                    XmlNode currentNode = stateEntryNode;
                                    foreach (string nodeName in nodeNames)
                                    {
                                        XmlNode nodeNameNode = currentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == nodeName.ToLower()).FirstOrDefault();
                                        if (nodeNameNode == null)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            currentNode = nodeNameNode;
                                            if (nodeName == nodeNames.Last())
                                            {
                                                uninterpretedActionNode = currentNode;
                                            }
                                        }
                                    }
                                    if (uninterpretedActionNode != null)
                                    {
                                        stateElement.Entry=uninterpretedActionNode.Attributes["name"]?.Value;
                                    }
                                }
                                

                                XmlNode stateExitNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:State.exit".ToLower()).FirstOrDefault();

                                if (stateExitNode != null)
                                {
                                    List<string> nodeNames = new List<string>();
                                    nodeNames.Add("UML:ActionSequence");
                                    nodeNames.Add("UML:ActionSequence.action");
                                    nodeNames.Add("UML:UninterpretedAction");

                                    XmlNode uninterpretedActionNode = null;
                                    XmlNode currentNode = stateExitNode;
                                    foreach (string nodeName in nodeNames)
                                    {
                                        XmlNode nodeNameNode = currentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == nodeName.ToLower()).FirstOrDefault();
                                        if (nodeNameNode == null)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            currentNode = nodeNameNode;
                                            if (nodeName == nodeNames.Last())
                                            {
                                                uninterpretedActionNode = currentNode;
                                            }
                                        }
                                    }
                                    if (uninterpretedActionNode != null)
                                    {
                                        stateElement.Exit=uninterpretedActionNode.Attributes["name"]?.Value;
                                    }
                                }

                                 XmlNode stateDoNode = stateNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "UML:State.internalTransition".ToLower()).FirstOrDefault();

                                if (stateDoNode != null)
                                {
                                    List<string> nodeNames = new List<string>();
                                    nodeNames.Add("UML:Transition");
                                    nodeNames.Add("UML:Transition.effect");
                                    nodeNames.Add("UML:ActionSequence");
                                    nodeNames.Add("UML:ActionSequence.action");
                                    nodeNames.Add("UML:UninterpretedAction");

                                    XmlNode uninterpretedActionNode = null;
                                    XmlNode currentNode = stateDoNode;
                                    foreach (string nodeName in nodeNames)
                                    {
                                        XmlNode nodeNameNode = currentNode.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == nodeName.ToLower()).FirstOrDefault();
                                        if (nodeNameNode == null)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            currentNode = nodeNameNode;
                                            if (nodeName == nodeNames.Last())
                                            {
                                                uninterpretedActionNode = currentNode;
                                            }
                                        }
                                    }
                                    if (uninterpretedActionNode != null)
                                    {
                                        stateElement.Do=uninterpretedActionNode.Attributes["name"]?.Value;
                                    }
                                }
                                if (stateElement.Type != 0)
                                {
                                    compositeStateElement.Childs.Add(stateElement);
                                }



                            }
                        }
                        compositeStates.Add(compositeStateElement);
                    }

                }
            }
            foreach (UMLElement compositeState in compositeStates)
            {
                foreach (UMLState stateElement in compositeState.Childs)
                {
                    List<UMLTransition> transitions2 = tarnsitions.Where(transition => stateElement.XmiId == transition.SourceRef).ToList();
                    if (transitions2.Count > 0)
                    {
                        stateElement.Childs = new List<UMLElement>();
                        foreach (UMLTransition transition in transitions2)
                        {
                            stateElement.Childs.Add(transition);
                        }
                    }
                }
                result.Add(compositeState);
            }

            return result;

        }
    }
}