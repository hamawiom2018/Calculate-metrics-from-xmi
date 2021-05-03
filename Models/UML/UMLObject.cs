using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLObject : UMLElement
    {
        public string InstancedClassRef{get;set;}
        public string InstancedClassName{get;set;}
        public List<UMLObjectAttribute> Attributes{get;set;}
        
    }

}