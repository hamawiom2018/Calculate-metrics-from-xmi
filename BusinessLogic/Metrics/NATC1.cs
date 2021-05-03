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
    public class NATC1
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NATC1(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getAttributesCount(classElement);

            return result;
        }
        private int getAttributesCount(UMLClass classElement)
        {
            if (classElement.Attributes != null)
            {
                return classElement.Attributes.Count;
            }
            else
            {
                return 0;
            }


        }
    }

}