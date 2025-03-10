using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models.Dtos.Department;
using api.Models.Dtos.Sector;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/sector")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        private readonly ISectorRepository _repo;

        public SectorController(ISectorRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("/get-all-sectors")]
        public async Task<IActionResult> GetAll() {
            var sectors = await _repo.GetAll();

            return Ok(sectors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var sector = await _repo.GetById(id);

            if (sector == null )
                return NotFound();
            
            return Ok(sector.ToSectorWithIdDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SectorDto sectorDto) {
            var createdSector = await _repo.Create(sectorDto);

            return CreatedAtAction(nameof(GetById), new {id = createdSector.id}, createdSector.ToSectorDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSectorDto updateDto) {
            var updatedSector = await _repo.Update(id, updateDto);

            if (updatedSector == null)
                return NotFound();
            
            return Ok(updatedSector);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var sectorModel = await _repo.Delete(id);

            if (sectorModel == null)
                return NotFound();

            return NoContent();
        }
    }
}