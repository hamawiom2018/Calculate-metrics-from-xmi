
using System.Collections.Generic;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace master_project.BusinessLogic.Metrics
{
    public class MetricHelper
    {
        public string getElementByXmiId(string xmiId, string sElement)
        {
            string result = "";
            JArray a = JArray.Parse(sElement);
            foreach (var element in a)
            {
                string id = element["XmiId"].ToString();
                if (id == xmiId)
                {
                    result = element.ToString();
                }
                else if (element["Childs"] != null)
                {
                    //JArray childsArray =JArray.Parse(element["Childs"].ToString());
                    string childs = element["Childs"].ToString();
                    if (!string.IsNullOrEmpty(childs))
                    {
                        string childResult = getElementByXmiId(xmiId, childs);
                        if (childResult != "")
                        {
                            result = childResult;
                            break;
                        }
                    }


                }
            }



            return result;
        }
        public List<string> getChildRefs(UMLClass classElement, List<UMLElement> diagramElements, string sDiagramElements)
        {
            List<string> parentRefs = new List<string>();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Type == (int)AppEnums.Type.Generalization)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(element.XmiId, sDiagramElements);
                    if (!string.IsNullOrEmpty(sElement))
                    {
                        UMLGeneralization generalizationElement = JsonConvert.DeserializeObject<UMLGeneralization>(sElement);
                        if (generalizationElement.ParentRef == classElement.XmiId)
                        {
                            parentRefs.Add(generalizationElement.ChildRef);
                        }
                    }

                }
                if (element.Childs?.Count > 0)
                {
                    parentRefs.AddRange(getChildRefs(classElement, element.Childs, sDiagramElements));
                }

            }
            return parentRefs;
        }
        public List<string> getParentRefs(UMLClass classElement, List<UMLElement> diagramElements, string sDiagramElements)
        {
            List<string> parentRefs = new List<string>();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Type == (int)AppEnums.Type.Generalization)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sElement = metricHelper.getElementByXmiId(element.XmiId, sDiagramElements);
                    if (!string.IsNullOrEmpty(sElement))
                    {
                        UMLGeneralization generalizationElement = JsonConvert.DeserializeObject<UMLGeneralization>(sElement);
                        if (generalizationElement.ChildRef == classElement.XmiId)
                        {
                            parentRefs.Add(generalizationElement.ParentRef);
                        }
                    }

                }
                if (element.Childs?.Count > 0)
                {
                    parentRefs.AddRange(getParentRefs(classElement, element.Childs, sDiagramElements));
                }

            }
            return parentRefs;
        }
        public List<string> getAllParentsRefs(UMLClass classElement, List<UMLElement> diagramElements, string sDiagramElements)
        {
            List<string> parentRefs = new List<string>();
             parentRefs.AddRange(getParentRefs(classElement, diagramElements, sDiagramElements));
            foreach (string childRef in parentRefs.ToArray())
            {
                string sChildElement = getElementByXmiId(childRef, sDiagramElements);
                UMLClass classParentElement = JsonConvert.DeserializeObject<UMLClass>(sChildElement);

                parentRefs.AddRange(getAllParentsRefs(classParentElement, diagramElements, sDiagramElements));

            }

            return parentRefs;
        }
        public List<string> getAllChildsRefs(UMLClass classElement, List<UMLElement> diagramElements, string sDiagramElements)
        {
            List<string> chidlRefs = new List<string>();
             chidlRefs.AddRange(getChildRefs(classElement, diagramElements, sDiagramElements));
            foreach (string childRef in chidlRefs.ToArray())
            {
                string sChildElement = getElementByXmiId(childRef, sDiagramElements);
                UMLClass classChildElement = JsonConvert.DeserializeObject<UMLClass>(sChildElement);

                chidlRefs.AddRange(getAllChildsRefs(classChildElement, diagramElements, sDiagramElements));

            }

            return chidlRefs;
        }
        public List<UMLClass> getAllClasses(List<UMLElement> elements,string sDiagramElements)
        {
            List<UMLClass> allClasses = new List<UMLClass>();
            foreach (var element in elements)
            {
                if (element.Type == (int)AppEnums.Type.Class || element.Type == (int)AppEnums.Type.InnerClass)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sClassElement = metricHelper.getElementByXmiId(element.XmiId, sDiagramElements);
                    UMLClass classElement = JsonConvert.DeserializeObject<UMLClass>(sClassElement);
                    allClasses.Add(classElement);
                }
                if (element.Childs?.Count > 0)
                {
                    allClasses.AddRange(getAllClasses(element.Childs,sDiagramElements));
                }
            }
            return allClasses;
        }
    }

}