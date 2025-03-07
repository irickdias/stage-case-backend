using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Process;
using api.Models.Mappers;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        // private readonly ApplicationDBContext _context;
        private readonly ProcessService _service;

        public ProcessController(ProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/get-all-process")]
        public IActionResult GetAll() {
            var processes = _service.GetAllProcesses();

            return Ok(processes);
        }

        [HttpGet]
        [Route("/get-hierarchy")]
        public IActionResult GetHierarchy() {
            var hierarchy = _service.GetProcessesHierarchy();

            return Ok(hierarchy);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var process = _service.GetProcessById(id);

            if (process == null)
                return NotFound();

            return Ok(process.ToProcessDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProcessDto processDto)
        {
            var createdProcess = _service.CreateProcess(processDto);
            // _context.Processes.Add(processModel);
            // _context.SaveChanges(); // commit

            return CreatedAtAction(nameof(GetById), new { id = createdProcess.id }, createdProcess.ToProcessDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateProcessDto updateDto)
        {
            // var processModel = _context.Processes.FirstOrDefault(x => x.id == id);

            // if (processModel == null)
            //     return NotFound();

            // processModel.name = updateDto.name;
            // processModel.tools = updateDto.tools;
            // processModel.responsibles = updateDto.responsibles;
            // processModel.documentation = updateDto.documentation;
            // processModel.priority = updateDto.priority;
            // processModel.finished = updateDto.finished;
            // processModel.createdOn = updateDto.createdOn;
            // processModel.sectorId = updateDto.sectorId;
            // processModel.parentProcessId = updateDto.parentProcessId;

            // _context.SaveChanges();

            var updatedProcess = _service.UpdateProcess(id, updateDto);

            if(updatedProcess == null)
                return NotFound();
            
            return Ok(updatedProcess);
        }
    }
}