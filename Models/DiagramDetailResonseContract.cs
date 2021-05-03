using System;
using System.Collections.Generic;
using master_project.Models.UML;
using Microsoft.AspNetCore.Http;

namespace master_project.Models
{
    public class DiagramDetailResponseContract
    {
        public int Id { get; set; }
        public string UploadName { get; set; }
        public string AuthorEmail { get; set; }
        public string DiagramName { get; set; }
        public string Elements{get;set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}