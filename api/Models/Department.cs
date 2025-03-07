using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Department
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public List<Sector> sectors { get; set; } = new List<Sector>();
    }
}