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
    public class NASM
    {
        
        List<UMLElement> _diagramElements;
        public NASM(List<UMLElement> diagramElements)
        {
            _diagramElements = diagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            result =getAssociationsCount(_diagramElements);

            return result;
        }
        private int getAssociationsCount(List<UMLElement> diagramElements)
        {
            int result = diagramElements.Where(element => element.Type == (int)AppEnums.Type.Association).Count();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Childs?.Count > 0)
                {
                    result += getAssociationsCount(element.Childs);
                }
            }

            return result;
        }
    }

}