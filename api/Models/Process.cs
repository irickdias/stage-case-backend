using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Process
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string Tools { get; set; } = string.Empty;

        [Column(TypeName = "varchar(200)")]
        public string Responsibles { get; set; } = string.Empty;

        [Column(TypeName = "varchar(500)")]
        public string Documentation { get; set; } = string.Empty;

        public string prioprity { get; set; } = string.Empty;

        public bool finished { get; set; } = false;

        public int? parentProcessId { get; set; } // Null se for um processo principal
        
        [ForeignKey("parentProcessId")]
        public Process? parentProcess { get; set; }
        
        public List<Process> children { get; set; } = new();
    }
}