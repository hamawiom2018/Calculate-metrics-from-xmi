using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLOperationParameter : UMLElement
    {
        public bool IsClassDataType{get;set;}
        public string DataTypeName{get;set;}
        public bool IsArray{get;set;}
        public string Kind{get;set;}
    }

}