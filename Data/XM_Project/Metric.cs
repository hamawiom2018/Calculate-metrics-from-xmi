using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace master_project.Data.XMI_Project{
    public class Metric{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        public string MetricCode{get;set;}
        public string MetricName{get;set;}
        public string MetricDescription{get;set;}
        public int TargetType{get;set;}
        public string TargetTypeName{get;set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<MetricElement> MetricElements { get; set; }

    }
}