using System.Collections.Generic;
using System.Linq;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace master_project.BusinessLogic.Metrics
{
    public class NAGM
    {

        List<UMLElement> _diagramElements;
        string _sDiagramElements;
        public NAGM(List<UMLElement> diagramElements, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _sDiagramElements = sDiagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            result = getActorsCount(_diagramElements);

            return result;
        }
        private int getActorsCount(List<UMLElement> diagramElements)
        {
            int result = diagramElements.Where(element => isAggregation(element)).Count();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Childs?.Count > 0)
                {
                    result += getActorsCount(element.Childs);
                }
            }

            return result;
        }

        private string getElementByXmiId(string xmiId, string sElement)
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
        private bool isAggregation(UMLElement element)
        {
            bool result = false;
            if (element.Type == (int)AppEnums.Type.Association)
            {
                string sElement = getElementByXmiId(element.XmiId, _sDiagramElements);

                UMLAssociation association = JsonConvert.DeserializeObject<UMLAssociation>(sElement);
                if ((association.Element1Aggregation != null && association.Element1Aggregation != "none")
                || (association.Element2Aggregation != null && association.Element2Aggregation != "none"))
                {
                    result = true;
                }
            }
            return result;
        }
    }

}