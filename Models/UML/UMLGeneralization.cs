using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLGeneralization : UMLElement
    {
        public string ParentRef { get; set; }
        public string ParentName { get; set; }
        public int ParentType { get; set; }
        public string ChildRef { get; set; }
        public string ChildName { get; set; }
        public int ChildType { get; set; }

    }

}