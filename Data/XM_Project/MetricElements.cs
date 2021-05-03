using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace master_project.Data.XMI_Project{
    public class MetricElement{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        public int Element{get;set;}
        public string ElementName{get;set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int  MetricId{get;set;}
        public Metric Metric{get;set;}
    }
}