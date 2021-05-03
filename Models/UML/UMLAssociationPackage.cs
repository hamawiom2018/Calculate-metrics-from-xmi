using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLAssociationPackage : UMLElement
    {
        public string Package1Ref { get; set; }
        public string Package1Name { get; set; }
        public string Package1Aggregation { get; set; }
        public string Package1Multiplicity { get; set; }

        public string Package2Ref { get; set; }
        public string Package2Name { get; set; }
        public string Package2Aggregation { get; set; }
        public string Package2Multiplicity { get; set; }
        public string StereoType{get;set;}




    }
}