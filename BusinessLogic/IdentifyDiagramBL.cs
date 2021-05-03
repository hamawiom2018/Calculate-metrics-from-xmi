using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;


namespace master_project.BusinessLogic
{
    public class IdentifyDiagramBL
    {
        ApplicationUser _identity;
        public IdentifyDiagramBL(ApplicationUser Identity)
        {
            _identity = Identity;
        }

        public string GetDiagramName(List<UMLElement> elements)
        {
            List<int> elementsTypes = getElementsTypes(elements, new List<int>());
            elementsTypes = elementsTypes.Distinct().ToList();
            //Activity Diagram
            if (elementsTypes.Contains((int)AppEnums.Type.Activity)
            || elementsTypes.Contains((int)AppEnums.Type.ActivityFinalNode)
            || elementsTypes.Contains((int)AppEnums.Type.InitialNode))
            {
                return "Acitivity Diagram";
            }
            else if (elementsTypes.Contains((int)AppEnums.Type.Component)
           || elementsTypes.Contains((int)AppEnums.Type.Connector)
           || elementsTypes.Contains((int)AppEnums.Type.Port))
            {
                return "Composite Structure Diagram";
            }
            else if (elementsTypes.Contains((int)AppEnums.Type.SimpleState)
           || elementsTypes.Contains((int)AppEnums.Type.PseudoState)
           || elementsTypes.Contains((int)AppEnums.Type.CompositeState))
            {
                return "State Machine Diagram";
            }
            else if (elementsTypes.Contains((int)AppEnums.Type.Message)
           || elementsTypes.Contains((int)AppEnums.Type.LifeLine)
           || elementsTypes.Contains((int)AppEnums.Type.CombinedFragment))
            {
                return "Sequence Diagram";
            }else if (elementsTypes.Contains((int)AppEnums.Type.UseCase)
           || elementsTypes.Contains((int)AppEnums.Type.Actor))
            {
                return "Use Case Diagram";
            }
            else if (elementsTypes.Contains((int)AppEnums.Type.Object))
            {
                return "Object Diagram";
            }
            else if (elementsTypes.Contains((int)AppEnums.Type.Class) ||
            elementsTypes.Contains((int)AppEnums.Type.Interface))
            {
                return "Class Diagram";
            }else if (elementsTypes.Contains((int)AppEnums.Type.Package))
            {
                return "Package Diagram";
            }
            return "";

        }
        public List<int> getElementsTypes(List<UMLElement> elements, List<int> elementsTypes)
        {
            elementsTypes=elementsTypes.Distinct().ToList();
            foreach (var element in elements)
            {
                if (!elementsTypes.Where(type => type == element.Type).Any())
                {
                    elementsTypes.Add(element.Type);
                }
                if (element.Childs != null)
                {
                    elementsTypes.AddRange(getElementsTypes(element.Childs, elementsTypes));
                }
            }
            return elementsTypes;
        }


    }

}