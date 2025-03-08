using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
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
        private readonly IDepartmentRepository _repo;

        public DepartmentController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("/get-all-departments")]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _repo.GetAll();

            return Ok(departments);
        }

        [HttpGet]
        [Route("/get-all-departments-sectors")]
        public async Task<IActionResult> GetAllDepartmentsSectors()
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

            var ds = await _repo.GetAllDepartmentsAndSectors();

            return Ok(ds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var department = await _repo.GetById(id);

            if (department == null)
                return NotFound();

            return Ok(department.ToDepartmentWithIdDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDto departmentDto)
        {
            var createdDepartment = await _repo.Create(departmentDto);

            return CreatedAtAction(nameof(GetById), new { id = createdDepartment.id }, createdDepartment.ToDepartmentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDepartmentDto updateDto)
        {
            var updatedDepartment = await _repo.Update(id, updateDto);

            if (updatedDepartment == null)
                return NotFound();

            return Ok(updatedDepartment);
        }
    }
}