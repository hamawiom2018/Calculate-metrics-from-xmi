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
    public class NSUBC
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NSUBC(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getChildClassesCount(classElement,_diagramElements);

            return result;
        }
       
        private List<string> getChildRef(UMLClass classElement, List<UMLElement> diagramElements)
        {
            List<string> childRefs=new List<string>();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Type == (int)AppEnums.Type.Generalization)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(element.XmiId, _sDiagramElements);
                    if (!string.IsNullOrEmpty(sElement))
                    {
                        UMLGeneralization generalizationElement = JsonConvert.DeserializeObject<UMLGeneralization>(sElement);
                        if (generalizationElement.ParentRef == classElement.XmiId)
                        {
                            childRefs.Add(generalizationElement.ChildRef);
                        }
                    }

                }
                if(element.Childs?.Count>0){
                    childRefs.AddRange(getChildRef(classElement,element.Childs));
                }

            }
            return childRefs;
        }
        private int getChildClassesCount(UMLClass classElement, List<UMLElement> diagramElements)
        {
            List<string> childRefs = getChildRef(classElement,diagramElements);

            return childRefs.Count();


        }
    }

}