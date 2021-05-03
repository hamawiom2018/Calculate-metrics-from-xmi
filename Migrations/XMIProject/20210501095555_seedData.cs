using Microsoft.EntityFrameworkCore.Migrations;
using System;
namespace master_project.Migrations.XMIProject
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NPM","Number of the packages in a model ","This metric counts the number of packages in a model. Package is a way of managing closely related modelling elements together. Also by using packages, naming conflicts can be avoided.",0,"Diagram",DateTime.Now,DateTime.Now});

migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NCM","Number of the classes in a model ","A class in a model is an instance of the meta-class “class”. This metric counts the number of classes in a model.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NAM","Number of actors in a model","This metric computes the number of actors in a model.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NUM","Number of the use cases in a model","The rationale behind the inclusion of this metric is that a use case represents a coherent unit of functionality provided by a system, a subsystem, or a class.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NOM","Number of the objects in a model","In a similar manner that a class is an instance of the metaclass “Class”, an object is an instance of a class.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NMM","Number of the messages in a model","A message is an instance of the metaclass “Message”. Messages are exchanged between objects manifesting various interactions.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NASM","Number of the associations in a model","An association is a connection, or a  link, between classes. This metric is useful for estimating the scale of relationships between classes.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NAGM","Number of the aggregations in a model","An aggregation is a special form of association that specifies a whole-part relationship between the aggregate (whole) and a component part.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NIM","Number of the inheritance relations in a model","This metric counts the number of generalization relationships between classes existing in a model.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NATC1","Number of the attributes in a class - unweighted ","This metric counts the number of attributes in a class. This does not apply a weighting scheme, meaning public, private and protected attributes are treated equal.",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NATC2","Number of the attributes in a class- weighted","This metric is a weighted version of NATC1. That is, it applies different weights to each metric depending on their visibility",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NOPC1","Number of the operations in a class - unweighted","This is an unweighted metric that counts the number of operations in a class",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NOPC2","Number of the operations in a class - weighted","This metric is same as NOPC1 except different weights are applied.",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NASC","Number of the associations linked to a class","The number of associations including aggregations is counted",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"CBC","Coupling between classes","This metric counts the number of associations in a class and attributes whose parameters are of the class type",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"DIT","Depth of inheritance tree","This metric count the depth of inheritance (means parent class and parent of that parent class until the last parent).",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NSUPC","Number of the superclasses of a class","This counts the direct parents of a class.",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NSUPC*","Number of the elements in the transitive closure of the superclasses of a class","This counts the transitive closure of the superclasses of a class.",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NSUBC","Number of the subclasses of a class","This counts the direct children of a class",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"NSUBC*","Number of the elements in the transitive closure of the subclasses of a class","This metric count the number of the elements in the transitive closure of the subclasses of a class",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"DAC","Data Abstraction Coupling","The number of attributes in a class that have another class as their type.",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"SIZE2","Two Size Metrics","This metrics counts the number of properties (including the attributes and methods) defined in a class",2,"Class",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"MHF","Method Hiding Factor","The MHF numerator is the sum of the invisibilities of all methods defined in all classes.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"AHF","Attribute Hiding Factor","The AHF is defined as a quotient between the sum of the invisibilities of all attributes defined in all of the classes and the total number of attributes defined in the system under consideration.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"MIF","Method Inheritance Factor","The MIF is defined as a quotient between the sum of inherited methods in all classes of the system under consideration and the total number of available methods (locally defined and include those inherited) for all classes.",0,"Diagram",DateTime.Now,DateTime.Now});
migrationBuilder.InsertData(
    table: "Metrics",
    columns: new[] { "MetricCode", "MetricName","MetricDescription","TargetType","TargetTypeName","CreatedDate","UpdatedDate" },
    values: new object[] {"AIF","Attribute Inheritance Factor","The AIF is defined as a quotient between the sum of inherited attributes in all classes of the system under consideration and the total number of available attributes (locally defined plus inherited) for all classes.",0,"Diagram",DateTime.Now,DateTime.Now});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
