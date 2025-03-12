using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Models
{
    public class Process
    {
        public int id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string tools { get; set; } = string.Empty;

        [Column(TypeName = "varchar(200)")]
        public string responsibles { get; set; } = string.Empty;

        [Column(TypeName = "varchar(500)")]
        public string documentation { get; set; } = string.Empty;

        public string priority { get; set; } = string.Empty;

        public bool finished { get; set; } = false;

        public DateTime createdOn { get; set; } = DateTime.Now;


        // relacionamento com a tebela de setores
        public int? sectorId { get; set; }
        public Sector? sector { get; set; }


        // auto relacionamento
        public int? parentProcessId { get; set; }

        [JsonIgnore]
        [ForeignKey("parentProcessId")]
        public Process? parentProcess { get; set; }
        
        public List<Process> children { get; set; } = new();
    }
}