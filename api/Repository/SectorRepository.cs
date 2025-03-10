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

        public async Task<Sector?> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var sector = await _context.Sectors.FirstOrDefaultAsync(s => s.id == id);

                    if (sector == null)
                        return null;

                    await DeleteAllProcessesFromSector(id);

                    await _context.Database.ExecuteSqlRawAsync(@"
                        DELETE FROM Sectors WHERE id = {0}", id);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return sector;
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            throw new NotImplementedException();
        }

        private async Task DeleteAllProcessesFromSector(int sectorId)
        {
            // Deleta os processos de setores associados ao departamento, de forma recursiva, porem direto do banco
            var deleteProcessSql = @"
            WITH ProcessHierarchy AS (
                SELECT id, parentProcessId
                FROM Processes
                WHERE sectorId = {0}
                UNION ALL
                SELECT p.id, p.parentProcessId
                FROM Processes p
                INNER JOIN ProcessHierarchy ph ON p.parentProcessId = ph.id
            )
            DELETE FROM Processes WHERE id IN (SELECT id FROM ProcessHierarchy)"; // Talvez seja necessario colocar DISTINCT, para eliminar duplicatas

            await _context.Database.ExecuteSqlRawAsync(deleteProcessSql, sectorId);
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
