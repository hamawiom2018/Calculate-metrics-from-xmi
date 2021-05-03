using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLDependencyObject : UMLElement
    {
        public string ClientObjectRef { get; set; }
        public string ClientObjectName { get; set; }
        public string SupplierObjectRef { get; set; }
        public string SupplierObjectName { get; set; }
        public string StereoType{get;set;}
    }

}