using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Sector
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        // public DateTime createdOn { get; set; } = DateTime.Now;
        public int? departmentId { get; set; } // ? indica que pode receber nulo
        public Department? department { get; set; }
        public List<Process> processes { get; set; } = new List<Process>();
    }
}