using System.Collections.Generic;

namespace master_project.Models.UML{
    public class UMLElement{
        public int Id { get; set; }
        public string XmiId { get; set; }
        public string Name{get;set;}
        public string Visibility{get;set;}
        public int Type{get;set;}

        public List<UMLElement> Childs{get;set;}

    }
}