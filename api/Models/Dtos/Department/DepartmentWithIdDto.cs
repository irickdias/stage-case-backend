using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Dtos.Department
{
    public class DepartmentWithIdDto
    {
        public int id { get; set; }
        public string name{ get; set; } = string.Empty;
    }
}