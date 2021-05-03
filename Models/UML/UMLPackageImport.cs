using master_project.Models.UML;

namespace master_project.BusinessLogic
{
    public class UMLPackageImport : UMLElement
    {
        public string ImportedPackageRef { get; set; }
        public string ImportedPackageName { get; set; }
    }
}