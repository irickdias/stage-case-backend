using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Department;
using api.Models.Mappers;
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

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var department = _context.Departments.Find(id);

            if(department == null )
                return NotFound();
            
            return Ok(department);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DepartmentDto departmentDto) {
            var departmentModel = departmentDto.ToDepartmentFromCreateDto();
            _context.Departments.Add(departmentModel);
            _context.SaveChanges(); // commit

            return CreatedAtAction(nameof(GetById), new {id = departmentModel.id}, departmentModel.ToDepartmentDto());
        }
    }
}