using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Department;
using api.Models.Mappers;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentController(DepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/get-all-departments")]
        public IActionResult GetAll()
        {
            var departments = _service.GetAllDepartments();

            return Ok(departments);
        }

        [HttpGet]
        [Route("/get-all-departments-sectors")]
        public IActionResult GetAllDepartmentsSectors()
        {
            // var departments = _service.Departments
            // .Select(d => new
            // {
            //     d.id,
            //     d.name,
            //     sectors = d.sectors.Select(s => new
            //     {
            //         s.id,
            //         s.name
            //     }).ToList()
            // })
            // .ToList();

            var ds = _service.GetAllDepartmentsAndSectors();

            return Ok(ds);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var department = _service.GetDepartmentById(id);

            if (department == null)
                return NotFound();

            return Ok(department.ToDepartmentWithIdDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] DepartmentDto departmentDto)
        {
            var createdDepartment = _service.CreateDepartment(departmentDto);

            return CreatedAtAction(nameof(GetById), new { id = createdDepartment.id }, createdDepartment.ToDepartmentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateDepartmentDto updateDto)
        {
            var updatedDepartment = _service.UpdateDepartment(id, updateDto);

            if (updatedDepartment == null)
                return NotFound();

            return Ok(updatedDepartment);
        }
    }
}