using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLDependency : UMLElement
    {
        public string ClientRef { get; set; }
        public string ClientName { get; set; }
        public int ClientType { get; set; }
        public string SupplierRef { get; set; }
        public string SupplierName { get; set; }
        public int SupplierType { get; set; }

        public string StereoType{get;set;}
    }

}