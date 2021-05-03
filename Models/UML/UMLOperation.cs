using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLOperation : UMLElement
    {
        public bool IsClassDataType{get;set;}
        public string DataTypeName{get;set;}
        public bool IsArray{get;set;}

        public List<UMLOperationParameter> Parameters{get;set;}
    }

}