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
    public class MIF
    {

        List<UMLElement> _diagramElements;
        string _sDiagramElements;
        public MIF(List<UMLElement> diagramElements, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _sDiagramElements = sDiagramElements;

        }
        public double Evaluate()
        {
            double result = getMIF();

            return result;
        }

        private bool checkIfOverriden(UMLOperation op1, UMLOperation op2)
        {
            //check equals parameters
            bool isEqualParams = true;
            if (op1.Parameters != null || op2.Parameters != null)
            {
                isEqualParams = op1.Parameters?.Count() == op2.Parameters?.Count();
                if (isEqualParams)
                {
                    var matchParams = op1.Parameters.Select(parameter1 => op2.Parameters.Where(parameter2 => parameter1.Name == parameter2.Name
                       && parameter1.DataTypeName == parameter2.DataTypeName
                       && parameter1.IsArray == parameter2.IsArray).Any()).ToList();
                    isEqualParams = !matchParams.Contains(false);
                }
            }
            bool result = (op1.Visibility == op2.Visibility
            && op1.Name == op2.Name
            && op1.DataTypeName == op2.DataTypeName
            && isEqualParams);
            return result;
        }
        private double getMIF()
        {
            MetricHelper metricHelper = new MetricHelper();

            List<UMLClass> allClasses =metricHelper.getAllClasses(_diagramElements,_sDiagramElements);
            double mi = 0, ma = 0;
            foreach (UMLClass classElement in allClasses)
            {



                //get all inherited mehods
                List<string> parentRefs = metricHelper.getAllParentsRefs(classElement, _diagramElements, _sDiagramElements);
                List<UMLClass> parentClasses = parentRefs.Distinct().Select(parentRef => JsonConvert.DeserializeObject<UMLClass>(metricHelper.getElementByXmiId(parentRef, _sDiagramElements))).ToList();
                List<UMLOperation> repeatedInheritedOperations = parentClasses.SelectMany(parentclass => parentclass.Operations == null ? new List<UMLOperation>() : parentclass.Operations.Where(operation => operation.Visibility != "private").ToList()).ToList();
                List<UMLOperation> inheritedOperations = new List<UMLOperation>();

                foreach (var inheritedOperation in repeatedInheritedOperations)
                {
                    if (!inheritedOperations.Where(op2 => checkIfOverriden(op2, inheritedOperation)).Any())
                    {
                        inheritedOperations.Add(inheritedOperation);
                    }
                }
                List<UMLOperation> inheritedNotOverridenOperations = new List<UMLOperation>();
                List<UMLOperation> notInheritedOperations = new List<UMLOperation>();
                if (classElement.Operations != null)
                {
                    inheritedNotOverridenOperations = inheritedOperations.Where(operation => !classElement.Operations.Where(opClass => checkIfOverriden(opClass, operation)).Any()).ToList();
                    //notInheritedOperations = classElement.Operations.Where(op1 => inheritedNotOverridenOperations.Where(op2 => op2 == op1).Any()).ToList();
                }
                else
                {
                    inheritedNotOverridenOperations = inheritedOperations;
                }

                mi += inheritedNotOverridenOperations.Count();
                ma += (classElement.Operations != null ? classElement.Operations.Count() : 0) + inheritedNotOverridenOperations.Count();



            }
            double result = mi / ma;
            //
            // double totalOperations = allClasses.Select(classElement => classElement.Operations != null ? classElement.Operations.Count : 0).Sum();
            // double result = mdc / totalOperations;
            return result;
        }
        
    }

}