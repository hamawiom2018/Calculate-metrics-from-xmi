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
    public class NOPC2
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NOPC2(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getOperationsCount(classElement);

            return result;
        }
        private double getOperationsCount(UMLClass classElement)
        {
            if (classElement.Operations != null)
            {
                double result=0;
                foreach(UMLOperation attribute in classElement.Operations){
                    if(attribute.Visibility?.ToLower()=="private"){
                        result+=0;
                    }else if(attribute.Visibility?.ToLower()=="protected"){
                        result+=0.5;
                    }else if(attribute.Visibility?.ToLower()=="public"){
                        result+=1;
                    }
                }
                return result;
            }
            else
            {
                return 0;
            }


        }
    }

}