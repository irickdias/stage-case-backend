using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models.Dtos.Process;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessRepository _repo;

        public ProcessController(IProcessRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("/get-all-process")]
        public async Task<IActionResult> GetAll() {
            var processes = await _repo.GetAll();

            return Ok(processes);
        }

        [HttpGet]
        [Route("/get-hierarchy")]
        public async Task<IActionResult> GetHierarchy([FromQuery] QueryObject query) {
            var hierarchy = await _repo.GetProcessesHierarchy(query);

            return Ok(hierarchy);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var process = await _repo.GetById(id);

            if (process == null)
                return NotFound();

            return Ok(process.ToProcessDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProcessDto processDto)
        {
            var createdProcess = await _repo.Create(processDto);

            return CreatedAtAction(nameof(GetById), new { id = createdProcess.id }, createdProcess.ToProcessDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProcessDto updateDto)
        {
            var updatedProcess = await _repo.Update(id, updateDto);

            if(updatedProcess == null)
                return NotFound();
            
            return Ok(updatedProcess);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var processModel = await _repo.Delete(id);

            if (processModel == null)
                return NotFound();

            return NoContent();
        }
    }
}