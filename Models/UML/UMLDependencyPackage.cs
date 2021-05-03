using System.Collections.Generic;
using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLDependencyPackage : UMLElement
    {
        public string ClientPackageRef { get; set; }
        public string ClientPackageName { get; set; }
        public string SupplierPackageRef { get; set; }
        public string SupplierPackageName { get; set; }

        public string StereoType{get;set;}
    }

}