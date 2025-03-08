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
