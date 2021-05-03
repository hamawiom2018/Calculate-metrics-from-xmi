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
    public class NPM
    {
        List<UMLElement> _diagramElements;
        public NPM(List<UMLElement> diagramElements)
        {
            _diagramElements = diagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            result =getPackagesCount(_diagramElements);

            return result;
        }
        private int getPackagesCount(List<UMLElement> diagramElements)
        {
            int result = diagramElements.Where(element => element.Type == (int)AppEnums.Type.Package).Count();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Childs?.Count > 0)
                {
                    result += getPackagesCount(element.Childs);
                }
            }

            return result;
        }
    }

}