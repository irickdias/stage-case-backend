using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Dtos.Process
{
    public class ProcessDto
    {
        public string name { get; set; } = string.Empty;
        public string tools { get; set; } = string.Empty;
        public string responsibles { get; set; } = string.Empty;
        public string documentation { get; set; } = string.Empty;
        public string priority { get; set; } = string.Empty;
        public bool finished { get; set; } = false;
        public DateTime createdOn { get; set; } = DateTime.Now;
        public int? sectorId { get; set; }
        public int? parentProcessId { get; set; }
    }
}