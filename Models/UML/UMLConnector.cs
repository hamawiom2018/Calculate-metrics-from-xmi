using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLConnector : UMLElement
    {
        public string Element1Ref { get; set; }
        public string Element1Name { get; set; }
        public int Element1Type { get; set; }
        public string Element2Ref { get; set; }
        public string Element2Name { get; set; }
        public int Element2Type { get; set; }
        public string Kind { get; set; }
    }
}