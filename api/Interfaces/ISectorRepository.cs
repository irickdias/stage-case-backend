using api.Models;
using api.Models.Dtos.Sector;

namespace api.Interfaces
{
    public interface ISectorRepository
    {
        Task<List<SectorWithIdDto>> GetAll();
        Task<Sector?> GetById(int id);
        Task<Sector> Create(SectorDto sectorDto);
        Task<SectorDto?> Update(int id, UpdateSectorDto updateDto);
    }
}
