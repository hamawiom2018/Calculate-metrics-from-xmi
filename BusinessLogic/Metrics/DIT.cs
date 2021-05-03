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
    public class DIT
    {

        List<UMLElement> _diagramElements;
        string _elementRef;
        string _sDiagramElements;
        public DIT(List<UMLElement> diagramElements, string elementRef, string sDiagramElements)
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
            result = getClassGenralizationsDepthCount(classElement,_diagramElements);

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
                        if (generalizationElement.ChildRef == classElement.XmiId && 
                        (generalizationElement.ChildType==(int)AppEnums.Type.Class ||generalizationElement.ChildType==(int)AppEnums.Type.InnerClass)) 
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
        private int getClassGenralizationsDepthCount(UMLClass classElement, List<UMLElement> diagramElements)
        {
            int result = 0;
            List<string> parentRefs = getParentRef(classElement,diagramElements);
            
            if (parentRefs.Count > 0)
            {
                result += 1;
                List<int> depth=new List<int>();
                foreach (string prentRef in parentRefs)
                {
                    int depthItem=0;
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(prentRef, _sDiagramElements);
                    UMLClass classElementParent=(UMLClass)JsonConvert.DeserializeObject<UMLClass>(sElement);
                    depthItem+= getClassGenralizationsDepthCount(classElementParent,_diagramElements);
                    depth.Add(depthItem);
                }
                result+=depth.Max();

            }





            return result;


        }
    }

}