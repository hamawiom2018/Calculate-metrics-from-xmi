using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;


namespace master_project.BusinessLogic.Metrics
{
    public class AIF
    {

        List<UMLElement> _diagramElements;
        string _sDiagramElements;
        public AIF(List<UMLElement> diagramElements, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _sDiagramElements = sDiagramElements;

        }
        public double Evaluate()
        {
            double result = getAIF();

            return result;
        }

        private bool checkIfOverriden(UMLAttribute a1, UMLAttribute a2)
        {
            //check equals parameters
            
            bool result=(a1.Visibility == a2.Visibility
            && a1.Name == a2.Name
            && a1.DataTypeName == a2.DataTypeName
            && a1.IsArray == a2.IsArray);
            return result;
        }
        private double getAIF()
        {
            MetricHelper metricHelper = new MetricHelper();
            List<UMLClass> allClasses =metricHelper.getAllClasses(_diagramElements,_sDiagramElements);
            
            double mi = 0,ma=0;
            foreach (UMLClass classElement in allClasses)
            {

                

                    //get all inherited mehods
                    List<string> parentRefs = metricHelper.getAllParentsRefs(classElement, _diagramElements, _sDiagramElements);
                    List<UMLClass> parentClasses = parentRefs.Distinct().Select(parentRef => JsonConvert.DeserializeObject<UMLClass>(metricHelper.getElementByXmiId(parentRef, _sDiagramElements))).ToList();
                    List<UMLAttribute> repeatedInheritedAttributes = parentClasses.SelectMany(parentclass => parentclass.Attributes==null?new List<UMLAttribute>():parentclass.Attributes.Where(attribute => attribute.Visibility != "private").ToList()).ToList();
                    List<UMLAttribute> inheritedAttributes = new List<UMLAttribute>();

                    foreach (var inheritedAttribute in repeatedInheritedAttributes)
                    {
                        if (!inheritedAttributes.Where(op2=>checkIfOverriden(op2,inheritedAttribute)).Any())
                        {
                            inheritedAttributes.Add(inheritedAttribute);
                        }
                    }
                    List<UMLAttribute> inheritedNotOverridenAttributes = new List<UMLAttribute>();
                    List<UMLAttribute> notInheritedOperations = new List<UMLAttribute>();
                    if (classElement.Attributes != null)
                    {
                        inheritedNotOverridenAttributes = inheritedAttributes.Where(attr => !classElement.Attributes.Where(attrClass => checkIfOverriden(attrClass, attr)).Any()).ToList();
                        //notInheritedOperations = classElement.Operations.Where(op1 => inheritedNotOverridenOperations.Where(op2 => op2 == op1).Any()).ToList();
                    }else{
                        inheritedNotOverridenAttributes=inheritedAttributes;
                    }

                    mi+=inheritedNotOverridenAttributes.Count();
                    ma+=(classElement.Operations!=null?classElement.Operations.Count():0)+inheritedNotOverridenAttributes.Count();

                

            }
            double result = mi / ma;
            //
           // double totalOperations = allClasses.Select(classElement => classElement.Operations != null ? classElement.Operations.Count : 0).Sum();
           // double result = mdc / totalOperations;
            return result;
        }
        
    }

}