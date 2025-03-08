using api.Models;
using api.Models.Dtos.Department;

namespace api.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentWithIdDto>> GetAll();
        Task<List<DepartmentAndSectorsDto>> GetAllDepartmentsAndSectors();
        Task<Department?> GetById(int id);
        Task<Department> Create(DepartmentDto departmentDto);
        Task<DepartmentDto?> Update(int id, UpdateDepartmentDto updateDto);
    }
}
