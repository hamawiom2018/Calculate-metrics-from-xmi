using System;
using System.Collections.Generic;
using master_project.Models.UML;
using Newtonsoft.Json;

namespace master_project.Models
{
    public class ReadXmiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string resultElements { get; set; }
        public string Name { get; set; }
        public string DiagramName { get; set; }
        public string XmlContent { get; set; }
        
    }
}