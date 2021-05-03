using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLTransition : UMLElement
    {
        public string SourceRef { get; set; }
        public string SourceName { get; set; }
        public int SourceType { get; set; }


        public string TargetRef { get; set; }
        public string TargetName { get; set; }
        public int TargetType { get; set; }
        public string Trigger { get; set; }
        public string Guard { get; set; }

    }

}