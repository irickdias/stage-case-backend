using System.Diagnostics;
using System.Linq;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Models.Dtos.Department;
using api.Models.Dtos.Sector;
using api.Models.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDBContext _context;

        public DepartmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Department> Create(DepartmentDto departmentDto)
        {
            var departmentModel = departmentDto.ToDepartmentFromCreateDto();
            await _context.Departments.AddAsync(departmentModel);
            await _context.SaveChangesAsync();

            return departmentModel;
        }

        public async Task<Department?> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verifica se o departamento existe
                    var department = await _context.Departments.FirstOrDefaultAsync(d => d.id == id);
                    
                    if (department == null)
                        return null;

                    // Verifica se há processos associados ao departamento
                    if (await HasProcessesInDepartment(id))
                        return null; // Retorna null para indicar que não pode excluir

                    // Deleta os setores do departamento
                    await _context.Database.ExecuteSqlRawAsync(@"
                    DELETE FROM Sectors WHERE departmentId = {0}", id);

                    // Deleta o departamento
                    await _context.Database.ExecuteSqlRawAsync(@"
                    DELETE FROM Departments WHERE id = {0}", id);

                    // Salva e confirma a transação
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return department;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<bool> HasProcessesInDepartment(int departmentId)
        {
            // 1️⃣ Obtém os IDs dos setores do departamento
            var sectorIds = await _context.Sectors
                .Where(s => s.departmentId == departmentId)
                .Select(s => s.id)
                .ToListAsync();

            if (!sectorIds.Any())
                return false; // Não há setores, então não pode haver processos

            return await _context.Processes.AnyAsync(p => p.sectorId.HasValue && sectorIds.Contains(p.sectorId.Value));
        }


        public async Task<List<DepartmentWithIdDto>> GetAll()
        {
            return await _context.Departments.Select(d => d.ToDepartmentWithIdDto()).ToListAsync();
        }

        public async Task<List<DepartmentAndSectorsDto>> GetAllDepartmentsAndSectors()
        {
            var ds = await _context.Departments.Select(d => new DepartmentAndSectorsDto
            {
                id = d.id,
                name = d.name,
                sectors = d.sectors.Select(s => new SectorWithIdDto
                {
                    id = s.id,
                    name = s.name,
                    departmentId = s.departmentId
                }).ToList()
            }).ToListAsync();

            return ds;
        }

        public async Task<Department?> GetById(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<DepartmentDto?> Update(int id, UpdateDepartmentDto updateDto)
        {
            var departmentModel = await _context.Departments.FirstOrDefaultAsync(d => d.id == id);

            if (departmentModel == null)
                return null;

            departmentModel.name = updateDto.name;

            await _context.SaveChangesAsync();

            return departmentModel.ToDepartmentDto();
        }

     
    }
}
