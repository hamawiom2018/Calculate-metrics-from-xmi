using Microsoft.EntityFrameworkCore;
using System;
namespace master_project.Data.XMI_Project
{
    public class XMIProjectContext : DbContext
    {
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricElement> MetricElements { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlServer("Server=localhost;Database=XMI_Project2;User Id=sa;Password=Aa123456;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 1, MetricCode = "NPM", MetricName = "Number of the packages in a model ", MetricDescription = "This metric counts the number of packages in a model. Package is a way of managing closely related modelling elements together. Also by using packages, naming conflicts can be avoided.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 2, MetricCode = "NCM", MetricName = "Number of the classes in a model ", MetricDescription = "A class in a model is an instance of the meta-class “class”. This metric counts the number of classes in a model.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 3, MetricCode = "NAM", MetricName = "Number of actors in a model", MetricDescription = "This metric computes the number of actors in a model.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 4, MetricCode = "NUM", MetricName = "Number of the use cases in a model", MetricDescription = "The rationale behind the inclusion of this metric is that a use case represents a coherent unit of functionality provided by a system, a subsystem, or a class.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 5, MetricCode = "NOM", MetricName = "Number of the objects in a model", MetricDescription = "In a similar manner that a class is an instance of the metaclass “Class”, an object is an instance of a class.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 6, MetricCode = "NMM", MetricName = "Number of the messages in a model", MetricDescription = "A message is an instance of the metaclass “Message”. Messages are exchanged between objects manifesting various interactions.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 7, MetricCode = "NASM", MetricName = "Number of the associations in a model", MetricDescription = "An association is a connection, or a  link, between classes. This metric is useful for estimating the scale of relationships between classes.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 8, MetricCode = "NAGM", MetricName = "Number of the aggregations in a model", MetricDescription = "An aggregation is a special form of association that specifies a whole-part relationship between the aggregate (whole) and a component part.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 9, MetricCode = "NIM", MetricName = "Number of the inheritance relations in a model", MetricDescription = "This metric counts the number of generalization relationships between classes existing in a model.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 10, MetricCode = "NATC1", MetricName = "Number of the attributes in a class - unweighted ", MetricDescription = "This metric counts the number of attributes in a class. This does not apply a weighting scheme, meaning public, private and protected attributes are treated equal.", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 11, MetricCode = "NATC2", MetricName = "Number of the attributes in a class- weighted", MetricDescription = "This metric is a weighted version of NATC1. That is, it applies different weights to each metric depending on their visibility", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 12, MetricCode = "NOPC1", MetricName = "Number of the operations in a class - unweighted", MetricDescription = "This is an unweighted metric that counts the number of operations in a class", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 13, MetricCode = "NOPC2", MetricName = "Number of the operations in a class - weighted", MetricDescription = "This metric is same as NOPC1 except different weights are applied.", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 14, MetricCode = "NASC", MetricName = "Number of the associations linked to a class", MetricDescription = "The number of associations including aggregations is counted", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 15, MetricCode = "CBC", MetricName = "Coupling between classes", MetricDescription = "This metric counts the number of associations in a class and attributes whose parameters are of the class type", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 16, MetricCode = "DIT", MetricName = "Depth of inheritance tree", MetricDescription = "This metric count the depth of inheritance (means parent class and parent of that parent class until the last parent).", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 17, MetricCode = "NSUPC", MetricName = "Number of the superclasses of a class", MetricDescription = "This counts the direct parents of a class.", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 18, MetricCode = "NSUPC*", MetricName = "Number of the elements in the transitive closure of the superclasses of a class", MetricDescription = "This counts the transitive closure of the superclasses of a class.", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 19, MetricCode = "NSUBC", MetricName = "Number of the subclasses of a class", MetricDescription = "This counts the direct children of a class", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 20, MetricCode = "NSUBC*", MetricName = "Number of the elements in the transitive closure of the subclasses of a class", MetricDescription = "This metric count the number of the elements in the transitive closure of the subclasses of a class", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 21, MetricCode = "DAC", MetricName = "Data Abstraction Coupling", MetricDescription = "The number of attributes in a class that have another class as their type.", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 22, MetricCode = "SIZE2", MetricName = "Two Size Metrics", MetricDescription = "This metrics counts the number of properties (including the attributes and methods) defined in a class", TargetType = 2, TargetTypeName = "Class", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 23, MetricCode = "MHF", MetricName = "Method Hiding Factor", MetricDescription = "The MHF numerator is the sum of the invisibilities of all methods defined in all classes.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 24, MetricCode = "AHF", MetricName = "Attribute Hiding Factor", MetricDescription = "The AHF is defined as a quotient between the sum of the invisibilities of all attributes defined in all of the classes and the total number of attributes defined in the system under consideration.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 25, MetricCode = "MIF", MetricName = "Method Inheritance Factor", MetricDescription = "The MIF is defined as a quotient between the sum of inherited methods in all classes of the system under consideration and the total number of available methods (locally defined and include those inherited) for all classes.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
            //modelBuilder.Entity<Metric>().HasData(new Metric { Id = 26, MetricCode = "AIF", MetricName = "Attribute Inheritance Factor", MetricDescription = "The AIF is defined as a quotient between the sum of inherited attributes in all classes of the system under consideration and the total number of available attributes (locally defined plus inherited) for all classes.", TargetType = 0, TargetTypeName = "Diagram", CreatedDate = DateTime.Now });
        }
    }

}