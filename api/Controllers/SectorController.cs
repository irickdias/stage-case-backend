using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models.Dtos.Department;
using api.Models.Dtos.Sector;
using api.Models.Mappers;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/sector")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        private readonly SectorService _service;

        public SectorController(SectorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/get-all-sectors")]
        public IActionResult GetAll() {
            var sectors = _service.GetAllSectors();

            return Ok(sectors);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var sector = _service.GetSectorById(id);

            if(sector == null )
                return NotFound();
            
            return Ok(sector.ToSectorWithIdDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] SectorDto sectorDto) {
            var createdSector = _service.CreateSector(sectorDto);

            return CreatedAtAction(nameof(GetById), new {id = createdSector.id}, createdSector.ToSectorDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateSectorDto updateDto) {
            var updatedSector = _service.UpdateSector(id, updateDto);

            if(updatedSector == null)
                return NotFound();
            
            return Ok(updatedSector);
        }
    }
}