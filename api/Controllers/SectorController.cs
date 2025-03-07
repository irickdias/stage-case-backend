using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
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
        private readonly ApplicationDBContext _context;

        public SectorController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/get-all-sectors")]
        public IActionResult GetAll() {
            var sectors = _context.Sectors.Select(x => new {
                x.id,
                x.name,
                x.departmentId
            })
            .ToList();

            return Ok(sectors);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var sector = _context.Sectors.Find(id);

            if(sector == null )
                return NotFound();
            
            return Ok(sector);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SectorDto sectorDto) {
            var sectorModel = sectorDto.ToSectorFromCreateDto();
            _context.Sectors.Add(sectorModel);
            _context.SaveChanges(); // commit

            return CreatedAtAction(nameof(GetById), new {id = sectorModel.id}, sectorModel.ToSectorDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateSectorDto updateDto) {
            var sectorModel = _context.Sectors.FirstOrDefault(x => x.id == id);

            if(sectorModel == null)
                return NotFound();
            
            sectorModel.name = updateDto.name;
            sectorModel.departmentId = updateDto.departmentId;

            _context.SaveChanges();

            return Ok(sectorModel.ToSectorDto());
        }
    }
}