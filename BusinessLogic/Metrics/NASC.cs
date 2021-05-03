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
    public class NASC
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public NASC(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _elementRef = elementRef;
            _sDiagramElements = sDiagramElements;

        }
        public int Evaluate()
        {
            int result = -1;
            MetricHelper metricHelper = new MetricHelper();
            string sElement = metricHelper.getElementByXmiId(_elementRef, _sDiagramElements);
            UMLClass classElement = JsonConvert.DeserializeObject<UMLClass>(sElement);
            result = getClassAssociationsCount(classElement, _diagramElements);

            return result;
        }
        private int getClassAssociationsCount(UMLClass classElement, List<UMLElement> diagramElements)
        {
            int result = 0;
            foreach (UMLElement element in diagramElements)
            {
                if (element.Type == (int)AppEnums.Type.Association)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(element.XmiId, _sDiagramElements);
                    if (!string.IsNullOrEmpty(sElement))
                    {
                        UMLAssociation associationElement = JsonConvert.DeserializeObject<UMLAssociation>(sElement);
                        if (associationElement.Element1Ref == classElement.XmiId || associationElement.Element2Ref == classElement.XmiId)
                        {
                            result+=1;
                        }
                    }
                    
                }
                if(element.Childs?.Count>0){
                   result+= getClassAssociationsCount(classElement, element.Childs);
                }
            }




            return result;


        }
    }

}