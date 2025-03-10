using System.Diagnostics;
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

                    // Deleta os processos recursivamente usando SQL
                    await DeleteAllProcessesFromDepartment(id);

                    // 3Deleta os setores do departamento usando SQL
                    await _context.Database.ExecuteSqlRawAsync(@"
                        DELETE FROM Sectors WHERE departmentId = {0}", id);

                    // 4️⃣ Deleta o departamento usando SQL
                    await _context.Database.ExecuteSqlRawAsync(@"
                        DELETE FROM Departments WHERE id = {0}", id);

                    // salva
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

        private async Task DeleteAllProcessesFromDepartment(int departmentId)
        {
            // Deleta os processos de setores associados ao departamento, de forma recursiva, porem direto do banco
            var deleteProcessSql = @"
            WITH ProcessHierarchy AS (
                SELECT id, parentProcessId
                FROM Processes
                WHERE sectorId IN (SELECT id FROM Sectors WHERE departmentId = {0})
                UNION ALL
                SELECT p.id, p.parentProcessId
                FROM Processes p
                INNER JOIN ProcessHierarchy ph ON p.parentProcessId = ph.id
            )
            DELETE FROM Processes WHERE id IN (SELECT id FROM ProcessHierarchy)"; // Talvez seja necessario colocar DISTINCT, para eliminar duplicatas

            await _context.Database.ExecuteSqlRawAsync(deleteProcessSql, departmentId);
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
