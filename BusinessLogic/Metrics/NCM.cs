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
    public class NCM
    {
        
        List<UMLElement> _diagramElements;
        public NCM(List<UMLElement> diagramElements)
        {
            _diagramElements = diagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            result =getClassesCount(_diagramElements);

            return result;
        }
        private int getClassesCount(List<UMLElement> diagramElements)
        {
            int result = diagramElements.Where(element => element.Type == (int)AppEnums.Type.Class).Count();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Childs?.Count > 0)
                {
                    result += getClassesCount(element.Childs);
                }
            }

            return result;
        }
    }

}