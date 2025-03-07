using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Department;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        [Route("/get-all-departments")]
        public IActionResult GetAll()
        {
            var departments = _context.Departments.Select(x => new
            {
                x.id,
                x.name
            })
            .ToList();

            return Ok(departments);
        }

        [HttpGet]
        [Route("/get-all-departments-sectors")]
        public IActionResult GetAllDepartmentsSectors()
        {
            var departments = _context.Departments
            .Select(d => new
            {
                d.id,
                d.name,
                sectors = d.sectors.Select(s => new
                {
                    s.id,
                    s.name
                }).ToList()
            })
            .ToList();

            return Ok(departments);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var department = _context.Departments.Find(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DepartmentDto departmentDto)
        {
            var departmentModel = departmentDto.ToDepartmentFromCreateDto();
            _context.Departments.Add(departmentModel);
            _context.SaveChanges(); // commit

            return CreatedAtAction(nameof(GetById), new { id = departmentModel.id }, departmentModel.ToDepartmentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateDepartmentDto updateDto)
        {
            var departmentModel = _context.Departments.FirstOrDefault(x => x.id == id);

            if (departmentModel == null)
                return NotFound();

            departmentModel.name = updateDto.name;

            _context.SaveChanges();

            return Ok(departmentModel.ToDepartmentDto());
        }
    }
}