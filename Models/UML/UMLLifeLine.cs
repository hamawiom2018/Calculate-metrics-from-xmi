using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLLifeLine : UMLElement
    {
        public List<UMLMessage> IncomingMessages{get;set;}
        public List<UMLMessage> OutgoingMessages{get;set;}
        public string ElementRef{get;set;}
        public int ElmenetType{get;set;}
        public string ElementName{get;set;}

    }

}