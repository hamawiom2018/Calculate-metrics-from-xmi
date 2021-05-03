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
    public class SIZE2
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public SIZE2(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getSize2(classElement);

            return result;
        }
        private double getSize2(UMLClass classElement)
        {
            NATC1 natc1=new NATC1(_diagramElements,_elementRef,_sDiagramElements);

            NOPC1 nopc1=new NOPC1(_diagramElements,_elementRef,_sDiagramElements);

            

            return nopc1.Evaluate()+natc1.Evaluate();


        }
    }

}