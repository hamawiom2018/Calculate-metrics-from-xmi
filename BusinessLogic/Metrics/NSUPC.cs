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
    public class NSUPC
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NSUPC(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getParentClassesCount(classElement,_diagramElements);

            return result;
        }
       
        private List<string> getParentRef(UMLClass classElement, List<UMLElement> diagramElements)
        {
            List<string> parentRefs=new List<string>();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Type == (int)AppEnums.Type.Generalization)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(element.XmiId, _sDiagramElements);
                    if (!string.IsNullOrEmpty(sElement))
                    {
                        UMLGeneralization generalizationElement = JsonConvert.DeserializeObject<UMLGeneralization>(sElement);
                        if (generalizationElement.ChildRef == classElement.XmiId)
                        {
                            parentRefs.Add(generalizationElement.ParentRef);
                        }
                    }

                }
                if(element.Childs?.Count>0){
                    parentRefs.AddRange(getParentRef(classElement,element.Childs));
                }

            }
            return parentRefs;
        }
        private int getParentClassesCount(UMLClass classElement, List<UMLElement> diagramElements)
        {
            List<string> parentRefs = getParentRef(classElement,diagramElements);

            return parentRefs.Count();


        }
    }

}