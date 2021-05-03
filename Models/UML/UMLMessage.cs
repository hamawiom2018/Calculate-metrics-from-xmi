using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLMessage : UMLElement
    {
        public string MessgaeKind{get;set;}
        public string MessgaeSort{get;set;}
        public string StereoType{get;set;}
        public string ElementFromRef{get;set;}
        public string ElementFromName{get;set;}
        public int ElementFromType{get;set;}

        public string ElementToRef{get;set;}
        public string ElementToName{get;set;}
        public int ElementToType{get;set;}
    }

}