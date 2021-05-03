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
    public class NSUBC2
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NSUBC2(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _elementRef = elementRef;
            _sDiagramElements = sDiagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            MetricHelper metricHelper = new MetricHelper();
            string sElement = metricHelper.getElementByXmiId(_elementRef, _sDiagramElements);
            UMLClass classElement = JsonConvert.DeserializeObject<UMLClass>(sElement);
            result = getClassParentTransitiveClosureCount(classElement,_diagramElements);

            return result;
        }
       
        
        private int getClassParentTransitiveClosureCount(UMLClass classElement, List<UMLElement> diagramElements)
        {
            int result = 0;
            MetricHelper metricHelper=new MetricHelper();
            List<string> childRefs = metricHelper.getChildRefs(classElement,diagramElements,_sDiagramElements);
            
            if (childRefs.Count > 0)
            {
                result += 1;
                List<int> depth=new List<int>();
                foreach (string childRef in childRefs)
                {
                    int depthItem=0;
                    string sElement = metricHelper.getElementByXmiId(childRef, _sDiagramElements);
                    UMLClass classElementParent=(UMLClass)JsonConvert.DeserializeObject<UMLClass>(sElement);
                    depthItem+= getClassParentTransitiveClosureCount(classElementParent,_diagramElements);
                    depth.Add(depthItem);
                }
                result+=depth.Sum();

            }





            return result;


        }
    }

}