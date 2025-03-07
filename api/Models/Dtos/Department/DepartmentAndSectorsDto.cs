using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Dtos.Sector;

namespace api.Models.Dtos.Department
{
    public class DepartmentAndSectorsDto
    {
        public int id { get; set; }
        public string name{ get; set; } = string.Empty;
        public List<SectorWithIdDto> sectors { get; set; } = new();
    }
}