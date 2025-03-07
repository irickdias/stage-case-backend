using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Dtos.Sector
{
    public class SectorWithIdDto
    {
        public int id { get; set; }
        public string name{ get; set; } = string.Empty;
        public int? departmentId { get; set; }
    }
}