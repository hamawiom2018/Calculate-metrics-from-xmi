using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLClass : UMLElement
    {
        public List<UMLOperation> Operations{get;set;} 
        public List<UMLAttribute> Attributes{get;set;}
    }
}