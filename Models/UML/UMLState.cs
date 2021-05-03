using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLState : UMLElement
    {
        public int StateType{get;set;}
        public string Kind{get;set;}
        public string Entry{get;set;}
        public string Do{get;set;}
        public string Exit{get;set;}
    }

}