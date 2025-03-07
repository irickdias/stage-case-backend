using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Models.Dtos.Sector;
using api.Models.Mappers;

namespace api.Services
{
    public class SectorService
    {
        private readonly ApplicationDBContext _context;

        public SectorService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<SectorWithIdDto> GetAllSectors() {
            var sectors = _context.Sectors.Select(s => s.ToSectorWithIdDto()).ToList();

            return sectors;
        }

        public Sector? GetSectorById(int id) {
            var sector = _context.Sectors.Find(id);

            return sector;
        }

        public Sector CreateSector(SectorDto sectorDto) {
            var sectorModel = sectorDto.ToSectorFromCreateDto();
            _context.Sectors.Add(sectorModel);
            _context.SaveChanges();

            return sectorModel;
        }

        public SectorDto? UpdateSector(int id, UpdateSectorDto updateDto) {
            var sectorModel = _context.Sectors.FirstOrDefault(s => s.id == id);

            if(sectorModel == null)
                return null;
            
            sectorModel.name = updateDto.name;
            sectorModel.departmentId = updateDto.departmentId;

            _context.SaveChanges();

            return sectorModel.ToSectorDto();
        }
    }
}