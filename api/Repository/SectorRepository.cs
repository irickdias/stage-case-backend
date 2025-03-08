using api.Data;
using api.Interfaces;
using api.Models;
using api.Models.Dtos.Sector;
using api.Models.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class SectorRepository : ISectorRepository
    {
        private readonly ApplicationDBContext _context;

        public SectorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Sector> Create(SectorDto sectorDto)
        {
            var sectorModel = sectorDto.ToSectorFromCreateDto();
            await _context.Sectors.AddAsync(sectorModel);
            await _context.SaveChangesAsync();

            return sectorModel;
        }

        public async Task<List<SectorWithIdDto>> GetAll()
        {
            return await _context.Sectors.Select(s => s.ToSectorWithIdDto()).ToListAsync();
        }

        public async Task<Sector?> GetById(int id)
        {
            return await _context.Sectors.FindAsync(id);
        }

        public async Task<SectorDto?> Update(int id, UpdateSectorDto updateDto)
        {
            var sectorModel = await _context.Sectors.FirstOrDefaultAsync(s => s.id == id);

            if (sectorModel == null)
                return null;

            sectorModel.name = updateDto.name;
            sectorModel.departmentId = updateDto.departmentId;

            _context.SaveChanges();

            return sectorModel.ToSectorDto();
        }
    }
}
