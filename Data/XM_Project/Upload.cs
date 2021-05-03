using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace master_project.Data.XMI_Project{
    public class Upload{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        public string UploadName{get;set;}
        public string AuthorEmail{get;set;}
        public string DiagramName{get;set;}
        public byte[] FileContent{get;set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}