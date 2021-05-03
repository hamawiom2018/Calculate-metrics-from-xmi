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
    public class NUM
    {
        
        List<UMLElement> _diagramElements;
        public NUM(List<UMLElement> diagramElements)
        {
            _diagramElements = diagramElements;

        }
        public double Evaluate()
        {
            double result = -1;
            result =getUseCasesCount(_diagramElements);

            return result;
        }
        private int getUseCasesCount(List<UMLElement> diagramElements)
        {
            int result = diagramElements.Where(element => element.Type == (int)AppEnums.Type.UseCase).Count();
            foreach (UMLElement element in diagramElements)
            {
                if (element.Childs?.Count > 0)
                {
                    result += getUseCasesCount(element.Childs);
                }
            }

            return result;
        }
    }

}