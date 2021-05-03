using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLAssociationObject : UMLElement
    {
        public string Object1Ref { get; set; }
        public string Object1Name { get; set; }
        public string Object1Aggregation { get; set; }
        public string Object1Multiplicity { get; set; }

        public string Object2Ref { get; set; }
        public string Object2Name { get; set; }
        public string Object2Aggregation { get; set; }
        public string Object2Multiplicity { get; set; }
        public string StereoType{get;set;}




    }
}