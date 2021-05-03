using System;
using Microsoft.AspNetCore.Http;

namespace master_project.Models
{
    public class DiagramResponseContract
    {
        public int Id { get; set; }
        public string UploadName { get; set; }
        public string AuthorEmail { get; set; }
        public string DiagramName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}