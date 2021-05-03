using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLProperty : UMLElement
    {
        public string PropertyRef{get;set;}
        public string PropertyName{get;set;}
        public int PropertyType{get;set;}
    }

}