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
    public class DAC
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public DAC(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            int result=0;
            if (classElement.Attributes != null)
            {
                result+= classElement.Attributes.Where(attribute=>attribute.IsClassDataType).Count();
            }
            

            return result;


        }
    }

}