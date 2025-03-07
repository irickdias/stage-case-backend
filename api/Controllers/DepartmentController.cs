using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public DepartmentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("/get-all")]
        public IActionResult GetAll() {
            var departments = _context.Departments.Select(p => new {
                p.id,
                p.name
            })
            .ToList();

            return Ok(departments);
        }
    }
}