using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Process;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProcessController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/get-all-process")]
        public IActionResult GetAll() {
            var processes = _context.Processes.ToList();

            return Ok(processes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var process = _context.Processes.Find(id);

            if (process == null)
                return NotFound();

            return Ok(process);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProcessDto processDto)
        {
            var processModel = processDto.ToProcessFromCreateDto();
            _context.Processes.Add(processModel);
            _context.SaveChanges(); // commit

            return CreatedAtAction(nameof(GetById), new { id = processModel.id }, processModel.ToProcessDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateProcessDto updateDto)
        {
            var processModel = _context.Processes.FirstOrDefault(x => x.id == id);

            if (processModel == null)
                return NotFound();

            processModel.name = updateDto.name;
            processModel.tools = updateDto.tools;
            processModel.responsibles = updateDto.responsibles;
            processModel.documentation = updateDto.documentation;
            processModel.priority = updateDto.priority;
            processModel.finished = updateDto.finished;
            processModel.createdOn = updateDto.createdOn;
            processModel.sectorId = updateDto.sectorId;
            processModel.parentProcessId = updateDto.parentProcessId;

            _context.SaveChanges();

            return Ok(processModel.ToProcessDto());
        }
    }
}