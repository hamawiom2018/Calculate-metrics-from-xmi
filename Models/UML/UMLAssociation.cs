using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLAssociation : UMLElement
    {
        public string Element1Ref { get; set; }
        public string Element1Name { get; set; }
        public string Element1Aggregation { get; set; }
        public string Element1Multiplicity { get; set; }
        public int Element1Type { get; set; }

        public string Element2Ref { get; set; }
        public string Element2Name { get; set; }
        public string Element2Aggregation { get; set; }
        public string Element2Multiplicity { get; set; }
        public int Element2Type { get; set; }

        public string StereoType { get; set; }




    }
}